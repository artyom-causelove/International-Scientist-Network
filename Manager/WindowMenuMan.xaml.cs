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
using IBSR_Manager_WPF.Manager.Pages.Welcome;
using IBSR_Manager_WPF;

namespace IBSR_Manager_WPF.Manager
{
    /// <summary>
    /// Логика взаимодействия для WindowMenuMan.xaml
    /// </summary>
    public partial class WindowMenuMan : Window
    {
        private PageWelcomeAdd pw;
        private PageAdd pa;
        private PageAlter pal;
        private PageDelete pd;

        public WindowMenuMan()
        {
            InitializeComponent();
            pw = new PageWelcomeAdd();
            pa = new PageAdd();
            pal = new PageAlter();
            pd = new PageDelete();
            Main.Content = pw;
        }

        private void MenuHome_click(object sender, RoutedEventArgs e)
        {
            Main.Content = pw;
        }

        private void MenuAdd_click(object sender, RoutedEventArgs e)
        {
            Main.Content = pa;
        }

        private void MenuAlter_click(object sender, RoutedEventArgs e)
        {
            Main.Content = pal;
        }

        private void LogOut_click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Activate();
            mw.Show();
            this.Close();
        }

        private void MenuDelete_click(object sender, RoutedEventArgs e)
        {
            Main.Content = pd;
        }
    }
}
