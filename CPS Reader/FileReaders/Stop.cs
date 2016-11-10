using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_Reader.FileReaders {
    class Stop {

        public int id0;
        public int id1;
        public StopType Type;

        public Stop(int id0, int id1, StopType Type) {
            this.id0 = id0;
            this.id1 = id1;
            this.Type = Type;
        }

        public virtual object GetAdditionalInfo() {
            return null;
        }

    }
}
