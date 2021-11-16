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
    public class StoppingOnTheRouteOperations
    {
        private AvtovokzalDBContext DBContext;

        public StoppingOnTheRouteOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public StoppingOnTheRouteOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(StoppingOnTheRouteModel RowToCreate)
        {
            DBContext.StoppingOnTheRoute.Add(new StoppingOnTheRoute()
            {
                StopLocalityID = RowToCreate.StopLocalityID
            });
            Save();
        }

        public void Delete(int ID)
        {
            StoppingOnTheRoute RowToDelete = DBContext.StoppingOnTheRoute.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.StoppingOnTheRoute.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(StoppingOnTheRouteModel RowToUpdate)
        {
            StoppingOnTheRoute DALRawToUpdate = DBContext.StoppingOnTheRoute.Find(RowToUpdate.ID);
            DALRawToUpdate.StopLocalityID = RowToUpdate.StopLocalityID;
            Save();
        }

        public List<StoppingOnTheRouteModel> GetAll()
        {
            return (DBContext.StoppingOnTheRoute.ToList().Select(i => new StoppingOnTheRouteModel(i)).ToList());
        }

        public StoppingOnTheRouteModel GetCruise(int ID)
        {
            return new StoppingOnTheRouteModel(DBContext.StoppingOnTheRoute.Find(ID));
        }
    }
}
