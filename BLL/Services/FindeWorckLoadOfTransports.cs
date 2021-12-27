using BLL.DBInteraction;
using BLL.Models;
using DAL.EFClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FindeWorckLoadOfTransports
    {
        public class StoredProcedureResult
        {
            public int ID { get; set; }
            public int Workload { get; set; }
        }
        public class FinalResult
        {
            public int TransportID { get; set; }
            public string Model { get; set; }
            public int Workload { get; set; }

            public FinalResult(StoredProcedureResult SPR)
            {
                TransportID = SPR.ID;
                Workload = SPR.Workload;
            }
        }

        static DBDataOperations DBComunication;
        static List<TransportModel> TransportInfo = new List<TransportModel>();

        public FindeWorckLoadOfTransports(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;
            TransportInfo = DBComunication.Transport.GetAll();
        }

        public List<FinalResult> FindeResult()
        {
            List<StoredProcedureResult> SPResult = StoredProcedureExecute();
            List<FinalResult> FinalResult = new List<FinalResult>();

            for (int i = 0; i < SPResult.Count; ++i)
            {
                FinalResult.Add(new FinalResult(SPResult[i]));
            }
            for (int i = 0; i < SPResult.Count; ++i)
            {
                for (int j = 0; j < TransportInfo.Count; ++j)
                {
                    if (FinalResult[i].TransportID == TransportInfo[j].ID)
                    {
                        FinalResult[i].Model = TransportInfo[j].Model;
                    }
                }
            }

            return (FinalResult);
        }

        private static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_WorkloadOfTransport").ToList();

            var data = result.GroupBy(i => new { i.ID, i.Workload })
                .Select(i => new StoredProcedureResult
                {
                    ID = i.Key.ID,
                    Workload = i.Key.Workload
                }).ToList();

            return (data);
        }
    }
}
