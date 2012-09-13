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

		public void Log(LogLevel lvl, string Message)
		{
			listView1.Items.Add(new ListViewItem(new string[] {DateTime.Now.ToString(), lvl.ToString(), Message}));
			listView1.EnsureVisible(listView1.Items.Count - 1);
		}

		private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
