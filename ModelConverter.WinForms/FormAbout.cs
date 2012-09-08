using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelConverter.WinForms
{
	public partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();

			labelVersion.Text = labelVersion.Text.Replace("x.x.x.x", Assembly.GetExecutingAssembly().GetName().Version.ToString());

			Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
