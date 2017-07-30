﻿using System.Linq;
using System.Collections.Generic;
using Roguelancer.Functionality;
using Roguelancer.Interfaces;
using Roguelancer.Models;
using Microsoft.Xna.Framework;
namespace Roguelancer.Settings {
    /// <summary>
    /// Game Settings
    /// </summary>
    public class GameSettings : IGameSettings {
        #region "public properties"
        /// <summary>
        /// Model
        /// </summary>
        public GameSettingsModel Model { get; set; }
        #endregion
        #region "public methods"
        public GameSettings(RoguelancerGame game) {
            var b = false;
            Model = new GameSettingsModel(game);
            Model.Font = NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "Font", "LucidaFont"); // Font
            Model.FontSmall = NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "FontSmall", "LucidiaFontSmall"); // Small Font
            Model.SensorTexture = NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "SensorTexture"); // Sensor Texture
            if (bool.TryParse(NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "BloomEnabled"), out b)) { Model.BloomEnabled = b; } // Bloom Enabled
            if (bool.TryParse(NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "FullScreen"), out b)) { Model.FullScreen = b; } // Full Screen
            Model.BulletMass = NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "Bullet", "Mass", 1.0f); // Bullet
            Model.BulletThrusterForce = NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "Bullet", "ThrusterForce", 44000.0f); // Thruster Force
            Model.BulletDragFactor = NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "Bullet", "DragFactor", 0.97f); // Drag Factor
            Model.BulletRechargeRate = NativeMethods.ReadINIInt(Model.GameSettingsIniFile, "Bullet", "RechargeRate", 240); // Recharge Rate
            Model.PlayerShipShakeValue = NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "PlayerShip", "ShakeValue", .8f); // Shake Value
            Model.MenuBackgroundTexture = NativeMethods.ReadINI(Model.GameSettingsIniFile, "Settings", "menu_background"); // Menu Background
            Model.Resolution = new Vector2(NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "Graphics", "ResolutionX", 1280f), NativeMethods.ReadINIFloat(Model.GameSettingsIniFile, "Graphics", "ResolutionY", 1024)); // Resolution
            Model.CameraSettings = new CameraSettings(Model.CameraSettingsIniFile); // Camera Settings
            Model.ModelSettings = new List<SettingsModelObject>(); // Model Settings
            for (var i = 1; i < NativeMethods.ReadINIInt(Model.ModelSettingsIniFile, "settings", "count", 0) + 1; ++i) {
                Model.ModelSettings.Add(new SettingsModelObject(
                    NativeMethods.ReadINI(Model.ModelSettingsIniFile, i.ToString().Trim(), "path"),
                    (Enum.ModelType)NativeMethods.ReadINIInt(Model.ModelSettingsIniFile, i.ToString().Trim(), "type", 0),
                    NativeMethods.ReadINIBool(Model.ModelSettingsIniFile, i.ToString().Trim(), "enabled", false),
                    i
                ));
            }
            for (var i = 1; i < NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, "settings", "count", 0) + 1; ++i) {
                Model.StarSystemSettings.Add(new StarSystemSettings(
                    i,
                    NativeMethods.ReadINI(Model.SystemsSettingsIniFile, i.ToString().Trim(), "path", ""),
                    Model.SystemIniStartPath,
                    Model.ModelSettings,
                    ModelWorldObjects.Read(i, Model.ModelSettings, Model.PlayerIniFile, "settings"),
                    new StarSettings(
                        NativeMethods.ReadINIBool(Model.SystemsSettingsIniFile, i.ToString(), "starsEnabled", false),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "amountOfStarsPerSheet", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "maxPositionX", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "maxPositionY", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "maxSize", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "maxPositionIncrementY", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "maxPositionStartingY", 0),
                        NativeMethods.ReadINIInt(Model.SystemsSettingsIniFile, i.ToString(), "numberOfStarSheets", 0)
                    )
                ));
            }
            if (System.IO.File.Exists(Model.CommoditiesIniFile)) {
                for (var i = 1; i < NativeMethods.ReadINIInt(Model.CommoditiesIniFile, "Settings", "Count", 0) + 1; ++i) {
                    Model.CommoditiesModels.Add(new CommodityModel() {
                        CommodityId = i,
                        Description = NativeMethods.ReadINI(Model.CommoditiesIniFile, i.ToString(), "Description", ""),
                        Body = NativeMethods.ReadINI(Model.CommoditiesIniFile, i.ToString(), "Body", ""),
                        Prices = Model.StationPriceModels.Where(p => p.CommoditiesId == i).ToList(), 
                        ImagePath = NativeMethods.ReadINI(Model.CommoditiesIniFile, i.ToString(), "ImagePath", ""),
                        ImagePathContainer = NativeMethods.ReadINI(Model.CommoditiesIniFile, i.ToString(), "ImagePathContainer", ""),
                    });
                }
            }
            if (System.IO.File.Exists(Model.CommoditiesSettingsIniFile)) {
                for (var i = 1; i < NativeMethods.ReadINIInt(Model.CommoditiesSettingsIniFile, "Settings", "Count", 0) + 1; ++i) {
                    var n = NativeMethods.ReadINIInt(Model.CommoditiesSettingsIniFile, i.ToString(), "commodities_index", 0);
                    var _commodity = Model.CommoditiesModels.Where(com => com.CommodityId == n);
                    if (_commodity.Any()) {
                        var commodity = _commodity.FirstOrDefault();
                        Model.StationPriceModels.Add(new StationPriceModel() {
                            IsSelling = NativeMethods.ReadINIBool(Model.CommoditiesSettingsIniFile, i.ToString(), "IsSelling", false),
                            Price = NativeMethods.ReadINIDecimal(Model.CommoditiesSettingsIniFile, i.ToString(), "Price", decimal.Zero),
                            Selling = NativeMethods.ReadINIDecimal(Model.CommoditiesSettingsIniFile, i.ToString(), "Selling", decimal.Zero),
                            StarSystemId = NativeMethods.ReadINIInt(Model.CommoditiesSettingsIniFile, i.ToString(), "system_index", 0),
                            StationId = NativeMethods.ReadINIInt(Model.CommoditiesSettingsIniFile, i.ToString(), "station_index", 0),
                            CommoditiesId = n,
                            Qty = NativeMethods.ReadINIInt(Model.CommoditiesSettingsIniFile, i.ToString(), "qty", 0),
                            StationPriceID = i,
                            ImagePath = commodity.ImagePath,
                            ImagePathContainer = commodity.ImagePathContainer
                        });
                    }
                }
            }
        }
        #endregion
    }
}