using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectOutput;

namespace ProPinballSlave
{
	public class FeedbackSlave
	{
		private ProPinballBridge.ProPinballFeedback _bridge;

		public void Start()
		{
			// init DOF
			var dofHome = Environment.GetEnvironmentVariable("DOF_HOME");
			if (dofHome == null) {
				throw new Exception("Please set the DOF_HOME environment variable to the folder where your DirectOutput \"config\" folder is located.");
			}
			var dofConfig = Path.Combine(dofHome, "config", "GlobalConfig_Dmdext.xml");
			if (!File.Exists(dofConfig)) {
				throw new Exception($"Could not find \"{dofConfig}\". Are you sure your DOF_HOME environment variable is pointing to the right folder?");
			}
			var pinball = new Pinball();
			pinball.Setup(dofConfig, null, "Timeshock");
			pinball.Init();

			CreateBridge();
			unsafe
			{
				_bridge.GetFeedback((flasherId, flasherName, flasherIntensity) => {
					pinball.ReceiveData(Convert.ToChar(TableElementTypeEnum.Lamp), flasherId, flasherIntensity > 0.5 ? 1 : 0);
					Console.WriteLine("Flasher {0} ({1}): {2}", flasherId, new string(flasherName), flasherIntensity);

				}, (solenoidId, solenoidName, solenoidStatus) => {
					pinball.ReceiveData(Convert.ToChar(TableElementTypeEnum.Solenoid), solenoidId, solenoidStatus);
					Console.WriteLine("Solenoid {0} ({1}): {2}", solenoidId, new string(solenoidName), solenoidStatus);

				}, (flipperId, flipperName, flipperStatus) => {
					pinball.ReceiveData(Convert.ToChar(TableElementTypeEnum.Solenoid), flipperId + 128, flipperStatus);
					Console.WriteLine("Flipper {0} ({1}): {2}", flipperId, new string(flipperName), flipperStatus);

				}, (buttonId, buttonName, buttonStatus) => {
					pinball.ReceiveData(Convert.ToChar(TableElementTypeEnum.LED), buttonId, buttonStatus);
					Console.WriteLine("Button {0} ({1}): {2}", buttonId, new string(buttonName), buttonStatus);

				}, msg => {
					Console.WriteLine("ERROR: {0}", new string(msg));

				}, () => {
					Console.WriteLine("All done!");

				});
			}
		}

		private void CreateBridge()
		{
			_bridge = new ProPinballBridge.ProPinballFeedback(392);
			if (_bridge.Status != 0) {
				unsafe
				{
					throw new Exception("Error connecting: " + new string(_bridge.Error));
				}
			}
		}

		static void Main(string[] args)
		{
			try {
				new FeedbackSlave().Start();

			} catch (Exception e) {
				Console.WriteLine("Error: {0}", e.Message);
			}
		}
	}
}
