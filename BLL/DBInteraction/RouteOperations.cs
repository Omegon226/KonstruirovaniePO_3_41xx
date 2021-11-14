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
    public class RouteOperations
    {
        private AvtovokzalDBContext DBContext;

        public RouteOperations()
        {
            DBContext = new AvtovokzalDBContext();
        }
        public RouteOperations(AvtovokzalDBContext DBContextFromMain)
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

        public void Create(RouteModel RowToCreate)
        {
            DBContext.Route.Add(new Route()
            {
                TravelTimeInHours = RowToCreate.TravelTimeInHours,
                Hidden = RowToCreate.Hidden

            });
            Save();
        }

        public void Delete(int ID)
        {
            Route RowToDelete = DBContext.Route.Find(ID);
            if (RowToDelete != null)
            {
                DBContext.Route.Remove(RowToDelete);
                Save();
            }
        }

        public void Update(RouteModel RowToUpdate)
        {
            Route DALRawToUpdate = DBContext.Route.Find(RowToUpdate.ID);
            DALRawToUpdate.TravelTimeInHours = RowToUpdate.TravelTimeInHours;
            DALRawToUpdate.Hidden = RowToUpdate.Hidden;
        }

        public List<RouteModel> GetAll()
        {
            return (DBContext.Route.ToList().Select(i => new RouteModel(i)).ToList());
        }

        public RouteModel GetCruise(int ID)
        {
            return new RouteModel(DBContext.Route.Find(ID));
        }
    }
}
