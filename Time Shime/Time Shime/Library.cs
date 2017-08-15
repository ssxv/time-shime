using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using System;

public class Library {

	public bool started {
		get {
			return BackgroundTaskRegistration.AllTasks.Count > 0;
		}
	}
	
	public async Task start() {
		if (!started) {
			try {
				await BackgroundExecutionManager.RequestAccessAsync();
				BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
				builder.Name = typeof(Time_Shime.Background.Trigger).FullName;
				builder.SetTrigger(new TimeTrigger(60, false));
				builder.TaskEntryPoint = builder.Name;
				builder.Register();
			} catch {}
		}
	}
}