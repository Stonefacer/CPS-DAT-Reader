using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ext.System.Windows.Forms {

    public static class Ext {

        public static ListViewItem Add(this ListView.ListViewItemCollection src, params object[] Values) {
            ListViewItem lvi = new ListViewItem(Values.Select(x => x.ToString()).ToArray());
            src.Add(lvi);
            return lvi;
        }

        public static void ScrollToEnd(this TextBox txt) {
            txt.SelectionStart = txt.TextLength;
            txt.ScrollToCaret();
        }

        public static int GetRowsCount(this TextBox txt) {
            int id = txt.Text.IndexOf("\r\n");
            int count = 1;
            while(id!=-1&&id<txt.Text.Length){
                id = txt.Text.IndexOf("\r\n", id+2);
                count++;
            }
            return count;
        }

    }

}
