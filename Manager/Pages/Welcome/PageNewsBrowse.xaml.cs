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

namespace IBSR_Manager_WPF.Manager.Pages.Welcome
{
    /// <summary>
    /// Логика взаимодействия для PageNewsBrowse.xaml
    /// </summary>
    public partial class PageNewsBrowse : Page
    {
        string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageNewsBrowse()
        {
            InitializeComponent();
            download_news();
        }

        public void download_news()
        {
            stack.Children.Clear();
            stacktext.Children.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectNews", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    ThicknessConverter th_conv = new ThicknessConverter();
                    ImageSourceConverter img_conv = new ImageSourceConverter();
                    Image img = new Image();
                    TextBlock content = new TextBlock();
                    TextBlock source = new TextBlock();
                    content.Text = reader[2].ToString();
                    source.Text = reader[3].ToString();
                    byte[] img_byte = (byte[])reader[4];
                    if (img_byte == null)
                    {
                        img.SetValue(Image.SourceProperty, null);
                    } else
                    {
                        img.SetValue(Image.SourceProperty, img_conv.ConvertFrom(img_byte));
                    }
                    content.SetValue(TextBlock.MarginProperty, th_conv.ConvertFrom("0, 20, 15, 0"));
                    content.TextWrapping = TextWrapping.Wrap;
                    source.SetValue(TextBlock.MarginProperty, th_conv.ConvertFrom("10, 0, 15, 30"));
                    img.SetValue(Image.MarginProperty, th_conv.ConvertFrom("5, 0, 10, 5"));
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    content.Height = 140;
                    content.Width = 500;
                    img.Height = 200;
                    img.Width = 230;
                    stack.Children.Add(img);
                    stacktext.Children.Add(content);
                    stacktext.Children.Add(source);
                }
            }
        }
    }
}
