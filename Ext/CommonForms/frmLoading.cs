using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif
using System.Windows.Forms;

namespace Ext.CommonForms {

#if NET45

    public partial class frmLoading : Form {

        private static Task _TaskToExecute;

        public static void ShowDialog(Task task, Image image, String text) {
            using(frmLoading frm = new frmLoading()) {
                frm.pictureBox1.Image = image;
                frm.label1.Text = text;
                _TaskToExecute = task;
                frm.ShowDialog();
            }
        }

        private frmLoading() {
            InitializeComponent();
        }

        private async void frmLoading_Shown(Object sender, EventArgs e) {
            await _TaskToExecute;
            this.Close();
        }
    }
#endif
}
