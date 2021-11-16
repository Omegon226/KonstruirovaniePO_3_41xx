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
    public class StopSequencesOperations
    {
        private AvtovokzalDBContext DBContext;

        public StopSequencesOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public StopSequencesOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(StopSequencesModel RowToCreate)
        {
            DBContext.StopSequences.Add(new StopSequences()
            {
                IndexNumber = RowToCreate.IndexNumber,
                StoppingID = RowToCreate.StoppingID,
                StopRouteID = RowToCreate.StopRouteID,
                TripPrice = RowToCreate.TripPrice,
                TravelTimeToStop = RowToCreate.TravelTimeToStop
            });
            Save();
        }

        public void Delete(int ID)
        {
            StopSequences RowToDelete = DBContext.StopSequences.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.StopSequences.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(StopSequencesModel RowToUpdate)
        {
            StopSequences DALRawToUpdate = DBContext.StopSequences.Find(RowToUpdate.ID);
            DALRawToUpdate.IndexNumber = RowToUpdate.IndexNumber;
            DALRawToUpdate.StoppingID = RowToUpdate.StoppingID;
            DALRawToUpdate.StopRouteID = RowToUpdate.StopRouteID;
            DALRawToUpdate.TripPrice = RowToUpdate.TripPrice;
            DALRawToUpdate.TravelTimeToStop = RowToUpdate.TravelTimeToStop;
            Save();
        }

        public List<StopSequencesModel> GetAll()
        {
            return (DBContext.StopSequences.ToList().Select(i => new StopSequencesModel(i)).ToList());
        }

        public StopSequencesModel GetCruise(int ID)
        {
            return new StopSequencesModel(DBContext.StopSequences.Find(ID));
        }
    }
}
