using Silk.NET.SDL;
using Silk.NET.Maths;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TheAdventure.Models;

using ImageSharpImage = SixLabors.ImageSharp.Image;

namespace TheAdventure
{
    public unsafe partial class GameRenderer
    {
        private static GameRenderer? _instance;
        private readonly Sdl _sdl;
        private readonly Renderer* _renderer;
        private readonly GameLogic _gameLogic;
        private readonly Dictionary<int, nint> _texturePointers = new();
        private readonly Dictionary<int, TextureData> _textureData = new();
        private int _index = 0;
        private DateTimeOffset _lastFrameRenderedAt = DateTimeOffset.MinValue;

        // Singleton accessor
        public static GameRenderer GetInstance(Sdl sdl, GameWindow gameWindow, GameLogic gameLogic)
        {
            return _instance ??= new GameRenderer(sdl, gameWindow, gameLogic);
        }

        // Accessor if needed after init
        public static GameRenderer Instance => _instance 
            ?? throw new InvalidOperationException("GameRenderer is not initialized. Call GetInstance() first.");

        private GameRenderer(Sdl sdl, GameWindow gameWindow, GameLogic gameLogic)
        {
            _sdl = sdl ?? throw new ArgumentNullException(nameof(sdl));
            _gameLogic = gameLogic ?? throw new ArgumentNullException(nameof(gameLogic));

            _renderer = (Renderer*)gameWindow.CreateRenderer();
            if (_renderer == null)
                throw new InvalidOperationException("Renderer creation failed.");
        }

        public static int LoadTexture(string fileName, out TextureData textureData)
        {
            if (_instance == null)
                throw new InvalidOperationException("GameRenderer instance is not initialized. Call GetInstance first.");

            using var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var image = SixLabors.ImageSharp.Image.Load<Rgba32>(fStream);
            if (image == null)
            {
                textureData = default;
                return -1;
            }

            textureData = new TextureData
            {
                Width = image.Width,
                Height = image.Height,
            };

            var imageRawData = new byte[textureData.Width * textureData.Height * 4];
            image.CopyPixelDataTo(imageRawData.AsSpan());

            fixed (byte* data = imageRawData)
            {
                Surface* imageSurface = _instance._sdl.CreateRGBSurfaceWithFormatFrom(
                    (void*)data,
                    textureData.Width,
                    textureData.Height,
                    32,
                    textureData.Width * 4,
                    (uint)PixelFormatEnum.Rgba8888
                );

                if (imageSurface == null)
                    throw new InvalidOperationException("Failed to create image surface.");

                var imageTexture = _instance._sdl.CreateTextureFromSurface(_instance._renderer, imageSurface);
                _instance._sdl.FreeSurface(imageSurface);

                if (imageTexture == null)
                    throw new InvalidOperationException("Failed to create texture from surface.");

                int id = _instance._index++;
                _instance._texturePointers[id] = (nint)imageTexture;
                _instance._textureData[id] = textureData;
                return id;
            }
        }

        public void RenderGameObject(RenderableGameObject gameObject)
        {
            if (_texturePointers.TryGetValue(gameObject.TextureId, out var imageTexture))
            {
                var textureSrc = gameObject.TextureSource;
                var textureDest = gameObject.TextureDestination;
                var rotCenter = gameObject.TextureRotationCenter;

                _sdl.RenderCopyEx(_renderer, (Texture*)imageTexture, &textureSrc, &textureDest,
                    gameObject.TextureRotation, &rotCenter, RendererFlip.None);
            }
        }

        public void Render()
        {
            _sdl.RenderClear(_renderer);

            var now = DateTimeOffset.UtcNow;
            var timeSinceLastFrame = _lastFrameRenderedAt > DateTimeOffset.MinValue
                ? (int)(now - _lastFrameRenderedAt).TotalMilliseconds
                : 0;

            _gameLogic.RenderTerrain(this);
            _gameLogic.RenderAllObjects(timeSinceLastFrame, this);
            _lastFrameRenderedAt = now;

            _sdl.RenderPresent(_renderer);
        }

        public void RenderTexture(int textureId, Rectangle<int> src, Rectangle<int> dst)
        {
            if (_texturePointers.TryGetValue(textureId, out var texture))
            {
                _sdl.RenderCopy(_renderer, (Texture*)texture, &src, &dst);
            }
        }
    }
}
