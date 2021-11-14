using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class  RouteModel
    {
        public int ID { get; set; }
        public int? TravelTimeInHours { get; set; }
        public bool? Hidden { get; set; }

        public RouteModel()
        {

        }
        public RouteModel(Route DALinfo)
        {
            this.ID = DALinfo.ID;
            this.TravelTimeInHours = DALinfo.TravelTimeInHours;
            this.Hidden = DALinfo.Hidden;
        }
    }
}