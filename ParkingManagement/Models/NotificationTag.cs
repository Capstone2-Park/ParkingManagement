using ParkingManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Models
{
    internal class NotificationTag
    {
        public VehicleSession Session { get; set; }
        public Client Client { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
