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

namespace IBSR_Manager_WPF.Scientist.Groups
{
    /// <summary>
    /// Логика взаимодействия для PageGroups.xaml
    /// </summary>
    public partial class PageGroups : Page
    {
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString;
        private int id = IBSR_Manager_WPF.Properties.Settings.Default.Id;

        public PageGroups()
        {
            InitializeComponent();
            download_info();
        }

        public void send_appl(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sendAnApplication", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_scientist", id);
                command.Parameters.AddWithValue("@ID_group", int.Parse(button.DataContext.ToString()));
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            download_info();
        }

        public void download_info()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                stack_group.Children.Clear();
                ThicknessConverter tconv = new ThicknessConverter();
                CornerRadiusConverter crconv = new CornerRadiusConverter();
                ImageSourceConverter isconv = new ImageSourceConverter();
                FontFamilyConverter ffconv = new FontFamilyConverter();
                FontSizeConverter fsconv = new FontSizeConverter();
                SqlCommand command = new SqlCommand("selectAccesingGroup", connection);
                command.Parameters.AddWithValue("@ID_scientist", id);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StackPanel stackPanelBorder = new StackPanel();
                    StackPanel stackPanelSecond = new StackPanel();
                    stackPanelBorder.Orientation = Orientation.Horizontal;
                    stackPanelSecond.VerticalAlignment = VerticalAlignment.Center;
                    stackPanelSecond.HorizontalAlignment = HorizontalAlignment.Right;
                    Border border = new Border();
                    border.BorderBrush = Brushes.MediumSlateBlue;
                    border.SetValue(Border.CornerRadiusProperty, crconv.ConvertFromString("15"));
                    border.SetValue(Border.BorderThicknessProperty, tconv.ConvertFromString("3"));
                    border.Height = 100;
                    border.SetValue(Border.MarginProperty, tconv.ConvertFromString("20"));
                    Image image = new Image();
                    try
                    {
                        byte[] img_byte = (byte[])reader[5];
                        image.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                    }
                    catch (InvalidCastException ex)
                    {
                    }
                    image.Height = 75;
                    image.Width = 75;
                    image.Stretch = Stretch.Fill;
                    image.SetValue(Image.MarginProperty, tconv.ConvertFromString("8"));
                    TextBlock textBlock = new TextBlock();
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Width = 500;
                    stackPanel.Orientation = Orientation.Horizontal;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.SetValue(TextBlock.FontFamilyProperty, ffconv.ConvertFromString("Lucida Sans Unicode"));
                    textBlock.SetValue(TextBlock.FontSizeProperty, fsconv.ConvertFromString("35"));
                    textBlock.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("8"));
                    textBlock.Text = reader[4].ToString();
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(textBlock);
                    Button button = new Button();
                    button.DataContext = reader[0].ToString();
                    button.Click += send_appl;
                    button.Background = Brushes.MediumSlateBlue;
                    button.BorderBrush = Brushes.MediumSlateBlue;
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        SqlCommand command1 = new SqlCommand("checkNotifyHas", connection1);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;
                        command1.Parameters.AddWithValue("@ID_scientist", id);
                        command1.Parameters.AddWithValue("@ID_group", int.Parse(reader[0].ToString()));
                        connection1.Open();
                        var reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            button.IsEnabled = false;
                        }
                    }
                    StackPanel stackPanelBtn = new StackPanel();
                    stackPanelBtn.Orientation = Orientation.Horizontal;
                    MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
                    packIcon.VerticalAlignment = VerticalAlignment.Center;
                    packIcon.HorizontalAlignment = HorizontalAlignment.Right;
                    packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.MarginProperty, tconv.ConvertFromString("0, 0, 8, 0"));
                    packIcon.Height = 20;
                    packIcon.Width = 20;
                    packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.Flag);
                    stackPanelBtn.Children.Add(packIcon);
                    TextBlock textBlockBtn = new TextBlock();
                    textBlockBtn.Text = "Send an application";                 
                    stackPanelBtn.Children.Add(textBlockBtn);
                    button.Content = stackPanelBtn;
                    stackPanelSecond.Children.Add(button);
                    stackPanelBorder.Children.Add(stackPanel);
                    stackPanelBorder.Children.Add(stackPanelSecond);
                    border.Child = stackPanelBorder;
                    stack_group.Children.Add(border);
                }
            }
        }
    }
}
