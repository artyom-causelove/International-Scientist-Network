using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для PageAddLabaratory.xaml
    /// </summary>
    public partial class PageAddLabaratory : Page
    {
        public PageAddLabaratory()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("labaratoryInsert", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Labaratory_title", labttxt.Text);
                command.Parameters.AddWithValue("@Location", locationtxt.Text);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
                return;
            }
        }
    }
}
