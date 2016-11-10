using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Data {
    public static class DataSaverFactory {
        public static IDataSaver Instance(string FileName) {
            if(FileName.Length < 5)
                throw new ArgumentOutOfRangeException();
            if(FileName.EndsWith(".txt"))
                return new PlainTextDataSaver(FileName);
            throw new ArgumentException("Output file format is not supported");
        }
    }
}
