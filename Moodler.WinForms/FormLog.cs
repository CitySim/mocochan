using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moodler.Converter;
using System.IO;
using Moodler.Converter.Logging;

namespace Moodler.WinForms
{
	public partial class FormLog : Form, ILogProvider
	{
		public FormLog()
		{
			InitializeComponent();
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		public void Log(LogLevel lvl, string Message)
		{
			// loop over all lines in message
			using (StringReader reader = new StringReader(Message))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					ListViewItem item = new ListViewItem(new string[] { DateTime.Now.ToString(), lvl.ToString(), line });
					switch (lvl)
					{
						case LogLevel.Fatal:
							item.BackColor = Color.DarkRed;
							item.ForeColor = Color.White;
							break;
						case LogLevel.Error:
							item.BackColor = Color.Red;
							item.ForeColor = Color.White;
							break;
						case LogLevel.Warning:
							item.BackColor = Color.Orange;
							item.ForeColor = Color.White;
							break;
					}
					listView1.Items.Add(item);
				}
			}

			listView1.EnsureVisible(listView1.Items.Count - 1);

			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
