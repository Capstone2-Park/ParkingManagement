using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    internal class RegularParkingSession
    {
        public int SessionID { get; set; }                // Primary Key, auto-incremented
        public string RegularVehicleID { get; set; }      // Auto-generated ID like "RP000001" for display
        public string PlateNumber { get; set; }
        public string VehicleType { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }            // Nullable for when vehicle hasn't left
        public decimal? TotalAmount { get; set; }         // Nullable for when amount isn't calculated yet
        public string QRCodeData { get; set; }            // Data that was encoded into the QR (e.g., SessionID)
    }
}
