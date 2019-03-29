using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
using IBSR_Manager_WPF.Scientist;

namespace IBSR_Manager_WPF.Scientist.Experiments
{
    /// <summary>
    /// Логика взаимодействия для PageRecord.xaml
    /// </summary>
    public partial class PageRecord : Page
    {
        private int id_exp;
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString1;
        private WindowMenuSci parent;

        public PageRecord(WindowMenuSci parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        public void init(int id_exp)
        {
            this.id_exp = id_exp;
            parent.main_frame.Content = this;
            listview.Items.Clear();
            equ_txt.Items.Clear();
            group_txt.Items.Clear();
            time_txt.Text = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectAllEqu", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    equ_txt.Items.Add(reader[0].ToString());
                }
                equ_txt.SelectedIndex = 0;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectGroupByLeader", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_scientist", IBSR_Manager_WPF.Properties.Settings.Default.Id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    group_txt.Items.Add(reader[4].ToString());
                }
                group_txt.SelectedIndex = 0;
            }
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            parent.Btn_experiment_Click(null, null);
        }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            listview.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("searchLabByEqu", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@equ_name", equ_txt.Text);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listview.Items.Add(reader[1].ToString() + " | " + reader[2].ToString());
                }
            }
        }

        private void Btn_record_Click(object sender, RoutedEventArgs e)
        {
            SqlDateTime start_date = SqlDateTime.Parse(time_txt.Text);
            int temp = 0;
            char[] ch = listview.SelectedItem.ToString().ToCharArray();
            for (int i = 0; i < ch.Length; ++i)
            {
                if (ch[i] == '|')
                {
                    temp = i;
                }
            }               
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("insertOccupation", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@group_name", group_txt.Text);
                command.Parameters.AddWithValue("@lab_name", listview.SelectedItem.ToString().Substring(0, temp - 1));
                command.Parameters.AddWithValue("@end_date", start_date);
                command.Parameters.AddWithValue("@id_exp", id_exp);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();                
            }
            Btn_close_Click(null, null);
        }
    }
}
