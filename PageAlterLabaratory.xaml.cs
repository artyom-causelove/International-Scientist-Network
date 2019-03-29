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
    public partial class PageAlterLabaratory : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageAlterLabaratory()
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
                labttxt.SelectedItem = null;
                labttxt.Items.Clear();
                while (reader.Read())
                {
                    labttxt.Items.Add(reader[0]);
                }
            }
        }

        private void Alter_Click(object sender, RoutedEventArgs e)
        {           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("labaratoryAlter", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Labaratory_past_title", labttxt.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Labaratory_title", newtitletxt.Text);
                command.Parameters.AddWithValue("@Location", locationtxt.Text);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                combobox_fill();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
        }

        private void Labttxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                SqlCommand command = new SqlCommand("labaratorySelectAll", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddWithValue("@Labaratory_title", labttxt.SelectedItem.ToString());
                } catch (NullReferenceException)
                {
                    return;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    locationtxt.Text = reader[1].ToString();
                    newtitletxt.Text = reader[0].ToString();
                }
            }
        }
    }
}
