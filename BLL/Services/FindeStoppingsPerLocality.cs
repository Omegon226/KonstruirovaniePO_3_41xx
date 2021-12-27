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
    public class  FindeStoppingsPerLocality
    {
        public class StoredProcedureResult
        {
            public int ID { get; set; }
            public int AmountOfStoppings { get; set; }
        }
        public class FinalResult
        {
            public int LocalityID { get; set; }
            public string Name { get; set; }
            public int AmountOfStoppings { get; set; }

            public FinalResult(StoredProcedureResult SPR)
            {
                LocalityID = SPR.ID;
                AmountOfStoppings = SPR.AmountOfStoppings;
            }
        }

        static DBDataOperations DBComunication;
        static List<LocalityModel> LocalityInfo = new List<LocalityModel>();

        public FindeStoppingsPerLocality(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;
            LocalityInfo = DBComunication.Locality.GetAll();
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
                for (int j = 0; j < LocalityInfo.Count; ++j)
                {
                    if (FinalResult[i].LocalityID == LocalityInfo[j].ID)
                    {
                        FinalResult[i].Name = LocalityInfo[j].Name;
                    }
                }
            }

            return (FinalResult);
        }

        private static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("SP_StoppingsPerLocality").ToList();

            var data = result.GroupBy(i => new { i.ID, i.AmountOfStoppings })
                .Select(i => new StoredProcedureResult
                {
                    ID = i.Key.ID,
                    AmountOfStoppings = i.Key.AmountOfStoppings
                }).ToList();

            return (data);
        }
    }
}
