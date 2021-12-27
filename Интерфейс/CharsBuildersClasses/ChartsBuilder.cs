using BLL.DBInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Интерфейс.CharsBuildersClasses
{
    public class ChartsBuilder
    {
        private DBDataOperations DBComunication;

        public DependenceOfSalaryOnLengthOfServiceChart DependenceOfSalaryOnLengthOfService;
        public DriversSalaryChart DriversSalary;
        public UsersStatusCountChart UsersStatusCount;
        public AmountOfCreatedCruisesOnTheRouteChart AmountOfCreatedCruisesOnTheRoute;
        public AmountOfStoppingOnTheRouteChart AmountOfStoppingOnTheRoute;
        public AmountOfSoldTicketsFoAllCruisesChart AmountOfSoldTicketsFoAllCruises;
        public WorckLoadOfDriversChart WorckLoadOfDrivers;
        public WorckLoadOfTransportsChart WorckLoadOfTransports;

        public ChartsBuilder(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;

            DependenceOfSalaryOnLengthOfService = new DependenceOfSalaryOnLengthOfServiceChart();
            DriversSalary = new DriversSalaryChart();
            UsersStatusCount = new UsersStatusCountChart();
            AmountOfCreatedCruisesOnTheRoute = new AmountOfCreatedCruisesOnTheRouteChart();
            AmountOfStoppingOnTheRoute = new AmountOfStoppingOnTheRouteChart();
            AmountOfSoldTicketsFoAllCruises = new AmountOfSoldTicketsFoAllCruisesChart();
            WorckLoadOfDrivers = new WorckLoadOfDriversChart(DBComunication);
            WorckLoadOfTransports = new WorckLoadOfTransportsChart(DBComunication);
        }
    }
}
