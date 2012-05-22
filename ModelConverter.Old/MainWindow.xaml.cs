using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ModelConverter.Model;
using System.Globalization;

namespace ModelConverter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IPlugin> Plugins = new List<IPlugin>();
        string fileFilter = "";

        public MainWindow()
        {
            InitializeComponent();

            //load Plugins
            string PluginDirectory = System.IO.Path.GetFullPath(".");

            string allFormats = "All Formats|";

            foreach (string file in Directory.GetFiles(PluginDirectory))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                try
                {
                    assembly = Assembly.LoadFrom(file);
                }
                catch (Exception ex)
                {
                    if (System.IO.Path.GetFileName(file).EndsWith(".dll"))
                    {
                        Log(new LogMessage("Could not load DLL," + ex.Message + ", File: " + System.IO.Path.GetFileName(file), LogLevel.Warning));
                    }
                }


                foreach (System.Type type in assembly.GetTypes())
                {
                    if (type.IsPublic && !type.IsAbstract && type.GetInterface("ModelConverter.Model.IPlugin") != null)
                    {
                        IPlugin newPlugin = (IPlugin)Activator.CreateInstance(type);

                        foreach (IPlugin plugin in Plugins)
                        {
                            foreach (string extension in newPlugin.fileExtensions.Keys)
                            {
                                if (plugin.fileExtensions.Keys.Contains(extension))
                                {
                                    Log(new LogMessage(
                                        "Ignored Plugin, already loaded a Plugin for " + extension +
                                        ", File: " + System.IO.Path.GetFileName(file), LogLevel.Warning
                                        ));
                                    continue;
                                }
                            }
                        }

                        Plugins.Add(newPlugin);

                        if (newPlugin.canRead)
                        {
                            foreach (KeyValuePair<string, string> extension in newPlugin.fileExtensions)
                            {
                                allFormats += "*." + extension.Key + ";";
                                fileFilter += "|" + extension.Value + "|*." + extension.Key;
                            }
                        }

                        if (newPlugin.canWrite)
                        {
                            foreach (KeyValuePair<string, string> extension in newPlugin.fileExtensions)
                            {
                                comboBoxFormat.Items.Add(extension.Key + " | " + extension.Value);
                            }
                            comboBoxFormat.SelectedIndex = 0;
                            buttonExport.IsEnabled = true;
                        }

                        Log(new LogMessage(
                            "Loaded Plugin " + newPlugin.Name +
                            ", File: " + System.IO.Path.GetFileName(file), LogLevel.Info
                            ));
                    }
                }
            }
            //loading complete
            allFormats = allFormats.Substring(0, allFormats.Length - 1);
            fileFilter = allFormats + fileFilter + "|All Files|*.*";
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            while (listBoxImport.SelectedItems.Count != 0)
            {
                listBoxImport.Items.Remove(listBoxImport.SelectedItems[0]);
            }
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Files to import...";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Filter += fileFilter;
            openFileDialog.ShowDialog();

            foreach (string file in openFileDialog.FileNames)
            {
                Log(new LogMessage("Check File: " + file, LogLevel.Info));
                if (listBoxImport.Items.Contains(file))
                {
                    Log(new LogMessage("Already loaded File: " + file, LogLevel.Info));
                }
                else if (LoadFile(file) == null)
                {
                    Log(new LogMessage("Could not load File: " + file, LogLevel.Error));
                }
                else
                {
                    listBoxImport.Items.Add(file);
                }
            }
        }

        private void checkBoxOtherFolder_Click(object sender, RoutedEventArgs e)
        {
            textBoxFolder.IsEnabled = Convert.ToBoolean(checkBoxOtherFolder.IsChecked);
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            textBoxLog.Text = String.Empty;
            Log(new LogMessage("cleared Log", LogLevel.Info));

            //get plugin
            string extension = (comboBoxFormat.SelectedItem as string).Split(new string[] { " | "},  StringSplitOptions.None)[0];
            IPlugin exportPlugin = GetPlugin(extension);
            
            //get scale factor
            float factor=1.0f;
            if(!float.TryParse(textBoxFactor.Text, System.Globalization.NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out factor))
            {
                Log(new LogMessage("Can not parse Scale Factor", LogLevel.Error));
                return;
            }

            //check plugin
            if(!exportPlugin.canWrite)
            {
                Log(new LogMessage("can not write " +comboBoxFormat.SelectedItem, LogLevel.Error));
                return;
            }


            foreach (string file in listBoxImport.Items)
            {
                string outputPath;
                if (checkBoxOtherFolder.IsChecked == true)
                {
                    outputPath = Path.Combine(
                        textBoxFolder.Text,
                        Path.GetFileNameWithoutExtension(file) + "."+ extension
                        );
                }
                else
                {
                    outputPath = Path.Combine(
                        Path.GetDirectoryName(file),
                        Path.GetFileNameWithoutExtension(file) + "." + extension
                        );
                }
                Log(new LogMessage("exporting " + Path.GetFileName(file) + " to " + outputPath, LogLevel.Info));
                List<LogMessage> pluginLog;
                BaseModel model = LoadFile(file);
                model.Scale(factor);
                if(checkBoxRecalculateNormals.IsChecked==true)
                    model.RecalculateNormals();
                exportPlugin.Write(outputPath, model, out pluginLog);
                Log(pluginLog);
            }
            Log(new LogMessage("Export complete!", LogLevel.Info));
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow(Plugins);
            window.ShowDialog();
        }

        private void Log(LogMessage logMessage)
        {
            if (textBoxLog.Text == String.Empty)
            {
                textBoxLog.Text = logMessage.ToString();
            }
            else
            {
                textBoxLog.Text += Environment.NewLine + logMessage.ToString();
            }
            textBoxLog.ScrollToEnd();  
        }

        private void Log(List<LogMessage> logMessages)
        {
            string text = "";
            foreach (LogMessage logMessage in logMessages)
            {
                text += Environment.NewLine + logMessage.ToString();
            }
            if (textBoxLog.Text == String.Empty)
            {
                textBoxLog.Text += text.Substring(2);
            }
            else
            {
                textBoxLog.Text += text;
            }
            textBoxLog.ScrollToEnd();
        }

        private IPlugin GetPlugin(string fileType)
        {
            foreach (IPlugin plugin in Plugins)
            {
                foreach (string fileExtension in plugin.fileExtensions.Keys)
                {
                    if (fileExtension.ToUpper() == fileType.ToUpper())
                    {
                        return plugin;
                    }
                }
            }
            return null;
        }

        private BaseModel LoadFile(string modelPath)
        {
            string extension = Path.GetExtension(modelPath);
            extension = extension.Substring(1);
            IPlugin plugin = GetPlugin(extension);
            if (plugin == null)
            {
                Log(new LogMessage("No Plugin for: *." + extension, LogLevel.Warning));
                return null;
            }
            List<LogMessage> pluginLog;
            BaseModel model = plugin.Read(modelPath, out pluginLog);
            Log(pluginLog);
            return model;
        }

        private void buttonShow_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in listBoxImport.SelectedItems)
            {
                new ViewWindow(LoadFile(item.ToString())).Show();
            }
        }
    }
}
