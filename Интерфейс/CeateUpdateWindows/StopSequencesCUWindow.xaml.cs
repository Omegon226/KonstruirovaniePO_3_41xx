using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Интерфейс.CeateUpdateWindows
{
    /// <summary>
    /// Логика взаимодействия для StopSequencesCUWindow.xaml
    /// </summary>
    public partial class StopSequencesCUWindow : Window
    {
        public StopSequencesCUWindow()
        {
            InitializeComponent();
        }
        public StopSequencesCUWindow(List<LocalityModel> allLocality, List<StoppingOnTheRouteModel> allStoppingOnTheRoute)
        {
            InitializeComponent();
            LocalityDataGrid.ItemsSource = allLocality;
            StoppingOnTheRouteDataGrid.ItemsSource = allStoppingOnTheRoute;

        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            int Hours = 0;
            int Minutes = 0;
            int Seconds = 0;
            int PriceForStoppping = 0;
            int IndexOfStopping = 0;

            bool SuccessOfHoursParse = Int32.TryParse(TravelTimeToStopHoursTextBox.Text, out Hours);
            bool SuccessOfMinutesParse = Int32.TryParse(TravelTimeToStopMinutesTextBox.Text, out Minutes);
            bool SuccessOfSecondsParse = Int32.TryParse(TravelTimeToStopSecondsTextBox.Text, out Seconds);
            bool SuccessOfPriceForStoppping = Int32.TryParse(TripPriceTextBox.Text, out PriceForStoppping);
            bool SuccessOfIndexOfStopping = Int32.TryParse(IndexNumberIntegerUpDown.Text, out IndexOfStopping);

            if (SuccessOfHoursParse && SuccessOfMinutesParse && SuccessOfSecondsParse && SuccessOfPriceForStoppping && SuccessOfIndexOfStopping)
            {
                bool CorectnessOfHoursNumber = ValidationOfHours(Hours);
                bool CorectnessOfMinutesNumber = ValidationOfMinutes(Minutes);
                bool CorectnessOfSecondsNumber = ValidationOfSeconds(Seconds);
                bool CorectnessOfPriceForStoppping = ValidationOfPriceForStoppping(PriceForStoppping);
                bool CorectnessOfIndexOfStopping = ValidationOfIndexOfStopping(IndexOfStopping);

                if (CorectnessOfHoursNumber && CorectnessOfMinutesNumber && CorectnessOfSecondsNumber && CorectnessOfPriceForStoppping && CorectnessOfIndexOfStopping)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значения времени введены или индекса остановки, или цена остановки неверно! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }
        private bool ValidationOfPriceForStoppping(int PriceForStoppping)
        {
            if (PriceForStoppping >= 0)
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfIndexOfStopping(int PriceIndexOfStoppingForStoppping)
        {
            if (PriceIndexOfStoppingForStoppping > 0)
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfHours(int Hours)
        {
            if ((0 <= Hours) && (Hours <= 23))
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfMinutes(int Minutes)
        {
            if ((0 <= Minutes) && (Minutes <= 59))
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfSeconds(int Seconds)
        {
            if ((0 <= Seconds) && (Seconds <= 59))
            {
                return (true);
            }
            return (false);
        }
    }
}
