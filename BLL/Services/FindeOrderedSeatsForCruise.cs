using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeOrderedSeatsForCruise
    {
        public class StoredProcedureResult
        {
            public int SeatNumberOnTheTransport { get; set; }
            public int CruiseID { get; set; }
            public DateTime? RaceDepartureTime { get; set; }
        }

        public class FinalResult
        {
            public List<int> OrderedSeats { get; set; }
        }

        private static List<StoredProcedureResult> SPResult = new List<StoredProcedureResult>();
        private static FinalResult Result = new FinalResult();

        public static FinalResult CreateResult(DateTime CruiseDepartureTime, int CruiseID)
        {
            SPResult = StoredProcedureExecute(CruiseDepartureTime, CruiseID);
            Result.OrderedSeats = FindeOrderedSeats();
            return (Result);
        }
        private static List<int> FindeOrderedSeats()
        {
            List<int> OrderedSeats = new List<int>();
            for (int i = 0; i < SPResult.Count; ++i)
            {
                OrderedSeats.Add(SPResult[i].SeatNumberOnTheTransport);
            }
            return (OrderedSeats);
        }
        private static List<StoredProcedureResult> StoredProcedureExecute(DateTime CruiseDepartureTime, int CruiseID)
        {
            System.Data.SqlClient.SqlParameter paramRaceDepartureTimeForCruise = new System.Data.SqlClient.SqlParameter("@RaceDepartureTimeForCruise", CruiseDepartureTime);
            System.Data.SqlClient.SqlParameter paramCruiseIDOfTicket = new System.Data.SqlClient.SqlParameter("@CruiseIDOfTicket", CruiseID);
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_FindeTicketsUsingCruiseIDAndRaceDepartureTime @RaceDepartureTimeForCruise,@CruiseIDOfTicket", 
                new object[] { paramRaceDepartureTimeForCruise, paramCruiseIDOfTicket }).ToList();

            var data = result.GroupBy(i => new { i.SeatNumberOnTheTransport, i.CruiseID, i.RaceDepartureTime })
                .Select(i => new StoredProcedureResult
                {
                    SeatNumberOnTheTransport = i.Key.SeatNumberOnTheTransport,
                    CruiseID = i.Key.CruiseID,
                    RaceDepartureTime = i.Key.RaceDepartureTime,
                }).ToList();
            return (data);

        }
    }
}
