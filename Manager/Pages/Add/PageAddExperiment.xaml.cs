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
    /// Логика взаимодействия для PageAddExperiment.xaml
    /// </summary>
    public partial class PageAddExperiment : Page
    {
        public PageAddExperiment()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
            int managerID = IBSR_Manager_WPF.Properties.Settings.Default.Id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("experimentInsert", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title", exptitletxt.Text);
                command.Parameters.AddWithValue("@Start_date", startdatetxt.Text);
                command.Parameters.AddWithValue("@End_date", enddatetxt.Text);
                command.Parameters.AddWithValue("@Status", expstatustxt.Text);
                command.Parameters.AddWithValue("@ID_manager", managerID.ToString());
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
        }
    }
}
