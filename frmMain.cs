using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EISCF_QueryNotification
{
    public partial class frmMain : Form
    {
        private string m_sUser;
        private bool m_blnIsQA;
        private bool m_blnIsExcerptorEditor;

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        protected override CreateParams CreateParams
        {
            get
            {
                const int CP_DISABLE_CLOSE_BUTTON = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CP_DISABLE_CLOSE_BUTTON;
                return cp;
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            MoveToLowerRightCorner();
        }

        public void MoveToLowerRightCorner()
        {
            int x = Screen.PrimaryScreen.WorkingArea.Width - (this.Width - 5);
            int y = Screen.PrimaryScreen.WorkingArea.Height - (this.Height - 5);
            this.Location = new Point(x, y);
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.Refresh();

            string sUser = Environment.UserName;

#if DEBUG
            sUser = "ESCORO";
#endif

            m_sUser = "";
            if (!IsUserValid(sUser, ref m_sUser))
            {
                //MessageBox.Show("Not a valid user id!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            m_blnIsQA = false;
            m_blnIsExcerptorEditor = false;

            CheckUserTask(m_sUser, ref m_blnIsQA, ref m_blnIsExcerptorEditor);

            if (!m_blnIsQA && !m_blnIsExcerptorEditor)
            {
                this.Close();
                return;
            }

            tmeTimer.Stop();
            tmeTimer.Interval = Convert.ToInt32(60000 * GlobalVar.dblInterval);

            PopulateUser();
        }

        private void PopulateUser()
        {
            tmeTimer.Stop();

            DataTable dtQueried = new DataTable();
            if (m_blnIsQA)
            {
                dtQueried = GetQueriedUser();
            }

            DataTable dtResponded = new DataTable();
            if (m_blnIsExcerptorEditor)
            {
                dtResponded = GetRespondedUser(m_sUser);
            }

            pnlNotification.Controls.Clear();
            if (dtQueried.Rows.Count <= 0 && dtResponded.Rows.Count <= 0)
            {
                tmeTimer.Start();
                this.WindowState = FormWindowState.Minimized;
                return;
            }

            int iHeight = 0;

            if (dtQueried.Rows.Count > 0)
            {
                for (int iIdx = 0; iIdx < dtQueried.Rows.Count; iIdx++)
                {
                    ucInfo oInfo = new ucInfo();

                    oInfo.lblNotif.Text = $"{Convert.ToString(dtQueried.Rows[iIdx]["QueryUser"])} has query.";
                    oInfo.Dock = DockStyle.Top;

                    pnlNotification.Controls.Add(oInfo);

                    iHeight += oInfo.Height;
                }
            }

            if (dtResponded.Rows.Count > 0)
            {
                for (int iIdx = 0; iIdx < dtResponded.Rows.Count; iIdx++)
                {
                    ucInfo oInfo = new ucInfo();

                    oInfo.lblNotif.Text = $"{Convert.ToString(dtResponded.Rows[iIdx]["ResponseUser"])} responded to your query.";
                    oInfo.Dock = DockStyle.Top;

                    pnlNotification.Controls.Add(oInfo);

                    iHeight += oInfo.Height;
                }
            }

            this.Height = iHeight + 45;
            MoveToLowerRightCorner();

            tmeTimer.Start();
        }

        private DataTable GetQueriedUser()
        {
            DataTable dtList = new DataTable();

            string sQuery = "select Distinct QueryUser from QueryHandling where [Status]='Open'";

            using (SqlDataAdapter daData = new SqlDataAdapter(sQuery, GlobalVar.sConnString))
            {
                daData.Fill(dtList);
            }

            return dtList;
        }

        private DataTable GetRespondedUser(string sUser)
        {
            DataTable dtList = new DataTable();

            string sQuery = "select Distinct ResponseUser from QueryHandling where [Status]='Responded' and QueryUser=@User";

            using (SqlDataAdapter daData = new SqlDataAdapter(sQuery, GlobalVar.sConnString))
            {
                daData.SelectCommand.Parameters.AddWithValue("@User", sUser);
                daData.Fill(dtList);
            }

            return dtList;
        }

        private bool IsUserValid(string sUserID, ref string sUser)
        {
            sUser = "";

            string sQuery = "select top 1 ReaxysID from [ReaxysUserID] where UserID=@UserID";
            DataTable dtUser = new DataTable();
            using (SqlDataAdapter daData = new SqlDataAdapter(sQuery, GlobalVar.sConnString))
            {
                daData.SelectCommand.Parameters.AddWithValue("@UserID", sUserID);
                daData.Fill(dtUser);
            }

            if (dtUser.Rows.Count <= 0)
                return false;

            sUser = Convert.ToString(dtUser.Rows[0]["ReaxysID"]);

            return (!string.IsNullOrWhiteSpace(sUser));
        }

        private void CheckUserTask(string sUserID, ref bool blnIsQA, ref bool blnIsExcerpEditor)
        {
            blnIsQA = false;
            blnIsExcerpEditor = false;

            string sQuery = "select IsQA, UserGroup from [Users] where UserID=@UserID";
            DataTable dtUser = new DataTable();
            using (SqlDataAdapter daData = new SqlDataAdapter(sQuery, GlobalVar.sConnString))
            {
                daData.SelectCommand.Parameters.AddWithValue("@UserID", sUserID);
                daData.Fill(dtUser);
            }

            if (dtUser.Rows.Count <= 0)
                return;

            string sUserGroup = Convert.ToString(dtUser.Rows[0]["UserGroup"]);

            blnIsQA = Convert.ToBoolean(dtUser.Rows[0]["IsQA"]);
            blnIsExcerpEditor = (sUserGroup.Equals("Excerptor", StringComparison.OrdinalIgnoreCase) || sUserGroup.Equals("Editor", StringComparison.OrdinalIgnoreCase));
        }

        private void tmeTimer_Tick(object sender, EventArgs e)
        {
            PopulateUser();
        }

        private void pnlNotification_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (pnlNotification.Controls.Count > 0)
            {
                Control oControl = pnlNotification.Controls[0];
                this.Height = (oControl.Height * pnlNotification.Controls.Count) + 45;
                MoveToLowerRightCorner();
            }
        }
    }
}
