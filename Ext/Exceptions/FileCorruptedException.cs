using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Exceptions {


    [Serializable]
    public class FileCorruptedException : Exception {

        public string Path { get; private set; }
        public string Reason { get; private set; }

        public FileCorruptedException() {
            Path = "<Unknown>";
            Reason = "";
        }

        public FileCorruptedException(String Path) {
            this.Path = Path;
        }

        public FileCorruptedException(String Path, String Reason) {
            this.Path = Path;
            this.Reason = Reason;
        }

    }
}
