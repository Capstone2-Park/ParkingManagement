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
        public int RegSlotID { get; set; }
        public string VehicleType { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal? TotalAmount { get; set; }
        public string QRCodeImage { get; set; }
        public string Discount { get; set; }
        public int SessionID { get; set; }

        [ForeignKey("SessionID")]
        public virtual RegularParkingSession Session { get; set; }

        [ForeignKey("RegSlotID")]
        public virtual RegularSlot RegularSlot { get; set; }
    }
}
