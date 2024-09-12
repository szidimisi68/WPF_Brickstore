using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
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
        List<string> fajlok = new();
        public MainWindow()
        {
            InitializeComponent();
            dgTablazat.ItemsSource = legok;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            legok.Clear();
            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.Multiselect = false;
            //ofd.Filter = "BSX files (*.bsx)|*.bsx";
            if (ofd.ShowDialog() == true)
            {
                fajlok = Directory.GetFiles(ofd.FolderName).ToList();
            }
            lbFolder.Content = ofd.FolderName.ToString();
            lbxList.ItemsSource = fajlok.Select(x=> x.Remove(0, ofd.FolderName.Length+1)).ToList();
        }

        private void Betolt(string neve)
        {
            FilterReset();
            legok.Clear();
            XDocument xaml = XDocument.Load(neve);
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
            cbKategoriak.SelectedIndex = kategoriak.Count() - 1;
            filtered = legok;
        }
        private void FilterReset()
        {
            tbAzon.Text = "";
            tbNev.Text = "";
            cbKategoriak.SelectedItem = "none";
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

            kategoriak.Clear();
            kategoriak = filtered.Select(x => x.CategoryName).Distinct().ToList();
            kategoriak.Add("none");
            cbKategoriak.ItemsSource = kategoriak;

            if (cbKategoriak.SelectedIndex != kategoriak.Count() - 1)
            {
                filtered = new ObservableCollection<Lego>(filtered.Where(x => x.CategoryName == cbKategoriak.SelectedItem));
            }

            dgTablazat.ItemsSource = filtered;

            lbElemek.Content = filtered.Count();
        }

        private void tbNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filters();            
        }

        private void cbKategoriak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filters();
        }

        private void lbxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Betolt(lbFolder.Content.ToString() + "\\" + lbxList.SelectedItem.ToString());
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            FilterReset();
        }
    }
}