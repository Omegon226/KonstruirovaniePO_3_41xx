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
    /// Логика взаимодействия для TransportCUWindow.xaml
    /// </summary>
    public partial class TransportCUWindow : Window
    {
        public TransportCUWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool SuccessOfRegistrationNumberParse = RegistrationNumberTextBox.Text.All(Char.IsLetterOrDigit);
            bool SuccessOfModelParse = ModelTextBox.Text.All(Char.IsLetterOrDigit);

            if (SuccessOfRegistrationNumberParse && SuccessOfModelParse)
            {
                bool CorectnessOfRegistrationNumber = ValidationOfRegistrationNumber();
                bool CorectnessOfAmountOfSeats = ValidationOfAmountOfSeats();

                if (CorectnessOfRegistrationNumber && CorectnessOfAmountOfSeats)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значение регистрационного номера или кол-во мест слишком большое неверно! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }
        private bool ValidationOfRegistrationNumber()
        {
            if ((RegistrationNumberTextBox.Text.Length >= 6) && (RegistrationNumberTextBox.Text.Length <= 9))
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfAmountOfSeats()
        {
            if ((NumberOfSeatsIntegerUpDown.Value > 0) && (NumberOfSeatsIntegerUpDown.Value <= 200))
            {
                return (true);
            }
            return (false);
        }
    }
}
