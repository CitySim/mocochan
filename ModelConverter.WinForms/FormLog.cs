using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModelConverter.Model;

namespace ModelConverter.WinForms
{
	public partial class FormLog : Form, ILogProvider
	{
		public FormLog()
		{
			InitializeComponent();
		}

		private void FormLog_Load(object sender, EventArgs e)
		{

		}

		public void Log(LogLevel lvl, string Message)
		{
			listView1.Items.Insert(0, new ListViewItem(new string[] {DateTime.Now.ToString(), lvl.ToString(), Message}));
		}
	}
}
