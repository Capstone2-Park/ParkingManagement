using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class RegularSlot
    {
        public int RegSlotID { get; set; }
        public int? SessionID { get; set; }
        public int AvailableSlotM { get; set; }
        public int AvailableSlotV { get; set; }
        public int OccupiedSlotM { get; set; }
        public int OccupiedSlotV { get; set; }


        [ForeignKey("SessionID")]
        public virtual required RegularParkingSession Session { get; set; }
    }
}
