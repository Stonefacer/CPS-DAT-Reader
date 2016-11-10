using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Ext.System.Core;

namespace CPS_Reader.FileReaders {
    static class Decoders {
        public static void ThreeBitLeadOne(byte[] Data, VoicesGroup Group, List<Stop> Result) {
            string str1 = string.Join("", Enumerable.ToArray<string>(Enumerable.Select<byte, string>(Enumerable.Take<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)Data, Group.Offset), Group.Len), (Func<byte, string>)(x => Convert.ToString(x, 2).PadLeft(8, '0')))));
#if DEBUG
            string str2 = string.Join(" ", Enumerable.ToArray<string>(Enumerable.Select<byte, string>(Enumerable.Take<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)Data, Group.Offset), 64), (Func<byte, string>)(x => Convert.ToString(x, 16).PadLeft(2, '0')))));
            Trace.WriteLine(string.Format("{0}: {1}", Group.Type.ToString(), str2));
#endif
            int sid = 0;
            while(sid < str1.Length - 2) {
                if(str1[sid] == '0') {
                    sid += 3;
                    continue;
                }
                Result.Add(new Stop(sid / 3, str1.Substring(sid + 1, 2).ToIntEx(2), Group.Type));
                sid += 3;
            }
        }

        public static void BitMask(byte[] Data, VoicesGroup Group, List<Stop> Result) {
            string str = string.Join("", Enumerable.ToArray<string>(Enumerable.Select<byte, string>(Enumerable.Take<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)Data, Group.Offset), Group.Len), (Func<byte, string>)(x => Convert.ToString(x, 2).PadLeft(8, '0')))));
            int id = str.IndexOf('1', 0);
            while(id != -1) {
                if(id/3!=2)
                    Result.Add(new Stop(id / 3, id % 3, StopType.CP));
                id = str.IndexOf('1', id+1);
            }
            //List<string> list = new List<string>();
            //string str = string.Join("", Enumerable.ToArray<string>(Enumerable.Select<byte, string>(Enumerable.Take<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)Data, Group.Offset), Group.Len), (Func<byte, string>)(x => Convert.ToString(x, 2).PadLeft(8, '0')))));
            //foreach(KeyValuePair<int, string[]> keyValuePair in Group.Table) {
            //    int num = (keyValuePair.Key - 1) * 3;
            //    for(int index = 0; index < 3; ++index) {
            //        if((int)str[num + index] == 49)
            //            list.Add(keyValuePair.Value[index]);
            //    }
            //}
        }

        public static void StructureA(byte[] Data, VoicesGroup Group, List<Stop> Result) {
            int num = BitConverter.ToInt32(Data, Group.Offset) >> 4;
            if((num & 15) == 3) {
                int key = ((int)Data[Group.Offset + 4] & 240) >> 4 | ((int)Data[Group.Offset + 3] & 15) << 4;
                Result.Add(new Stop(key, StopsFactory.UserMIDIFlag, Group.Type));
            } else if((num & 15) == 2) {
                int[] numArray1 = new int[3] { (num & 3840) >> 8, ((num & 983040) >> 16) - 1, ((num & 251658240) >> 24) - 1 };
                if(numArray1.Any(x => x < 0))
                    return;
                Result.Add(StopsFactory.CreateMIDIStop(numArray1[0], numArray1[1], numArray1[2], Group.Type));
            }
        }

        public static void StructureB(byte[] Data, VoicesGroup Group, List<Stop> Result) {
            int num = BitConverter.ToInt32(Data, Group.Offset) >> 1;
            if((num & 15) == 3) {
                int key = (int)Data[Group.Offset + 4] >> 1;
                Result.Add(new Stop(key, StopsFactory.UserMIDIFlag|StopsFactory.UserMIDIFlagB, Group.Type));
            } else if((num & 15) == 2) {
                int[] numArray1 = new int[3] { (num & 3840) >> 8, ((num & 983040) >> 16) - 1, ((num & 251658240) >> 24) - 1 };
                if(numArray1.Any(x => x < 0))
                    return;
                Result.Add(StopsFactory.CreateMIDIStop(numArray1[0], numArray1[1], numArray1[2], Group.Type, true));
            }
        }
    }
}
