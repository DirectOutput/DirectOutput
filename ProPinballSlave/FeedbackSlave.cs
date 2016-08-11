using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPinballSlave
{
	public class FeedbackSlave
	{

		private IObservable<string> _data;
		private ProPinballBridge.ProPinballFeedback _bridge;

		public void Start()
		{
			CreateBridge();
			unsafe
			{
				_bridge.GetFeedback(msg => {
					Console.WriteLine(new string(msg));

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
