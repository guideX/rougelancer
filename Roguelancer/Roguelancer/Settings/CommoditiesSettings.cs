﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.org
using System.Linq;
using Roguelancer.Functionality;
using Roguelancer.Models;
using System;
using System.Collections.Generic;
namespace Roguelancer.Settings {
    /// <summary>
    /// Commodities Settings
    /// </summary>
    public class CommoditiesSettings {
        /// <summary>
        /// Commodities Models
        /// </summary>
        public List<CommodityModel> CommoditiesModels { get; set; }
        /// <summary>
        /// Station Price models
        /// </summary>
        public List<StationPriceModel> StationPriceModels { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public CommoditiesSettings(string iniFileCommodities, string iniFilePrices) {
            try {
                StationPriceModels = new List<StationPriceModel>();
                CommoditiesModels = new List<CommodityModel>();
                //if (System.IO.File.Exists(iniFileCommodities)) {
                    //var n = Convert.ToInt32(IniFile.ReadINI(iniFileCommodities, "Settings", "Count", "0"));
                    //for (var i = 1; i <= n - 1; ++i) {

                    //}
                //}
                if (System.IO.File.Exists(iniFilePrices)) {
                    var n = IniFile.ReadINIInt(iniFilePrices, "Settings", "Count", 0);
                    for (var i = 1; i <= n - 1; ++i) {
                        StationPriceModels.Add(new StationPriceModel() {
                            IsSelling = Convert.ToBoolean(IniFile.ReadINI(iniFilePrices, i.ToString(), "IsSelling", "false")),
                            Price = Convert.ToDecimal(IniFile.ReadINI(iniFilePrices, i.ToString(), "Price", "0.00")),
                            Selling = Convert.ToDecimal(IniFile.ReadINI(iniFilePrices, i.ToString(), "Selling", "0.00")),
                            StarSystemId = IniFile.ReadINIInt(iniFilePrices, i.ToString(), "system_index", 0),
                            StationId = IniFile.ReadINIInt(iniFilePrices, i.ToString(), "station_index", 0),
                            CommoditiesId = IniFile.ReadINIInt(iniFilePrices, i.ToString(), "commodities_index", 0),
                            StationPriceId = i
                        });
                    }
                }
                if (System.IO.File.Exists(iniFileCommodities)) {
                    var n = IniFile.ReadINIInt(iniFileCommodities, "Settings", "Count", 0);
                    for (var i = 1; i <= n - 1; ++i) {
                        CommoditiesModels.Add(new CommodityModel() {
                            CommodityId = i,
                            Description = IniFile.ReadINI(iniFileCommodities, i.ToString(), "Description", ""),
                            Body = IniFile.ReadINI(iniFileCommodities, i.ToString(), "Body", "")
                        });
                        //CommoditiesModels[i].Prices = StationPriceModels.Where(p => p.CommoditiesId == i).ToList();
                    }
                }
            } catch {
                throw;
            }
        }
    }
}