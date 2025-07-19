using ParkingManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    internal class RegularParkingSlot
    {
        [Key]
        public int SlotID { get; set; }

        [StringLength(50)]
        public string VehicleStatus { get; set; }


        public string RegularVehicleID { get; set; }

        public string SlotNumber { get; set; }

        public string SlotStatus { get; set; }

        // Navigation properties
        [ForeignKey("RegularVehicleID")]
        public virtual RegularParkingSession RegularVehicle { get; set; }
    }
}
