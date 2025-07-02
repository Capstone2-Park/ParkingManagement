using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Model
{

    public class Vehicle
    {
        public string VehicleID { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public string ClientID { get; set; }
        public string VehicleType { get; set; }
        public Client Owner { get; set; }
    }
}
