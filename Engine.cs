using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Silk.NET.Maths;
using TheAdventure.Models;
using TheAdventure.Models.Data;
using TheAdventure.Scripting;

namespace TheAdventure
{
    public class Engine
    {
        private GameRenderer _renderer;
        private bool _disposed = false;
        private readonly Input _input;
        private readonly ScriptEngine _scriptEngine = new();

        private readonly Dictionary<int, GameObject> _gameObjects = new();
        private readonly Dictionary<string, TileSet> _loadedTileSets = new();
        private readonly Dictionary<int, Tile> _tileIdMap = new();

        private Level _currentLevel = new();
        private PlayerObject? _player;

        private DateTimeOffset _lastUpdate = DateTimeOffset.Now;
private DifficultySettings _difficultySettings = new();

        private DateTimeOffset _lastBombTime = DateTimeOffset.MinValue;
private GameWindow _gameWindow;
private void ApplyDifficultySettings(string difficulty)
{
    switch (difficulty)
    {
        case "Easy":
            _difficultySettings = new DifficultySettings
            {
                PlayerSpeed = 150, // pixels per second
                BombCooldown = 7000, // milliseconds
                BombDuration = 2000 // milliseconds
            };
            break;
        case "Hard":
            _difficultySettings = new DifficultySettings
            {
                PlayerSpeed = 90, // pixels per second
                BombCooldown = 1000, // milliseconds
                BombDuration = 2000 // milliseconds
            };
            break;
        default: // Normal
            _difficultySettings = new DifficultySettings
            {
                PlayerSpeed = 120, // pixels per second
                BombCooldown = 2000, // milliseconds
                BombDuration = 2000 // milliseconds
            };
            break;
    }

    Console.WriteLine($"{difficulty} mode settings applied.");
}
        public Engine(GameRenderer renderer, Input input, GameWindow gameWindow)
    {
        _renderer = renderer;
        _input = input;
        _gameWindow = gameWindow;

        _input.OnMouseClick += (_, coords) => AddBomb(coords.x, coords.y);

        // Initialize difficulty settings
        ApplyDifficultySettings("Normal"); // Default to Normal difficulty
    }
public void StartGame(string difficulty = "Normal")
{
    // Initialize game with selected difficulty
    Console.WriteLine($"Starting game with difficulty: {difficulty}");

    // Load the game world
    SetupWorld(difficulty);
}

private void SetupWorld(string difficulty)
{
    ApplyDifficultySettings(difficulty);

    _player = new PlayerObject(
        SpriteSheet.Load(_renderer, "Player.json", "Assets"),
        100, 100,
        _difficultySettings
    );
    _player.OnPlayerDeath += ShowGameOverScreen;

    var levelContent = File.ReadAllText(Path.Combine("Assets", "terrain.tmj"));
    var level = JsonSerializer.Deserialize<Level>(levelContent);
    if (level == null)
    {
        throw new Exception("Failed to load level");
    }

    foreach (var tileSetRef in level.TileSets)
    {
        var tileSetContent = File.ReadAllText(Path.Combine("Assets", tileSetRef.Source));
        var tileSet = JsonSerializer.Deserialize<TileSet>(tileSetContent);
        if (tileSet == null)
        {
            throw new Exception("Failed to load tile set");
        }

        foreach (var tile in tileSet.Tiles)
        {
            tile.TextureId = _renderer.LoadTexture(Path.Combine("Assets", tile.Image), out _);
            _tileIdMap.Add(tile.Id!.Value, tile);
        }

        _loadedTileSets.Add(tileSet.Name, tileSet);
    }

    if (level.Width == null || level.Height == null)
    {
        throw new Exception("Invalid level dimensions");
    }

    if (level.TileWidth == null || level.TileHeight == null)
    {
        throw new Exception("Invalid tile dimensions");
    }

    _renderer.SetWorldBounds(new Rectangle<int>(0, 0, level.Width.Value * level.TileWidth.Value,
        level.Height.Value * level.TileHeight.Value));

    _currentLevel = level;

    _scriptEngine.LoadAll(Path.Combine("Assets", "Scripts"));

    // Apply difficulty settings
    ApplyDifficultySettings(difficulty);
}

        

