﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelancer.Models;
using Roguelancer.Interfaces;
using Roguelancer.Enum;
using Roguelancer.Helpers;
using Roguelancer.Settings;
namespace Roguelancer.Objects {
    /// <summary>
    /// Bullets
    /// </summary>
    public class Bullets : IGame {
        #region "public properties"
        /// <summary>
        /// Bullets Model
        /// </summary>
        public Model BulletsModel { get; set; }
        #endregion
        #region "private properties"
        /// <summary>
        /// Model
        /// </summary>
        private BulletsModel _model { get; set; }
        /// <summary>
        /// Particle Systems Settings
        /// </summary>
        private ParticleSystemSettingsModel _particleSystemSettings { get; set; }
        #endregion
        #region "public methods"
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="game"></param>
        public Bullets(RoguelancerGame game) {
            Reset();
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            _model.AreBulletsAvailable = true;
            _model.RechargeRate = game.Settings.Model.BulletRechargeRate;
        }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            BulletsModel = game.Content.Load<Model>("bullet");
            _model.PlayerShip = ShipHelper.GetPlayerShip(game); // Get Player Ship
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            if (game.Input.InputItems.Keys.LeftControl.IsKeyDown || game.Input.InputItems.Keys.RightControl.IsKeyDown || game.Input.InputItems.Mouse.RightButton) {
                if (_model.AreBulletsAvailable) {
                    game.Camera.Shake(10f, 0f, false);
                    _model.Bullets.Add(new BulletObject(_model.PlayerShip, game, new Vector3(-100f, -200f, 0f), particleSystemSettings: _particleSystemSettings));
                    _model.Bullets.Add(new BulletObject(_model.PlayerShip, game, new Vector3(-100f, 700f, 0f), particleSystemSettings: _particleSystemSettings));
                    _model.AreBulletsAvailable = false;
                    _model.WeaponRechargedTime = DateTime.Now.AddMilliseconds(_model.RechargeRate);
                }
            }
            if (!_model.AreBulletsAvailable) {
                if (DateTime.Now >= _model.WeaponRechargedTime) {
                    _model.AreBulletsAvailable = true;
                    _model.WeaponRechargedTime = new DateTime();
                }
            }
            for (int i = 0; i <= _model.Bullets.Count - 1; i++) {
                _model.Bullets[i].Update(game); // Update Bullet
                if (_model.Bullets[i].BulletModel.DeathDate <= DateTime.Now) {
                    _model.Bullets[i].Dispose(game);
                    _model.Bullets.RemoveAt(i); // Remove old bullets
                }
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            foreach (var bullet in _model.Bullets) {
                bullet.Draw(game);
            }
        }
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset() {
            _model = new BulletsModel();
            _particleSystemSettings = new ParticleSystemSettingsModel() {
                CameraArc = 2,
                CameraRotation = 0f,
                CameraDistance = 110,
                FireRingSystemParticles = 20,
                SmokePlumeParticles = 20,
                SmokeRingParticles = 20,
                Fire = true,
                Enabled = false,
                Smoke = true,
                SmokeRing = true,
                Explosions = true,
                Projectiles = true,
                ExplosionTexture = "Textures\\Explosion",
                FireTexture = "Textures\\Fire",
                SmokeTexture = "Textures\\Smoke"
            };
            _model.Bullets = new List<IBullet>();
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose(RoguelancerGame game) {
            _model = null;
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        public void Reset(RoguelancerGame game) {
        }
        #endregion
    }
    /// <summary>
    /// Bullet
    /// </summary>
    public class BulletObject : IBullet {
        #region "public properties"
        /// <summary>
        /// Bullet Model
        /// </summary>
        public BulletModel BulletModel { get; set; }
        #endregion
        #region "public methods"
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="texture"></param> 
        public BulletObject(ShipObject playerShipModel, RoguelancerGame game, Vector3 startupPosition, int deathSeconds = 3, int scale = 3, string modelPath = "bullet", float bulletThrust = .5f, ParticleSystemSettingsModel particleSystemSettings = null) {
            Reset(playerShipModel, game, startupPosition, deathSeconds, scale, modelPath, bulletThrust, particleSystemSettings);
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            BulletModel.Model.Initialize(game);
        }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            BulletModel.Model.LoadContent(game);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            Vector3 force, acceleration;
            var elapsed = (float)game.GameTime.ElapsedGameTime.TotalSeconds;
            var rotationAmount = new Vector2();
            if (BulletModel.PlayerShip == null) { BulletModel.PlayerShip = ShipHelper.GetPlayerShip(game); }
            BulletModel.Model.CurrentThrust = BulletModel.BulletThrust + BulletModel.PlayerShip.Model.CurrentThrust;
            BulletModel.Model.Rotation = rotationAmount;
            BulletModel.Model.UpdatePosition();
            force = BulletModel.Model.Direction * BulletModel.Model.CurrentThrust * BulletModel.ThrustForce;
            acceleration = force / BulletModel.Mass;
            BulletModel.Model.Velocity += acceleration * elapsed;
            BulletModel.Model.Velocity *= BulletModel.DragFactor;
            BulletModel.Model.Position += BulletModel.Model.Velocity * elapsed;
            if (BulletModel.LimitAltitude) {
                BulletModel.Model.Position.Y = Math.Max(BulletModel.Model.Position.Y, BulletModel.Model.MinimumAltitude);
            }
            BulletModel.Model.Update(game);
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            if (BulletModel.Model != null) {
                BulletModel.Model.Draw(game);
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose(RoguelancerGame game) {
            BulletModel.Model.Dispose(game);
            BulletModel = new BulletModel(game);
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        public void Reset(ShipObject playerShipModel, RoguelancerGame game, Vector3 startupPosition, int deathSeconds = 3, int scale = 3, string modelPath = "bullet", float bulletThrust = .5f, ParticleSystemSettingsModel particleSystemSettings = null) {
            BulletModel = new BulletModel(game);
            BulletModel.BulletThrust = bulletThrust;
            BulletModel.PlayerShip = playerShipModel;
            BulletModel.DeathDate = DateTime.Now.AddSeconds(deathSeconds);
            BulletModel.Model = new GameModel(game, particleSystemSettings, null);
            BulletModel.Model.UseScale = true;
            BulletModel.Model.Scale = scale;
            BulletModel.Model.WorldObject = new ModelWorldObjects(
                "bullet",
                "",
                BulletModel.PlayerShip.Model.Position + startupPosition,
                new Vector3(0f, 0f, 0f),
                new Settings.SettingsModelObject(
                    modelPath,
                    ModelType.Bullet,
                    true,
                    13
                ),
                1,
                BulletModel.PlayerShip.Model.Up,
                BulletModel.PlayerShip.Model.Right,
                BulletModel.PlayerShip.Model.Velocity,
                BulletModel.PlayerShip.Model.CurrentThrust,
                BulletModel.PlayerShip.Model.Direction,
                1.0f,
                0,
                0
            );
            Initialize(game);
            LoadContent(game);
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        public void Reset(RoguelancerGame game) {
        }
        #endregion
    }
}