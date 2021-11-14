using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class LocalityModel
    {
        public int ID { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }

        public LocalityModel()
        {

        }
        public LocalityModel(Locality DALinfo)
        {
            this.ID = DALinfo.ID;
            this.Region = DALinfo.Region;
            this.Name = DALinfo.Name;
        }
    }
}