using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Ext.System.Core;

using CPS_Reader.FileReaders;

namespace Readers {
    public partial class Form1 : Form {
        private BackgroundWorker _bw = (BackgroundWorker)null;
        private bool _ParseEmptyBlocks = false;
        private int _MaxM = 0;
        private List<Stop> _Result = new List<Stop>();
        private Queue<Line> _MainQueue = new Queue<Line>();
        private string _Path = (string)null;

        public Form1() {
            this.InitializeComponent();
        }

        private void DrawHR() {
            listView1.Items.Add(new ListViewItem() { BackColor = System.Drawing.Color.LightGray });
        }

        private void _bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.SetBusyGUI(false);
            string str = "Done";
            if(e.Error != null) {
                e.Error.Log();
                str = "Failure";
            }
            this.lblState.Text = str;
            this.btnParse.Text = "Start";
            this.SetBusyGUI(false);
            this._bw = (BackgroundWorker)null;
            listView1.Items.Clear();
            DrawText();
        }

        private void _bw_DoWork(object sender, DoWorkEventArgs e) {
            this.Parse();
        }

        private void WriteLine() {
            this._MainQueue.Enqueue(Line.Empty);
        }

        private void WriteLine(string str, int tab = 0) {
            this._MainQueue.Enqueue(new Line(str, tab));
        }

        private void WriteLine(string[] strs, int tab = 0) {
            foreach(string Text in strs)
                this._MainQueue.Enqueue(new Line(Text, tab));
        }

        private void WriteLine(string Format, params object[] args) {
            this.WriteLine(string.Format(Format, args), 0);
        }

        private void Parse() {
            Reader rd = null;
            if(_Path.ToLower().EndsWith(".cps"))
                rd = new CPSReader();
            else
                rd = new DATReader();
            rd.ReOpen(_Path);
            bool flag = rd.Reset();
            _Result.Clear();
            while(!rd.IsEnd(_MaxM) && flag) {
                if(!_ParseEmptyBlocks && rd.IsEmpty()) {
                    flag = rd.Next();
                    continue;
                }
                _Result.Add(StopsFactory.CreateSeparator(rd.M, rd.P));
                rd.GetSW(_Result);
                rd.GetGT(_Result);
                rd.GetPD(_Result);
                rd.GetCP(_Result);
                flag = rd.Next();
            }
            rd.Close();
        }

        private void WriteText(List<Stop> ResSW, List<Stop> ResGT, List<Stop> ResPD, List<Stop> ResCP, string Text = null) {
            int id = 0;
            bool Stop = false;
            while(true) {
                Stop = true;
                string[] buf = new string[5];
                if(Text.IsNotNullAndNotEmpty()) {
                    buf[0] = Text;
                    Text = null;
                    Stop = false;
                }
                if(id < ResSW.Count) {
                    buf[2] = StopsFactory.GetStopName(ResSW[id]);
                    Stop = false;
                }
                if(id < ResGT.Count) {
                    buf[1] = StopsFactory.GetStopName(ResGT[id]);
                    Stop = false;
                }
                if(id < ResPD.Count) {
                    buf[3] = StopsFactory.GetStopName(ResPD[id]);
                    Stop = false;
                }
                if(id < ResCP.Count) {
                    buf[4] = StopsFactory.GetStopName(ResCP[id]);
                    Stop = false;
                }
                if(Stop)
                    break;
                else
                    listView1.Items.Add(new ListViewItem(buf));
                id++;
            }

        }

        private void DrawText() {
            List<Stop> ResGT = new List<Stop>();
            List<Stop> ResSW = new List<Stop>();
            List<Stop> ResPD = new List<Stop>();
            List<Stop> ResCP = new List<Stop>();
            string Text = null;
            for(int i = 0; i < _Result.Count;) {
                while(i < _Result.Count) {
                    var Cur = _Result[i];
                    switch(Cur.Type) {
                        case StopType.Swell:
                        case StopType.SwellDAT:
                            ResSW.Add(Cur);
                            break;
                        case StopType.Great:
                        case StopType.GreatDAT:
                            ResGT.Add(Cur);
                            break;
                        case StopType.Pedal:
                        case StopType.PedalDAT:
                            ResPD.Add(Cur);
                            break;
                        case StopType.CP:
                            ResCP.Add(Cur);
                            break;
                    }
                    if(Cur.Type == StopType.Ctrl && (Cur.id1 & StopsFactory.SeparatorFlag) != 0) {
                        if(i != 0) {
                            WriteText(ResSW, ResGT, ResPD, ResCP, Text);
                            ResSW.Clear();
                            ResGT.Clear();
                            ResPD.Clear();
                            ResCP.Clear();
                            DrawHR();
                        }
                        Text = StopsFactory.SeparatorToString(Cur);
                    }
                    i++;
                }
            }
            WriteText(ResSW, ResGT, ResPD, ResCP, Text);
        }

        private void SetBusyGUI(bool State) {
            this.btnBrowse.Enabled = !State;
            this.nudMaxM.Enabled = !State;
            this.chbEmpty.Enabled = !State;
        }

