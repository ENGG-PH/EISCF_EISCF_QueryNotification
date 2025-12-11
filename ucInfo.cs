using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EISCF_QueryNotification
{
    public partial class ucInfo : UserControl
    {
        public ucInfo()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;

            if (parent != null)
            {
                parent.Controls.Remove(this); // Remove from panel
                this.Dispose();                // Free resources
            }
        }
    }
}
