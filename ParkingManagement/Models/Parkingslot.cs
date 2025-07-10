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

        [Required]
        [StringLength(50)]
        public string VehicleType { get; set; }

        [Required]
        [StringLength(50)]
        public string VehicleStatus { get; set; }

        [Required]
        public string VehicleID { get; set; }

        [Required]
        public string ClientID { get; set; }

        [Required]
        public string SlotNumber { get; set; }

        [Required]
        public string SlotStatus { get; set; }

        // Navigation properties
        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }

        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }

        
    }
}
