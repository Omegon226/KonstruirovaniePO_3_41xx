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
    /// Логика взаимодействия для TicketCUWindow.xaml
    /// </summary>
    public partial class TicketCUWindow : Window
    {
        public TicketCUWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool SuccessOfSurnameParse = SurnameTextBox.Text.All(Char.IsLetter);
            bool SuccessOfNameParse = NameTextBox.Text.All(Char.IsLetter);
            bool SuccessOfPatronomicParse = PatronymicTextBox.Text.All(Char.IsLetter);

            bool SuccessOfFullNameInfo = SuccessOfSurnameParse && SuccessOfNameParse && SuccessOfPatronomicParse;

            bool SuccessOfIndetificationInformation = IndentificationInformationTextBox.Text.All(Char.IsLetterOrDigit);

            bool SuccessOfRaceDepartureYearInformation = RaceDepartureTimeYearParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfRaceDepartureMonthInformation = RaceDepartureTimeMonthParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfRaceDepartureDayInformation = RaceDepartureTimeDayParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfRaceDepartureHourInformation = RaceDepartureTimeHourParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfRaceDepartureMinuteInformation = RaceDepartureTimeMinuteParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfRaceDepartureSecondInformation = RaceDepartureTimeSecondParametrTextBox.Text.All(Char.IsDigit);

            bool SuccessOfRaceDepartureDateTime = SuccessOfRaceDepartureYearInformation && SuccessOfRaceDepartureMonthInformation && SuccessOfRaceDepartureDayInformation
                && SuccessOfRaceDepartureHourInformation && SuccessOfRaceDepartureMinuteInformation && SuccessOfRaceDepartureSecondInformation;

            bool SuccessOfDateOfIssueYearInformation = DateOfIssueYearParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfDateOfIssueMonthInformation = DateOfIssueMonthParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfDateOfIssueDayInformation = DateOfIssueDayParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfDateOfIssueHourInformation = DateOfIssueHourParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfDateOfIssueMinuteInformation = DateOfIssueMinuteParametrTextBox.Text.All(Char.IsDigit);
            bool SuccessOfDateOfIssueSecondInformation = DateOfIssueSecondParametrTextBox.Text.All(Char.IsDigit);

            bool SuccessOfDateOfIssueDateTime = SuccessOfDateOfIssueYearInformation && SuccessOfDateOfIssueMonthInformation && SuccessOfDateOfIssueDayInformation
                && SuccessOfDateOfIssueHourInformation && SuccessOfDateOfIssueMinuteInformation && SuccessOfDateOfIssueSecondInformation;

            if (SuccessOfFullNameInfo && SuccessOfIndetificationInformation && SuccessOfRaceDepartureDateTime && SuccessOfDateOfIssueDateTime)
            {
                bool CorectnessOfIndetificationInformatio = ValidationOfIndetificationInformatio(IndentificationInformationTextBox.Text);

                bool CorectnessOfRaceDepartureYear = ValidationOfYears(Int32.Parse(RaceDepartureTimeYearParametrTextBox.Text));
                bool CorectnessOfRaceDepartureMonth = ValidationOfMonths(Int32.Parse(RaceDepartureTimeMonthParametrTextBox.Text));
                bool CorectnessOfRaceDepartureDay = ValidationOfDays(Int32.Parse(RaceDepartureTimeDayParametrTextBox.Text));
                bool CorectnessOfRaceDepartureHours = ValidationOfHours(Int32.Parse(RaceDepartureTimeHourParametrTextBox.Text));
                bool CorectnessOfRaceDepartureMinute = ValidationOfMinutes(Int32.Parse(RaceDepartureTimeMinuteParametrTextBox.Text));
                bool CorectnessOfRaceDepartureSecond = ValidationOfSeconds(Int32.Parse(RaceDepartureTimeSecondParametrTextBox.Text));

                bool CorectnessOfRaceDepartureDateTime = CorectnessOfRaceDepartureYear && CorectnessOfRaceDepartureMonth && CorectnessOfRaceDepartureDay
                && CorectnessOfRaceDepartureHours && CorectnessOfRaceDepartureMinute && CorectnessOfRaceDepartureSecond;

                bool CorectnessOfDateOfIssueYear = ValidationOfYears(Int32.Parse(DateOfIssueYearParametrTextBox.Text));
                bool CorectnessOfDateOfIssueMonth = ValidationOfMonths(Int32.Parse(DateOfIssueMonthParametrTextBox.Text));
                bool CorectnessOfDateOfIssueDay = ValidationOfDays(Int32.Parse(DateOfIssueDayParametrTextBox.Text));
                bool CorectnessOfDateOfIssueHours = ValidationOfHours(Int32.Parse(DateOfIssueHourParametrTextBox.Text));
                bool CorectnessOfDateOfIssueMinute = ValidationOfMinutes(Int32.Parse(DateOfIssueMinuteParametrTextBox.Text));
                bool CorectnessOfDateOfIssueSecond = ValidationOfSeconds(Int32.Parse(DateOfIssueSecondParametrTextBox.Text));

                bool CorectnessOfDateOfIssueDateTime = CorectnessOfDateOfIssueYear && CorectnessOfDateOfIssueMonth && CorectnessOfDateOfIssueDay
                && CorectnessOfDateOfIssueHours && CorectnessOfDateOfIssueMinute && CorectnessOfDateOfIssueSecond;

                if (CorectnessOfIndetificationInformatio && CorectnessOfRaceDepartureDateTime && CorectnessOfDateOfIssueDateTime)
                {
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Значения времени или индентификационная информация, введены неверно! Измените их");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Видимо какое то поле имеет неправильный формат ввода");
                return;
            }
        }
        private bool ValidationOfIndetificationInformatio(string IndentificationInfo)
        {
            // Проверка по сведетельству о рождении
            if ((9 <= IndentificationInfo.Length) && (IndentificationInfo.Length <= 12)) 
            {
                return (true);
            }
            // Проверка по паспорту
            if (IndentificationInfo.Length == 10)
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfYears(int Years)
        {
            if ((1900 <= Years) && (Years <= 3000))
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfMonths(int Months)
        {
            if ((1 <= Months) && (Months <= 12))
            {
                return (true);
            }
            return (false);
        }
        private bool ValidationOfDays(int Days)
        {
            if ((1 <= Days) && (Days <= 31))
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