        public void ProcessFrame()
        {
            var currentTime = DateTimeOffset.Now;
            var msSinceLastFrame = (currentTime - _lastUpdate).TotalMilliseconds;
            _lastUpdate = currentTime;

            if (_player == null)
            {
                return;
            }

            double up = _input.IsUpPressed() ? 1.0 : 0.0;
            double down = _input.IsDownPressed() ? 1.0 : 0.0;
            double left = _input.IsLeftPressed() ? 1.0 : 0.0;
            double right = _input.IsRightPressed() ? 1.0 : 0.0;
            bool isAttacking = _input.IsKeyAPressed() && (up + down + left + right <= 1);
            bool addBomb = _input.IsKeyBPressed();

            _player.UpdatePosition(up, down, left, right, 48, 48, msSinceLastFrame);
            if (isAttacking)
            {
                _player.Attack();
            }
            
            _scriptEngine.ExecuteAll(this);

            if (addBomb)
            {
                AddBomb(_player.Position.X, _player.Position.Y, false);
            }
        }

        public void RenderFrame()
        {
            _renderer.SetDrawColor(0, 0, 0, 255);
            _renderer.ClearScreen();

            var playerPosition = _player!.Position;
            _renderer.CameraLookAt(playerPosition.X, playerPosition.Y);

            RenderTerrain();
            RenderAllObjects();

            _renderer.PresentFrame();
        }

        public void RenderAllObjects()
        {
            var toRemove = new List<int>();
            foreach (var gameObject in GetRenderables())
            {
                gameObject.Render(_renderer);
                if (gameObject is TemporaryGameObject { IsExpired: true } tempGameObject)
                {
                    toRemove.Add(tempGameObject.Id);
                }
            }

            foreach (var id in toRemove)
            {
                _gameObjects.Remove(id, out var gameObject);

                if (_player == null)
                {
                    continue;
                }

                var tempGameObject = (TemporaryGameObject)gameObject!;
                var deltaX = Math.Abs(_player.Position.X - tempGameObject.Position.X);
                var deltaY = Math.Abs(_player.Position.Y - tempGameObject.Position.Y);
                if (deltaX < 32 && deltaY < 32)
                {
                    _player.GameOver();
                }
            }

            _player?.Render(_renderer);
        }

        public void RenderTerrain()
        {
            foreach (var currentLayer in _currentLevel.Layers)
            {
                for (int i = 0; i < _currentLevel.Width; ++i)
                {
                    for (int j = 0; j < _currentLevel.Height; ++j)
                    {
                        int? dataIndex = j * currentLayer.Width + i;
                        if (dataIndex == null)
                        {
                            continue;
                        }

                        var currentTileId = currentLayer.Data[dataIndex.Value] - 1;
                        if (currentTileId == null)
                        {
                            continue;
                        }

                        var currentTile = _tileIdMap[currentTileId.Value];

                        var tileWidth = currentTile.ImageWidth ?? 0;
                        var tileHeight = currentTile.ImageHeight ?? 0;

                        var sourceRect = new Rectangle<int>(0, 0, tileWidth, tileHeight);
                        var destRect = new Rectangle<int>(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        _renderer.RenderTexture(currentTile.TextureId, sourceRect, destRect);
                    }
                }
            }
        }

        public IEnumerable<RenderableGameObject> GetRenderables()
        {
            foreach (var gameObject in _gameObjects.Values)
            {
                if (gameObject is RenderableGameObject renderableGameObject)
                {
                    yield return renderableGameObject;
                }
            }
        }

        public (int X, int Y) GetPlayerPosition()
        {
            return _player!.Position;
        }


public void AddBomb(int X, int Y, bool translateCoordinates = true)
{
    var currentTime = DateTimeOffset.Now;
    var timeSinceLastBomb = (currentTime - _lastBombTime).TotalMilliseconds;

    if (timeSinceLastBomb < _difficultySettings.BombCooldown)
    {
        return; // Bomb cooldown has not expired
    }

    _lastBombTime = currentTime;

    var worldCoords = translateCoordinates ? _renderer.ToWorldCoordinates(X, Y) : new Vector2D<int>(X, Y);

    SpriteSheet spriteSheet = SpriteSheet.Load(_renderer, "BombExploding.json", "Assets");
    spriteSheet.ActivateAnimation("Explode");

    TemporaryGameObject bomb = new(spriteSheet, _difficultySettings.BombDuration / 1000.0, (worldCoords.X, worldCoords.Y));
    _gameObjects.Add(bomb.Id, bomb);
}
public void DisposeGame()
{
    if (_disposed)
    {
        return; // If already disposed, do nothing
    }

    _renderer.Dispose();
    _gameWindow.Dispose();

    _disposed = true; // Set the flag to indicate disposal
}
public void ShowGameOverScreen()
{
    // Dispose of the game window and renderer
    _renderer.Dispose();
    _gameWindow.Dispose();

    // Show the game over screen
    var gameOverScreen = new GameOverScreen(this);
    gameOverScreen.ShowDialog();
}
    }
}