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

namespace IBSR_Manager_WPF.Manager.Pages.Delete
{
    /// <summary>
    /// Логика взаимодействия для PageDeleteExperiment.xaml
    /// </summary>
    public partial class PageDeleteExperiment : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageDeleteExperiment()
        {
            InitializeComponent();
            combobox_fill();
        }

        private void combobox_fill()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("experimentSelectAll", connect);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    expttxt.Items.Add(reader[0]);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("experimentDelete", connect);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Exp_title", expttxt.SelectedItem.ToString());
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
            expttxt.Items.Clear();
            combobox_fill();
        }
    }
}
