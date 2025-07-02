using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    internal class VehicleSession
    {
        public int SessionID { get; set; } // <--- CHANGE THIS FROM string TO int
        public string VehicleID { get; set; }
        public string DurationType { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
