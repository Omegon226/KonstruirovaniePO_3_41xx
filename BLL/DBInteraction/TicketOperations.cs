using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using DAL.EFClasses;
using DAL;

namespace BLL.DBInteraction
{
    public class TicketOperations
    {
        private AvtovokzalDBContext DBContext;

        public TicketOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public TicketOperations(AvtovokzalDBContext DBContextFromMain)
        {
            DBContext = DBContextFromMain;
        }

        public bool Save()
        {
            if (DBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public void Create(TicketModel RowToCreate)
        {
            DBContext.Ticket.Add(new Ticket()
            {
                DateOfIssue = RowToCreate.DateOfIssue,
                IdentificationInformation = RowToCreate.IdentificationInformation,
                SeatNumberOnTheTransport = RowToCreate.SeatNumberOnTheTransport,
                FullName = RowToCreate.FullName,
                CruiseID = RowToCreate.CruiseID,
                UserID = RowToCreate.UserID,
                RaceDepartureTime = RowToCreate.RaceDepartureTime
            });
            Save();
        }

        public void Delete(int ID)
        {
            Ticket RowToDelete = DBContext.Ticket.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Ticket.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(TicketModel RowToUpdate)
        {
            Ticket DALRawToUpdate = DBContext.Ticket.Find(RowToUpdate.ID);
            DALRawToUpdate.DateOfIssue = RowToUpdate.DateOfIssue;
            DALRawToUpdate.IdentificationInformation = RowToUpdate.IdentificationInformation;
            DALRawToUpdate.SeatNumberOnTheTransport = RowToUpdate.SeatNumberOnTheTransport;
            DALRawToUpdate.FullName = RowToUpdate.FullName;
            DALRawToUpdate.CruiseID = RowToUpdate.CruiseID;
            DALRawToUpdate.UserID = RowToUpdate.UserID;
            DALRawToUpdate.RaceDepartureTime = RowToUpdate.RaceDepartureTime;
            Save();
        }

        public List<TicketModel> GetAll()
        {
            return (DBContext.Ticket.ToList().Select(i => new TicketModel(i)).ToList());
        }

        public TicketModel GetCruise(int ID)
        {
            return new TicketModel(DBContext.Ticket.Find(ID));
        }
    }
}
