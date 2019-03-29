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
    /// Логика взаимодействия для PageAlterLabaratory.xaml
    /// </summary>
    public partial class PageAlterExperiment : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageAlterExperiment()
        {
            InitializeComponent();
            combobox_fill();
        }

        private void combobox_fill()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("experimentSelectAll", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                expttxt.SelectedItem = null;
                expttxt.Items.Clear();
                while (reader.Read())
                {
                    expttxt.Items.Add(reader[0]);
                }
            }
        }

        private void Alter_Click(object sender, RoutedEventArgs e)
        {           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("experimentAlter", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title_past", expttxt.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Title", newtitletxt.Text);
                command.Parameters.AddWithValue("@Start_date", startdatetxt.Text);
                command.Parameters.AddWithValue("@End_date", enddatetxt.Text);
                command.Parameters.AddWithValue("@Status", newstatustxt.Text);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                combobox_fill();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
        }

        private void expttxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                SqlCommand command = new SqlCommand("experimentSelectCol", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@Title", expttxt.SelectedItem.ToString());
                } catch (NullReferenceException)
                {
                    return;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newtitletxt.Text = reader[0].ToString();
                    startdatetxt.Text = reader[1].ToString();
                    enddatetxt.Text = reader[2].ToString();
                    newstatustxt.Text = reader[3].ToString();
                }
            }
        }
    }
}
