using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class MonthlyFinanceSum
    {
        public int MonthlyFinanceID { get; set; }  
        public DateTime MonthStart { get; set; }     
        public DateTime MonthEnd { get; set; }
        public int TotalTransaction { get; set; }
        public int TotalRentTransaction { get; set; }   
        public int TotalRegTransaction { get; set; }
        public decimal TotalRentRevenue { get; set; }
        public decimal TotalRegRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
