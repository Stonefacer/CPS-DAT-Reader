using System;
using System.Collections.Generic;

namespace CPS_Reader.FileReaders {

    internal class CPSReader : Reader {
        private VoicesGroup _CP;
        private VoicesGroup _Great;
        private VoicesGroup _Pedal;
        private VoicesGroup _Swell;
        private VoicesGroup _UserGT_A;
        private VoicesGroup _UserGT_B;
        private VoicesGroup _UserPD_A;
        private VoicesGroup _UserPD_B;
        private VoicesGroup _UserSW_A;
        private VoicesGroup _UserSW_B;

        public override int MemLen { get { return 328; } }

        public CPSReader() {
            this.InitTables();
        }

        private void InitTables() {
            this._Great = new VoicesGroup() {
                Offset = 106,
                Len = 5,
                Type = StopType.Great,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
            this._Swell = new VoicesGroup() {
                Offset = 32,
                Len = 5,
                Type = StopType.Swell,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
            this._Pedal = new VoicesGroup() {
                Offset = 254,
                Len = 5,
                Type = StopType.Pedal,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.ThreeBitLeadOne)
            };
            this._CP = new VoicesGroup() {
                Offset = 0,
                Len = 2,
                Type = StopType.CP,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.BitMask)
            };
            this._UserGT_A = new VoicesGroup() {
                Offset = 111,
                Len = 6,
                Type = StopType.Great,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureA)
            };
            this._UserSW_A = new VoicesGroup() {
                Offset = 37,
                Len = 6,
                Type = StopType.Swell,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureA)
            };
            this._UserPD_A = new VoicesGroup() {
                Offset = 259,
                Len = 6,
                Type = StopType.Pedal,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureA)
            };
            this._UserGT_B = new VoicesGroup() {
                Offset = 138,
                Len = 6,
                Type = StopType.Great,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureB)
            };
            this._UserSW_B = new VoicesGroup() {
                Offset = 64,
                Len = 6,
                Type = StopType.Swell,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureB)
            };
            this._UserPD_B = new VoicesGroup() {
                Offset = 286,
                Len = 6,
                Type = StopType.Pedal,
                Decoder = new VoicesGroup.VoicesDecoder(Decoders.StructureB)
            };
        }

        public override void GetGT(List<Stop> Result) {
            _Great.Decode(this._CurrentMemoryChunk, Result);
            _UserGT_A.Decode(this._CurrentMemoryChunk, Result);
            _UserGT_B.Decode(this._CurrentMemoryChunk, Result);
        }

        public override void GetSW(List<Stop> Result) {
            _Swell.Decode(this._CurrentMemoryChunk, Result);
            _UserSW_A.Decode(this._CurrentMemoryChunk, Result);
            _UserSW_B.Decode(this._CurrentMemoryChunk, Result);
        }

        //public string[] GetUserSW() {
        //    return Enumerable.ToArray<string>(Enumerable.Union<string>((IEnumerable<string>)this.UserSW_A.Decode(this.CurrentMemoryChunk), (IEnumerable<string>)this.UserSW_B.Decode(this.CurrentMemoryChunk)));
        //}

        public override void GetPD(List<Stop> Result) {
            _Pedal.Decode(this._CurrentMemoryChunk, Result);
            _UserPD_A.Decode(this._CurrentMemoryChunk, Result);
            _UserPD_B.Decode(this._CurrentMemoryChunk, Result);
        }

        //public string[] GetUserPD() {
        //    return Enumerable.ToArray<string>(Enumerable.Union<string>((IEnumerable<string>)this.UserPD_A.Decode(this.CurrentMemoryChunk), (IEnumerable<string>)this.UserPD_B.Decode(this.CurrentMemoryChunk)));
        //}

        public override void GetCP(List<Stop> Result) {
            this._CP.Decode(this._CurrentMemoryChunk, Result);
        }

        public override bool IsEnd(int M) {
            return this.M > M;
        }

        public override bool IsEmpty() {
            return this.P > 5;
        }
    }
}