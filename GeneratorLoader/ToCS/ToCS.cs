using System;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using GeneratorLoader;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.Shell;
//using VSOLE = Microsoft.VisualStudio.OLE.Interop;
using System.CodeDom;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using VSLangProj80;
using System;
using System.Runtime.InteropServices;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;


namespace FileGenLoaderDomain
{

    [Guid("29012265-6f22-4bd3-9f10-1d7082bf4e2e")]
    [ComVisible(true)]
    [CodeGeneratorRegistration(typeof(ToCS), "C# Class Generator", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(ToCS))]
    public class ToCS : BaseCodeGeneratorWithSite
    {

#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "ToCS";
#pragma warning restore 0414

        private StringBuilder log = new StringBuilder();

        public static string BasePath { get; set; }
        public static string ConfigName = "\\config.xml";
        public static Config Config { get; set; }

    
        protected override byte[] GenerateCode(string inputFileContent)
        {
            StringBuilder codes = new StringBuilder();
            System.Diagnostics.Trace.WriteLine("Start GenerateOnFile...");
            log.Clear();
            log.AppendLine("//Code Gen By " + System.Reflection.Assembly.GetCallingAssembly().FullName);
            log.AppendLine("//Start Run:");
            log.AppendLine("//CodeBase " + System.Reflection.Assembly.GetCallingAssembly().CodeBase);
            log.AppendLine("//wszInputFilePath:" + InputFilePath);
            log.AppendLine("//wszDefaultNamespace:" + FileNameSpace);

            if (Config == null)
            {
                try
                {
                    //插件搜索
                    BasePath = new FileInfo(System.Reflection.Assembly.GetCallingAssembly().CodeBase.Replace(@"file:///", "")).Directory.FullName;

                    if (File.Exists(BasePath + ConfigName))
                    {

                        Config = Serializable.Deserialize4File(typeof(Config), BasePath + ConfigName) as Config;

                    }
                }
                catch (Exception e)
                {
                    log.AppendLine(e.ToString());
                }
            }
            log.AppendLine("//BasePath:" + BasePath);
            System.Diagnostics.Trace.WriteLine(log.ToString());
            codes.AppendLine(log.ToString());

            log.Clear();
            if (Config == null)
            {
                Config = new Config();
            }

            if (Config.PlugPath == null)
            {
                System.Diagnostics.Trace.WriteLine("Run PlugManager...");

                try
                {
                    new PlugManager().Show();
                }
                catch
                {

                }
            }
            else
            {
                bool isMatch = false;
               
                try
                {
                    foreach (var config in Config.PlugConfigs)
                    {
                        if (InputFilePath.EndsWith(config.Exction))
                        {
                            var assembly = Assembly.Load(File.ReadAllBytes(Config.PlugPath  + "\\" + config.DllName));
                            if (assembly != null)
                            {
                                var type = assembly.GetType(config.Type).BaseType;
                                if (type != null && type.Name == typeof(BasePlug).Name)
                                {
                                    var plug = Activator.CreateInstance(assembly.GetType(config.Type));
                                    if (plug != null)
                                    {
                                        log.AppendLine("//User Plug:" + config.DllName + " Type:" + config.Type);
                                        var method = plug.GetType().GetMethod("GenerateOnFile");
                                        if (method != null)
                                        {
                                            //plug.GenerateOnFile(codes, wszInputFilePath, bstrInputFileContents,
                                            //                    wszDefaultNamespace);
                                            method.Invoke(plug, new object[]
                                                                {
                                                                    codes,
                                                                    InputFilePath,
                                                                    inputFileContent,
                                                                    FileNameSpace,
                                                                    Config.PlugPath + config.DllName
                                                                });
                                            isMatch = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    isMatch = false;
                    MessageBox.Show(e.ToString());
                }
                if (!isMatch)
                {
                    System.Diagnostics.Trace.WriteLine("Run PlugManager...");

                    try
                    {
                        new PlugManager().Show();
                    }
                    catch
                    {

                    }
                }
            }
            codes.AppendLine(log.ToString());

            using (StringWriter writer = new StringWriter(new StringBuilder()))
            {
                if (this.CodeGeneratorProgress != null)
                {
                    //Report that we are done
                    this.CodeGeneratorProgress.Progress(100, 100);
                }
                writer.Write(codes.ToString());
                writer.Flush();


                //Get the Encoding used by the writer. We're getting the WindowsCodePage encoding, 
                //which may not work with all languages
                Encoding enc = Encoding.GetEncoding(writer.Encoding.WindowsCodePage);

                //Get the preamble (byte-order mark) for our encoding
                byte[] preamble = enc.GetPreamble();
                int preambleLength = preamble.Length;

                //Convert the writer contents to a byte array
                byte[] body = enc.GetBytes(writer.ToString());

                //Prepend the preamble to body (store result in resized preamble array)
                Array.Resize<byte>(ref preamble, preambleLength + body.Length);
                Array.Copy(body, 0, preamble, preambleLength, body.Length);

                //Return the combined byte array
                return preamble;                    

            }

        }
    }
}
