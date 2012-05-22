using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ModelConverter.Model;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Diagnostics;

namespace ModelConverter
{
    /// <summary>
    /// Interaktionslogik für WindowAbout.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow(List<IPlugin> Plugins)
        {
            InitializeComponent();
            listViewPlugins.DataContext = Plugins;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            DateTime date = new DateTime(2000, 1, 1)
                .AddDays(version.Build)
                .AddSeconds(version.Revision * 2);
            TextBlockVersion.Content = "Version: " + date.ToShortDateString();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://creativecommons.org/licenses/by-nc-sa/3.0/de/");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/CitySim/ModelConverter");
        }
    }
}