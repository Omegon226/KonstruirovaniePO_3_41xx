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
    public class TransportOperations
    {
        private AvtovokzalDBContext DBContext;

        public TransportOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public TransportOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(TransportModel RowToCreate)
        {
            DBContext.Transport.Add(new Transport()
            {
                NumberOfSeats = RowToCreate.NumberOfSeats,
                RegistrationNumber = RowToCreate.RegistrationNumber,
                Model = RowToCreate.Model,
                Hidden = RowToCreate.Hidden
            });
            Save();
        }

        public void Delete(int ID)
        {
            Transport RowToDelete = DBContext.Transport.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Transport.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(TransportModel RowToUpdate)
        {
            Transport DALRawToUpdate = DBContext.Transport.Find(RowToUpdate.ID);
            DALRawToUpdate.NumberOfSeats = RowToUpdate.NumberOfSeats;
            DALRawToUpdate.RegistrationNumber = RowToUpdate.RegistrationNumber;
            DALRawToUpdate.Model = RowToUpdate.Model;
            DALRawToUpdate.Hidden = RowToUpdate.Hidden;
        }

        public List<TransportModel> GetAll()
        {
            return (DBContext.Transport.ToList().Select(i => new TransportModel(i)).ToList());
        }

        public TransportModel GetCruise(int ID)
        {
            return new TransportModel(DBContext.Transport.Find(ID));
        }
    }
}
