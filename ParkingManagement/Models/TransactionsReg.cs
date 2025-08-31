using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class TransactionsReg
    {
        public int TransactionID { get; set; }   // Primary Key
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Default GETDATE()

        public decimal CashInHand { get; set; }      // Amount given by client
        public decimal Change { get; set; }          // Change returned

        // Foreign Key
        public int? TotalID { get; set; }      // Nullable because FK may not always be set

        // Navigation property (if using ORM like EF Core)
        public RegularParkingTotals RegularParkingTotals { get; set; }
    }
}
