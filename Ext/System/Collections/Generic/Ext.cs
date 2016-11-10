using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Collections.Generic {
    public static class Ext {

        public static T GetLast<T>(this List<T> src) where T :class {
            if (src.Count < 1)
                return null;
            return src[src.Count - 1];
        }

        public static TT GetLast<TT, T>(this List<T> src) where T : class where TT :class {
            int id = 1;
            TT Res = null;
            while(src.Count - id >= 0) {
                Res = src[src.Count - id] as TT;
                if (Res != null)
                    break;
            }
            return Res;
        }

        public static void AddOrDoWork<T>(this List<T> src, T Itm, Action<T> Work) {
            if (src.Contains(Itm))
                Work(Itm);
            else
                src.Add(Itm);
        }

    }
}
