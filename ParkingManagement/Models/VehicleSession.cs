using ParkingManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class VehicleSession
    {
        public int SessionID { get; set; } // INT PRIMARY KEY IDENTITY
        public int? SlotID { get; set; } // INT FOREIGN KEY REFERENCES ParkingSlot(SlotID)
        public string VehicleID { get; set; }
        public string DurationType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal TotalAmount { get; set; }
        public Vehicle Vehicle { get; set; }
    }

}
