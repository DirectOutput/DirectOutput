using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPinballSlave
{
	interface IDirectOutputCom
	{

		string GetVersion();
		string GetName();
		string GetDllPath();

		void Finish();

		void UpdateTableElement(string tableElementTypeChar, int number, int value);
		void UpdateNamedTableElement(string tableElementName, int value);
		void Init(string hostingApplicationName, string tableFileName = "", string gameName = "");

		void ShowFrontend();

		string GetConfiguredTableElmentDescriptors();
		string TableMappingFileName();
	}
}
