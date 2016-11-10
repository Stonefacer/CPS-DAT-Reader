using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_Reader.FileReaders {
    class VoicesGroup {
        public StopType Type;
        public int Offset;
        public int Len;
        public VoicesGroup.VoicesDecoder Decoder;

        public static VoicesGroup Create(StopType Type, int Offset, int Len, VoicesGroup.VoicesDecoder Decoder) {
            return new VoicesGroup() {
                Type = Type,
                Offset = Offset,
                Len = Len,
                Decoder = Decoder
            };
        }

        public void Decode(byte[] Memory, List<Stop> Result) {
            this.Decoder(Memory, this, Result);
        }

        public delegate void VoicesDecoder(byte[] Memory, VoicesGroup Group, List<Stop> Result);
    }
}
