using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_Reader.FileReaders {
    interface IReader {
        int MemLen { get; }
        int M { get; }
        int P { get; }

        bool Reset();
        void ReOpen(string FilePath);
        void Close();

        bool Next();
        bool Prev();
        string GetPosition();

        void GetGT(List<Stop> Result);
        void GetSW(List<Stop> Result);
        void GetPD(List<Stop> Result);
        void GetCP(List<Stop> Result);

    }
}
