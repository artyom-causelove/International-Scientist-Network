using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace IBSR_Manager_WPF.Scientist.Profile
{
    /// <summary>
    /// Логика взаимодействия для PageProfileInfo.xaml
    /// </summary>
    public partial class PageProfileInfo : Page
    {
        private string connectionString;
        private int id;
        private string img_loc = "";

        public PageProfileInfo()
        {
            InitializeComponent();
            connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
            id = IBSR_Manager_WPF.Properties.Settings.Default.Id;
            download_info();
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

        public void download_info()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ImageSourceConverter img_conv = new ImageSourceConverter();
                SqlCommand command = new SqlCommand("scientistSelectAll", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txt_name.Text = reader[2].ToString() + " " + reader[3].ToString();
                    try {
                        byte[] img_byte = (byte[])reader[8];
                        img_face.SetValue(Image.SourceProperty, img_conv.ConvertFrom(img_byte));
                    } catch (InvalidCastException ex)
                    {
                    }
                    txt_age.Text = "Your age: " + reader[4].ToString();
                    txt_email.Text = "Your email: " + reader[6].ToString();
                }
            }
        }

        private void Btn_alterPassword_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("alterPassword", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", this.id);
                string password = password_box.Text, pasResult;
                byte[] tempPas = ASCIIEncoding.ASCII.GetBytes(password);
                byte[] tempHash = new MD5CryptoServiceProvider().ComputeHash(tempPas);
                pasResult = byteArrayToString(tempHash);
                command.Parameters.AddWithValue("@Password", pasResult);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                password_box.Text = "";
                txt_status.Text = "Success";
            }
        }

        private void Btn_selectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|All Files (*.*)|*.*";
            dlg.Title = "Select picture";
            if (dlg.ShowDialog() == true)
            {
                ImageSourceConverter conv = new ImageSourceConverter();
                img_face.SetValue(Image.SourceProperty, conv.ConvertFromString(dlg.FileName.ToString()));
                img_loc = dlg.FileName;
                byte[] img = null;
                FileStream fs = new FileStream(img_loc, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                img = br.ReadBytes((int)fs.Length);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("scientistImageAlter", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Image", img);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }
    }
}
