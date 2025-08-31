using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class RegularParkingSession
    {
        public int SessionID { get; set; }
      
        public string RegularVehicleID { get; set; }
        public string VehicleType { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Discount { get; set; }
    }
}


