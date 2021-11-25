using BLL.DBInteraction;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Интерфейс.CeateUpdateWindows;

namespace Интерфейс.CRUDClasses
{
    public class CRUDLogicForRoute
    {
        DBDataOperations DBComunication;

        public CRUDLogicForRoute()
        {

        }
        public CRUDLogicForRoute(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;
        }

        public RouteModel Create()
        {
            RouteCUWindow CreateWindow = new RouteCUWindow();          

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                RouteModel NewObject = new RouteModel();

                NewObject.TravelTimeInHours = CreateWindow.TravelTimeInHoursIntegerUpDown.Value;
                if (CreateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = false;
                }
                if (CreateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    NewObject.Hidden = true;
                }

                return (NewObject);
            }
        }
        public RouteModel Update(RouteModel ph)
        {
            RouteCUWindow UpdateWindow = new RouteCUWindow();

            UpdateWindow.TravelTimeInHoursIntegerUpDown.Value = ph.TravelTimeInHours;
            if (ph.Hidden == false)
            {
                UpdateWindow.HiddenNoRadioButton.IsChecked = true;
                UpdateWindow.HiddenYesRadioButton.IsChecked = false;
            }
            else
            {
                UpdateWindow.HiddenNoRadioButton.IsChecked = false;
                UpdateWindow.HiddenYesRadioButton.IsChecked = true;
            }

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.TravelTimeInHours = UpdateWindow.TravelTimeInHoursIntegerUpDown.Value;
                if (UpdateWindow.HiddenNoRadioButton.IsChecked == true)
                {
                    ph.Hidden = false;
                }
                if (UpdateWindow.HiddenYesRadioButton.IsChecked == true)
                {
                    ph.Hidden = true;
                }

                return (ph);
            }
        }
    }
}