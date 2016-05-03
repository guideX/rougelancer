﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.com
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Roguelancer.Particle {
    /// <summary>
    /// Particle Star Sheet
    /// </summary>
    public class ParticleStarSheet {
        #region "PRIVATE VARIABLES"
        /// <summary>
        /// Verts
        /// </summary>
        private VertexPositionTexture[] _verts;
        /// <summary>
        /// Vertex Color Array
        /// </summary>
        private Color[] _vertexColorArray;
        /// <summary>
        /// Particle Vertex Buffer
        /// </summary>
        private VertexBuffer _particleVertexBuffer;
        /// <summary>
        /// Max Position
        /// </summary>
        private Vector3 _maxPosition;
        /// <summary>
        /// Max Particles
        /// </summary>
        private int _maxParticles;
        /// <summary>
        /// Rnd
        /// </summary>
        private static Random _rnd = new Random();
        /// <summary>
        /// Particle Effect
        /// </summary>
        private Effect _particleEffect;
        /// <summary>
        /// Particle Colors texture
        /// </summary>
        private Texture2D _particleColorsTexture;
        #endregion
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="game"></param>
        /// <param name="_MaxPosition"></param>
        /// <param name="_MaxParticles"></param>
        /// <param name="_ParticleColorsTexture"></param>
        /// <param name="_ParticleEffect"></param>
        /// <param name="_MaxSize"></param>
        public ParticleStarSheet(RoguelancerGame game, Vector3 maxPosition, int maxParticles, Texture2D particleColorsTexture, Effect particleEffect, int maxSize) {
            _maxParticles = maxParticles;
            _particleEffect = particleEffect;
            _particleColorsTexture = particleColorsTexture;
            _maxPosition = maxPosition;
            _verts = new VertexPositionTexture[_maxParticles * 4];
            _vertexColorArray = new Color[_maxParticles];
            Color[] colors = new Color[_particleColorsTexture.Width * _particleColorsTexture.Height];
            _particleColorsTexture.GetData(colors);
            for (var i = 0; i < _maxParticles; ++i) {
                float size = (float)_rnd.NextDouble() * maxSize;
                //float size = 10000f;
                Vector3 position = new Vector3(_rnd.Next(-(int)_maxPosition.X, (int)_maxPosition.X), _rnd.Next(-(int)_maxPosition.Y, (int)_maxPosition.Y), _maxPosition.Z);
                _verts[i * 4] = new VertexPositionTexture(position, new Vector2(0, 0));
                _verts[(i * 4) + 1] = new VertexPositionTexture(new Vector3(position.X, position.Y + size, position.Z), new Vector2(0, 1));
                _verts[(i * 4) + 2] = new VertexPositionTexture(new Vector3(position.X + size, position.Y, position.Z), new Vector2(1, 0));
                _verts[(i * 4) + 3] = new VertexPositionTexture(new Vector3(position.X + size, position.Y + size, position.Z), new Vector2(1, 1));
                _vertexColorArray[i] = colors[(_rnd.Next(0, _particleColorsTexture.Height) * _particleColorsTexture.Width) + _rnd.Next(0, _particleColorsTexture.Width)];
            }
            _particleVertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionTexture), _verts.Length, BufferUsage.None);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            //lCamera = game.Camera;
            //lGraphicsDevice = _GraphicsDevice;
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            game.GraphicsDevice.SetVertexBuffer(_particleVertexBuffer);
            for (int i = 0; i < _maxParticles; ++i) {
                _particleEffect.Parameters["WorldViewProjection"].SetValue(game.Camera.View * game.Camera.Projection);
                _particleEffect.Parameters["particleColor"].SetValue(_vertexColorArray[i].ToVector4());
                foreach (var effectPass in _particleEffect.CurrentTechnique.Passes) {
                    effectPass.Apply();
                    game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _verts, i * 4, 2);
                }
            }
        }
    }
    /// <summary>
    /// Particle Explosion
    /// </summary>
    public class ParticleExplosion {
        /// <summary>
        /// Verts
        /// </summary>
        private VertexPositionTexture[] _verts;
        /// <summary>
        /// Vertex Direction Array
        /// </summary>
        private Vector3[] _vertexDirectionArray;
        /// <summary>
        /// Vertex Color Array
        /// </summary>
        private Color[] _vertexColorArray;
        /// <summary>
        /// Particle Vertex Buffer
        /// </summary>
        private VertexBuffer _particleVertexBuffer;
        /// <summary>
        /// Position
        /// </summary>
        private Vector3 _position;
        /// <summary>
        /// Life Left
        /// </summary>
        private int _lifeLeft;
        /// <summary>
        /// Num Particles Per Round
        /// </summary>
        private int _numParticlesPerRound;
        /// <summary>
        /// Max Particles
        /// </summary>
        private int _maxParticles;
        /// <summary>
        /// Rnd
        /// </summary>
        private static Random _rnd = new Random();
        /// <summary>
        /// Round Time
        /// </summary>
        private int _roundTime;
        /// <summary>
        /// Time Since Last Round
        /// </summary>
        private int _timeSinceLastRound = 0;
        /// <summary>
        /// Graphics Device
        /// </summary>
        private GraphicsDevice _graphicsDevice;
        /// <summary>
        /// Particle Effect
        /// </summary>
        private Effect _particleEffect;
        /// <summary>
        /// Particle Colors Texture
        /// </summary>
        private Texture2D _particleColorsTexture;
        /// <summary>
        /// End of Live Particles Index
        /// </summary>
        private int _endOfLiveParticlesIndex = 0;
        /// <summary>
        /// End of Dear Particles Index
        /// </summary>
        private int _endOfDeadParticlesIndex = 0;
        /// <summary>
        /// Is Dead
        /// </summary>
        public bool IsDead {
            get {
                return _endOfDeadParticlesIndex == _maxParticles; 
            }
        }
        /// <summary>
        /// Particle Explosion
        /// </summary>
        /// <param name="_GraphicsDevice"></param>
        /// <param name="_Position"></param>
        /// <param name="_LifeLeft"></param>
        /// <param name="_RoundTime"></param>
        /// <param name="_NumParticlesPerRound"></param>
        /// <param name="_MaxParticles"></param>
        /// <param name="_ParticleColorsTexture"></param>
        /// <param name="_ParticleEffect"></param>
        /// <param name="_MaxSize"></param>
        public ParticleExplosion(GraphicsDevice graphicsDevice, Vector3 position, int lifeLeft, int roundTime, int numParticlesPerRound, int maxParticles, Texture2D particleColorsTexture, Effect particleEffect, int maxSize) {
            _position = position;
            _lifeLeft = lifeLeft;
            _numParticlesPerRound = numParticlesPerRound;
            _maxParticles = maxParticles;
            _roundTime = roundTime;
            _graphicsDevice = graphicsDevice;
            _particleEffect = particleEffect;
            _particleColorsTexture = particleColorsTexture;
            InitializeParticleVertices(maxSize);
        }
        /// <summary>
        /// Initialize Particle Vertices
        /// </summary>
        /// <param name="_MaxSize"></param>
        public void InitializeParticleVertices(int maxSize) {
            _verts = new VertexPositionTexture[_maxParticles * 4];
            _vertexDirectionArray = new Vector3[_maxParticles];
            _vertexColorArray = new Color[_maxParticles];
            var colors = new Color[_particleColorsTexture.Width * _particleColorsTexture.Height];
            _particleColorsTexture.GetData(colors);
            for (var i = 0; i < _maxParticles; ++i) {
                float size = (float)_rnd.NextDouble() * maxSize;
                _verts[i * 4] = new VertexPositionTexture(_position, new Vector2(0, 0));
                _verts[(i * 4) + 1] = new VertexPositionTexture(new Vector3(_position.X, _position.Y + size, _position.Z), new Vector2(0, 1));
                _verts[(i * 4) + 2] = new VertexPositionTexture(new Vector3(_position.X + size, _position.Y, _position.Z), new Vector2(1, 0));
                _verts[(i * 4) + 3] = new VertexPositionTexture(new Vector3(_position.X + size, _position.Y + size, _position.Z), new Vector2(1, 1));
                var direction = new Vector3((float)_rnd.NextDouble() * 2 - 1,(float)_rnd.NextDouble() * 2 - 1,(float)_rnd.NextDouble() * 2 - 1);
                direction.Normalize();
                direction *= (float)_rnd.NextDouble();
                _vertexDirectionArray[i] = direction;
                _vertexColorArray[i] = colors[(_rnd.Next(0, _particleColorsTexture.Height) * _particleColorsTexture.Width) + _rnd.Next(0, _particleColorsTexture.Width)];
            }
            _particleVertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionTexture), _verts.Length, BufferUsage.None);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            if (_lifeLeft > 0) {
                _lifeLeft -= gameTime.ElapsedGameTime.Milliseconds;
            }
            _timeSinceLastRound += gameTime.ElapsedGameTime.Milliseconds;
            if (_timeSinceLastRound > _roundTime) {
                _timeSinceLastRound -= _roundTime;
                if (_endOfLiveParticlesIndex < _maxParticles) {
                    _endOfLiveParticlesIndex += _numParticlesPerRound;
                    if (_endOfLiveParticlesIndex > _maxParticles)
                        _endOfLiveParticlesIndex = _maxParticles;
                }
                if (_lifeLeft <= 0) {
                    if (_endOfDeadParticlesIndex < _maxParticles) {
                        _endOfDeadParticlesIndex += _numParticlesPerRound;
                        if (_endOfDeadParticlesIndex > _maxParticles) {
                            _endOfDeadParticlesIndex = _maxParticles;
                        }
                    }
                }
            }
            for (var i = _endOfDeadParticlesIndex; i < _endOfLiveParticlesIndex; ++i) {
                _verts[i * 4].Position += _vertexDirectionArray[i];
                _verts[(i * 4) + 1].Position += _vertexDirectionArray[i];
                _verts[(i * 4) + 2].Position += _vertexDirectionArray[i];
                _verts[(i * 4) + 3].Position += _vertexDirectionArray[i];
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            _graphicsDevice.SetVertexBuffer(_particleVertexBuffer);
            if (_endOfLiveParticlesIndex - _endOfDeadParticlesIndex > 0) {
                for (var i = _endOfDeadParticlesIndex; i < _endOfLiveParticlesIndex; ++i) {
                    _particleEffect.Parameters["WorldViewProjection"].SetValue(game.Camera.View * game.Camera.Projection);
                    _particleEffect.Parameters["particleColor"].SetValue(_vertexColorArray[i].ToVector4());
                    foreach (var effectPass in _particleEffect.CurrentTechnique.Passes) {
                        effectPass.Apply();
                        _graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _verts, i * 4, 2);
                    }
                }
            }
        }
    }
}