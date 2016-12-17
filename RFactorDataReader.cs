using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace iDash
{
    public abstract class RFactorDataReader
    {  
        protected abstract Boolean InitialiseInternal();

        public abstract Object ReadGameData();
                
        public abstract void Dispose();

        public Boolean Initialise()
        {
            Boolean initialised = InitialiseInternal();

            return initialised;
        }        

        public virtual void stop()
        {
            // no op - only implemented by UDP reader
        }
    }
}
