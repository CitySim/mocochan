using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelConverter.Model;
using ModelConverter.WinForms.Options;

namespace ModelConverter.WinForms
{
	public partial class FormMain : Form
	{
		Converter converter;
		ConvertSettings convertSettings;
		FormLog logWindow;

		public FormMain()
		{
			InitializeComponent();

			logWindow = new FormLog();
			logWindow.VisibleChanged += logWindow_VisibleChanged;
			logWindow.Show();

			converter = new Converter(new ConverterSettings(), logWindow);
			convertSettings = new ConvertSettings(converter);

			converter.loadPlugins(Path.GetFullPath("."));

			convertSettings.loadSettings();

			propertyGrid1.SelectedObject = convertSettings;
		}

		void logWindow_VisibleChanged(object sender, EventArgs e)
		{
			logToolStripMenuItem.Checked = logWindow.Visible;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FormAbout().ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFiles = new OpenFileDialog();
			openFiles.CheckFileExists = true;
			openFiles.Multiselect = true;
			openFiles.Title = "Select Models to import";

			DialogResult result = openFiles.ShowDialog();

			if (result == DialogResult.Cancel)
			{
				return;
			}

			foreach (string file in openFiles.FileNames)
			{
				string ext = Path.GetExtension(file).Substring(1);
				if (!converter.extensions.ContainsKey(ext))
				{
					if (Properties.Settings.Default.unknownFileType)
					{
						MessageBox.Show("Found no Plugin for " + ext, "No Plugin", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
					}
					continue;
				}

				IPlugin plugin = converter.extensions[ext];
				listView1.Items.Add(new ListViewItem(new string[] { "", file, converter.getTargetPath(file), plugin.Name }));
			}
		}

		private void logToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (logToolStripMenuItem.Checked)
			{
				logWindow.Hide();
			}
			else
			{
				logWindow.Show();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listView1.Items)
			{
				item.ImageIndex = 0;
				converter.Convert(item.SubItems[1].Text);
				item.ImageIndex = 1;
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.lastOutputDir = converter.settings.outputDir;
			Properties.Settings.Default.lastExportType = converter.settings.exportType;

			Properties.Settings.Default.Save();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FormSettings().ShowDialog();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listView1.SelectedItems)
			{
				listView1.Items.Remove(item);
			}
		}
	}
}
