using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_Reader.FileReaders {
    class DATReader : Reader {

        private VoicesGroup _GT;
        private VoicesGroup _SW;
        private VoicesGroup _PD;

        public override int MemLen { get { return 0x2EE; } }

        public DATReader() {
            InitTables();
        }

        private void InitTables() {
            _SW = new VoicesGroup() {
                Offset = 0x4E,
                Len = 4,
                Type = StopType.SwellDAT,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
            _GT = new VoicesGroup() {
                Offset = 0xA2,
                Len = 4,
                Type = StopType.GreatDAT,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
            _PD = new VoicesGroup() {
                Offset = 0x14A,
                Len = 4,
                Type = StopType.PedalDAT,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
        }

        public override void GetCP(List<Stop> Result) {

        }

        public override void GetGT(List<Stop> Result) {
            _GT.Decode(_CurrentMemoryChunk, Result);
        }

        public override void GetPD(List<Stop> Result) {
            _PD.Decode(_CurrentMemoryChunk, Result);
        }

        public override void GetSW(List<Stop> Result) {
            _SW.Decode(_CurrentMemoryChunk, Result);
        }

        protected override void UpdateOffset() {
            _Offset = 0x20;
            _Offset += (P-1) * 0x02EE;
        }

        public override bool Next() {
            P++;
            if(P > 18) {
                P = 18;
                return false;
            }
            UpdateOffset();
            return Seek();
        }

        public override bool Prev() {
            if(P == 1)
                return false;
            P--;
            UpdateOffset();
            return Seek();
        }

        public override bool IsEnd(int M) {
            return false;
        }

        public override bool IsEmpty() {
            return false;
        }

    }
}
