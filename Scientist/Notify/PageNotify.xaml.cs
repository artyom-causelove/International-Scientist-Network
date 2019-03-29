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

namespace IBSR_Manager_WPF.Scientist.Notify
{
    /// <summary>
    /// Логика взаимодействия для PageNotify.xaml
    /// </summary>
    public partial class PageNotify : Page
    {
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString1;
        private int id = IBSR_Manager_WPF.Properties.Settings.Default.Id;
        private WindowMenuSci wms;

        public PageNotify(WindowMenuSci wms)
        {
            InitializeComponent();
            this.wms = wms;
            download_notify();
        }

        public void download_notify()
        {
            ThicknessConverter tconv = new ThicknessConverter();
            main_stack.Children.Clear();
            Button btn_clear = new Button();
            MaterialDesignThemes.Wpf.PackIcon packIcon1 = new MaterialDesignThemes.Wpf.PackIcon();
            packIcon1.Kind = MaterialDesignThemes.Wpf.PackIconKind.NotificationClearAll;
            packIcon1.VerticalAlignment = VerticalAlignment.Center;
            TextBlock temp = new TextBlock();
            temp.Text = "Clear";
            temp.VerticalAlignment = VerticalAlignment.Center;
            temp.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("7, 0, 0, 0"));
            btn_clear.Height = 25;
            btn_clear.Width = 100;
            btn_clear.SetValue(Button.MarginProperty, tconv.ConvertFromString("0, 7, 0, 0"));
            btn_clear.Click += Btn_clear_Click;
            StackPanel stack1 = new StackPanel();
            stack1.Orientation = Orientation.Horizontal;
            stack1.VerticalAlignment = VerticalAlignment.Center;
            stack1.Children.Add(packIcon1);
            stack1.Children.Add(temp);         
            btn_clear.Content = stack1;
            main_stack.Children.Add(btn_clear);
            ImageSourceConverter isconv = new ImageSourceConverter();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectAllNotifyById", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    wms.setNewNotify();
                    TextBlock textBlock = new TextBlock();
                    Image image = new Image();
                    Button button = new Button();
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.SetValue(StackPanel.MarginProperty, tconv.ConvertFromString("30, 0, 0, 0"));
                    stackPanel.Orientation = Orientation.Horizontal;                    
                    image.Width = 70;
                    image.Height = 70;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    image.Stretch = Stretch.Fill;
                    image.SetValue(Image.MarginProperty, tconv.ConvertFromString("20, 5, 5, 5"));
                    textBlock.Width = 450;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
                    packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.KindProperty, MaterialDesignThemes.Wpf.PackIconKind.Done);
                    packIcon.Foreground = Brushes.White;
                    packIcon.VerticalAlignment = VerticalAlignment.Center;
                    packIcon.HorizontalAlignment = HorizontalAlignment.Center;
                    packIcon.Height = 20;
                    packIcon.Width = 20;
                    button.Content = packIcon;
                    button.Width = 45;
                    button.Height = 45;
                    //ID_notify, ID_scientist, ID_group
                    string data = (reader[0].ToString() + " ");
                    data += (reader[2].ToString() + " ");
                    data += (reader[3].ToString());
                    button.DataContext = data;
                    button.Click += Btn_accept_Click;
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        ImageSourceConverter img_conv = new ImageSourceConverter();
                        SqlCommand command1 = new SqlCommand("scientistSelectAll", connection1);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;
                        command1.Parameters.AddWithValue("@Id", int.Parse(reader[2].ToString()));
                        connection1.Open();
                        var reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            textBlock.Text = reader1[5].ToString() + " " + reader1[2].ToString() + " " + reader1[3].ToString() + " wants to enjoy into ";
                        }
                    }
                    using (SqlConnection connection3 = new SqlConnection(connectionString))
                    {
                        SqlCommand command3 = new SqlCommand("selectGroupAll", connection3);
                        command3.CommandType = System.Data.CommandType.StoredProcedure;
                        command3.Parameters.AddWithValue("@Id", int.Parse(reader[3].ToString()));
                        command3.Parameters.AddWithValue("@Name", "");
                        connection3.Open();
                        var reader5 = command3.ExecuteReader();
                        while (reader5.Read())
                        {
                            textBlock.Text += (reader5[4].ToString() + ".");
                            try
                            {
                                byte[] img_byte = (byte[])reader5[5];
                                image.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                            }
                            catch (InvalidCastException ex)
                            {
                            }
                        }
                    }                    
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(button);
                    main_stack.Children.Add(stackPanel);
                }
            }
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("clearAllScientistNotify", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            download_notify();
        }

        public void Btn_accept_Click(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            string data = button.DataContext.ToString();
            string id_notify = "", id_sci = "", id_group = "";
            char[] arr = data.ToCharArray();
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i] != ' ')
                {
                    id_notify += arr[i];
                } else
                {
                    for (i = i + 1; i < arr.Length; ++i)
                    {
                        if (arr[i] != ' ')
                        {
                            id_sci += arr[i];
                        } else
                        {
                            for (i = i + 1; i < arr.Length; ++i)
                            {
                                id_group += arr[i];
                            }
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("acceptScientistNotify", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_notify", int.Parse(id_notify));
                command.Parameters.AddWithValue("@ID_scientist", int.Parse(id_sci));
                command.Parameters.AddWithValue("@ID_group", int.Parse(id_group));
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            download_notify();
        }
    }
}
