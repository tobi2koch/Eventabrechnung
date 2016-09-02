using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eventabrechnung
{
    public partial class start : Form
    {

        // nützliche Festlegungen
        Color bgColorActive = Color.FromArgb(40, 50, 60);
        Color bgColorInactive = System.Drawing.Color.LightSlateGray;
        Point sichtbar = new Point(192, 0);
        Point hidden = new Point(8500, 0);


        public start()
        {
            InitializeComponent();
        }

        private void start_Load(object sender, EventArgs e)
        {
            this.Size = new Size(202, 210);
            pnBackground.Location = hidden;
            pnProjekte.Location = hidden;
            pnlEventsAnlegen.Location = hidden;
        }

        private void changeFormSize(int status)
        {
            if (status == 0)
            {
                this.Size = new System.Drawing.Size(202, 210);
            }
            else
                this.Size = new System.Drawing.Size(900, 600);
        }


        /// <summary>
        /// Teilnehmer eintragen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void cmdCancel_Click(object sender, EventArgs e)
        {
            pnBackground.Location = hidden;
            //this.Size = new Size(400, 600);
            changeFormSize(0);
            cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
        }

        private void cmdTeilnehmerAnlegen_Click_1(object sender, EventArgs e)
        {
            pnBackground.Location = sichtbar;
            pnProjekte.Location = hidden;
            pnlEventsAnlegen.Location = hidden;
            pnTeilnehmerZuordnen.Location = hidden;

            changeFormSize(1);
            cmdTeilnehmerAnlegen.BackColor = bgColorActive;
            cmdProjektAnlegen.BackColor = bgColorInactive;
            cmdEventsAnlegen.BackColor = bgColorInactive;
            cmdTeilnehmerZuordnen.BackColor = bgColorInactive;
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtBenutzer.Text = "";
            txtName.Text = "";
            txtPassword.Text = "";
        }

        private void cmdSendTeilnehmer_Click(object sender, EventArgs e)
        {
            SqlCommand com;
            String SQLstring;
            int anz;
            Guid g = Guid.NewGuid();

            SQLstring = "Insert into tblTeilnehmer (Teilnehmer_ID, Name, Benutzername, Kennwort) "
                + "Values(@id, @name, @benutzer, @pw)";
            com = new SqlCommand(SQLstring, global.g_conn);
            com.Parameters.AddWithValue("@id", g);
            com.Parameters.AddWithValue("@name", txtName.Text);
            com.Parameters.AddWithValue("@benutzer", txtBenutzer.Text);
            com.Parameters.AddWithValue("@pw", txtPassword.Text);

            anz = com.ExecuteNonQuery();
            if (anz == 1)
            {
                MessageBox.Show("Eintrag erfolgreich");
                cmdClear_Click(null, null);
            }
            else
                MessageBox.Show("Konnte nicht ausgeführt werden");
        }

        /// <summary>
        /// Projekte anlgegen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void cmdProjektAnlegen_Click(object sender, EventArgs e)
        {
            changeFormSize(1);
            pnBackground.Location = hidden;
            pnlEventsAnlegen.Location = hidden;
            pnTeilnehmerZuordnen.Location = hidden;
            pnProjekte.Location = sichtbar;

            cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
            cmdEventsAnlegen.BackColor = bgColorInactive;
            cmdProjektAnlegen.BackColor = bgColorActive;
            cmdTeilnehmerZuordnen.BackColor = bgColorInactive;
        }

        private void cmdClearProjekt_Click(object sender, EventArgs e)
        {
            txtProjektname.Text = "";
            txtProjektbeschreibung.Text = "";
        }

        private void cmdCancelProjekt_Click(object sender, EventArgs e)
        {
            pnProjekte.Location = hidden;
            changeFormSize(0);
            cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
        }

        private void cmdAddProjekt_Click(object sender, EventArgs e)
        {
            SqlCommand com;
            String SQLstring;
            Guid g = Guid.NewGuid();
            int anz;

            SQLstring = "Insert into tblProjekt (PROJEKT_ID, Bezeichnung, Projektname)"
                + "Values (@id, @bezeichnung, @name)";
            com = new SqlCommand(SQLstring, global.g_conn);
            com.Parameters.AddWithValue("@id", g);
            com.Parameters.AddWithValue("@bezeichnung", txtProjektbeschreibung.Text);
            com.Parameters.AddWithValue("@name", txtProjektname.Text);

            anz = com.ExecuteNonQuery();
            if (anz == 1)
            {
                MessageBox.Show("Eintrag erfolgreich");
                cmdClearProjekt_Click(null, null);
                txtProjektname.Focus();
                cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
            }
            else
                MessageBox.Show("Konnte nicht ausgeführt werden");
        }

        // menu Teilnehmer zuordnen
        private void cmdTeilnehmerZuordnen_Click(object sender, EventArgs e)
        {
            changeFormSize(1);
            pnTeilnehmerZuordnen.Location = sichtbar;
            pnProjekte.Location = hidden;
            pnlEventsAnlegen.Location = hidden;
            pnBackground.Location = hidden;

            cmdTeilnehmerZuordnen.BackColor = bgColorActive;
            cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
            cmdProjektAnlegen.BackColor = bgColorInactive;
            cmdEventsAnlegen.BackColor = bgColorInactive;
            pnTeilnehmerZuordnen.Location = sichtbar;
            dropDownProjekts();
            addMemberToProjekt();
        }

        // EVENTS ANLEGEN
        private void cmdEventsAnlegen_Click(object sender, EventArgs e)
        {
            cmdTeilnehmerAnlegen.BackColor = bgColorInactive;
            cmdProjektAnlegen.BackColor = bgColorInactive;
            cmdEventsAnlegen.BackColor = bgColorActive;
            cmdTeilnehmerZuordnen.BackColor = bgColorInactive;

            pnBackground.Location = hidden;
            pnProjekte.Location = hidden;
            pnTeilnehmerZuordnen.Location = hidden;
            pnlEventsAnlegen.Location = sichtbar;

            eventZuordnen();
        }

        // DATA GRID VIEWS ERZEUGEN
        private void showDataGridTeilnehmer()
        {
            // TEILNEHMER LISTE (ALLE) ANZEIGEN //
            String Select;
            DataTable dt;
            SqlDataAdapter da;

            Select = "Select Teilnehmer_ID, Name from tblTeilnehmer";
            dt = new DataTable();
            da = new SqlDataAdapter(Select, global.g_conn);

            da.Fill(dt);
            dg_Teilnehmer.DataSource = dt;
            dg_Teilnehmer.Columns.Remove("Teilnehmer_ID");
        }

        // DATAGRID PROJEKTE
        private void showDataGridProjekte()
        {
            String Select = "select Projektname, COUNT(tblProjektTeilnehmer.ID_Projekt) AS Teilnehmer from tblProjekt "
                    + "JOIN tblProjektTeilnehmer ON tblProjektTeilnehmer.ID_Projekt = tblProjekt.Projekt_ID "
                    + "GROUP BY Projektname";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Select, global.g_conn);
            da.Fill(dt);
            dg_projekte.DataSource = dt;
        }

        // DATA GRID VIEWS ANZEIGEN
        private void cmdZeigeDinge_Click(object sender, EventArgs e)
        {
            showDataGridTeilnehmer();

        }

        private void cmdShowProjekts_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            showDataGridProjekte();
        }

        // TEILNEHMER ZU PROJEKTEN ZUORDNEN
        private void dropDownProjekts()
        {
            String SQLstring = "Select * from tblProjekt";
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            da = new SqlDataAdapter(SQLstring, global.g_conn);
            da.Fill(dt);
            cmbProjektZuordnen.DataSource = dt;
            cmbProjektZuordnen.DisplayMember = "Projektname";
            cmbProjektZuordnen.ValueMember = "Projekt_ID";

        }

        private void addMemberToProjekt()
        {
            String SQLstring = "Select Name, Teilnehmer_ID from tblTeilnehmer order by name";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(SQLstring, global.g_conn);
            da.Fill(dt);
            checklist_AddMember.DataSource = dt;
            checklist_AddMember.DisplayMember = "Name";
            checklist_AddMember.ValueMember = "Teilnehmer_ID";
        }

        private void cmd_cancelAddToProjekt_Click(object sender, EventArgs e)
        {
            pnTeilnehmerZuordnen.Location = hidden;
            //changeFormSize(0);
            cmdTeilnehmerZuordnen.BackColor = bgColorInactive;
        }

        private void cmd_ClearAddToProjekt_Click(object sender, EventArgs e)
        {
            //checklist_AddMember.Items.Clear();
        }

        private void cmd_AddToProjekt_Click(object sender, EventArgs e)
        {
            SqlCommand com;
            String SQLstring;
            int anz;


            SQLstring = "INSERT INTO tblProjektTeilnehmer (ProjektTeilnehmer_ID, ID_Projekt, ID_Teilnehmer)"
                + "VALUES (@guid, @projekt_ID, @teilnehmer_ID)";

            for (int i = 0; i < checklist_AddMember.CheckedItems.Count; i++)
            {
                Guid g = Guid.NewGuid();
                com = new SqlCommand(SQLstring, global.g_conn);
                com.Parameters.AddWithValue("@guid", g);
                com.Parameters.AddWithValue("@projekt_ID", cmbProjektZuordnen.SelectedValue);
                com.Parameters.AddWithValue("@teilnehmer_ID", ((DataRowView)(checklist_AddMember.CheckedItems[i]))[1]);

                anz = com.ExecuteNonQuery();
                if (anz == 1)
                {
                    MessageBox.Show("Erfolgreich");
                }
                else
                    MessageBox.Show("hat nicht geklappt", "Kleine Fehlermeldung", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void eventZuordnen()
        {
            String SQLstring = "Select * from tblProjekt";
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            da = new SqlDataAdapter(SQLstring, global.g_conn);
            da.Fill(dt);
            cmbProjektZuordnen.DataSource = dt;
            cmbProjektZuordnen.DisplayMember = "Projektname";
            cmbProjektZuordnen.ValueMember = "Projekt_ID";

            String SQLinsert = "INSERT INTO tblEreignis (Ereignis_ID, ID_Projekt, ID_Teilnehmer, Betrag) "
                + "VALUES(@idEreignis, @idProjekt, @idTeilnehmer, @betrag)";
            SqlCommand com;
            int anz;


        }
    }
}
