using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CPS_Reader.FileReaders {
    abstract class Reader : IReader {

        private FileStream _File;
        protected byte[] _CurrentMemoryChunk = null;
        protected long _Offset = 0L;

        public int M { get; protected set; }
        public int P { get; protected set; }

        abstract public int MemLen {get;}

        public Reader() {
            _CurrentMemoryChunk = new byte[MemLen];
        }

        virtual protected void UpdateOffset() {
            this._Offset = 32L;
            this._Offset = this._Offset + (long)((this.M - 1) * 136 + ((this.M - 1) * 15 + (this.P - 1)) * 328);
            if(this.P != 16)
                return;
            this._Offset = this._Offset - 192L;
        }

        virtual public bool Next() {
            this.P = this.P + 1;
            if(this.P > 16) {
                this.P = 1;
                this.M = this.M + 1;
            }
            this.UpdateOffset();
            return Seek();
        }

        protected bool Seek() {
            return this._File.Seek(this._Offset, SeekOrigin.Begin) == this._Offset && this._File.Read(this._CurrentMemoryChunk, 0, MemLen) == MemLen;
        }

        virtual public bool Prev() {
            this.P = this.P - 1;
            if(this.P < 1) {
                this.P = 16;
                this.M = this.M - 1;
                if(this.M < 1) {
                    this.P = 1;
                    this.M = 1;
                    return false;
                }
            }
            this.UpdateOffset();
            return Seek();
        }

        public string GetPosition() {
            return string.Format("M{0}P{1}", (object)this.M, (object)this.P);
        }

        abstract public void GetCP(List<Stop> Result);

        abstract public void GetGT(List<Stop> Result);

        abstract public void GetPD(List<Stop> Result);

        abstract public void GetSW(List<Stop> Result);

        abstract public bool IsEnd(int M);

        abstract public bool IsEmpty();

        virtual public bool Reset() {
            this.P = 1;
            this.M = 1;
            this.UpdateOffset();
            return this._File.Seek(this._Offset, SeekOrigin.Begin) == this._Offset && this._File.Read(this._CurrentMemoryChunk, 0, MemLen) == MemLen;
        }

        public void ReOpen(string FilePath) {
            FileStream fileStream = this._File;
            if(fileStream != null)
                fileStream.Close();
            this._File = new FileStream(FilePath, FileMode.Open);
        }

        public void Close() {
            FileStream fileStream = this._File;
            if(fileStream != null)
                fileStream.Close();
            this._File = (FileStream)null;
        }
    }
}
