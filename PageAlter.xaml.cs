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

namespace IBSR_Manager_WPF
{
    /// <summary>
    /// Логика взаимодействия для PageAlter.xaml
    /// </summary>
    public partial class PageAlter : Page
    {
        PageAlterLabaratory pal;
        PageAlterEquipment paq;
        PageAlterExperiment paexp;


        public PageAlter()
        {
            InitializeComponent();
            pal = new PageAlterLabaratory();
            paq = new PageAlterEquipment();
            paexp = new PageAlterExperiment();
            AddFrame.Content = pal;
        }

        private void labaratory_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pal;
        }


        private void equipment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = paq;

        }

        private void experiment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = paexp;
        }
    }


}
