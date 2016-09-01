using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Eventabrechnung
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.InitialCatalog = "Eventabrechnung";
            csb.UserID = "Max Mustermann";
            csb.Password = "mustermann";
            csb.DataSource = "(local)";

            global.g_conn = new SqlConnection(csb.ConnectionString);
            global.g_conn.Open();

            Application.Run(new start());
        }
    }
}
