namespace Ssepan.Application
{
    public partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblCodebase = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AccessibleDescription = null;
            this.lblTitle.AccessibleName = null;
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Font = null;
            this.lblTitle.Name = "lblTitle";
            // 
            // lblVersion
            // 
            this.lblVersion.AccessibleDescription = null;
            this.lblVersion.AccessibleName = null;
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Font = null;
            this.lblVersion.Name = "lblVersion";
            // 
            // lblDescription
            // 
            this.lblDescription.AccessibleDescription = null;
            this.lblDescription.AccessibleName = null;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Font = null;
            this.lblDescription.Name = "lblDescription";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AccessibleDescription = null;
            this.lblCopyright.AccessibleName = null;
            resources.ApplyResources(this.lblCopyright, "lblCopyright");
            this.lblCopyright.Font = null;
            this.lblCopyright.Name = "lblCopyright";
            // 
            // lblCodebase
            // 
            this.lblCodebase.AccessibleDescription = null;
            this.lblCodebase.AccessibleName = null;
            resources.ApplyResources(this.lblCodebase, "lblCodebase");
            this.lblCodebase.Font = null;
            this.lblCodebase.Name = "lblCodebase";
            // 
            // pbIcon
            // 
            this.pbIcon.AccessibleDescription = null;
            this.pbIcon.AccessibleName = null;
            resources.ApplyResources(this.pbIcon, "pbIcon");
            this.pbIcon.BackgroundImage = null;
            this.pbIcon.Font = null;
            this.pbIcon.ImageLocation = null;
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.TabStop = false;
            // 
            // frmAbout
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.lblCodebase);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIcon);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblCodebase;
    }
}