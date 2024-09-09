using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Lego> legok = new();
        List<string> kategoriak = new();
        ObservableCollection<Lego> filtered = new();
        public MainWindow()
        {
            InitializeComponent();
            dgTablazat.ItemsSource = legok;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            legok.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BSX files (*.bsx)|*.bsx";
            if (ofd.ShowDialog() == true)
            {
                XDocument xaml = XDocument.Load(ofd.FileName);
                foreach (var elem in xaml.Descendants("Item"))
                {
                    string id = elem.Element("ItemID").Value;
                    string nev = elem.Element("ItemName").Value;
                    string category = elem.Element("CategoryName").Value;
                    string color = elem.Element("ColorName").Value;
                    string qt = elem.Element("Qty").Value;

                    legok.Add(new Lego(id, nev, category, color, qt));
                }
                dgTablazat.ItemsSource = legok;
                lbElemek.Content = legok.Count();
                kategoriak = legok.Select(x => x.CategoryName).Distinct().ToList();
                kategoriak.Add("none");
                cbKategoriak.ItemsSource = kategoriak;
                cbKategoriak.SelectedIndex = kategoriak.Count()-1;
                filtered = legok;
               
            }
        }

        private void Filters()
        {
            filtered = legok as ObservableCollection<Lego>;
            if (tbNev.Text != "")
            {
                filtered = new ObservableCollection<Lego>(filtered.Where(x => x.ItemName.ToLower().StartsWith(tbNev.Text.ToLower())));
            }
            if (tbAzon.Text != "")
            {
                filtered = new ObservableCollection<Lego>(filtered.Where(x => x.ItemID.ToLower().StartsWith(tbAzon.Text.ToLower())));
            }

            if (cbKategoriak.SelectedIndex != kategoriak.Count() - 1)
            {
                filtered = new ObservableCollection<Lego>(filtered.Where(x => x.CategoryName == cbKategoriak.SelectedItem));
            }

            dgTablazat.ItemsSource = filtered;

            lbElemek.Content = filtered.Count();
            kategoriak.Clear();
            kategoriak = legok.Select(x => x.CategoryName).Distinct().ToList();
            kategoriak.Add("none");
            cbKategoriak.ItemsSource = kategoriak;
        }

        private void tbNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filters();            
        }

        private void cbKategoriak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filters();
        }
    }
}