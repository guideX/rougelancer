﻿using Roguelancer.Interfaces;
using Roguelancer.Models;
namespace Roguelancer.Bloom {
    /// <summary>
    /// Bloom Handler
    /// </summary>
    public class BloomHandler : IBloomHandler {
        #region "public properties"
        /// <summary>
        /// Bloom Handler Model
        /// </summary>
        public BloomHandlerModel Model { get; set; }
        #endregion
        #region "public methods"
        /// <summary>
        /// Bloom Handler
        /// </summary>
        /// <param name="game"></param>
        public BloomHandler(RoguelancerGame game) {
            if (game.Settings.Model.BloomEnabled) {
                Model = new BloomHandlerModel() {
                    Bloom = new BloomComponent(game)
                };
            }
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        public void Initialize(RoguelancerGame game) {
            if (game.Settings.Model.BloomEnabled) {
                game.Components.Add(Model.Bloom);
            }
        }
        /// <summary>
        /// Load Content
        /// </summary>
        public void LoadContent(RoguelancerGame game) {
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="_BloomVisible"></param>
        public void Update(RoguelancerGame game) {
            if (game.Settings.Model.BloomEnabled) {
                Model.Bloom.Model.Settings = BloomSettingsModel.PresetSettings[1];
                Model.Bloom.Visible = true;
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        public void Draw(RoguelancerGame game) {
            if (game.Settings.Model.BloomEnabled) {
                Model.Bloom.BeginDraw();
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="game"></param>
        public void Dispose(RoguelancerGame game) {
            //throw new System.NotImplementedException();
        }
        /// <summary>
        /// Reset
        /// </summary>
        /// <param name=""></param>
        public void Reset(RoguelancerGame game) {
            //throw new System.NotImplementedException();
        }
        #endregion
    }
}