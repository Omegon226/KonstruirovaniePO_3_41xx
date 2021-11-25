using BLL.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Интерфейс.CharsBuildersClasses
{
    public class DependenceOfSalaryOnLengthOfServiceChart
    {
        public static List<FindeDriver.StoredProcedureResult> DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();
        public static List<FindeDriver.StoredProcedureResult> DriversForChart = new List<FindeDriver.StoredProcedureResult>();
        public static ChartValues<int> Salarys = new ChartValues<int>();
        public string[] Labels = new string[DriversForChartDependenceOfSalaryOnLengthOfService.Count];
        public Func<double, string> YFormatter = value => value.ToString("C");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public DependenceOfSalaryOnLengthOfServiceChart()
        {
            //SetInformationForChart();
        }

        public void SetInformationForChart()
        {
            DriversForChart = SortDriversByTheirExperience();
            UpdateDriversForChartDependenceOfSalaryOnLengthOfService();

            if (SeriesCollection.Count != 0)
            {
                DriversForChart = null;
                DriversForChart = new List<FindeDriver.StoredProcedureResult>();
                Salarys = null;
                Salarys = new ChartValues<int>();
                Labels = null;
                Labels = new string[DriversForChartDependenceOfSalaryOnLengthOfService.Count];
                SeriesCollection = null;
                SeriesCollection = new SeriesCollection();
            }

            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Salarys.Add(DriversForChart[i].Salary);
            }
            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Labels[i] = DriversForChart[i].Experience.ToString() + "Года/Лет Стажа " + DriversForChart[i].FullName;
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Зависимость зарплаты от стажа работы",
                    Values = Salarys
                }
            };
        }
        private List<FindeDriver.StoredProcedureResult> SortDriversByTheirExperience()
        {
            List<FindeDriver.StoredProcedureResult> DriversSort = new List<FindeDriver.StoredProcedureResult>();
            string[] Labels = new string[DriversForChartDependenceOfSalaryOnLengthOfService.Count];

            int countOfDrivers = DriversForChartDependenceOfSalaryOnLengthOfService.Count;
            for (int i = 0; i < countOfDrivers; i++)
            {
                int minExperience = int.MaxValue;
                int index = 0;
                for (int j = 0; j < DriversForChartDependenceOfSalaryOnLengthOfService.Count; j++)
                {
                    if (DriversForChartDependenceOfSalaryOnLengthOfService[j].Experience < minExperience)
                    {
                        minExperience = DriversForChartDependenceOfSalaryOnLengthOfService[j].Experience;
                        index = j;
                    }
                }
                DriversSort.Add(DriversForChartDependenceOfSalaryOnLengthOfService[index]);
                DriversForChartDependenceOfSalaryOnLengthOfService.RemoveAt(index);
            }

            return (DriversSort);
        }
        public void UpdateDriversForChartDependenceOfSalaryOnLengthOfService()
        {
            DriversForChartDependenceOfSalaryOnLengthOfService = FindeDriver.StoredProcedureExecute();
        }
    }
}
