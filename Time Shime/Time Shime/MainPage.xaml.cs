using System;
using Time_Shime.Background;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Time_Shime {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page {

		public Library library = new Library();

		public MainPage() {
			this.InitializeComponent();
			PackageVersion number = Package.Current.Id.Version; // Get app version
			version.Text += string.Format(" {0}.{1}.{2}\r\n", number.Major, number.Minor, number.Build);

			if (!library.started) {
				start();
				TileUpdateTask.doTask();
				symbol.Symbol = Symbol.Accept;
				status.Text = "App is running.\nGo ahead and pin this to Start menu.";
			}

			if (library.started) {
				symbol.Symbol = Symbol.Accept;
				status.Text = "App is running.\nGo ahead and pin this to Start menu.";
			}
		}

		private async void start() {
			await library.start();
		}

		private void ShareAppButton_Click(object sender, RoutedEventArgs e) {
			DataTransferManager.ShowShareUI();
			DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
		}

		private void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
			args.Request.Data.SetText("Hi, try the new Time Shime app from the store at https://www.microsoft.com/store/apps/9njz27vw06tq");
			args.Request.Data.Properties.Title = Package.Current.DisplayName;
		}

		private async void Twitter_Click(object sender, RoutedEventArgs e) {
			var url = new Uri("https://twitter.com/satyendraXV");
			await Windows.System.Launcher.LaunchUriAsync(url);
		}

		private async void Facebook_Click(object sender, RoutedEventArgs e) {
			var url = new Uri("https://www.facebook.com/satyendra1529singh");
			await Windows.System.Launcher.LaunchUriAsync(url);
		}

		private async void Linkedin_Click(object sender, RoutedEventArgs e) {
			var url = new Uri("https://www.linkedin.com/in/satyendra-singh-36785a122");
			await Windows.System.Launcher.LaunchUriAsync(url);
		}
	}
}