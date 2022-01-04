using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlDataFromHowKteam
{
    public class MenuTreeItem
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public ObservableCollection<MenuTreeItem> Items { get; set; }
        public MenuTreeItem()
        {
            this.Items = new ObservableCollection<MenuTreeItem>();
        }
    }
}
