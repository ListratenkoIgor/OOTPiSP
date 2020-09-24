using System;
using System.IO;
using System.Windows.Forms;
using OOPlab.Properties;
using System.Reflection;

namespace OOPlab
{
    public partial class PluginForm : Form
    {
        private byte[] _Serialized_Data;
        private string Filename;
        public PluginForm(byte[] data, string filename)
        {
            InitializeComponent();
            GetPlugins(Directory.GetCurrentDirectory() + "/Plugins");
            _Serialized_Data = data;
            Filename = filename;
        }
        private void GetPlugins(string Path)
        {
            if (!Directory.Exists(Path))
            {
                return;
            }

            Plugin curr = cbbPlugin.SelectedItem as Plugin;
            cbbPlugin.BeginUpdate();
            cbbPlugin.Items.Clear();

            foreach (string f in Directory.GetFiles(Path))
            {
                FileInfo fi = new FileInfo(f);

                if (fi.Extension.Equals(".dll"))
                {
                    Plugin plugin = new Plugin(f);
                    if (plugin.Type == PluginInterface.PluginType.Unknown)
                        continue;
                    cbbPlugin.Items.Add(plugin);
                }
            }
            cbbPlugin.EndUpdate();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm._curr_Plugin = cbbPlugin.SelectedItem as Plugin;
            if (MainForm._curr_Plugin != null)
            {
                byte[] data = Plugin.ActivatePlugin(MainForm._curr_Plugin, _Serialized_Data, true);
                using (FileStream fs = new FileStream(Filename, FileMode.OpenOrCreate))
                {
                    fs.Write(data, 0, data.Length);
                }
                Plugin.SetCustomFileProperty(Filename, MainForm._curr_Plugin.Filename);
            }
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            MainForm._curr_Plugin = null;
            using (FileStream fs = new FileStream(Filename, FileMode.OpenOrCreate))
            {
                fs.Write(_Serialized_Data, 0, _Serialized_Data.Length);
            }
            Plugin.SetCustomFileProperty(Filename, "");
            this.Close();
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
