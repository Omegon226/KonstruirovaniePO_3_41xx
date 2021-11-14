using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EFClasses;

namespace BLL.Models
{
    public class  StopSequencesModel
    {
        public int ID { get; set; }
        public int? IndexNumber { get; set; }
        public int StoppingID { get; set; }
        public int StopRouteID { get; set; }
        public float? TripPrice { get; set; }
        public TimeSpan? TravelTimeToStop { get; set; }

        public StopSequencesModel()
        {

        }
        public StopSequencesModel(StopSequences DALinfo)
        {
            this.ID = DALinfo.ID;
            this.IndexNumber = DALinfo.IndexNumber;
            this.StoppingID = DALinfo.StoppingID;
            this.StopRouteID = DALinfo.StopRouteID;
            this.TripPrice = DALinfo.TripPrice;
            this.TravelTimeToStop = DALinfo.TravelTimeToStop;
        }
    }
}