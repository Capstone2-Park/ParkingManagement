using System;

namespace ParkingManagement.Models
{
    public class SlotHistory
    {
        public int SlotHistoryID { get; set; }
        public DateTime Date { get; set; }

        // Snapshots for the day
        public int AvailableSlotM { get; set; } // Motorcycle slots
        public int AvailableSlotV { get; set; } // Vehicle slots
        public int AvailableParkingSlots { get; set; } // Parkingslot with SlotStatus == "Available"
    }
}