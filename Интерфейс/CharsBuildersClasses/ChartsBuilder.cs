using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Интерфейс.CharsBuildersClasses
{
    public class ChartsBuilder
    {
        public DependenceOfSalaryOnLengthOfServiceChart DependenceOfSalaryOnLengthOfService;
        public DriversSalaryChart DriversSalary;
        public UsersStatusCountChart UsersStatusCount;
        public AmountOfCreatedCruisesOnTheRouteChart AmountOfCreatedCruisesOnTheRoute;
        public AmountOfStoppingOnTheRouteChart AmountOfStoppingOnTheRoute;

        public ChartsBuilder()
        {
            DependenceOfSalaryOnLengthOfService = new DependenceOfSalaryOnLengthOfServiceChart();
            DriversSalary = new DriversSalaryChart();
            UsersStatusCount = new UsersStatusCountChart();
            AmountOfCreatedCruisesOnTheRoute = new AmountOfCreatedCruisesOnTheRouteChart();
            AmountOfStoppingOnTheRoute = new AmountOfStoppingOnTheRouteChart();
        }
    }
}
