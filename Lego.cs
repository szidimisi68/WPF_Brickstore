using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Lego
    {
        private string itemID;
        private string itemName;
        private string categoryName;
        private string colorName;
        private string qty;

        public Lego(string itemID, string itemName, string categoryName, string colorName, string qty)
        {
            this.itemID = itemID;
            this.itemName = itemName;
            this.categoryName = categoryName;
            this.colorName = colorName;
            this.qty = qty;
        }

        public string ItemID { get => itemID; }
        public string ItemName { get => itemName; }
        public string CategoryName { get => categoryName; }
        public string ColorName { get => colorName; }
        public string Qty { get => qty; }
    }
}
