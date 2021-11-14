using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class DriverModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int? Experience { get; set; }
        public int? Salary { get; set; }
        public bool? Hidden { get; set; }

        public DriverModel()
        {

        }
        public DriverModel(Driver DALinfo)
        {
            this.ID = DALinfo.ID;
            this.FullName = DALinfo.FullName;
            this.Experience = DALinfo.Experience;
            this.Salary = DALinfo.Salary;
            this.Hidden = DALinfo.Hidden;
        }
    }
}