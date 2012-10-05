using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelConverter.WinForms.Options
{
	class DirectoryEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = "Select Folder exported Files are saved to.";

			dialog.SelectedPath = (string)value;
			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.Cancel)
			{
				return value;
			}

			return dialog.SelectedPath;
		}
	}
}
