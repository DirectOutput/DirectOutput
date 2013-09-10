using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.GlobalConfiguration;

namespace LedControlConverter.Settings
{
   public class ConverterSettings
    {
       public LedControlIniFileList InputFiles = new LedControlIniFileList();

       public List<string> ConfigsToConvert = new List<string>();

       public CabinetConfigModeEnum CabinetConfigMode = 0;
       public string NewCabinetConfigFilename = "";
       public string ExistingCabinetConfigFilename = "";
       public bool AutoDetectOutputControllers=false;
       


    }
}
