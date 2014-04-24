using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MocoChan.WinForms
{
	public partial class FormSettings : Form
	{
		public FormSettings()
		{
			InitializeComponent();

			chkUnknownFileType.Checked = Properties.Settings.Default.unknownFileType;
		}

		private void chkUnknownFileType_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.unknownFileType = chkUnknownFileType.Checked;
			Properties.Settings.Default.Save();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
