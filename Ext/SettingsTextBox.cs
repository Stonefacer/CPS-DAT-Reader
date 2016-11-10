using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms.Ext {

    public partial class SettingsTextBox : UserControl {

        public enum ResizeControl {
            None,
            TextBox
        }

        public class CheckerEventArgs : EventArgs {

            public SettingsTextBox Source { get; private set; }
            public string Message { get; set; }

            public CheckerEventArgs(SettingsTextBox src) {
                Source = src;
            }

        }

        public Label label {
            get {
                return label1;
            }
        }
        public TextBox textBox {
            get {
                return textBox1;
            }
        }

        #region Browsable

        [Browsable(true)]
        public event EventHandler<CheckerEventArgs> Checker;

        private ResizeControl _ResizingType;

        [Browsable(true)]
        public ResizeControl ResizingType {
            get {
                return _ResizingType;
            }
            set {
                _ResizingType = value;
                UpdateResizeFactor();
            }
        }

        [Browsable(true)]
        public string Title {
            get {
                return label.Text;
            }
            set {
                label.Text = value;
            }
        }

        [Browsable(true)]
        public override string Text {
            get {
                return textBox.Text;
            }
            set {
                textBox.Text = value;
            }
        }

        [Browsable(true)]
        public bool IsCorrect {
            get {
                return label2.Text == "";
            }
        }

        #endregion

        public SettingsTextBox() {
            InitializeComponent();
            _ResizingType = ResizeControl.TextBox;
            UpdateResizeFactor();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            if (Checker != null) {
                var ea = new CheckerEventArgs(this);
                Checker(sender, ea);
                label2.Text = ea.Message;
            }
        }

        private void SettingsTextBox_Load(object sender, EventArgs e) {
            label2.Text = "";
        }

        private void UpdateResizeFactor() {
            textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            switch (_ResizingType) {
                case ResizeControl.TextBox:
                    textBox1.Anchor |= AnchorStyles.Right;
                    label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
            }
        }

    }
}
