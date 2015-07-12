using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new Database2Entities())
            {
                var other_teams = db.Teams.Where(t => t.PictureUrl == "Some URL" && t.Name.EndsWith("A"));
                db.Teams.Select(t => t.Id).FirstOrDefault(id => id == new Guid())
                var teams = from team in db.Teams
                            where team.PictureUrl == "Some URL"
                            select new
                            {
                                Name = team.Name,
                                URL = team.PictureUrl
                            };
//                this.DataContext = teams.ToList();
                this.DataContext = other_teams.ToList();
            }
        }
    }
}
