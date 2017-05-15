using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace FileGenLoaderDomain
{
    public partial class PlugManager : Form
    {
        public PlugManager()
        {
            InitializeComponent();
        }


        static void Main(string[] args)
        {
            ToCS.BasePath = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                ToCS.BasePath = args[0];
                ToCS.Config =
                    Serializable.Deserialize4File(typeof (Config), ToCS.BasePath + ToCS.ConfigName) as
                    Config;

            }
            catch
            {

            }
            if ( ToCS.Config == null)
            {

                ToCS.Config = new Config();
            }
            Application.Run(new PlugManager());
        }

        private Config Config = null;

        private void PlugManager_Load(object sender, EventArgs e)
        {
            Config = ToCS.Config;
            LoadConfig();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        public  void LoadConfig()
        {
            this.textBox1.Text = Config.PlugPath;
            if (string.IsNullOrEmpty(this.textBox1.Text))
                return;

            var files = new List<FileInfo>();

            if (System.IO.Directory.Exists(Config.PlugPath))
            {
                foreach (var fileInfo in new System.IO.DirectoryInfo(Config.PlugPath).GetFiles("*.dll"))
                {
                    files.Add(fileInfo);
                }

                foreach (var fileInfo in new System.IO.DirectoryInfo(Config.PlugPath).GetFiles("*.exe"))
                {
                    files.Add(fileInfo);
                }
            }
            
        

            Config.PlugConfigs = new List<Config.PlugConfg>();
            this.listView1.Items.Clear();

            foreach (var fileInfo in files)
            {
                Assembly assembly = Assembly.Load(File.ReadAllBytes(fileInfo.FullName));
                foreach (var type in assembly.GetTypes())
                {
                    if(type.BaseType!=null && type.BaseType.Name ==typeof(BasePlug).Name)
                    {
                        var plug = Activator.CreateInstance(type);

                        string exction = plug.GetType().GetProperty("Exction").GetValue(plug, null) as string;
                        var config = new Config.PlugConfg()
                                         {
                                              DllName = fileInfo.Name,
                                              Exction = exction,
                                              Type = type.FullName
                                         };
                        Config.PlugConfigs.Add(config);

                        this.listView1.Items.Add(new ListViewItem(new string[] { config.Exction, config.Type, config.DllName}));
                    }
                }
            }
       
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToCS.Config.PlugPath = this.textBox1.Text;
            Serializable.Serialiaze2File(ToCS.Config, ToCS.BasePath + ToCS.ConfigName);
        }
    }
}
