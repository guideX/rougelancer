﻿using Roguelancer;
using Roguelancer.Functionality;
using Roguelancer.Helpers;
using Roguelancer.Models;
using Microsoft.Xna.Framework;
using System;
using Roguelancer.Objects;
using Roguelancer.Interfaces;
using Roguelancer.Enum;
/// <summary>
/// Player Ship Extensions
/// </summary>
public static class PlayerShipExtensions {
    /// <summary>
    /// Get Mouse Rotation Amount
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    /// <param name="w"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static Vector2 GetMouseRotationAmount(this PlayerShipControl playerShipControl, RoguelancerGame game, PlayerShipControlModel model) {
        var w = game.Graphics.Model.GraphicsDeviceManager.PreferredBackBufferWidth / model.UpdateDirectionX;
        var y = game.Graphics.Model.GraphicsDeviceManager.PreferredBackBufferHeight / model.UpdateDirectionY;
        return new Vector2() {
            X = (game.Input.InputItems.Mouse.Vector.X - w) / -w,
            Y = (game.Input.InputItems.Mouse.Vector.Y - y) / -y
        };
    }
    /// <summary>
    /// Move Forward
    /// </summary>
    /// <param name="game"></param>
    public static void MoveForward(this IPlayerShipControl playerShipControl, RoguelancerGame game, GameModel model) {
        var playerShip = ShipHelper.GetPlayerShip(game.Objects.Model);
        game.Camera.Shake(playerShip.ShipModel.PlayerShipControl.Model.ShakeValue, 0f, false);
        if (model.CurrentThrust == PlayerShipControlModel.MaxThrustAmount) {
            model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
        } else if (model.CurrentThrust < PlayerShipControlModel.MaxThrustAmount) {
            model.CurrentThrust = model.CurrentThrust + PlayerShipControlModel.ThrustAddSpeed;
        } else {
            model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
        }
    }
    /// <summary>
    /// Stop Shaking
    /// </summary>
    public static void StopShaking(this PlayerShipControl playerShipControl, RoguelancerGame game) {
        game.Camera.Model.Shaking = false;
    }
    /// <summary>
    /// Use After Burn Thrust
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    /// <param name="model"></param>
    public static void UseAfterBurnThrust(this PlayerShipControl playerShipControl, RoguelancerGame game, GameModel model) {
        game.Camera.Shake(playerShipControl.Model.ShakeValue, 0f, false);
        if (model.CurrentThrust == PlayerShipControlModel.MaxThrustAfterburnerAmount) {
            model.CurrentThrust = PlayerShipControlModel.MaxThrustAfterburnerAmount;
        } else if (model.CurrentThrust < PlayerShipControlModel.MaxThrustAfterburnerAmount) {
            model.CurrentThrust = model.CurrentThrust + PlayerShipControlModel.ThrustAfterBurnerAddAmount;
        } else {
            model.CurrentThrust = PlayerShipControlModel.MaxThrustAfterburnerAmount;
        }
    }
    /// <summary>
    /// Get Input Rotation Amount
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static Vector2 GetInputRotationAmount(this PlayerShipControl playerShipControl, RoguelancerGame game, GameModel model) {
        var result = new Vector2();
        if (!playerShipControl.Model.UseAutoDock) {
            if (game.Input.InputItems.Mouse.LeftButton && game.Input.InputItems.Toggles.MouseMode && !game.Input.InputItems.Toggles.FreeMouseMode) {
                result = GetMouseRotationAmount(playerShipControl, game, playerShipControl.Model);
            } else if (!game.Input.InputItems.Toggles.MouseMode && game.Input.InputItems.Toggles.FreeMouseMode) {
                result = GetMouseRotationAmount(playerShipControl, game, playerShipControl.Model);
            }
            if (game.Input.InputItems.Keys.Left.IsKeyDown) result.X = playerShipControl.Model.RotationXLeftAdd; // Left
            if (game.Input.InputItems.Keys.Right.IsKeyDown) result.X = playerShipControl.Model.RotationXRightAdd; // Add Rotation Right
            if (game.Input.InputItems.Keys.Up.IsKeyDown) result.Y = playerShipControl.Model.RotationYUpAdd; // Up
            if (game.Input.InputItems.Keys.Down.IsKeyDown) result.Y = playerShipControl.Model.RotationYDownAdd; // Down
            result = result * playerShipControl.Model.RotationRate * (float)game.GameTime.ElapsedGameTime.TotalSeconds; // Slow Rotation Amount
            if (model.Up.Y < 0) result.X = -result.X;
        }
        return result;
    }
    /// <summary>
    /// Update Thrust
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    /// <param name="model"></param>
    public static void UpdateThrust(this PlayerShipControl playerShipControl, RoguelancerGame game, GameModel model) {
        if (!playerShipControl.Model.UseAutoDock) {
            if (playerShipControl.Model.UseInput) {
                if (game.Input.InputItems.Keys.W.IsKeyDown) {
                    playerShipControl.MoveForward(game, model);
                } else {
                    playerShipControl.StopShaking(game);
                }
                if (game.Input.InputItems.Toggles.Cruise) { // Cruising
                    if (game.Input.InputItems.Keys.S.IsKeyDown) {
                        model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
                        game.Input.InputItems.Toggles.Cruise = false;
                    } else {
                        model.CurrentThrust = PlayerShipControlModel.MaxCruiseSpeed;
                    }
                } else {
                    if (game.Input.InputItems.Keys.Tab.IsKeyDown) { // Tab
                        playerShipControl.UseAfterBurnThrust(game, model); // Use Afterburn Thrust
                    } else {
                        if (model.CurrentThrust > PlayerShipControlModel.MaxThrustAmount) {
                            model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                            game.Camera.Shake(playerShipControl.Model.ShakeValue, 0f, false);
                        }
                    }
                    if (game.Input.InputItems.Keys.X.IsKeyDown) {
                        game.Camera.Shake(1f, 0f, false);
                        if (model.CurrentThrust > PlayerShipControlModel.MaxThrustReverse) {
                            model.CurrentThrust = model.CurrentThrust + PlayerShipControlModel.ThrustReverseSpeed;
                        }
                    }
                    if (game.Input.InputItems.Keys.S.IsKeyDown) {
                        playerShipControl.StopShaking(game);
                        if (model.CurrentThrust == 0) {
                        } else if (model.CurrentThrust > PlayerShipControlModel.MaxThrustAmount || model.CurrentThrust > -.0001) {
                            model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                        }
                        if (model.CurrentThrust < PlayerShipControlModel.ThrustMinNotZero) {
                            model.CurrentThrust = 0;
                        }
                        if (model.CurrentThrust != 0) model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                    }
                }
            } else {
                if (game.Input.InputItems.Keys.L.IsKeyDown) {
                    if (model.CurrentThrust == PlayerShipControlModel.MaxThrustAfterburnerAmount) {
                        model.CurrentThrust = PlayerShipControlModel.MaxThrustAfterburnerAmount;
                    } else if (model.CurrentThrust < PlayerShipControlModel.MaxThrustAfterburnerAmount) {
                        model.CurrentThrust = model.CurrentThrust + PlayerShipControlModel.ThrustAfterBurnerAddAmount;
                    } else {
                        model.CurrentThrust = PlayerShipControlModel.MaxThrustAfterburnerAmount;
                    }
                } else {
                    if (model.CurrentThrust > PlayerShipControlModel.MaxThrustAmount) {
                        model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                    } else {
                        if (game.Input.InputItems.Keys.P.IsKeyDown) {
                            if (model.CurrentThrust == PlayerShipControlModel.MaxThrustAmount) {
                                model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
                            } else if (model.CurrentThrust < PlayerShipControlModel.MaxThrustAmount) {
                                model.CurrentThrust = model.CurrentThrust + PlayerShipControlModel.ThrustAddSpeed;
                            } else {
                                model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
                            }
                        }
                    }
                }
                if (game.Input.InputItems.Keys.K.IsKeyDown) {
                    if (model.CurrentThrust == 0) {
                    } else if (model.CurrentThrust > PlayerShipControlModel.MaxThrustAmount || model.CurrentThrust > -.0001) {
                        model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                    }
                    if (model.CurrentThrust < -.00001f) {
                        model.CurrentThrust = 0;
                    }
                    if (model.CurrentThrust == 0) {
                        model.CurrentThrust = 0;
                    } else {
                        model.CurrentThrust = model.CurrentThrust - PlayerShipControlModel.ThrustSlowDownSpeed;
                    }
                }
            }
        } else {
            var playerShip = ShipHelper.GetPlayerShip(game.Objects.Model);
            switch (playerShipControl.Model.AutoDockStep) {
                case 0:
                    playerShip.FaceObject(playerShip.ShipModel.GoingToObject.Model);
                    model.CurrentThrust = PlayerShipControlModel.MaxThrustAmount;
                    playerShipControl.Model.AutoDockStep = 1;
                    break;
                case 1:
                    if (playerShip.ShipModel.GoingToObject != null) {
                        var distance = (int)Vector3.Distance(playerShip.Model.Position, playerShip.ShipModel.GoingToObject.Model.Position) / (int)HudEnums.DivisionDistanceValue;
                        if (distance < (int)Roguelancer.Enum.HudEnums.DockDistanceAccept) {
                            playerShipControl.Model.UseAutoDock = false;
                            playerShipControl.Model.AutoDockStep = 0;
                            playerShip.ShipModel.GoingToObject.Dock(game, playerShip, playerShip.ShipModel.GoingToObject.Model);
                        }
                    }
                    break;
                case 2:

                    break;
            }
        }
    }
    /// <summary>
    /// Update Position and Velocity
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    /// <param name="model"></param>
    public static void UpdatePositionAndVelocity(this PlayerShipControl playerShipControl, RoguelancerGame game, GameModel model) {
        Vector3 force, acceleration;
        force = model.Direction * model.CurrentThrust * playerShipControl.Model.ThrustForce;
        acceleration = force / playerShipControl.Model.Mass;
        model.Velocity += acceleration * (float)game.GameTime.ElapsedGameTime.TotalSeconds;
        model.Velocity *= PlayerShipControlModel.DragFactor;
        model.Position += model.Velocity * (float)game.GameTime.ElapsedGameTime.TotalSeconds;
        //if (PlayerShipControlModel.LimitAltitude) {
            //model.Position.Y = Math.Max(model.Position.Y, model.MinimumAltitude);
        //}
    }
    /// <summary>
    /// Check Goto
    /// </summary>
    /// <param name="playerShipControl"></param>
    /// <param name="game"></param>
    public static void CheckGoto(this PlayerShipControl playerShipControl, RoguelancerGame game) {
        var playerShip = ShipHelper.GetPlayerShip(game.Objects.Model);
        if (playerShip.ShipModel.GoingTo && playerShip.ShipModel.GoingToObject != null) {
            var distance = (int)Vector3.Distance(playerShip.Model.Position, playerShip.ShipModel.GoingToObject.Model.Position) / (int)HudEnums.DivisionDistanceValue;
            if (distance < (int)HudEnums.DockDistanceAccept * 2) {
                game.InGameActions.FullStop();
                DebugTextHelper.SetText(game, "Goto Completed", true);
            }
        }
    }
}