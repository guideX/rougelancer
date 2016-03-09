﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.org
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Roguelancer.Functionality;
namespace Roguelancer.Settings {
    /// <summary>
    /// Model World Objects
    /// </summary>
    public class ModelWorldObjects {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Startup Position
        /// </summary>
        public Vector3 StartupPosition { get; set; }
        /// <summary>
        /// Scaling
        /// </summary>
        public float Scaling { get; set; }
        /// <summary>
        /// Startup Model Rotation
        /// </summary>
        public Vector3 StartupRotation { get; set; }
        /// <summary>
        /// Settings Model Object
        /// </summary>
        public SettingsModelObject SettingsModelObject { get; set; }
        /// <summary>
        /// Star System ID
        /// </summary>
        public int StarSystemId { get; set; }
        /// <summary>
        /// Default Startup Position
        /// </summary>
        public Vector3 DefaultStartupPosition { get; set; }
        /// <summary>
        /// Default Model Rotation
        /// </summary>
        public Vector3 DefaultModelRotation { get; set; }
        /// <summary>
        /// Initial Model Up
        /// </summary>
        public Vector3 InitialModelUp { get; set; }
        /// <summary>
        /// Initial Model Right
        /// </summary>
        public Vector3 InitialModelRight { get; set; }
        /// <summary>
        /// Initial Velocity
        /// </summary>
        public Vector3 InitialVelocity { get; set; }
        /// <summary>
        /// Initial Current Thrust
        /// </summary>
        public float InitialCurrentThrust { get; set; }
        /// <summary>
        /// Initial Direction
        /// </summary>
        public Vector3 InitialDirection { get; set; }
        /// <summary>
        /// Cargo Space
        /// </summary>
        public int CargoSpace { get; set; }
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="startupPosition"></param>
        /// <param name="startupModelRotation"></param>
        /// <param name="settingsModelObject"></param>
        /// <param name="starSystemId"></param>
        /// <param name="initialModelUp"></param>
        /// <param name="initialModelRight"></param>
        /// <param name="initialVelocity"></param>
        /// <param name="initialCurrentThrust"></param>
        /// <param name="initialDirection"></param>
        public ModelWorldObjects(
                string description,
                Vector3 startupPosition,
                Vector3 startupModelRotation,
                SettingsModelObject settingsModelObject,
                int starSystemId,
                Vector3 initialModelUp,
                Vector3 initialModelRight,
                Vector3 initialVelocity,
                float initialCurrentThrust,
                Vector3 initialDirection,
                float scaling,
                int cargoSpace,
                int id
            ) {
            Description = description;
            StartupPosition = startupPosition;
            StartupRotation = startupModelRotation;
            SettingsModelObject = settingsModelObject;
            StarSystemId = starSystemId;
            InitialModelUp = initialModelUp;
            InitialModelRight = initialModelRight;
            InitialVelocity = initialVelocity;
            InitialCurrentThrust = initialCurrentThrust;
            InitialDirection = initialDirection;
            Scaling = scaling;
            CargoSpace = cargoSpace;
            ID = id;
        }
        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="oldObject"></param>
        /// <returns></returns>
        public static ModelWorldObjects Clone(ModelWorldObjects oldObject) {
            return new ModelWorldObjects(
                    oldObject.Description,
                    oldObject.StartupPosition,
                    oldObject.StartupRotation,
                    SettingsModelObject.Clone(oldObject.SettingsModelObject),
                    oldObject.StarSystemId,
                    oldObject.InitialModelUp,
                    oldObject.InitialModelRight,
                    oldObject.InitialVelocity,
                    oldObject.InitialCurrentThrust,
                    oldObject.InitialDirection,
                    oldObject.Scaling,
                    oldObject.CargoSpace,
                    oldObject.ID
                );
        }
        /// <summary>
        /// Read
        /// </summary>
        /// <param name="modelSettings"></param>
        /// <param name="iniFile"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static ModelWorldObjects Read(int ID, List<SettingsModelObject> modelSettings, string iniFile, string section) {
            var description = IniFile.ReadINI(iniFile, section, "description", "");
            var startupPosition = IniFile.ReadINIVector3(iniFile, section, "startup_position_x", "startup_position_y", "startup_position_z");
            var startupModelRotation = IniFile.ReadINIVector3(iniFile, section, "startup_model_rotation_x", "startup_model_rotation_y", "startup_model_rotation_z");
            var settingsModelObject = SettingsModelObject.Clone(modelSettings.Where(s => s.enabled == true && s.modelId == Convert.ToInt32(IniFile.ReadINI(iniFile, section, "model_index", "0"))).FirstOrDefault());
            var starSystemID = IniFile.ReadINIInt(iniFile, section, "system_index", 0);
            var initialModelUp = IniFile.ReadINIVector3(iniFile, section, "initial_model_up_x", "initial_model_up_y", "initial_model_up_z");
            var initialModelRight = IniFile.ReadINIVector3(iniFile, section, "initial_model_right_x", "initial_model_right_y", "initial_model_right_z");
            var initialVelocity = IniFile.ReadINIVector3(iniFile, section, "initial_velocity_x", "initial_velocity_y", "initial_velocity_z");
            var initialCurrentThrust = float.Parse(IniFile.ReadINI(iniFile, section, "initial_current_thrust", "0"));
            var initialDirection = IniFile.ReadINIVector3(iniFile, section, "initial_direction_x", "initial_direction_y", "initial_direction_z");
            var scaling = IniFile.ReadINIFloat(iniFile, section, "model_scaling");
            var cargoSpace = IniFile.ReadINIInt(iniFile, section, "cargo_space", 0);
            return new ModelWorldObjects(
                description,
                startupPosition,
                startupModelRotation,
                settingsModelObject,
                starSystemID,
                initialModelUp,
                initialModelRight,
                initialVelocity,
                initialCurrentThrust,
                initialDirection,
                scaling,
                cargoSpace,
                ID
            );
        }
    }
}