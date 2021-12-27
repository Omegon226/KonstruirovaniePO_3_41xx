using BLL.DBInteraction;
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
    public class WorckLoadOfTransportsChart
    {
        private DBDataOperations DBComunication;
        private static FindeWorckLoadOfTransports report;
        public List<FindeWorckLoadOfTransports.FinalResult> ResultOfSP;
        public static ChartValues<int> WorkloadForChart = new ChartValues<int>();
        public string[] Labels;
        public Func<double, string> Formatter = value => value.ToString("N");
        public SeriesCollection SeriesCollection = new SeriesCollection();

        public WorckLoadOfTransportsChart(DBDataOperations DBComunicationFromMain)
        {
            DBComunication = DBComunicationFromMain;
            report = new FindeWorckLoadOfTransports(DBComunication);
            ResultOfSP = report.FindeResult();
            Labels = new string[ResultOfSP.Count];
        }

        public void SetInformationForChart()
        {
            if (SeriesCollection.Count != 0)
            {
                WorkloadForChart = null;
                WorkloadForChart = new ChartValues<int>();
                Labels = null;
                Labels = new string[ResultOfSP.Count];
                SeriesCollection = null;
                SeriesCollection = new SeriesCollection();
            }

            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                WorkloadForChart.Add(ResultOfSP[i].Workload);
            }
            for (int i = 0; i < ResultOfSP.Count; ++i)
            {
                Labels[i] = "Марка: " + ResultOfSP[i].Model;
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Нагруженность",
                    Values = WorkloadForChart
                }
            };
        }
    }
}
