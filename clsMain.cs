using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EISCF_QueryNotification
{
    internal static class clsMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!ReadConfig())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static bool ReadConfig()
        {
            if (!File.Exists(String.Concat(Application.ExecutablePath, ".config")))
            {
                MessageBox.Show("Missing application configuration file.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            try
            {
                string sKey = "Tracking";
                GlobalVar.sConnString = ConfigurationManager.ConnectionStrings[sKey].ConnectionString;
                if (string.IsNullOrWhiteSpace(GlobalVar.sConnString))
                {
                    MessageBox.Show($"Missing '{sKey}' connection string in configuration file.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else if (!IsValidConnection(GlobalVar.sConnString))
                    return false;

                GlobalVar.dblInterval = 5;
                sKey = "Interval";
                string sValue = ConfigurationManager.AppSettings[sKey];
                if (!string.IsNullOrWhiteSpace(sValue))
                {
                    double dblValue = 0;
                    if (double.TryParse(sValue, out dblValue))
                    {
                        if (dblValue > 0)
                        {
                            GlobalVar.dblInterval = dblValue;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while reading configuration file.\n\n{ex.Message}", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
        }

        private static bool IsValidConnection(string sConnString)
        {
            try
            {
                using (SqlConnection oConn = new SqlConnection(sConnString))
                {
                    oConn.Open();
                    oConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error on connection string.\n\n{ex.Message}", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
    }

    internal static class GlobalVar
    {
        internal static double dblInterval;
        internal static string sConnString;
    }
}