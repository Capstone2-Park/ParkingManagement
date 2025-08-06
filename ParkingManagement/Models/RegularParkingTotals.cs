using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class RegularParkingTotals
    {
        public int TotalID { get; set; }
        public string PlateNumber { get; set; } // Added this property
        public string VehicleType { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal? TotalAmount { get; set; }
        public string SlotNumber { get; set; } // Add this property to fix the error  
        public string QRCodeImage { get; set; }
        public int SessionID { get; set; }
        public int SlotID { get; set; } // Foreign key for Parkingslot

        [ForeignKey("SessionID")]
        public virtual RegularParkingSession Session { get; set; }
        [ForeignKey("SlotID")]
        public virtual Parkingslot Slot { get; set; }
    }
}
