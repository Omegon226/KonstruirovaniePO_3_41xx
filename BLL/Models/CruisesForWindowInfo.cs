using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CruisesForWindowInfo
    {
        public PossibleCruises Cruise;

        public DateTime StartDate;
        public int AmountOfFreeSeats;
        public List<int> OccupiedSeats = new List<int>();

        public string CruiseStartDate { get; set; }
        public string CruiseStartPointLocalityName { get; set; }
        public string CruiseFullTimeInCruise { get; set; }
        public string CruiseEndPointLocalityName { get; set; }
        public string CruiseFullPrice { get; set; }
        public string CruiseAmountOfFreeSeats { get; set; }

        public CruisesForWindowInfo(PossibleCruises PossibleCruises)
        {
            this.Cruise = PossibleCruises;
        }
        public void SetStartDate(DateTime Date)
        {
            this.StartDate = Date;
        }
        public void InitioliseStringInfoForWindow()
        {
            this.CruiseStartDate = StartDate.ToString();
            this.CruiseStartPointLocalityName = "Нач: " + Cruise.StartPointLocalityName;
            this.CruiseFullTimeInCruise = Cruise.FullTimeInCruise.ToString() + " Часа";
            this.CruiseEndPointLocalityName = "Кон: " + Cruise.EndPointLocalityName;
            this.CruiseFullPrice = "Цена : " + Cruise.FullPrice.ToString() + " Руб.";
            this.CruiseAmountOfFreeSeats = AmountOfFreeSeats.ToString();
        }
    }
}
