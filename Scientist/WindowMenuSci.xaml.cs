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
using IBSR_Manager_WPF.Scientist.Welcome;
using IBSR_Manager_WPF.Scientist.Profile;
using IBSR_Manager_WPF.Scientist.Groups;
using IBSR_Manager_WPF.Scientist.Messages;
using System.Data.SqlClient;
using IBSR_Manager_WPF.Scientist.Notify;
using IBSR_Manager_WPF.Scientist.Experiments;

namespace IBSR_Manager_WPF.Scientist
{
    /// <summary>
    /// Логика взаимодействия для WindowMenuSci.xaml
    /// </summary>
    public partial class WindowMenuSci : Window
    {
        private PageExperiments pe;
        private PageWelcome pw;
        private PageProfileInfo ppi;
        private PageGroups pg;
        private PageMessageList pml;
        private PageNotify pn;

        public WindowMenuSci()
        {
            InitializeComponent();
            pw = new PageWelcome();
            ppi = new PageProfileInfo();
            pg = new PageGroups();
            pml = new PageMessageList();
            pn = new PageNotify(this);
            pe = new PageExperiments(this);
            main_frame.Content = pw;

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Btn_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            mainWindow.Show();
            this.Close();
        }

        public void setNewNotify()
        {
            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon.Height = 35;
            packIcon.Width = 35;
            packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.NotificationsActive);
            packIcon.Foreground = Brushes.LightBlue;
            btn_alert.Content = packIcon;
        }

        private void Btn_alert_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = pn;
            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon.Height = 35;
            packIcon.Width = 35;
            packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.Notifications);
            packIcon.Foreground = Brushes.White;
            btn_alert.Content = packIcon;
        }

        private void Btn_IBSR_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = pw;
        }

        private void Btn_profile_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = ppi;
        }

        private void Btn_groups_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = pg;
        }

        private void Btn_messages_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = pml;
        }


        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            pw.download_news();
            ppi.download_info();
            pg.download_info();
            pml.download_info(null, null);
            pn.download_notify();
            pe.download_exp();
        }

        public void Btn_experiment_Click(object sender, RoutedEventArgs e)
        {
            main_frame.Content = pe;
        }
    }
}
