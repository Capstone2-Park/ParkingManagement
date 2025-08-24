using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class DailyFinanceSum
    {
        public int DailyFinanceID { get; set; }          // Primary Key
        public DateTime ReportDateSum { get; set; }      // DATE
        public int TotalTransaction { get; set; }    
        public int TotalRentTransaction { get; set; }    // INT - total daily rent revenue
        public int TotalRegTransaction { get; set; }
        public decimal TotalRentRevenue { get; set; }    
        public decimal TotalRegRevenue { get; set; }      
        public decimal TotalRevenue { get; set; }
    }
}
