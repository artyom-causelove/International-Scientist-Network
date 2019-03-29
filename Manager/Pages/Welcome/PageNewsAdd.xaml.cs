using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;

namespace IBSR_Manager_WPF.Manager.Pages.Welcome
{
    /// <summary>
    /// Логика взаимодействия для PageNewsAdd.xaml
    /// </summary>
    public partial class PageNewsAdd : Page
    {
        PageNewsBrowse bnb;
        string img_loc = "";
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageNewsAdd(PageNewsBrowse bnb)
        {
            InitializeComponent();
            this.bnb = bnb;
        }

        private void clear()
        {
            imagebox.Source = null;
            sourcetxt.Text = "";
            contenttxt.Text = "";
        }

        private void clear_click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void select_click(object sender, RoutedEventArgs e)
        {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|All Files (*.*)|*.*";
                dlg.Title = "Select picture";
                if (dlg.ShowDialog() == true)
                {
                    ImageSourceConverter conv = new ImageSourceConverter();
                    imagebox.SetValue(Image.SourceProperty, conv.ConvertFromString(dlg.FileName.ToString()));
                    img_loc = dlg.FileName;
                }
        }

        private void commit_click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] img = null;
                FileStream fs = new FileStream(img_loc, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                img = br.ReadBytes((int)fs.Length);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("newsInsert", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_manager", IBSR_Manager_WPF.Properties.Settings.Default.Id);
                    command.Parameters.AddWithValue("@Caption", contenttxt.Text);
                    command.Parameters.AddWithValue("@Source", sourcetxt.Text);
                    command.Parameters.AddWithValue("@Image", img);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    clear();
                    statustxt.Foreground = Brushes.Green;
                    statustxt.Text = "Success";
                    bnb.download_news();
                }
            } catch (Exception ex)
            {
            }
        }
    }
}
