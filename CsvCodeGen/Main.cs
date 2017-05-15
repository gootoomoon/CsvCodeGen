using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace CsvCodeGen
{
    public  class MainClass
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length >= 1)
            {
                if (args[0].EndsWith(".xls") || args[0].EndsWith(".xlsx"))
                {
                    ExcelCodeBuilder.MyConvertExcelToXmlI(args[0], args.Length > 1 ? args[1] : args[0],
                        args.Length > 2 ? args[2] : null);
                }
                if (args.Length >= 2)
                {
                    if (args[0].EndsWith(".csproj") || args[1].EndsWith(".csproj"))
                    {
                        Main2(args);
                    }

                    Assembly windNet = null;
                    if (args[0] == "-json")
                    {
                        var fi = new FileInfo(args[1]);
                        AppDomain.CurrentDomain.AssemblyResolve += (sender, eve) =>
                        {
                            var list = eve.Name.Split(',');
                            if (File.Exists(fi.Directory.FullName + "//" + list[0] + ".dll"))
                            {
                                var load = Assembly.LoadFile(fi.Directory.FullName + "//" + list[0] + ".dll");
                                if (load.FullName.IndexOf("WindNet") >= 0)
                                {
                                    windNet = load;
                                }
                                return load;
                            }
                            return null;
                        };


                        System.Environment.CurrentDirectory = fi.Directory.FullName;

                        var ass = Assembly.LoadFile(fi.FullName);
                        foreach (var type in ass.GetTypes())
                        {
                            if ((type.FullName.IndexOf(".ExcelConfigs.") >= 0
                                 || type.FullName.IndexOf(".Configs.") >= 0) &&
                                type.GetProperty("Datas") != null)
                            {

                                try
                                {
                                    Console.WriteLine("Out:" + type.Name);
                                    var value = type.GetProperty("Datas").GetValue(null, null);

                                    var json = fastJSON.JSON.ToJSON(value);

                                    File.WriteAllText(args[2] + type.Name.Replace("Configs",".json") ,json);

                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Out Error:" + e.Message);
                                    Console.WriteLine("Out Error:" + e.InnerException);
                                }


                            }

                        }
                    }
                }


            }

            Console.WriteLine("CsvCodeGen xxx.xls out");
            Console.WriteLine("CsvCodeGen in.csproj out.csproj");
            Console.WriteLine("CsvCodeGen json indll/orexe outPath");
        }

        static void Main2(string[] args)
        {
            string input = args[0];
            string output = args[1];

            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml(File.ReadAllText(input));


            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml(File.ReadAllText(output));

            XmlNode node1 = null;
            XmlNode node2 = null;

            foreach (XmlNode childNode in doc1.ChildNodes[1].ChildNodes)
            {
                if (childNode.Name == "ItemGroup" && childNode.FirstChild.Name == "Compile")
                {
                    node1 = childNode;
                    break;
                }
            }

            foreach (XmlNode childNode in doc2.ChildNodes[1].ChildNodes)
            {
                if (childNode.Name == "ItemGroup" && childNode.FirstChild.Name == "Compile")
                {
                    node2 = childNode;
                    break;

                }
            }
            if (node2.InnerXml != node1.InnerXml)
            {
                node2.InnerXml = node1.InnerXml;
                var xw = new XmlTextWriter(output, Encoding.UTF8);
                doc2.WriteTo(xw);
                xw.Close();
            }
           
        }
    }
}
