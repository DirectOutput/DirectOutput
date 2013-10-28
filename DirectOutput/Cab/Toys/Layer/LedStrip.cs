using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out.AdressableLedStrip;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys.Layer
{
    public class LedStrip : ToyBaseUpdatable, IToy
    {

        public int NumberOfLeds { get; set; }

        public int FirstLedNumber { get; set; }

        public string OutputControllerName { get; set; }



        private WS2811StripController OutputController;


        [XmlIgnore]
        public LedStripLayerDictionary Layers { get; private set; }

        Cabinet Cabinet;
        public override void Init(Cabinet Cabinet)
        {
            this.Cabinet = Cabinet;

            if (Cabinet.OutputControllers.Contains(OutputControllerName) && Cabinet.OutputControllers[OutputControllerName] is WS2811StripController)
            {
                OutputController = (WS2811StripController)Cabinet.OutputControllers[OutputControllerName];
            }
        }


        public LedStrip()
        {
            Layers = new LedStripLayerDictionary();

        }


        public override void UpdateOutputs()
        {
            if (OutputController != null)
            {
                OutputController.SetRGBValues(FirstLedNumber, Layers.GetResultingValue());

            };
        }

        public override void Reset()
        {
            OutputController.SetRGBValues(FirstLedNumber, new int[NumberOfLeds, 3]);
        }
    }
}
