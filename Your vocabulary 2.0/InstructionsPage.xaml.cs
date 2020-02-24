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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Логика взаимодействия для InstructionsPage.xaml
    /// </summary>
    public partial class InstructionsPage : Page
    {
        public InstructionsPage()
        {
            InitializeComponent();
        }

        private void HideInstBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MW.InstructionFrame.Visibility = Visibility.Hidden;
            MainWindow.MW.InstBtn.Visibility = Visibility.Visible;
        }
    }
}
