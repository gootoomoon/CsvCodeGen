using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvCodeGen
{
    public class CsvConfigCodeBuilder : FileGenLoaderDomain.BasePlug
    {
        public override string Exction
        {
            get { return ".csv.txt"; }
        }

        public override void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
                                            string wszDefaultNamespace,string dllpath)
        {
            new ExcelCodeBuilder().GenerateOnFile(codes, wszInputFilePath.Replace(".txt", ""), bstrInputFileContents,
                                                  wszDefaultNamespace,dllpath);
        }
    }
    public class CsvCodeBuilder : FileGenLoaderDomain.BasePlug
    {
        
       
        public static Dictionary<string,string> GetTypeDefine(string wszInputFilePath,string dllpath)
        {
            var dic = new Dictionary<string, string>();
            if(File.Exists(wszInputFilePath + ".txt"))
            {
                foreach (var line in File.ReadAllLines(wszInputFilePath + ".txt"))
                {
                    var values = line.Split('|');
                    if(values.Length==2)
                    {
                        dic[values[0]] = values[1];
                    }
                }
            }
            var fileinfo = new FileInfo(wszInputFilePath);
            string path = fileinfo.Directory.FullName + "\\typeMap.txt";
            if (!File.Exists(path))
            {
                var cspath = new FileInfo(dllpath).Directory.FullName + "//Tpl//typeMap.txt";

                if (File.Exists(cspath))
                {
                    File.Copy(cspath, path, true);
                }
            }
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    var values = line.Split('|');
                    if (values.Length == 2)
                    {
                        dic[values[0]] = values[1];
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 自动翻译
        /// </summary>
        /// <param name="url">http://openapi.baidu.com/public/2.0/bmt/translate?client_id=2uieGC72Id4B8gbH4W4fY6OS&q={src}&from=zh&to=en</param>
        /// <param name="retMatch">"dst":"(?<dst>([A-Za-z\s]+))"</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetTranslateByUrl(string url, string retMatch, string input, string wszInputFilePath)
        {
            try
            {
                WebClient client = new WebClient();
                var ret = client.DownloadString(url.Replace("{src}", input)).Replace("'", "");

                Regex regex = new Regex(retMatch);
                var match = regex.Match(ret);
                var value = match.Groups["dst"].Value;
                StringBuilder sb = new StringBuilder();

                foreach (var subvalue in value.Split(' '))
                {
                    sb.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subvalue));

                }

                var retstr = sb.ToString().Trim();

                if (!string.IsNullOrEmpty(retstr))
                {
          
                    try
                    {
                        //保存到namemap.txt
                        var fileinfo = new FileInfo(wszInputFilePath);
                        string path = fileinfo.Directory.FullName + "\\nameMap.txt";
                        if (File.Exists(path))
                        {
                          
                            var sw = File.AppendText(path);
                            sw.WriteLine(input + "|" + retstr);
                            sw.Flush();
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                    catch(Exception e)
                    {

                        System.Diagnostics.Trace.Write(e.ToString());
                    }
                    return retstr;
                }
            }
            catch (Exception ex)
            {
               
            }
             return input;
        }

        public static Dictionary<string, string> GetTypeNameMapDefine(string wszInputFilePath,string dllpath)
        {
            var dic = new Dictionary<string, string>();
            var fileinfo = new FileInfo(wszInputFilePath);
            string path = fileinfo.Directory.FullName + "\\nameMap.txt";
            if (!File.Exists(path))
            {

                var cspath = new FileInfo(dllpath).Directory.FullName + "//Tpl//nameMap.txt";

                if (File.Exists(cspath))
                {
                    File.Copy(cspath, path , true);
                }
            }
            if (File.Exists(path))
            {
                var fr = File.OpenText(path);
                while (!fr.EndOfStream)
                {
                  var line =  fr.ReadLine();
                  var values = line.Split('|');
                  if (values.Length == 2)
                  {
                      dic[values[0]] = values[1];
                  }
                }
                fr.Close();
                fr.Dispose();
            }
          
            return dic;
        }

        public  static string HeadFix(string head)
        {
            var item = head[0].ToString();
            int x = 0;
            if(int.TryParse(item,out x))
            {
                head = "H" + head;
            }

            return head
                .Replace("-", "_")
                .Replace("$", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("（", "")
                .Replace("/", "")
                .Replace("）", "")
                .Replace(";", "")
                .Replace("_float_", "")
                .Replace("_int_", "")
                .Replace("_string_", "")
                ;

        }

        public override void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
                                            string wszDefaultNamespace,string dllpath)
        {
            codes.AppendLine("//Code Gen By CsvCodeBuilder...");
            var fileInfo = new FileInfo(wszInputFilePath);
            var classname = fileInfo.Name.Replace(Exction, "");
            classname = HeadFix(classname);
            string key = null;
            StringBuilder body = new StringBuilder();
            StringBuilder method = new StringBuilder();
            StringBuilder setbody = new StringBuilder();
            //建立主体
            {
                var lines = bstrInputFileContents.Split('\n');

                var heads = lines[0].Split(',');
                codes.AppendLine("//" + lines[0] + " //count:" + heads.Length);

                string args = "";
                var dic = GetTypeDefine(wszInputFilePath,dllpath);

                for (int i = 0; i < heads.Length; i++)
                {
                    string head = heads[i].Trim();
                    if (head == "")
                        head = "h" + i;
                    string type = "string";



                    head = HeadFix(head);

                    if (dic.ContainsKey(head))
                        type = dic[head];


                    if (key == null)
                        key = type;
                    setbody.AppendLine("this." + head + " = " + head + ";");

                    if(i==0)
                        body.AppendLine("\t\t[Config(Index = 0, IsKey = true)]");
                    else
                        body.AppendLine("\t\t[Config(Index = " + i + ")]");

                    body.AppendLine("\t\tpublic {0} {1} { get;set; }"
                                        .Replace("{0}", type)
                                        .Replace("{1}", head)
                        );




                    if (args != "")
                    {
                        args += ",";
                    }
                    args += type + " " + head;
                }



                method.AppendLine(
                    @"

public void Parse({args})
{
   {body}
}
"
                        .Replace("{args}", args)
                        .Replace("{body}", setbody.ToString())

                    );


                body.AppendLine(method.ToString());

            }

            string configs =
                @"
             [ConfigAutoParse(File = {file}, ParseType = {type})]
             public static Config<{key}, {class}> {class} = new Config<{key}, {class}>();
".Replace("{file}","\"" + fileInfo.Name + "\"")
 .Replace("{type}","\"CsvCodeGen.CsvParse\"")
 .Replace("{class}", classname)
 .Replace("{key}", key)
                ;//CsvParse

            //建立类
            codes.AppendLine(@"
using System;
using CsvCodeGen;
namespace {0} 
{

    [Serializable]
    public partial class {1} 
    {
{body}
    }

    
    public partial class ConfigList : CsvCodeGen.CfsConfig
    {
{config}
    }
}
"
                                 .Replace("{0}", wszDefaultNamespace)
                                 .Replace("{1}", classname)
                                 .Replace("{body}", body.ToString())
                                 .Replace("{ext}", "\"" + Exction + "\"")
                                 .Replace("{config}", configs)

                );




        }

        public override string Exction
        {
            get { return ".csv"; }
        }


    }
}
