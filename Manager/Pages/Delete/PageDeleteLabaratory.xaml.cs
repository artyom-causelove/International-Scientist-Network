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
    /// Логика взаимодействия для PageDeleteLabaratory.xaml
    /// </summary>
    public partial class PageDeleteLabaratory : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageDeleteLabaratory()
        {
            InitializeComponent();
            combobox_fill();
        }

        private void combobox_fill()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectLabName", connect);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    labttxt.Items.Add(reader[0]);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("labaratoryDelete", connect);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Lab_title", labttxt.SelectedItem.ToString());
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
                statustxt.Text = "Success";
                statustxt.FontSize = 14;
                statustxt.Foreground = Brushes.Green;
            }
            labttxt.Items.Clear();
            combobox_fill();
        }
    }
}
