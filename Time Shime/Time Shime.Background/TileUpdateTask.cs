using System;
using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.System.UserProfile;
using Windows.UI.Notifications;
using System.Linq;

namespace Time_Shime.Background {

	public sealed class TileUpdateTask {

		public static void doToastTask() {
			const string xml = @"<toast><visual version=""2"">
                                        <binding template=""ToastText02""><text id=""1"">Time Zone Changed</text><text id=""2"">By Background Task</text></binding>
                                   </visual></toast>";

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(xmlDocument));
		}

		public static void doTask() {
			var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
			var plannedUpdates = tileUpdater.GetScheduledTileNotifications();

			string language = GlobalizationPreferences.Languages.First();
			CultureInfo cultureInfo = new CultureInfo(language);

			DateTime now = DateTime.Now;
			DateTime planTill = now.AddHours(1);

			DateTime updateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);
			if (plannedUpdates.Count > 0)
				updateTime = plannedUpdates.Select(x => x.DeliveryTime.DateTime).Union(new[] { updateTime }).Max();

			// xml for Tile layout : https://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
			const string xml = @"<tile><visual>
                                        <binding template=""TileMedium""><text hint-style=""subtitle"" hint-wrap=""true"" id=""1"">{0}</text></binding>
                                        <binding template=""TileWide""><text hint-style=""title"" hint-wrap=""true"" id=""1"">{0}</text><text id=""2"">{1}</text></binding>
                                   </visual></tile>";

			string textTime = getTextTime(now);
			var tileXmlNow = string.Format(xml, textTime, now.ToString(cultureInfo.DateTimeFormat.LongDatePattern));
			XmlDocument documentNow = new XmlDocument();
			documentNow.LoadXml(tileXmlNow);

			tileUpdater.Update(new TileNotification(documentNow) { ExpirationTime = now.AddMinutes(1) });

			// for next 1 hour Schedule the notifications.
			for (var startPlanning = updateTime; startPlanning < planTill; startPlanning = startPlanning.AddMinutes(1)) {
				try {
					textTime = getTextTime(startPlanning);
					var tileXml = string.Format(xml, textTime, startPlanning.ToString(cultureInfo.DateTimeFormat.LongDatePattern));
					XmlDocument document = new XmlDocument();
					document.LoadXml(tileXml);

					ScheduledTileNotification scheduledNotification = new ScheduledTileNotification(document, new DateTimeOffset(startPlanning)) { ExpirationTime = startPlanning.AddMinutes(1) };
					tileUpdater.AddToSchedule(scheduledNotification);

				} catch (Exception e) {
					// do nothing
				}
			}
		}

		/**
         * Method to get the Text of a given time
         */
		private static string getTextTime(DateTime dateTime) {
			string hour = getText(dateTime.Hour % 12);
			string minute = (dateTime.Minute == 0) ? "Zero Zero" : getText(dateTime.Minute);
			return hour + ", " + minute;
		}

		/**
         * Method to get the Text of an integer. Very specific to this app.
         */
		private static string getText(int n) {
			switch (n) {
				case 0: return "Twelve";
				case 1: return "One";
				case 2: return "Two";
				case 3: return "Three";
				case 4: return "Four";
				case 5: return "Five";
				case 6: return "Six";
				case 7: return "Seven";
				case 8: return "Eight";
				case 9: return "Nine";
				case 10: return "Ten";
				case 11: return "Eleven";
				case 12: return "Twelve";
				case 13: return "Thirteen";
				case 14: return "Fourteen";
				case 15: return "Fifteen";
				case 16: return "Sixteen";
				case 17: return "Seventeen";
				case 18: return "Eighteen";
				case 19: return "Ninteen";
				case 20: return "Twenty";
				case 21: return "Twenty One";
				case 22: return "Twenty Two";
				case 23: return "Twenty Three";
				case 24: return "Twenty Four";
				case 25: return "Twenty Five";
				case 26: return "Twenty Six";
				case 27: return "Twenty Seven";
				case 28: return "Twenty Eight";
				case 29: return "Twenty Nine";
				case 30: return "Thirty";
				case 31: return "Thirty One";
				case 32: return "Thirty Two";
				case 33: return "Thirty Three";
				case 34: return "Thirty Four";
				case 35: return "Thirty Five";
				case 36: return "Thirty Six";
				case 37: return "Thirty Seven";
				case 38: return "Thirty Eight";
				case 39: return "Thirty Nine";
				case 40: return "Forty";
				case 41: return "Forty One";
				case 42: return "Forty Two";
				case 43: return "Forty Three";
				case 44: return "Forty Four";
				case 45: return "Forty Five";
				case 46: return "Forty Six";
				case 47: return "Forty Seven";
				case 48: return "Forty Eight";
				case 49: return "Forty Nine";
				case 50: return "Fifty";
				case 51: return "Fifty One";
				case 52: return "Fifty Two";
				case 53: return "Fifty Three";
				case 54: return "Fifty Four";
				case 55: return "Fifty Five";
				case 56: return "Fifty Six";
				case 57: return "Fifty Seven";
				case 58: return "Fifty Eight";
				case 59: return "Fifty Nine";
				default: return string.Empty;
			}
		}
	}
}