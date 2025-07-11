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
    public class Parkingslot
    {
        [Key]
        public int SlotID { get; set; }



      
        [StringLength(50)]
        public string VehicleStatus { get; set; }

       
        public string VehicleID { get; set; }

       
        public string ClientID { get; set; }

        public string SlotNumber { get; set; }

        public string SlotStatus { get; set; }

        // Navigation properties
        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }

        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }

        
    }
}
