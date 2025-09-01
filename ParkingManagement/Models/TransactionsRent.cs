using System;
using System.ComponentModel.DataAnnotations.Schema; // Add this using

namespace ParkingManagement.Models
{
    public class TransactionsRent
    {
        public int TransactionID { get; set; }   // Primary Key
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Default GETDATE()

        public decimal CashInHand { get; set; }      // Amount given by client
        public decimal Change { get; set; }          // Change returned

        // Foreign Key
        public int? SessionID { get; set; }      // Nullable because FK may not always be set

        [ForeignKey("SessionID")]
        public VehicleSession VehicleSession { get; set; }
    }
}
