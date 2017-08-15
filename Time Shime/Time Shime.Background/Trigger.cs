using System;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.System.UserProfile;
using Windows.UI.Notifications;

namespace Time_Shime.Background {

    public sealed class Trigger : IBackgroundTask {

        public void Run(IBackgroundTaskInstance taskInstance) {
            var defferal = taskInstance.GetDeferral();
			TileUpdateTask.doTask();
            defferal.Complete();
        }
    }
}
