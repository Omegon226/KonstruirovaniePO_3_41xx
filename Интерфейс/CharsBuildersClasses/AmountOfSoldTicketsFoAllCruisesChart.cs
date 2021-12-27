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
    public class AmountOfSoldTicketsFoAllCruisesChart
    {
        public static List<FindeAmountOfSoldTicketsForLiveTimeOfAllCruises.StoredProcedureResult> ResultOfSP = FindeAmountOfSoldTicketsForLiveTimeOfAllCruises.StoredProcedureExecute();
        public static ChartValues<int> CountOfStatuses = new ChartValues<int>();
        public string[] Labels = new string[ResultOfSP.Count];
        public Func<double, string> Formatter = value => value.ToString("N");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public AmountOfSoldTicketsFoAllCruisesChart()
        {
            //SetInformationForChart();
        }

        public void SetInformationForChart()
        {
            if (SeriesCollection.Count != 0)
            {
                CountOfStatuses = null;
                CountOfStatuses = new ChartValues<int>();
                SeriesCollection = null;
                SeriesCollection = new SeriesCollection();
            }

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                CountOfStatuses.Add(ResultOfSP[i].AmountOfSaldTickets);
                Labels[i] = "ID Рейса = " + ResultOfSP[i].CruiseID.ToString();
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Кол-во проданных билетов на рейс",
                    Values = CountOfStatuses
                }
            };
        }
    }
}
