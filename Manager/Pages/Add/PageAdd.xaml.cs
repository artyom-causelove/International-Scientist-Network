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
    /// Логика взаимодействия для PageAdd.xaml
    /// </summary>
    public partial class PageAdd : Page
    {
        PageAddScientist pas;
        PageAddManager pam;
        PageAddEquipment pae;
        PageAddLabaratory pal;
        PageAddExperiment paexp;

        public PageAdd()
        {
            InitializeComponent();
            pas = new PageAddScientist();
            pam = new PageAddManager();
            pae = new PageAddEquipment();
            pal = new PageAddLabaratory();
            paexp = new PageAddExperiment();
            AddFrame.Content = pas;
        }

        private void scientist_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pas;
        }

        private void manager_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pam;
        }

        private void equipment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pae;
        }

        private void labaratory_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pal;
        }

        private void experiment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = paexp;
        }
    }
}
