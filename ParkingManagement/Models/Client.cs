using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Model
{
   
    public class Client
    {
        public string ClientID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CpNumber { get; set; }
        public string IDPicture { get; set; } // Stores the file path
        public ICollection<Vehicle> VehicleList { get; set; } = new List<Vehicle>();
    }
}
