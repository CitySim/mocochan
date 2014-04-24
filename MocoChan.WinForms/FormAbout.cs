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

namespace MocoChan.WinForms
{
	public partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();

			lblVersion.Text = lblVersion.Text.Replace("x.x.x.x", Assembly.GetExecutingAssembly().GetName().Version.ToString());

			Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in loaded)
			{
				string name = assembly.FullName;
				ListViewGroup itemGroup = listAssemblies.Groups["framework"];

				// TODO: well, should check more than the name
				if (assembly.FullName.Contains("MocoChan"))
				{
					itemGroup = listAssemblies.Groups["MocoChan"];
				}

				listAssemblies.Items.Add(new ListViewItem(new string[] { assembly.GetName().Name, assembly.GetName().Version.ToString(), assembly.CodeBase }, itemGroup));
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
