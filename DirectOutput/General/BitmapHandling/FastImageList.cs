using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DirectOutput.General.Generic;

namespace DirectOutput.General.BitmapHandling
{
    public class FastImageList : NamedItemList<FastImage>
    {
        public new FastImage this[string Name]
        {
            get
            {
                try
                {
                    return base[Name];
                }
                catch
                {
                    if (!DontAddIfMissing)
                    {
                        try
                        {
                            FastImage F = new FastImage(Name, UseCache);
                            Add(F);
                            return F;
                        }
                        catch (Exception E)
                        {
                            throw new Exception("Could not add file {0} to the FastBitmapList.", E);
                        }
                    }
                    else
                    {
                        throw; 
                    }
                }
            }
        }

        private Boolean _DontAddIfMissing=false;

        public Boolean DontAddIfMissing
        {
            get { return _DontAddIfMissing; }
            set { _DontAddIfMissing = value; }
        }

        private bool _UseCache = false;

        public Boolean UseCache
        {
            get { return _UseCache; }
            set { _UseCache = value; }
        }
    }
}
