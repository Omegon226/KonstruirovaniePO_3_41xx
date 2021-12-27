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
    public class FindeAmountOfPurchesedTicketsForUsers
    {
        public class StoredProcedureResult
        {
            public int ID { get; set; }
            public int AmountOfPurchesedTickets { get; set; }
        }
        public class FinalResult
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public int AmountOfPurchesedTickets { get; set; }

            public FinalResult(StoredProcedureResult SPR)
            {
                UserID = SPR.ID;
                AmountOfPurchesedTickets = SPR.AmountOfPurchesedTickets;
            }
        }

        static DBDataOperations DBComunication;
        static List<UserModel> UsersInfo = new List<UserModel>();

        public FindeAmountOfPurchesedTicketsForUsers(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;
            UsersInfo = DBComunication.User.GetAll();
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
                for (int j = 0; j < UsersInfo.Count; ++j)
                {
                    if (FinalResult[i].UserID == UsersInfo[j].ID)
                    {
                        FinalResult[i].FullName = UsersInfo[j].FullName;
                    }
                }
            }

            return (FinalResult);
        }

        private static List<StoredProcedureResult> StoredProcedureExecute()
        {
            AvtovokzalDBContext DBContext = new AvtovokzalDBContext();
            var result = DBContext.Database.SqlQuery<StoredProcedureResult>("[SP_AmountOfPurchesedTicketsForUsers]").ToList();

            var data = result.GroupBy(i => new { i.ID, i.AmountOfPurchesedTickets })
                .Select(i => new StoredProcedureResult
                {
                    ID = i.Key.ID,
                    AmountOfPurchesedTickets = i.Key.AmountOfPurchesedTickets
                }).ToList();

            return (data);
        }
    }
}
