using BLL.DBInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Интерфейс.CRUDClasses
{
    public class CRUDOperations
    {
        DBDataOperations DBComunication;

        public CRUDLogicForUser User;
        public CRUDLogicForTransport Transport;
        public CRUDLogicForTicket Ticket;
        public CRUDLogicForStopSequences StopSequences;
        public CRUDLogicForStoppingOnTheRoute StoppingOnTheRoute;
        public CRUDLogicForRoute Route;
        public CRUDLogicForLocality Locality;
        public CRUDLogicForDriver Driver;
        public CRUDLogicForCruise Cruise;

        public CRUDOperations(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;

            User = new CRUDLogicForUser(DBComunication);
            Transport = new CRUDLogicForTransport(DBComunication);
            Ticket = new CRUDLogicForTicket(DBComunication);
            StopSequences = new CRUDLogicForStopSequences(DBComunication);
            StoppingOnTheRoute = new CRUDLogicForStoppingOnTheRoute(DBComunication);
            Route = new CRUDLogicForRoute(DBComunication);
            Locality = new CRUDLogicForLocality(DBComunication);
            Driver = new CRUDLogicForDriver(DBComunication);
            Cruise = new CRUDLogicForCruise(DBComunication);
        }
    }
}
