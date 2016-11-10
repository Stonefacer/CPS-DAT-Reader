using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core {
    public static class AdvMath {
        public static int GCD(int a, int b) {
            if (a == 0 || b == 0)
                throw new Exception("#0001");
            int d = 0;
            while ((a & 1) == 0 && (b & 1) == 0) {
                a >>= 1;
                b >>= 1;
                d++;
            }
            //while a ≠ b do
            //if a is even then a := a/2
            //else if b is even then b := b/2
            //else if a > b then a := (a – b)/2
            //else b := (b – a)/2
            while (a != b) {
                if ((a & 1) == 0)
                    a >>= 1;
                else if ((b & 1) == 0)
                    b >>= 1;
                else if (a > b)
                    a = (a - b) / 2;
                else
                    b = (b - a) / 2;
            }
            return (1 << d)*a;
        }

        public static int LCM(int a, int b) {
            return a * b / GCD(a, b);
        }
    }
}
