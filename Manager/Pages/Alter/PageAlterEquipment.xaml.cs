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
    public partial class PageAlterEquipment : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageAlterEquipment()
        {
            InitializeComponent();
            combobox_fill();
        }

        private void combobox_fill()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectLabName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                labtxt.SelectedItem = null;
                labtxt.Items.Clear();
                while (reader.Read())
                {
                    labtxt.Items.Add(reader[0]);
                    newlabtxt.Items.Add(reader[0]);
                }
            }
        }

        private void Alter_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("equipmentAlter", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Equipment_title_past", equtxt.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Lab_title_past", labtxt.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Lab_title", newlabtxt.Text);
                command.Parameters.AddWithValue("@Status", newstatustxt.Text);
                command.Parameters.AddWithValue("@Equipment_title", newnametxt.Text);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                combobox_fill();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
        }

        private void labtxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            equtxt.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("equipmentSelectAll", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@Lab_title", labtxt.SelectedItem.ToString());
                }
                catch (NullReferenceException)
                {
                    return;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                        equtxt.Items.Add(reader[0]);
                }
            }
        }

        private void equtxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("equipmentSelectCol", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@Equipment_title", equtxt.SelectedItem.ToString());
                }
                catch (NullReferenceException)
                {
                    return;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newnametxt.Text = reader[1].ToString();
                    newstatustxt.Text = reader[0].ToString();
                }
            }
        }
    }
}
