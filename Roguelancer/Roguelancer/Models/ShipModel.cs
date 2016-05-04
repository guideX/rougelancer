﻿using Roguelancer.Interfaces;

namespace Roguelancer.Models {
    /// <summary>
    /// Ship Model
    /// </summary>
    public class ShipModel {
        /// <summary>
        /// Money
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// Cargo Hold
        /// </summary>
        public CargoHoldModel CargoHold { get; set; }
        /// <summary>
        /// Hard Points
        /// </summary>
        //public List<HardPoint> HardPoints { get; set; }
        /// <summary>
        /// Player Ship Control
        /// </summary>
        public IPlayerShipControl PlayerShipControl;
    }
}
