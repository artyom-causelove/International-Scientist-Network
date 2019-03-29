using IBSR_Manager_WPF.Manager.Pages.Delete;
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
    /// Логика взаимодействия для PageDelete.xaml
    /// </summary>
    public partial class PageDelete : Page
    {
        PageDeleteLabaratory pdl = new PageDeleteLabaratory();
        PageDeleteEquipment pde = new PageDeleteEquipment();
        PageDeleteExperiment pdexp = new PageDeleteExperiment();

        public PageDelete()
        {
            InitializeComponent();
            AddFrame.Content = pdl;
        }

        private void Labaratory_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pdl;
        }

        private void Equipment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pde;
        }

        private void Experiment_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pdexp;
        }
    }
}
