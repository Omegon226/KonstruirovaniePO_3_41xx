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
    public class CRUDLogicForStoppingOnTheRoute
    {
        DBDataOperations DBComunication;

        List<LocalityModel> allLocalityCRUD;

        public CRUDLogicForStoppingOnTheRoute()
        {
            allLocalityCRUD = DBComunication.Locality.GetAll();
        }
        public CRUDLogicForStoppingOnTheRoute(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;

            allLocalityCRUD = DBComunication.Locality.GetAll();
        }

        public StoppingOnTheRouteModel Create()
        {
            StoppingOnTheRouteCUWindow CreateWindow = new StoppingOnTheRouteCUWindow();
            MakeSetupForStoppingOnTheRouteCUWindow(CreateWindow);

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                StoppingOnTheRouteModel NewObject = new StoppingOnTheRouteModel();

                NewObject.StopLocalityID = (int)CreateWindow.StopLocalityIDComboBox.SelectedValue;

                return (NewObject);
            }
        }
        public StoppingOnTheRouteModel Update(StoppingOnTheRouteModel ph)
        {
            StoppingOnTheRouteCUWindow UpdateWindow = new StoppingOnTheRouteCUWindow();
            MakeSetupForStoppingOnTheRouteCUWindow(UpdateWindow);

            UpdateWindow.StopLocalityIDComboBox.SelectedValue = ph.StopLocalityID;

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.StopLocalityID = (int)UpdateWindow.StopLocalityIDComboBox.SelectedValue;

                return (ph);
            }
        }
        private void MakeSetupForStoppingOnTheRouteCUWindow(StoppingOnTheRouteCUWindow Window)
        {
            Window.StopLocalityIDComboBox.ItemsSource = allLocalityCRUD;
            Window.StopLocalityIDComboBox.DisplayMemberPath = "Name";
            Window.StopLocalityIDComboBox.SelectedValuePath = "ID";
        }
    }
}
