using CefClient.WebServices;
using CefClient.WebServices.Models;
using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Template.DTO.ViewModels;

namespace CefClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string baseAdress { get; set; } = "http://localhost:10450/";
        private ClientDataCollector collector { get; set; }
        private IApiService _apiService { get; set; }
        public MainWindow()
        {
            var service = new ApiService();
            _apiService = service;
            Cef.Initialize(new CefSettings()
            {
                CachePath = service.CachePath,
                LogFile = service.LogFile
            }, true, true);
            InitializeComponent();
            collector = new ClientDataCollector(_apiService);
            selfHostedBrowser.RegisterJsObject(nameof(ClientDataCollector), collector);
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelfHostedLog.AppendText("hui");
        }

        private async void FreeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var response = await _apiService.CallApi<IEnumerable<SampleEntityViewModel>>("api/values");
        }

        private async void ProtMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var response = await _apiService.CallApi<string[]>("api/prvalues");
        }

        private void ToolsButton_Click(object sender, RoutedEventArgs e)
        {
            selfHostedBrowser.ShowDevTools();
        }

        private void SelHostedRenewButton_Click(object sender, RoutedEventArgs e)
        {
            selfHostedBrowser.Reload();
        }
    }
}
