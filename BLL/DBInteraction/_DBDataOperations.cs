using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using DAL.EFClasses;
using DAL;
using System.Data.Entity;

namespace BLL.DBInteraction
{
    public class DBDataOperations
    {
        private AvtovokzalDBContext DBContext;

        public CruiseOperations Cruise;
        public DayOfTheWeekOperations DayOfTheWeek;
        public DriverOperations Driver;
        public LocalityOperations Locality;
        public RouteOperations Route;
        public StoppingOnTheRouteOperations StoppingOnTheRoute;
        public StopSequencesOperations StopSequences;
        public TicketOperations Ticket;
        public TransportOperations Transport;
        public UserOperations User;

        public DBDataOperations()
        {
            this.DBContext = new AvtovokzalDBContext();

            Cruise = new CruiseOperations(DBContext);
            DayOfTheWeek = new DayOfTheWeekOperations(DBContext);
            Driver = new DriverOperations(DBContext);
            Locality = new LocalityOperations(DBContext);
            Route = new RouteOperations(DBContext);
            StoppingOnTheRoute = new StoppingOnTheRouteOperations(DBContext);
            StopSequences = new StopSequencesOperations(DBContext);
            Ticket = new TicketOperations(DBContext);
            Transport = new TransportOperations(DBContext);
            User = new UserOperations(DBContext);

            DBContext.Cruise.Load();
            DBContext.DayOfTheWeek.Load();
            DBContext.Driver.Load();
            DBContext.Locality.Load();
            DBContext.Route.Load();
            DBContext.StoppingOnTheRoute.Load();
            DBContext.Cruise.Load();
            DBContext.StopSequences.Load();
            DBContext.Ticket.Load();
            DBContext.Transport.Load();
            DBContext.User.Load();
        }
        public DBDataOperations(AvtovokzalDBContext NewDBContext)
        {
            this.DBContext = NewDBContext;

            Cruise = new CruiseOperations(DBContext);
            DayOfTheWeek = new DayOfTheWeekOperations(DBContext);
            Driver = new DriverOperations(DBContext);
            Locality = new LocalityOperations(DBContext);
            Route = new RouteOperations(DBContext);
            StoppingOnTheRoute = new StoppingOnTheRouteOperations(DBContext);
            StopSequences = new StopSequencesOperations(DBContext);
            Ticket = new TicketOperations(DBContext);
            Transport = new TransportOperations(DBContext);
            User = new UserOperations(DBContext);

            DBContext.Cruise.Load();
            DBContext.DayOfTheWeek.Load();
            DBContext.Driver.Load();
            DBContext.Locality.Load();
            DBContext.Route.Load();
            DBContext.StoppingOnTheRoute.Load();
            DBContext.Cruise.Load();
            DBContext.StopSequences.Load();
            DBContext.Ticket.Load();
            DBContext.Transport.Load();
            DBContext.User.Load();
        }

        public bool Save()
        {
            if (DBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
