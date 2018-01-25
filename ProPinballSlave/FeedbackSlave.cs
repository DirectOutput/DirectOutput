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
			DirectOutputHandler.Init("ProPinball", "Timeshock", "Timeshock");

			CreateBridge();
			unsafe
			{
				_bridge.GetFeedback((flasherId, flasherName, flasherIntensity) => {
					if (flasherIntensity > 0.8) {
						DirectOutputHandler.UpdateTableElement(TableElementTypeEnum.Lamp.ToString(), flasherId, 1);
					}
					if (flasherIntensity < 0.1) {
						DirectOutputHandler.UpdateTableElement(TableElementTypeEnum.Lamp.ToString(), flasherId, 0);
					}
					Console.WriteLine("{0} | Flasher {1} ({2}): {3}", DateTime.Now, flasherId, new string(flasherName), flasherIntensity);

				}, (solenoidId, solenoidName, solenoidStatus) => {
					DirectOutputHandler.UpdateTableElement(TableElementTypeEnum.Solenoid.ToString(), solenoidId, solenoidStatus);
					Console.WriteLine("{0} | Solenoid {1} ({2}): {3}", DateTime.Now, solenoidId, new string(solenoidName), solenoidStatus);

				}, (flipperId, flipperName, flipperStatus) => {
					DirectOutputHandler.UpdateTableElement(TableElementTypeEnum.Solenoid.ToString(), flipperId + 128, flipperStatus);
					Console.WriteLine("{0} | Flipper {1} ({2}): {3}", DateTime.Now, flipperId, new string(flipperName), flipperStatus);

				}, (buttonId, buttonName, buttonStatus) => {
					DirectOutputHandler.UpdateTableElement(TableElementTypeEnum.LED.ToString(), buttonId, buttonStatus);
					Console.WriteLine("{0} | Button {1} ({2}): {3}", DateTime.Now, buttonId, new string(buttonName), buttonStatus);

				}, msg => {
					Console.WriteLine("ERROR: {0}", new string(msg));
					DirectOutputHandler.Finish();

				}, () => {
					Console.WriteLine("All done!");
					DirectOutputHandler.Finish();

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
				Console.WriteLine(e.StackTrace);
				if (e.InnerException != null)
				{
					Console.WriteLine("-------------------------------------------------------------------------------");
					Console.WriteLine(e.InnerException.Message);
					Console.WriteLine(e.InnerException.StackTrace);
				}
			}
		}
	}
}
