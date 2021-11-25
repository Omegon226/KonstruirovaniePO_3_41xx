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
    public class CRUDLogicForLocality
    {
        DBDataOperations DBComunication;

        public CRUDLogicForLocality()
        {

        }
        public CRUDLogicForLocality(DBDataOperations DBComunicationFromWindow)
        {
            DBComunication = DBComunicationFromWindow;
        }

        public LocalityModel Create()
        {
            LocalityCUWindow CreateWindow = new LocalityCUWindow();

            bool? result = CreateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                LocalityModel NewObject = new LocalityModel();

                NewObject.Region = CreateWindow.RegionTextBox.Text;
                NewObject.Name = CreateWindow.NameTextBox.Text;

                return (NewObject);
            }
        }
        public LocalityModel Update(LocalityModel ph)
        {
            LocalityCUWindow UpdateWindow = new LocalityCUWindow();

            UpdateWindow.RegionTextBox.Text = ph.Region;
            UpdateWindow.NameTextBox.Text = ph.Name;

            bool? result = UpdateWindow.ShowDialog();
            if (result == false)
                return (null);
            else
            {
                ph.Region = UpdateWindow.RegionTextBox.Text;
                ph.Name = UpdateWindow.NameTextBox.Text;

                return (ph);
            }
        }
    }
}
