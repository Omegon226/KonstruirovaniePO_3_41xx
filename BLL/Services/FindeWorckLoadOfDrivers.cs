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
    public class FindeWorckLoadOfDrivers
    {
        public class StoredProcedureResult
        {
            public int ID { get; set; }
            public int Workload { get; set; }
        }
        public class FinalResult
        {
            public int DriverID { get; set; }
            public string FullName { get; set; }
            public int Workload { get; set; }

            public FinalResult(StoredProcedureResult SPR)
            {
                DriverID = SPR.ID;
                Workload = SPR.Workload;
            }
        }

        static DBDataOperations DBComunication;
        static List<DriverModel> DriversInfo = new List<DriverModel>();

        public FindeWorckLoadOfDrivers(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;
            DriversInfo = DBComunication.Driver.GetAll();
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
                for (int j = 0; j < DriversInfo.Count; ++j)
                {
                    if (FinalResult[i].DriverID == DriversInfo[j].ID)
                    {
                        FinalResult[i].FullName = DriversInfo[j].FullName;
                    }
                }
            }

            return (FinalResult);
        }

        private static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_WorkloadOfDrivers").ToList();

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
