using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Data {
    public interface IDataSaver : IDisposable {
        void AddData(string Format, params string[] args);
        void SetSettings(string Settings);
    }
}
