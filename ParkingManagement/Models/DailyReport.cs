using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    internal class DailyReport
    {
        public int DailyReportID { get; set; }
        public DateTime ReportDate { get; set; }
        public string ParkingUsage { get; set; }
        public string AvailableSlots { get; set; }
        public decimal TotalRevenue { get; set; }
        public string PeakHoursJson { get; set; } // JSON serialized dictionary: hour -> count

    }
}
