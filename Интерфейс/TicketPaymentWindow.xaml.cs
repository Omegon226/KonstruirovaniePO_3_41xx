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

namespace Интерфейс
{
    /// <summary>
    /// Логика взаимодействия для TicketPaymentWindow.xaml
    /// </summary>
    public partial class TicketPaymentWindow : Window
    {
        public TicketPaymentWindow(string FullPriceToPay)
        {
            InitializeComponent();
            FullPaymentTextBox.Text = FullPriceToPay;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
