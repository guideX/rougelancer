﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.com
using System.Linq;
using System.Collections.Generic;
using Roguelancer.Functionality;
using Roguelancer.Interfaces;
using Roguelancer.Settings;
using Roguelancer.Models;
using Roguelancer.Enum;
namespace Roguelancer.Objects {
    /// <summary>
    /// Ship Collection
    /// </summary>
    public class ShipCollection : IShipCollection {
        /// <summary>
        /// Ships
        /// </summary>
        public List<Ship> Ships { get; set; }
        #region "public functions"
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="game"></param>
        public ShipCollection(RoguelancerGame game) {
            Reset(game);
        }
        /// <summary>
        /// Get Player Ship
        /// </summary>
        /// <returns></returns>
        public Ship GetPlayerShip(RoguelancerGame game) {
            return game.Objects.Model.Ships.Ships.Where(s => s.ShipModel.PlayerShipControl.Model.UseInput).LastOrDefault();
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            foreach (var _ship in Ships) {
                _ship.Initialize(game);
            }
        }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            foreach (var _ship in Ships) {
                _ship.LoadContent(game);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            foreach (var _ship in Ships) {
                _ship.Update(game);
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            foreach (var _ship in Ships) {
                _ship.Draw(game);
            }
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="game"></param>
        public void Reset(RoguelancerGame game) {
            Ships = new List<Ship>();
            var playerShip = new Ship(game);
            Ship tempShip;
            playerShip.Model.WorldObject = game.Settings.StarSystemSettings[game.StarSystemId].Ships.Where(s => s.SettingsModelObject.modelType == ModelType.Ship && s.SettingsModelObject.isPlayer).FirstOrDefault();
            playerShip.ShipModel.PlayerShipControl.Model.UseInput = true;
            Ships.Add(playerShip);
            foreach (var modelWorldObject in game.Settings.StarSystemSettings[game.StarSystemId].Ships.Where(s => !s.SettingsModelObject.isPlayer).ToList()) {
                tempShip = new Ship(game);
                tempShip.Model = new GameModel(game, null);
                tempShip.Model.WorldObject = ModelWorldObjects.Clone(modelWorldObject);
                tempShip.ShipModel.PlayerShipControl.Model.UseInput = false;
                Ships.Add(Ship.Clone(tempShip, game));
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
        }
        #endregion
    }
    /// <summary>
    /// Ship
    /// </summary>
    public class Ship : IGame, ISensorObject, IDockableShip {
        #region "public variables"
        /// <summary>
        /// Docked
        /// </summary>
        public bool Docked { get; set; }
        /// <summary>
        /// Ship Model
        /// </summary>
        public ShipModel ShipModel { get; set; }
        /// <summary>
        /// Game Model
        /// </summary>
        public GameModel Model { get; set; }
        #endregion
        #region "public functions"
        /// <summary>
        /// Ship
        /// </summary>
        /// <param name="game"></param>
        public Ship(RoguelancerGame game) {
            ShipModel = new ShipModel();
            ShipModel.Money = 2000.00m;
            ShipModel.CargoHold = new CargoHoldModel();
            Model = new GameModel(game, null);
            ShipModel.PlayerShipControl = new PlayerShipControl();
            //HardPoints = new List<HardPoint>();
        }
        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="oldShip"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static Ship Clone(Ship oldShip, RoguelancerGame game) {
            Ship ship;
            ship = new Ship(game);
            ship.ShipModel.PlayerShipControl = oldShip.ShipModel.PlayerShipControl;
            ship.Model = oldShip.Model;
            ship.ShipModel.CargoHold = oldShip.ShipModel.CargoHold;
            return ship;
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            Model.Initialize(game);
            if (ShipModel.PlayerShipControl.Model.UseInput) {
                ShipModel.PlayerShipControl = new PlayerShipControl();
            }
        }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            Model.LoadContent(game);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            if (game.GameState.Model.CurrentGameState == GameStates.Playing) {
                if (ShipModel.PlayerShipControl.Model.UseInput) {
                    ShipModel.PlayerShipControl.UpdateModel(Model, game);
                    if (!game.Input.InputItems.Toggles.ToggleCamera) {
                        Model.Update(game);
                    }
                } else {
                    Model.Update(game);
                }
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            if (!Docked) {
                Model.Draw(game);
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
        }
        #endregion
    }
}