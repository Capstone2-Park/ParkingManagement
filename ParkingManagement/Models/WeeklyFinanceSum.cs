using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class WeeklyFinanceSum
    {
        public int WeeklyFinanceID { get; set; }          // Primary Key
        public DateTime WeekStart { get; set; }      // DATE
        public DateTime WeekEnd { get; set; }
        public int TotalTransaction { get; set; }
        public int TotalRentTransaction { get; set; }    // INT - total daily rent revenue
        public int TotalRegTransaction { get; set; }
        public decimal TotalRentRevenue { get; set; }
        public decimal TotalRegRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
