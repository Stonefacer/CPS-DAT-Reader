using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ext.System.Windows.Forms {
    public class LinesIterator {

        private Func<string> GetSource;
        private int _StartId = 0;
        private string _Source = "";

        public int ID { get; private set; }
        public string Current { get; private set; }

        public LinesIterator(string str) {
            GetSource = () => str;
            Reset();
        }

        public LinesIterator(TextBox txt):this(txt.Text) {

        }

        public void Reset() {
            ID = 0;
            _StartId = 0;
            Current = null;
            _Source = GetSource();
        }

        public bool Next() {
            if (_StartId >= _Source.Length)
                return false;
            int id = _Source.IndexOf("\r\n", _StartId);
            if(id==-1){
                Current = _Source.Substring(_StartId);
                _StartId = _Source.Length;
            } else {
                Current = _Source.Substring(_StartId, id - _StartId);
                _StartId = id + 2;
            }
            return true;
        }

    }
}
