using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core {
    public class SpeedCounter {

        private class Element{
            public long Time;
            public long Value;
        }

        private List<Element> _lst = new List<Element>();
        private int _MaxCount = 100;
        private long _LastAdd = 0;
        private long _PrevValue = 0;

        public int MaxCount {
            get {
                return _MaxCount;
            }
            set {
                if (value < 10 || value > 1000000)
                    return;
                _MaxCount = value;
            }
        }

        public string GetSpeed() {
            string res = GetSpeedDouble().ToString("F03");
            if (res.IndexOf(',') != -1) {
                res = res.TrimEnd('0').TrimEnd(',');
                if (res == "")
                    res = "0";
            }
            return res;
        }

        public double GetSpeedDouble() {
            double TotalValue = 0;
            double TotalTime = 0;
            for (int i = 1; i < _lst.Count; i++) {
                TotalValue += _lst[i].Value - _lst[i - 1].Value;
                TotalTime += _lst[i].Time - _lst[i - 1].Time;
            }
            TotalTime /= (double)TimeSpan.TicksPerSecond;
            if (TotalTime == 0)
                return 0;
            return TotalValue/TotalTime;
        }

        public void Add(long Value) {
            var CurTime = DateTime.Now.Ticks;
            _PrevValue = Value;
            _lst.Add(new Element() { Time = CurTime, Value = Value });
            _LastAdd = CurTime;
            while (_lst.Count > MaxCount)
                _lst.RemoveAt(0);
        }

        public string Remaining(long CurrentValue, long MaxValue) {
            var speed = GetSpeedDouble();
            if (speed == 0 || Double.IsNaN(speed))
                return "Unknown";
            double Count = MaxValue - CurrentValue;
            Count = Count / speed;
            if (Count > TimeSpan.MaxValue.TotalSeconds)
                return "Unknown";
            TimeSpan ts = TimeSpan.FromSeconds(Count);
            if (ts.Days > 10)
                return string.Format("{0}d", ts.Days);
            else if (ts.Days > 0)
                return string.Format("{0}d {1}h", ts.Days, ts.Hours);
            else if(ts.Hours > 0)
                return string.Format("{0}h {1}m", ts.Hours, ts.Minutes);
            else if(ts.Minutes > 0)
                return string.Format("{0}m {1}s", ts.Minutes, ts.Seconds);
            else
                return string.Format("{0}s", ts.Seconds);
        }

        public void Clear() {
            _lst.Clear();
        }

    }
}
