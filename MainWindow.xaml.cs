using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CrawlData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties
        ObservableCollection<MenuTreeItem> TreeItems;
        string HomePage = "https://www.howkteam.com/";

        HttpClient httpClient;
        HttpClientHandler handler;
        CookieContainer cookie = new CookieContainer();


        #endregion
        public MainWindow()
        {
            InitializeComponent();

            IniHttpClient();            

            TreeItems = new ObservableCollection<MenuTreeItem>();            
            treeMain.ItemsSource = TreeItems;
        }

        #region methods
        void IniHttpClient()
        {
            handler = new HttpClientHandler
            {
                CookieContainer = cookie,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true,
                UseDefaultCredentials = false
            };

            httpClient = new HttpClient(handler);

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) coc_coc_browser/63.4.154 Chrome/57.4.2987.154 Safari/537.36");
            /*
             * Header:
             * - Origin
             * - Host
             * - Referer
             * - :scheme
             * - accept
             * - Accept-Encoding
             * - Accept-Language
             * - User-Argent
             */


            httpClient.BaseAddress = new Uri(HomePage);
        }
        void AddItemIntoTreeViewItem(ObservableCollection<MenuTreeItem> root, MenuTreeItem node)
        {
            treeMain.Dispatcher.Invoke(new Action(()=> {
                root.Add(node);
            }));
        }

        string CrawlDataFromURL(string url)
        {
            string html = "";
            
            html = httpClient.GetStringAsync(url).Result;

            //html = httpClient.PostAsync(url,new StringContent("")).Result.Content.ReadAsStringAsync().Result;

            return html;
        }

        void Crawl(string url)
        {
            string htmlLearn = CrawlDataFromURL(url);
            var CourseList = Regex.Matches(htmlLearn, @"<div class=""info-course(.*?)</div>", RegexOptions.Singleline);
            foreach (var course in CourseList)
            {
                string courseName = Regex.Match(course.ToString(), @"(?=<h5>).*?(?=</h5>)").Value.Replace("<h5>","");
                string linkCourse = Regex.Match(course.ToString(), @"'(.*?)'", RegexOptions.Singleline).Value.Replace("'","");

                MenuTreeItem item = new MenuTreeItem();
                item.Name = courseName;
                item.URL = linkCourse;

                AddItemIntoTreeViewItem(TreeItems, item);

                string htmlCourse = CrawlDataFromURL(linkCourse);
                string sideBar = Regex.Match(htmlCourse, @"<div class=""sidebardetail"">(.*?)</ul>", RegexOptions.Singleline).Value;
                var listLecture = Regex.Matches(sideBar, @"<li(.*?)</li>", RegexOptions.Singleline);
                foreach (var lecture in listLecture)
                {
                    string lectureName = Regex.Match(lecture.ToString(), @"<li title=""(.*?)"">", RegexOptions.Singleline).Value.Replace("\">", "").Replace("<li title=\"", "");
                    string linkLecture = Regex.Match(lecture.ToString(), @"<a href=""(.*?)"">", RegexOptions.Singleline).Value.Replace("<a href=\"", "").Replace("\">","");

                    MenuTreeItem Subitem = new MenuTreeItem();
                    Subitem.Name = lectureName;
                    Subitem.URL = linkLecture;
                    item.Items.Add(Subitem);
                }
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuTreeItem item3 = new MenuTreeItem()
            {
                Name = "storesTab",
                URL = "https://concung.com/tim-sieu-thi.html",
                Items = new ObservableCollection<MenuTreeItem>()
                {
                    new MenuTreeItem() { Name = "shop-list show_find_store shop_list_croll", URL="https://concung.com/tim-sieu-thi.html"};
                }
            };

            AddItemIntoTreeViewItem(TreeItems[0].Items, item3);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Crawl("Learn");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string url =HomePage + (sender as Button).Tag.ToString();
            //wbMain.Navigate(url);
            Process.Start(url);
        }
    }
}
