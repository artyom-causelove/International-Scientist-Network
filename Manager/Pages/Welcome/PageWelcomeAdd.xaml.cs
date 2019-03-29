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

namespace IBSR_Manager_WPF.Manager.Pages.Welcome
{
    /// <summary>
    /// Логика взаимодействия для PageWelcomeAdd.xaml
    /// </summary>
    public partial class PageWelcomeAdd : Page
    {
        PageNewsBrowse pnb;
        PageNewsAdd pna;

        public PageWelcomeAdd()
        {
            InitializeComponent();
            pnb = new PageNewsBrowse();
            pna = new PageNewsAdd(pnb);
            AddFrame.Content = pnb;
        }

        private void news_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pnb;
        }

        private void addNews_click(object sender, RoutedEventArgs e)
        {
            AddFrame.Content = pna;
        }
    }
}
