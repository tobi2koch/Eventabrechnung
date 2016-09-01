using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eventabrechnung
{
    public partial class frmAddTeilnehmer : Form
    {
        public frmAddTeilnehmer()
        {
            InitializeComponent();
        }

        private void frmAddTeilnehmer_Load(object sender, EventArgs e)
        {



        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {

        }
    }
}
