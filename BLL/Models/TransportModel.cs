using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class TransportModel
    {
        public int ID { get; set; }
        public int? NumberOfSeats { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public bool? Hidden { get; set; }

        public TransportModel()
        {

        }
        public TransportModel(Transport DALinfo)
        {
            this.ID = DALinfo.ID;
            this.NumberOfSeats = DALinfo.NumberOfSeats;
            this.RegistrationNumber = DALinfo.RegistrationNumber;
            this.Model = DALinfo.Model;
            this.Hidden = DALinfo.Hidden;
        }
    }
}