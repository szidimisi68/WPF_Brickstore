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
        ObservableCollection<Lego> legok = new ObservableCollection<Lego>();
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
            }
        }


        private void tbNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Lego> filtered = new();
            if (tbNev.Text != "" && tbAzon.Text == "")
            {
                legok.Where(x => x.ItemName.ToLower().StartsWith(tbNev.Text.ToLower())).ToList().ForEach(x=> filtered.Add(x)) ;
            }
            else if (tbAzon.Text != "" && tbNev.Text == "")
            {
                legok.Where(x => x.ItemID.ToLower().StartsWith(tbAzon.Text.ToLower())).ToList().ForEach(x => filtered.Add(x));
            }
            else
            {
                legok.Where(x => x.ItemID.ToLower().StartsWith(tbAzon.Text.ToLower()) && x.ItemName.ToLower().StartsWith(tbNev.Text.ToLower())).ToList().ForEach(x => filtered.Add(x));
            }
            dgTablazat.ItemsSource = filtered;

            lbElemek.Content = filtered.Count();
        }
    }
}