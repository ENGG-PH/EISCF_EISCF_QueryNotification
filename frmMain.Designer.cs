namespace EISCF_QueryNotification
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlNotification = new System.Windows.Forms.Panel();
            this.tmeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pnlNotification
            // 
            this.pnlNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNotification.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlNotification.Location = new System.Drawing.Point(0, 0);
            this.pnlNotification.Name = "pnlNotification";
            this.pnlNotification.Size = new System.Drawing.Size(283, 65);
            this.pnlNotification.TabIndex = 0;
            this.pnlNotification.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.pnlNotification_ControlRemoved);
            // 
            // tmeTimer
            // 
            this.tmeTimer.Interval = 1;
            this.tmeTimer.Tick += new System.EventHandler(this.tmeTimer_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 65);
            this.Controls.Add(this.pnlNotification);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Query Notification";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmeTimer;
        public System.Windows.Forms.Panel pnlNotification;
    }
}

