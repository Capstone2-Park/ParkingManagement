using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string UserRole { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
