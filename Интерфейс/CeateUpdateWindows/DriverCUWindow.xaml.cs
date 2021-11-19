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
    /// Логика взаимодействия для DriverCUWindow.xaml
    /// </summary>
    public partial class DriverCUWindow : Window
    {
        public DriverCUWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            int Experience = 0;
            int Salary = 0;
    
            bool SuccessOfSurnameParse = SurnameTextBox.Text.All(Char.IsLetter);
            bool SuccessOfNameParse = NameTextBox.Text.All(Char.IsLetter);
            bool SuccessOfPatronomicParse = PatronymicTextBox.Text.All(Char.IsLetter);
            bool SuccessOfHoursParse = Int32.TryParse(ExperienceTextBox.Text, out Experience);
            bool SuccessOfMinutesParse = Int32.TryParse(SalaryTextBox.Text, out Salary);

            if (SuccessOfSurnameParse && SuccessOfNameParse && SuccessOfPatronomicParse && SuccessOfHoursParse && SuccessOfMinutesParse)
            {
                bool CorectnessOfExperienceNumber = ValidationOfExperience(Experience);
                bool CorectnessOfSalaryNumber = ValidationOfSalary(Salary);

                if (CorectnessOfExperienceNumber && CorectnessOfSalaryNumber)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значения опыта или зарплаты неверны! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }
        private bool ValidationOfExperience(int Experience)
        {
            if (Experience <= int.MaxValue)
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfSalary(int Salary)
        {
            if (Salary <= int.MaxValue)
            {
                return (true);
            }
            return (false);
        }
    }
}
