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
    /// Логика взаимодействия для PageAddManager.xaml
    /// </summary>
    public partial class PageAddManager : Page
    {
        public PageAddManager()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (emailtxt.Text != "")
            {
                string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("managerInsert", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", nametxt.Text);
                    command.Parameters.AddWithValue("@Surname", surntxt.Text);
                    command.Parameters.AddWithValue("@Age", agetxt.Text);
                    command.Parameters.AddWithValue("@Location", loctxt.Text);
                    command.Parameters.AddWithValue("@Address", addresstxt.Text);
                    command.Parameters.AddWithValue("@Email", emailtxt.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    statustxt.Text = "Success.";
                    statustxt.Foreground = Brushes.Green;
                    statustxt.FontSize = 14;
                }
            }
            else
            {
                statustxt.FontSize = 13;
                statustxt.Text = "Field email is emtpy.";
                statustxt.Foreground = Brushes.Red;
            }
        }
    }
}
