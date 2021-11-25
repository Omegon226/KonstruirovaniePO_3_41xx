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
    public class DriversSalaryChart
    {
        public static List<FindeDriver.StoredProcedureResult> DriversForChart = FindeDriver.StoredProcedureExecute();
        public static ChartValues<int> Salarys = new ChartValues<int>();
        public string[] Labels = new string[DriversForChart.Count];
        public Func<double, string> YFormatter = value => value.ToString("C");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public DriversSalaryChart()
        {
            //SetInformationForChart();
        }

        public void SetInformationForChart()
        {
            if (SeriesCollection.Count != 0)
            {
                Salarys = null;
                Salarys = new ChartValues<int>();
                Labels = null;
                Labels = new string[DriversForChart.Count];
                SeriesCollection = null;
                SeriesCollection = new SeriesCollection();
            }

            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Salarys.Add(DriversForChart[i].Salary);
            }
            for (int i = 0; i < DriversForChart.Count; ++i)
            {
                Labels[i] = DriversForChart[i].FullName;
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Зарплата водителей",
                    Values = Salarys
                }
            };
        }
    }
}
