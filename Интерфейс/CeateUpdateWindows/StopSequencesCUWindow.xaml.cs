﻿using System;
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

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
