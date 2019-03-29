using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IBSR_Manager_WPF.Scientist;
using IBSR_Manager_WPF.Manager;
using IBSR_Manager_WPF;

namespace IBSR_Manager_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static string byteArrayToString(byte[] input)
        {
            StringBuilder sOutput = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; ++i)
            {
                sOutput.Append(input[i].ToString("X2"));
            }
            return sOutput.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
            string prof = "";
            if (loginTextBox.Text == "")
            {
                loginTextBox.Background = Brushes.Red;
            }
            else
            {
                if ((bool)managerCheckBox.IsChecked)
                    prof = "Manager";
                else if ((bool)scientistCheckBox.IsChecked)
                    prof = "Scientist";
                else
                    MessageBox.Show("Select your profession", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (prof != "")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("checkLogIn" + prof, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", loginTextBox.Text);
                        string password = passwordTextBox.Password, pasResult;
                        byte[] tempPas = ASCIIEncoding.ASCII.GetBytes(password);
                        byte[] tempHash = new MD5CryptoServiceProvider().ComputeHash(tempPas);
                        pasResult = byteArrayToString(tempHash);
                        loginTextBox.Text = pasResult;
                        command.Parameters.AddWithValue("@Password", pasResult);
                        int res = -1;
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            res = (int) reader[0];
                            if (res > -1)
                            {
                                IBSR_Manager_WPF.Properties.Settings.Default.Id = res;
                                WindowLoad wl;
                                if (prof == "Manager")
                                {
                                    wl = new WindowLoad(true);
                                }
                                else
                                {
                                    wl = new WindowLoad(false);
                                }
                                wl.Activate();
                                wl.Show();
                                this.Close();
                            }
                            else
                            {
                                IBSR_Manager_WPF.Properties.Settings.Default.Id = res;
                                MessageBox.Show("NO!");
                            }
                        }
                    }
                }
            }
        }
    }
}
