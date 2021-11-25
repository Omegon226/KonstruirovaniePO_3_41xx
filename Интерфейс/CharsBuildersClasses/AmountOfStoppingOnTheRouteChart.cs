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
    public class AmountOfStoppingOnTheRouteChart
    {
        public static List<FindeAmountOfStoppingOnTheRoute.StoredProcedureResult> ResultOfSP = FindeAmountOfStoppingOnTheRoute.StoredProcedureExecute();
        public ChartValues<int> CountOfCruises = new ChartValues<int>();
        public string[] Labels = new string[ResultOfSP.Count];
        public Func<double, string> Formatter = value => value.ToString("N");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public AmountOfStoppingOnTheRouteChart()
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
                CountOfCruises.Add(ResultOfSP[i].AmountOfStoppings);
            }
            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                Labels[i] = "ID Маршрута = " + ResultOfSP[i].RouteID.ToString();
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во остановок",
                    Values = CountOfCruises
                }
            };
        }
    }
}
