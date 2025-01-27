﻿using Roguelancer.Collections;
using Roguelancer.Enum;
using Roguelancer.Interfaces;
using Roguelancer.Models;
namespace Roguelancer.Objects {
    /// <summary>
    /// Game Objects
    /// </summary>
    public class GameObjects : IGameObjects {
        #region "public properties"
        /// <summary>
        /// Game Objects
        /// </summary>
        public GameObjectsModel Model { get; set; }
        #endregion
        #region "public methods"
        /// <summary>
        /// Get Object Count
        /// </summary>
        /// <returns></returns>
        public int GetObjectCount() {
            return
                (
                    Model.DockingRings.Model.DockingRings.Count +
                    Model.Ships.Model.Ships.Count +
                    Model.Stations.Model.Stations.Count +
                    Model.Planets.Model.Planets.Count +
                    Model.JumpHoles.Model.JumpHoles.Count
                ) - 1;
            }
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="game"></param>
        public GameObjects(RoguelancerGame game) {
            Model = new GameObjectsModel() {
                Stations = new StationCollection(game),
                Planets = new PlanetCollection(game),
                Ships = new ShipCollection(game),
                Bullets = new BulletCollection(),
                JumpHoles = new JumpHoleCollection(),
                DockingRings = new DockingRingCollection(game),
            };
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            //Model.StarField = new Starfield(game.Camera.Model.Position.ToVector2(), game.GraphicsDevice, game.Content);
            //Model.Stars = new Particle.Starfields(game.Settings.Model.StarSystemSettings[game.CurrentStarSystemId].Model.StarSettings);
            Model.Stations.Initialize(game);
            Model.Planets.Initialize(game);
            //Model.Stars.Initialize(game);
            //Model.TradeLanes.Initialize(game);
            Model.Ships.Initialize(game);
            Model.Bullets.Initialize(game);
            Model.DockingRings.Initialize(game);
            Model.JumpHoles.Initialize(game);
            var playerShip = Helpers.ShipHelper.GetPlayerShip(game.Objects.Model);
        }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            Model.Stations.LoadContent(game);
            Model.Planets.LoadContent(game);
            //Model.StarField.LoadContent();
            //Model.Stars.LoadContent(game);
            //Model.TradeLanes.LoadContent(game);
            Model.Ships.LoadContent(game);
            Model.Bullets.LoadContent(game);
            Model.DockingRings.LoadContent(game);
            Model.JumpHoles.LoadContent(game);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            if (game.GameState.Model.CurrentGameState == GameStatesEnum.Playing) {
                Model.Stations.Update(game);
                Model.Planets.Update(game);
                //Model.Stars.Update(game);
                //Model.TradeLanes.Update(game);
                Model.Ships.Update(game);
                Model.Bullets.Update(game);
                Model.DockingRings.Update(game);
                Model.JumpHoles.Update(game);
            } else {
                Model.Stations.Update(game);
                Model.Planets.Update(game);
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            if (game.GameState.Model.CurrentGameState == GameStatesEnum.Playing) {
                Model.Stations.Draw(game);
                Model.Planets.Draw(game);
                var playerShip = Helpers.ShipHelper.GetPlayerShip(game.Objects.Model);
                //Model.Stars.Draw(game);
                //Model.StarField.Draw(game.Camera.Model.Position.ToVector2());
                //Model.TradeLanes.Draw(game);
                Model.Ships.Draw(game);
                Model.Bullets.Draw(game);
                Model.DockingRings.Draw(game);
                Model.JumpHoles.Draw(game);
            } else {
                Model.Stations.Draw(game);
                Model.Planets.Draw(game);
            }
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        public void Reset(RoguelancerGame game) {
            //Model.TradeLanes.Reset(game);
            //Model.Ships.Reset();
            Model.Stations.Reset(game);
            Model.Planets.Reset(game);
            var playerShip = Helpers.ShipHelper.GetPlayerShip(game.Objects.Model);
            Model.DockingRings.Reset(game);
            //Model.Stars.Reset(game);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose(RoguelancerGame game) {
            //Model.TradeLanes.Dispose(game);
            Model.Stations.Dispose(game);
            Model.Planets.Dispose(game);
            Model.DockingRings.Dispose(game);
            Model.JumpHoles.Dispose(game);
        }
        #endregion
    }
}