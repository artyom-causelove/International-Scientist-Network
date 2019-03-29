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

namespace IBSR_Manager_WPF
{
    /// <summary>
    /// Логика взаимодействия для WindowMenuMan.xaml
    /// </summary>
    public partial class WindowMenuMan : Window
    {
        private PageWelcome pw;
        private PageAdd pa;
        private PageAlter pal;

        public WindowMenuMan()
        {
            InitializeComponent();
            pw = new PageWelcome();
            pa = new PageAdd();
            pal = new PageAlter();
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
    }
}
