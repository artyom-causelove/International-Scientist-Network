using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using IBSR_Manager_WPF.Scientist;
using IBSR_Manager_WPF.Manager;

namespace IBSR_Manager_WPF
{
    /// <summary>
    /// Логика взаимодействия для WindowLoad.xaml
    /// </summary>
    public partial class WindowLoad : Window
    {
        bool prof;
        int t = 0;

        public WindowLoad(bool prof)
        {
            InitializeComponent();
            this.prof = prof;
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            t++;
            if (t == 8)
                DoubleAnimation_Completed(null, null);
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            if (prof)
            {
                WindowMenuMan wmn = new WindowMenuMan();
                wmn.Activate();
                wmn.Show();
            } else
            {
                WindowMenuSci wms = new WindowMenuSci();
                wms.Activate();
                wms.Show();
            }
            this.Close();
        }
    }
}
