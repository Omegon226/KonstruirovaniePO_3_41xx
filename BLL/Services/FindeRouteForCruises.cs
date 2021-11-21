using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeRouteForCruises
    {
        public class StoredProcedureResult
        {
            public int IDOfSequence { get; set; }
            public int RouteID { get; set; }
            public int? IndexNumber { get; set; }
            public int StoppingID { get; set; }
            public float? TripPrice { get; set; }
            public TimeSpan? TravelTimeToStop { get; set; }
            public int IDOfLocality { get; set; }
            public string RegionOfLocality { get; set; }
            public string NameOfLocality { get; set; }
        }

        public class FinalResult
        {
            public int RouteID { get; set; }

            public int? StartPointIndex { get; set; }
            public int StartPointStoppingID { get; set; }
            public int StartPointLocalityID { get; set; }
            public string StartPointLocalityName { get; set; }
            public int? EndPointIndex { get; set; }
            public int EndPointStoppingID { get; set; }
            public int EndPointLocalityID { get; set; }
            public string EndPointLocalityName { get; set; }

            public FinalResult(StoredProcedureResult StartPointOfRoute, StoredProcedureResult EndPointOfRoute)
            {
                this.RouteID = StartPointOfRoute.RouteID;

                this.StartPointIndex = StartPointOfRoute.IndexNumber;
                this.StartPointStoppingID = StartPointOfRoute.StoppingID;
                this.StartPointLocalityID = StartPointOfRoute.IDOfLocality;
                this.StartPointLocalityName = StartPointOfRoute.NameOfLocality;
                this.EndPointIndex = EndPointOfRoute.IndexNumber;
                this.EndPointStoppingID = EndPointOfRoute.StoppingID;
                this.EndPointLocalityID = EndPointOfRoute.IDOfLocality;
                this.EndPointLocalityName = EndPointOfRoute.NameOfLocality;
            }
        }

        static List<StoredProcedureResult> ResultOfSPForFindingRouts;
        static List<FinalResult> RoutesResult = new List<FinalResult>();

        public static List<FinalResult> FindeRouts(int StartPointID, int EndPointID)
        {
            ResultOfSPForFindingRouts = StoredProcedureExecute(StartPointID, EndPointID);
            FinalResult RouteToAddInResult;

            for (int i = 0; i < ResultOfSPForFindingRouts.Count - 1;)
            {
                if ((ResultOfSPForFindingRouts[i].RouteID == ResultOfSPForFindingRouts[i + 1].RouteID) &&
                    (ResultOfSPForFindingRouts[i].IndexNumber < ResultOfSPForFindingRouts[i + 1].IndexNumber))
                {
                    RouteToAddInResult = new FinalResult(ResultOfSPForFindingRouts[i], ResultOfSPForFindingRouts[i+1]);

                    RoutesResult.Add(RouteToAddInResult);
                    i += 2;
                }
                else 
                {
                    ++i;
                }
            }

            return (RoutesResult);
        }

        private static List<StoredProcedureResult> StoredProcedureExecute(int StartPointID, int EndPointID)
        {
            System.Data.SqlClient.SqlParameter paramStartPointID = new System.Data.SqlClient.SqlParameter("@StartLocalityID", StartPointID);
            System.Data.SqlClient.SqlParameter paramEndPointID = new System.Data.SqlClient.SqlParameter("@EndLocalityID", EndPointID);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_GetRawInformationAbouRouteUsingStartAndEndPointsIDs @StartLocalityID, @EndLocalityID", 
                new object[] { paramStartPointID, paramEndPointID }).ToList();

            var data = result.GroupBy(i => new { i.IDOfSequence, i.RouteID, i.IndexNumber, i.StoppingID, i.TripPrice, 
                i.TravelTimeToStop, i.IDOfLocality, i.RegionOfLocality, i.NameOfLocality })
                .Select(i => new StoredProcedureResult
                {
                    IDOfSequence = i.Key.IDOfSequence,
                    RouteID = i.Key.RouteID,
                    IndexNumber = i.Key.IndexNumber,
                    StoppingID = i.Key.StoppingID,
                    TripPrice = i.Key.TripPrice,
                    TravelTimeToStop = i.Key.TravelTimeToStop,
                    IDOfLocality = i.Key.IDOfLocality,
                    RegionOfLocality = i.Key.RegionOfLocality,
                    NameOfLocality = i.Key.NameOfLocality
                }).ToList();

            return (data);
        }
    }
}
