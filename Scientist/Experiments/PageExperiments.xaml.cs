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
using IBSR_Manager_WPF.Scientist;

namespace IBSR_Manager_WPF.Scientist.Experiments
{
    /// <summary>
    /// Логика взаимодействия для PageExperiments.xaml
    /// </summary>
    public partial class PageExperiments : Page
    {
        private PageRecord pageRecord;
        private WindowMenuSci parent;
        private string connectionString = IBSR_Manager_WPF.Properties.Settings.Default.IBSRHConnectionString1;
        private ThicknessConverter tconv = new ThicknessConverter();
        private CornerRadiusConverter crconv = new CornerRadiusConverter();
        private FontWeightConverter fwconv = new FontWeightConverter();

        public PageExperiments(WindowMenuSci wms)
        {
            InitializeComponent();
            this.parent = wms;
            pageRecord = new PageRecord(parent);
            download_exp();
        }

        public void record(Object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            pageRecord.init(int.Parse(button.DataContext.ToString()));
        }

        public void download_exp()
        {
            main_stack.Children.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("selectAllExp", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Border border = new Border();
                    border.SetValue(Border.MarginProperty, tconv.ConvertFromString("15"));
                    border.SetValue(Border.BorderThicknessProperty, tconv.ConvertFromString("2"));
                    border.SetValue(Border.CornerRadiusProperty, crconv.ConvertFromString("8"));
                    border.BorderBrush = Brushes.Blue;
                    StackPanel stackPanel0 = new StackPanel();
                    StackPanel stackPanel1 = new StackPanel();
                    StackPanel stackPanel2 = new StackPanel();
                    StackPanel stackPanel3 = new StackPanel();
                    stackPanel1.SetValue(StackPanel.MarginProperty, tconv.ConvertFromString("10"));
                    stackPanel1.Orientation = Orientation.Horizontal;
                    stackPanel2.Orientation = Orientation.Horizontal;
                    stackPanel3.Orientation = Orientation.Horizontal;
                    stackPanel2.Width = 550;
                    TextBlock title = new TextBlock();
                    TextBlock date = new TextBlock();
                    title.Text = reader[1].ToString();
                    date.Text = reader[2].ToString() + " --- " + reader[3].ToString();
                    title.SetValue(TextBlock.FontWeightProperty, fwconv.ConvertFromString("Bold"));
                    title.FontSize = 20;
                    title.VerticalAlignment = VerticalAlignment.Center;
                    title.Width = 300;
                    date.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("0, 0, 25, 0"));
                    date.VerticalAlignment = VerticalAlignment.Center;
                    stackPanel2.Children.Add(title);
                    stackPanel2.Children.Add(date);
                    Button button = new Button();
                    button.DataContext = reader[0].ToString();
                    button.Click += record;
                    button.Background = Brushes.Blue;
                    button.Width = 130;
                    button.Height = 30;
                    StackPanel stackPanel4 = new StackPanel();
                    stackPanel4.Orientation = Orientation.Horizontal;
                    MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
                    packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Flag;
                    packIcon.SetValue(MaterialDesignThemes.Wpf.PackIcon.MarginProperty, tconv.ConvertFromString("0, 0, 8, 0"));
                    packIcon.Width = 35;
                    packIcon.Height = 35;
                    packIcon.VerticalAlignment = VerticalAlignment.Center;
                    TextBlock buttonText = new TextBlock();
                    buttonText.Text = "Record";
                    buttonText.VerticalAlignment = VerticalAlignment.Center;
                    stackPanel4.Children.Add(packIcon);
                    stackPanel4.Children.Add(buttonText);
                    button.Content = stackPanel4;
                    stackPanel1.Children.Add(stackPanel2);
                    stackPanel1.Children.Add(stackPanel3);
                    stackPanel0.Children.Add(stackPanel1);
                    border.Child = stackPanel0;
                    bool flag = false;
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        SqlCommand command1 = new SqlCommand("selectAllGroupsById", connection1);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;
                        command1.Parameters.AddWithValue("@Id", IBSR_Manager_WPF.Properties.Settings.Default.Id);
                        connection1.Open();
                        TextBlock block = new TextBlock();                      
                        var reader1 = command1.ExecuteReader();
                        while (reader1.Read())
                        {
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                SqlCommand command2 = new SqlCommand("checkScientistInExp", connection2);
                                command2.CommandType = System.Data.CommandType.StoredProcedure;
                                command2.Parameters.AddWithValue("@ID_group", int.Parse(reader1[0].ToString()));
                                command2.Parameters.AddWithValue("@ID_exp", int.Parse(reader[0].ToString()));
                                connection2.Open();
                                var reader2 = command2.ExecuteReader();
                                while (reader2.Read())
                                {
                                    block.SetValue(TextBlock.MarginProperty, tconv.ConvertFromString("13, 0, 0, 10"));
                                    block.Text = reader1[4].ToString() + " takes part in this experiment.";
                                    stackPanel0.Children.Add(block);
                                }
                            }
                        }
                    }
                    stackPanel3.Children.Add(button);
                    main_stack.Children.Add(border);
                }
            }
        }
    }
}
