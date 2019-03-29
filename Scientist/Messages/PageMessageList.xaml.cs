using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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

namespace IBSR_Manager_WPF.Scientist.Messages
{
    /// <summary>
    /// Логика взаимодействия для PageMessageList.xaml
    /// </summary>
    public partial class PageMessageList : Page
    {
        private bool isLeader = false;
        private string selected_group;
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString1;
        private int Id = IBSR_Manager_WPF.Properties.Settings.Default.Id;

        public PageMessageList()
        {
            InitializeComponent();
            download_info(null, null);
        }

        public void download_info(object sender, RoutedEventArgs args)
        {
            middle.Height = 750;
            bottom_stack.Visibility = Visibility.Collapsed;
            top_stack.Visibility = Visibility.Collapsed;
            ImageSourceConverter isconv = new ImageSourceConverter();
            CornerRadiusConverter crconv = new CornerRadiusConverter();
            ThicknessConverter tconv = new ThicknessConverter();
            FontWeightConverter fwconv = new FontWeightConverter();
            BrushConverter bc = new BrushConverter();
            using (SqlConnection connection = new SqlConnection(connectionString)) // All groups info
            {
                group_message_stack.Children.Clear();
                SqlCommand command = new SqlCommand("selectAllGroupsById", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", this.Id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Image image_last_scientist = new Image();
                    image_last_scientist.Height = 50;
                    image_last_scientist.Width = 50;
                    Image image_group = new Image();
                    image_group.Height = 80;
                    image_group.Width = 80;
                    image_group.Stretch = Stretch.Fill;
                    image_last_scientist.Stretch = Stretch.Fill;
                    TextBlock name_last_scientist = new TextBlock();
                    TextBlock name_group = new TextBlock();
                    TextBlock text = new TextBlock();
                    name_group.SetValue(TextBlock.FontWeightProperty, fwconv.ConvertFromString("Bold"));
                    image_last_scientist.VerticalAlignment = VerticalAlignment.Center;
                    image_group.VerticalAlignment = VerticalAlignment.Center;
                    name_last_scientist.VerticalAlignment = VerticalAlignment.Center;
                    name_group.VerticalAlignment = VerticalAlignment.Center;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    name_last_scientist.FontSize = 18;
                    name_group.FontSize = 22;
                    text.FontSize = 15;
                    image_last_scientist.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    image_group.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    name_last_scientist.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    name_group.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    text.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("5"));
                    name_group.Text = reader[4].ToString();
                    try
                    {
                        byte[] img_byte = (byte[])reader[5];
                        image_group.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                    }
                    catch (InvalidCastException ex)
                    {
                    }
                    using (SqlConnection connection1 = new SqlConnection(connectionString)) // Last message info
                    {                     
                        SqlCommand command1 = new SqlCommand("selectTopMessage", connection1);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;
                        command1.Parameters.AddWithValue("@ID_group", int.Parse(reader[0].ToString()));
                        connection1.Open();
                        var reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            text.Text = reader1[1].ToString();
                            using (SqlConnection connection2 = new SqlConnection(connectionString)) // Scienist info
                            {
                                SqlCommand command2 = new SqlCommand("scientistSelectAll", connection2);
                                command2.CommandType = System.Data.CommandType.StoredProcedure;
                                command2.Parameters.AddWithValue("@Id", int.Parse(reader1[0].ToString()));
                                connection2.Open();
                                var reader2 = command2.ExecuteReader();
                                while (reader2.Read())
                                {
                                    try
                                    {
                                        byte[] img_byte = (byte[])reader2[8];
                                        image_last_scientist.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                                    }
                                    catch (InvalidCastException ex)
                                    {
                                    }
                                    name_last_scientist.Text = reader2[2].ToString();
                                }
                            }
                        }
                    }
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(image_group);
                    stackPanel.Children.Add(name_group);
                    stackPanel.Children.Add(image_last_scientist);
                    name_last_scientist.Text += ":";
                    stackPanel.Children.Add(name_last_scientist);
                    stackPanel.Children.Add(text);
                    Button button = new Button();
                    button.Background = Brushes.AntiqueWhite;
                    button.Click += setInfo;
                    button.DataContext = name_group.Text;
                    button.Foreground = Brushes.Black;
                    button.HorizontalContentAlignment = HorizontalAlignment.Left; 
                    button.Background = null;
                    button.BorderBrush = null;
                    button.Height = 100;
                    button.Width = group_message_stack.Width;
                    button.Content = stackPanel;
                    group_message_stack.Children.Add(button);
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = 2;
                    rectangle.Width = group_message_stack.Width;
                    rectangle.Fill = Brushes.Blue;
                    rectangle.Opacity = 0.2;
                    group_message_stack.Children.Add(rectangle);
                }
            }
        }

        private void setInfo(object sender, RoutedEventArgs args)
        {
            middle.Height = 600;
            top_stack.Visibility = Visibility.Visible;
            bottom_stack.Visibility = Visibility.Visible;
            group_message_stack.Children.Clear();
            Button button = (Button)sender;
            selected_group = button.DataContext.ToString();
            download_mess();
        }

        public void download_mess()
        {
            using (SqlConnection connection3 = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectGroupAll", connection3);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", selected_group);
                command.Parameters.AddWithValue("@Id", -1000);
                connection3.Open();
                var reader5 = command.ExecuteReader();
                while (reader5.Read())
                {
                    if (int.Parse(reader5[6].ToString()) == Id) {
                        isLeader = true;
                    } else
                    {
                        isLeader = false;
                    }
                }
            }
            ImageSourceConverter isconv = new ImageSourceConverter();
            ThicknessConverter tconv = new ThicknessConverter();
            group_message_stack.Children.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectAllMessagesByGroup", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", selected_group);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock date = new TextBlock();
                    date.Text = reader[2].ToString();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = reader[0].ToString();
                    Image image = new Image();
                    TextBlock sci_name = new TextBlock();
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        SqlCommand command1 = new SqlCommand("scientistSelectAll", connection1);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;
                        command1.Parameters.AddWithValue("@Id", int.Parse(reader[1].ToString()));
                        connection1.Open();
                        var reader1 = command1.ExecuteReader();                        
                        while (reader1.Read()) {
                            sci_name.Text = reader1[2].ToString() + ":";
                            try
                            {
                                byte[] img_byte = (byte[])reader1[8];
                                image.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                            }
                            catch (InvalidCastException ex)
                            {
                            }
                        }
                    }
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.SetValue(StackPanel.MarginProperty, tconv.ConvertFromString("7"));
                    image.SetValue(Image.MarginProperty, tconv.ConvertFromString("7, 0, 7, 0"));
                    image.Height = 50;
                    image.Width = 50;
                    sci_name.Width = 80;
                    image.Stretch = Stretch.Fill;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    sci_name.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("3, 0, 7, 0"));
                    sci_name.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("3, 0, 0, 0"));
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Width = 480;
                    date.VerticalAlignment = VerticalAlignment.Center;
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(sci_name);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(date);
                    group_message_stack.Children.Add(stackPanel);
                }

            }
        }

        private void Btn_alter_name_Click(object sender, RoutedEventArgs e)
        {
            if (isLeader)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("groupAlter", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", selected_group);
                    command.Parameters.AddWithValue("@New_name", txt_alter_name.Text);
                    command.Parameters.AddWithValue("@Specialization", txt_alter_specialization.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    if (txt_alter_name.Text != "")
                        selected_group = txt_alter_name.Text;
                    txt_alter_specialization.Text = "";
                    txt_alter_name.Text = "";
                }
            } else
            {
                MessageBox.Show("You are not a leader of the group.");
            }
        }

        private void po(object sender, RoutedEventArgs args)
        {
            download_mess();
        }

        private void Btn_alter_image_Click(object sender, RoutedEventArgs e)
        {
            if (isLeader)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|All Files (*.*)|*.*";
                dlg.Title = "Select picture";
                if (dlg.ShowDialog() == true)
                {
                    ImageSourceConverter conv = new ImageSourceConverter();
                    group_img.SetValue(Image.SourceProperty, conv.ConvertFromString(dlg.FileName.ToString()));
                    string img_loc = dlg.FileName;
                    byte[] img = null;
                    FileStream fs = new FileStream(img_loc, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    img = br.ReadBytes((int)fs.Length);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("groupImageAlter", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", selected_group);
                        command.Parameters.AddWithValue("@Image", img);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            } else
            {
                MessageBox.Show("You are not a leader of the group.");
            }
        }

        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sendMessage", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Text", txt_message.Text);
                command.Parameters.AddWithValue("@Group_name", selected_group);
                command.Parameters.AddWithValue("@ID_scientist", Id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                txt_message.Text = "";
                download_mess();
            }
        }

        private void Btn_people_Click(object sender, RoutedEventArgs e)
        {
            group_message_stack.Children.Clear();
            ThicknessConverter tconv = new ThicknessConverter();
            ImageSourceConverter isconv = new ImageSourceConverter();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectScientistByGroup", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", selected_group);
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.SetValue(StackPanel.MarginProperty, tconv.ConvertFromString("7, 7, 7, 0"));
                    stackPanel.Height = 80;
                    stackPanel.Orientation = Orientation.Horizontal;
                    Button button = new Button();
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    button.VerticalAlignment = VerticalAlignment.Center;
                    button.Content = "Kick";
                    button.DataContext = reader[0].ToString();
                    button.Click += btn_kick_Click;
                    TextBlock textBlock = new TextBlock();
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Text = reader[2].ToString() + " ";
                    textBlock.Text += reader[3].ToString();
                    textBlock.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("7, 0, 7, 0"));
                    TextBlock proftext = new TextBlock();
                    proftext.VerticalAlignment = VerticalAlignment.Center;
                    proftext.Text = reader[5].ToString();
                    proftext.Foreground = Brushes.LightSteelBlue;
                    Image image = new Image();
                    image.Height = 70;
                    image.Width = 70;
                    image.Stretch = Stretch.Fill;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    try
                    {
                        byte[] img_byte = (byte[])reader[8];
                        image.SetValue(Image.SourceProperty, isconv.ConvertFrom(img_byte));
                    }
                    catch (InvalidCastException ex)
                    {
                    }
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(proftext);
                    stackPanel.Width = 650;
                    StackPanel stackPanel1 = new StackPanel();
                    stackPanel1.Orientation = Orientation.Horizontal;
                    stackPanel1.Children.Add(stackPanel);
                    stackPanel1.Children.Add(button);
                    group_message_stack.Children.Add(stackPanel1);
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = 2;
                    rectangle.Width = group_message_stack.Width;
                    rectangle.Fill = Brushes.Blue;
                    rectangle.Opacity = 0.2;
                    group_message_stack.Children.Add(rectangle);
                }
                Button button1 = new Button();
                button1.Content = "Close";
                button1.Width = 100;
                button1.HorizontalAlignment = HorizontalAlignment.Center;
                button1.Click += po;
                button1.SetValue(Button.MarginProperty, tconv.ConvertFromString("0, 9, 0, 0"));
                group_message_stack.Children.Add(button1);
            }
        }

        public void btn_kick_Click(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            if (isLeader)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("kickScientistByIdFromGroup", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_scientist", int.Parse(button.DataContext.ToString()));
                    command.Parameters.AddWithValue("@ID_group", selected_group);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    Btn_people_Click(null, null);
                }
            }
            else
            {
                MessageBox.Show("You are not a leader of the group.");
            }
        }
    }
}
