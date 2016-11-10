namespace CPS_Reader.FileReaders {

    internal class StopMIDI : Stop {
        public int id2;
        public int id3;

        //PGM              MSB      LSB
        public StopMIDI(int id0, int id1, int id2, int id3, StopType Type) : base(id0, id1, Type) {
            this.id2 = id2;
            this.id3 = id3;
        }

        public override object GetAdditionalInfo() {
            return new int[] { id2, id3 };
        }
    }
}