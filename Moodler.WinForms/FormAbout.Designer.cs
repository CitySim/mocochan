namespace Moodler.WinForms
{
	partial class FormAbout
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Moodler", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Framework", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.picLogo = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.listAssemblies = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnOK = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.lblVersion);
			this.panel1.Controls.Add(this.lblTitle);
			this.panel1.Controls.Add(this.picLogo);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(653, 86);
			this.panel1.TabIndex = 0;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(87, 58);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(77, 13);
			this.lblVersion.TabIndex = 3;
			this.lblVersion.Text = "Version: x.x.x.x";
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(82, 12);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(165, 46);
			this.lblTitle.TabIndex = 2;
			this.lblTitle.Text = "Moodler";
			// 
			// picLogo
			// 
			this.picLogo.Image = global::Moodler.WinForms.Properties.Resources.Moodler_64;
			this.picLogo.Location = new System.Drawing.Point(12, 12);
			this.picLogo.Name = "picLogo";
			this.picLogo.Size = new System.Drawing.Size(64, 64);
			this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picLogo.TabIndex = 1;
			this.picLogo.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 89);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(101, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Loaded Assemblies:";
			// 
			// listAssemblies
			// 
			this.listAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listAssemblies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listAssemblies.FullRowSelect = true;
			listViewGroup1.Header = "Moodler";
			listViewGroup1.Name = "Moodler";
			listViewGroup1.Tag = "Moodler";
			listViewGroup2.Header = "Framework";
			listViewGroup2.Name = "framework";
			listViewGroup2.Tag = "framework";
			this.listAssemblies.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
			this.listAssemblies.Location = new System.Drawing.Point(12, 105);
			this.listAssemblies.Name = "listAssemblies";
			this.listAssemblies.Size = new System.Drawing.Size(629, 222);
			this.listAssemblies.TabIndex = 2;
			this.listAssemblies.UseCompatibleStateImageBehavior = false;
			this.listAssemblies.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 282;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Version";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "File";
			this.columnHeader3.Width = 391;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(566, 333);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// FormAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(653, 368);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.listAssemblies);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(650, 400);
			this.Name = "FormAbout";
			this.Text = "About Moodler";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox picLogo;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView listAssemblies;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
	}
}