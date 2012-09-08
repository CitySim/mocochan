using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelConverter.Model;

namespace ModelConverter.WinForms
{
	public partial class FormMain : Form
	{
		Converter converter;

		public FormMain()
		{
			InitializeComponent();

			converter = new Converter(new ConverterSettings(), null);

			propertyGrid1.SelectedObject = converter.settings;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FormAbout().ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
