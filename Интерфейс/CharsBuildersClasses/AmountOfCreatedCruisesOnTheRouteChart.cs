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
    public class AmountOfCreatedCruisesOnTheRouteChart
    {
        public static List<FindeAmountOfCruiseOnTheRoute.StoredProcedureResult> ResultOfSP = FindeAmountOfCruiseOnTheRoute.StoredProcedureExecute();
        public ChartValues<int> CountOfCruises = new ChartValues<int>();
        public string[] Labels = new string[ResultOfSP.Count];
        public Func<double, string> Formatter = value => value.ToString("N");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public AmountOfCreatedCruisesOnTheRouteChart()
        {
            //SetInformationForChart();
        }

        public void SetInformationForChart()
        {
            if (SeriesCollection.Count != 0)
            {
                CountOfCruises = null;
                CountOfCruises = new ChartValues<int>();
                Labels = null;
                Labels = new string[ResultOfSP.Count];
                SeriesCollection = null;
                SeriesCollection = new SeriesCollection();
            }

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                CountOfCruises.Add(ResultOfSP[i].AmountOfCruises);
            }
            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                Labels[i] = "ID Маршрута = " + ResultOfSP[i].IDOfRoute.ToString();
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во рейсов",
                    Values = CountOfCruises
                }
            };
        }
    }
}
