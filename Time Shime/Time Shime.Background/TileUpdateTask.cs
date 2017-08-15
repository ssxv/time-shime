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
                                        <binding template=""TileSquareText04""><text id=""1"">{0}</text></binding>
                                        <binding template=""TileWideText09""><text id=""1"">{0}</text><text id=""2"">{1}</text></binding>
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
			string hour = getText(dateTime.Hour);
			string minute = (dateTime.Minute == 0) ? "zero zero" : getText(dateTime.Minute);
			return hour + ", " + minute;
		}

		/**
         * Method to get the Text of an integer. Very specific to this app.
         */
		private static string getText(int n) {
			switch (n) {
				case 0: return "twelve";
				case 1: return "one";
				case 2: return "two";
				case 3: return "three";
				case 4: return "four";
				case 5: return "five";
				case 6: return "six";
				case 7: return "seven";
				case 8: return "eight";
				case 9: return "nine";
				case 10: return "ten";
				case 11: return "eleven";
				case 12: return "twelve";
				case 13: return "thirteen";
				case 14: return "fourteen";
				case 15: return "fifteen";
				case 16: return "sixteen";
				case 17: return "seventeen";
				case 18: return "eighteen";
				case 19: return "ninteen";
				case 20: return "twenty";
				case 21: return "twenty one";
				case 22: return "twenty two";
				case 23: return "twenty three";
				case 24: return "twenty four";
				case 25: return "twenty five";
				case 26: return "twenty six";
				case 27: return "twenty seven";
				case 28: return "twenty eight";
				case 29: return "twenty nine";
				case 30: return "thirty";
				case 31: return "thirty one";
				case 32: return "thirty two";
				case 33: return "thirty three";
				case 34: return "thirty four";
				case 35: return "thirty five";
				case 36: return "thirty six";
				case 37: return "thirty seven";
				case 38: return "thirty eight";
				case 39: return "thirty nine";
				case 40: return "forty";
				case 41: return "forty one";
				case 42: return "forty two";
				case 43: return "forty three";
				case 44: return "forty four";
				case 45: return "forty five";
				case 46: return "forty six";
				case 47: return "forty seven";
				case 48: return "forty eight";
				case 49: return "forty nine";
				case 50: return "fifty";
				case 51: return "fifty one";
				case 52: return "fifty two";
				case 53: return "fifty three";
				case 54: return "fifty four";
				case 55: return "fifty five";
				case 56: return "fifty six";
				case 57: return "fifty seven";
				case 58: return "fifty eight";
				case 59: return "fifty nine";
				default: return string.Empty;
			}
		}
	}
}