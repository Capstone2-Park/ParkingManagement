using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class CurrentUser
    {
        public int CurrentUserID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime TimeLoggedin { get; set; } = DateTime.Now;
        public DateTime TimeLoggedout { get; set; }
    }
}
