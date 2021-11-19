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
    /// Логика взаимодействия для CruiseCUWindow.xaml
    /// </summary>
    public partial class CruiseCUWindow : Window
    {
        public CruiseCUWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            int Hours = 0;
            int Minutes = 0;
            int Seconds = 0;

            bool SuccessOfHoursParse = Int32.TryParse(StartTimeHoursTextBox.Text, out Hours);
            bool SuccessOfMinutesParse = Int32.TryParse(StartTimeMinutesTextBox.Text, out Minutes);
            bool SuccessOfSecondsParse = Int32.TryParse(StartTimeSecondsTextBox.Text, out Seconds);

            if (SuccessOfHoursParse && SuccessOfMinutesParse && SuccessOfSecondsParse)
            {
                bool CorectnessOfHoursNumber = ValidationOfHours(Hours);
                bool CorectnessOfMinutesNumber = ValidationOfMinutes(Minutes);
                bool CorectnessOfSecondsNumber = ValidationOfSeconds(Seconds);

                if (CorectnessOfHoursNumber && CorectnessOfMinutesNumber && CorectnessOfSecondsNumber)
                {
                    DialogResult = true;
                    return;
                }
                else 
                {
                    MessageBox.Show("Значения времени введены неверно! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
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
