﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.com
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelancer.Interfaces;
using Roguelancer.Models;
namespace Roguelancer.Functionality {
    /// <summary>
    /// Debug Text
    /// </summary>
    public class DebugText : IDebugText {
        #region "public variables"
        /// <summary>
        /// Debug Text Model
        /// </summary>
        public DebugTextModel Model { get; set; }
        #endregion
        #region "private variables"
        /// <summary>
        /// Get Text
        /// </summary>
        /// <returns></returns>
        //public string GetText() {
            //return Model.Text;
        //}
        /// <summary>
        /// Set Text
        /// </summary>
        /// <param name="game"></param>
        /// <param name="value"></param>
        public void SetText(RoguelancerGame game, string value, bool timerEnabled) {
            Model.TimerEnabled = timerEnabled;
            if (value != null) {
                Model.CurrentShowTime = 0;
                Model.ShowEnabled = true;
                Model.Text = value;
            }
        }
        #endregion
        #region "public functions"
        /// <summary>
        /// Debug Text
        /// </summary>
        public DebugText() {
            Model = new DebugTextModel();
            Model.Text = "";
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="game"></param>
        //public void Initialize(RoguelancerGame game) { }
        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="game"></param>
        public void LoadContent(RoguelancerGame game) {
            Model.Font = game.Content.Load<SpriteFont>("FONTS\\" + game.Settings.Font);
            Model.FontPosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.Graphics.Model.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2);
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="game"></param>
        public void Update(RoguelancerGame game) {
            if (Model.ShowEnabled) {
                if (Model.TimerEnabled) {
                    Model.CurrentShowTime++;
                    if (Model.CurrentShowTime > Model.ShowTime) {
                        Model.ShowEnabled = false;
                        Model.CurrentShowTime = 0;
                        Model.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="game"></param>
        public void Draw(RoguelancerGame game) {
            if (Model.ShowEnabled) {
                var fontOrigin = Model.Font.MeasureString(Model.Text) / 2;
                game.Graphics.Model.SpriteBatch.DrawString(Model.Font, Model.Text, Model.FontPosition, Color.White, 0, fontOrigin, 3.0f, SpriteEffects.None, 0.5f);
            }
        }
        #endregion
    }
}