        private void button2_Click(object sender, EventArgs e) {
#if DEBUG
            System.Diagnostics.Trace.WriteLine("------------------- Start / Stop -------------------");
#endif
            if(this._bw != null) {
                this._bw.CancelAsync();
                this.btnParse.Text = "Start";
                this.SetBusyGUI(false);
                this._bw = (BackgroundWorker)null;
            } else if(this.txtInput.Text.IsNullOrEmpty()) {
                int num = (int)MessageBox.Show("Input cannot be empty.");
            } else {
                this._bw = new BackgroundWorker();
                this._bw.WorkerSupportsCancellation = true;
                this._bw.DoWork += new DoWorkEventHandler(this._bw_DoWork);
                this._bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._bw_RunWorkerCompleted);
                this._Path = this.txtInput.Text;
                this._MaxM = this.nudMaxM.Value.ToInt();
                this._bw.RunWorkerAsync();
                this.btnParse.Text = "Stop";
                this._ParseEmptyBlocks = this.chbEmpty.Checked;
                this.SetBusyGUI(true);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "Supported formats|*.cps;*.dat|All Files|*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.Multiselect = false;
            if(openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            this.txtInput.Text = openFileDialog1.FileName;
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void chbEmpty_CheckedChanged(object sender, EventArgs e) {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void releaseObject(object obj) {
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            } catch(Exception) {

            }
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e) {
            if(_Result.Count == 0) {
                MessageBox.Show("List is empty", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string Path = "";
            using(SaveFileDialog sfd = new SaveFileDialog() {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "Excel|*.xls",
                FilterIndex = 0
            }) {
                if(sfd.ShowDialog() != DialogResult.OK)
                    return;
                Path = sfd.FileName;
            }
            Save(_Result, Path);
        }

        private void Save(List<Stop> Result, string Path) {
            //XlsDocument doc = new XlsDocument();
            //var sheet = doc.Workbook.Worksheets.Add("CPS Reader");
            //var cells = sheet.Cells;
            //int row = 1;
            //int indexGT = 1;
            //int indexSW = 1;
            //int indexPD = 1;
            //int indexCP = 1;
            //foreach(var stop in Result) {
            //    switch(stop.Type) {
            //        case StopType.Ctrl:
            //            row += Math.Max(Math.Max(indexCP, indexGT), Math.Max(indexPD, indexSW));
            //            sheet.AddMergeArea(new MergeArea() { RowMin = (ushort)row, RowMax = (ushort)row, ColMin = 1, ColMax = 5 });
            //            var cell = cells.Add(row, 1, StopsFactory.SeparatorToString(stop));
            //            cell.VerticalAlignment = VerticalAlignments.Centered;
            //            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            //            cell.UseBackground = true;
            //            cell.PatternBackgroundColor = Colors.Grey;
            //            cell.Font.Height = 480;
            //            indexCP = 1;
            //            indexGT = 1;
            //            indexPD = 1;
            //            indexSW = 1;
            //            break;
            //        case StopType.Great:
            //        case StopType.GreatDAT:
            //            cell = cells.Add(row + indexGT, 1, StopsFactory.GetStopName(stop));
            //            indexGT++;
            //            break;
            //        case StopType.Pedal:
            //        case StopType.PedalDAT:
            //            cell = cells.Add(row + indexPD, 2, StopsFactory.GetStopName(stop));
            //            indexPD++;
            //            break;
            //        case StopType.Swell:
            //        case StopType.SwellDAT:
            //            cell = cells.Add(row + indexSW, 3, StopsFactory.GetStopName(stop));
            //            indexSW++;
            //            break;
            //        case StopType.CP:
            //            cell = cells.Add(row + indexCP, 4, StopsFactory.GetStopName(stop));
            //            indexCP++;
            //            break;
            //        default:
            //            throw new InvalidEnumArgumentException(stop.Type.ToString());
            //    }
            //}
            //ColumnInfo ci = new ColumnInfo(doc, sheet);
            //ci.Width = 10000;
            //ci.ColumnIndexStart = 0;
            //ci.ColumnIndexEnd = 5;
            //sheet.AddColumnInfo(ci);
            //try {
            //    FileStream fs = new FileStream(Path, FileMode.Create);
            //    doc.Save(fs);
            //    fs.Close();
            //} catch(Exception ex) {
            //    MessageBox.Show(ex.Message + "(" + ex.GetType().ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void rawTextToolStripMenuItem_Click(object sender, EventArgs e) {
            if(_Result.Count == 0) {
                MessageBox.Show("List is empty", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string Path = "";
            using(SaveFileDialog sfd = new SaveFileDialog() {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "Text file|*.txt",
                FilterIndex = 0
            }) {
                if(sfd.ShowDialog() != DialogResult.OK)
                    return;
                Path = sfd.FileName;
            }
            try {
                using(StreamWriter sw = new StreamWriter(Path, false)) {
                    StopType prevStop = StopType.Ctrl;
                    foreach(var stop in _Result) {
                        if(stop.Type == StopType.Ctrl) {
                            sw.WriteLine(StopsFactory.SeparatorToString(stop));
                        } else if(prevStop == stop.Type) {
                            sw.WriteLine(string.Format("\t{0}", StopsFactory.GetStopName(stop)));
                        } else {
                            prevStop = stop.Type;
                            sw.WriteLine(string.Format("{0}:\t{1}", StopsFactory.StopTypeToString(stop.Type), StopsFactory.GetStopName(stop)));
                        }
                    }
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void Form1_Load_1(object sender, EventArgs e) {

        }
    }
}
