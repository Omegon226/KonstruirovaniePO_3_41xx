using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class  StoppingOnTheRouteModel
    {
        public int ID { get; set; }
        public int StopLocalityID { get; set; }

        public StoppingOnTheRouteModel()
        {

        }
        public StoppingOnTheRouteModel(StoppingOnTheRoute DALinfo)
        {
            this.ID = DALinfo.ID;
            this.StopLocalityID = DALinfo.StopLocalityID;
        }
    }
}