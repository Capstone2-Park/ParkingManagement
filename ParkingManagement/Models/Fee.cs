using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Model
{
  
    public class Fee
    {
        public int FeeID { get; set; }
        public string VehicleType { get; set; }
        public decimal FeePerHour { get; set; }
    }
}
