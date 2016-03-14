﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.com
using System;
using Roguelancer.Objects;
namespace Roguelancer.Models {
    /// <summary>
    /// Bullet Model
    /// </summary>
    public class BulletModel {
        #region "public variables"
        /// <summary>
        /// Player Ship
        /// </summary>
        public Ship PlayerShip { get; set; }
        /// <summary>
        /// Mass
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// Thrust Force
        /// </summary>
        public float ThrustForce { get; set; }
        /// <summary>
        /// Drag Factor
        /// </summary>
        public float DragFactor { get; set; }
        /// <summary>
        /// Bullet Death Date
        /// </summary>
        public DateTime DeathDate { get; set; }
        /// <summary>
        /// Limit Altitude
        /// </summary>
        public bool LimitAltitude { get; set; }
        #endregion
        #region "public functions"
        /// <summary>
        /// Bullet Model
        /// </summary>
        public BulletModel(float mass = 1.0f, float thrustForce = 44000.0f, float dragFactor = 0.97f) {
            Mass = mass;
            ThrustForce = thrustForce;
            DragFactor = dragFactor;
        }
        #endregion
    }
}