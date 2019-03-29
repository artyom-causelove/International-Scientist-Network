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

namespace IBSR_Manager_WPF.Scientist.Welcome
{
    /// <summary>
    /// Логика взаимодействия для PageWelcome.xaml
    /// </summary>
    public partial class PageWelcome : Page
    {
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;

        public PageWelcome()
        {
            InitializeComponent();
            download_news();
        }

        public void download_news()
        {
            main_stack.Children.Clear();
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
                    FontFamilyConverter ffc = new FontFamilyConverter();
                    content.SetValue(TextBlock.FontFamilyProperty, ffc.ConvertFromString("Segoe UI"));
                    source.SetValue(TextBlock.FontFamilyProperty, ffc.ConvertFromString("Segoe UI"));
                    content.Text = reader[2].ToString();
                    source.Text = reader[3].ToString();
                    byte[] img_byte = (byte[])reader[4];
                    if (img_byte == null)
                    {
                        img.SetValue(Image.SourceProperty, null);
                    }
                    else
                    {
                        img.SetValue(Image.SourceProperty, img_conv.ConvertFrom(img_byte));
                    }
                    content.SetValue(TextBlock.MarginProperty, th_conv.ConvertFrom("15, 20, 0, 0"));
                    content.TextWrapping = TextWrapping.Wrap;
                    source.SetValue(TextBlock.MarginProperty, th_conv.ConvertFrom("15, 0, 0, 0"));
                    img.SetValue(Image.MarginProperty, th_conv.ConvertFrom("14, 0, 0, 0"));
                    content.Height = 140;
                    content.Width = 450;
                    img.Height = 200;
                    img.Width = 230;
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    main_stack.Children.Add(sp);
                    StackPanel sub_sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(sub_sp);
                    sub_sp.Children.Add(content);
                    sub_sp.Children.Add(source);
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.LightBlue;
                    rect.Height = 1;
                    rect.Width = 720;
                    main_stack.Children.Add(rect);
                }
            }
        }
    }
}
