using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace CsvCodeGen
{

    public class ExcelConfigCodeBuilder : FileGenLoaderDomain.BasePlug
    {
        public override string Exction
        {
            get { return ".xlsx.txt"; }
        }

        public override void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace,string dllpath)
        {
            new ExcelCodeBuilder().GenerateOnFile(codes, wszInputFilePath.Replace(".txt", ""), bstrInputFileContents,
                wszDefaultNamespace,dllpath);
        }
    }

    public class Excel2ConfigCodeBuilder : FileGenLoaderDomain.BasePlug
    {
        public override string Exction
        {
            get { return ".xls"; }
        }

        public override void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace, string dllpath)
        {
            new ExcelCodeBuilder().GenerateOnFile(codes, wszInputFilePath.Replace(".txt", ""), bstrInputFileContents,
                wszDefaultNamespace, dllpath);
        }
    }

    public class ExcelCodeBuilder : FileGenLoaderDomain.BasePlug
    {
        public override string Exction
        {
            get { return ".xlsx"; }
        }

         static ExcelCodeBuilder()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

         static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
         {
             if (args.Name.IndexOf("SharpZip") >= 0)
             {
                 var stream = typeof(ExcelCodeBuilder).Assembly.GetManifestResourceStream("CsvCodeGen.ICSharpCode.SharpZipLib.dll");
                 var buffer = new byte[stream.Length];
                 stream.Read(buffer, 0, buffer.Length);
                 Assembly ass = Assembly.Load(buffer);
                 return ass;
             }
             if (args.Name.IndexOf("Excel") >= 0)
             {
                 var stream = typeof(ExcelCodeBuilder).Assembly.GetManifestResourceStream("CsvCodeGen.Excel.dll");
                 var buffer = new byte[stream.Length];
                 stream.Read(buffer, 0, buffer.Length);
                 Assembly ass = Assembly.Load(buffer);
                 return ass;
             }
             return null;
         }

        public string FixRowName(string oldRowName)
        {
            string strRowName = oldRowName;


            if (strRowName.IndexOf("<") != -1)
            {
                strRowName = strRowName.Replace("<", "_x003C_");
            }
            if (strRowName.IndexOf(">") != -1)
            {
                strRowName = strRowName.Replace(">", "_x003E_");
            }
            if (strRowName.IndexOf("\"") != -1)
            {
                strRowName = strRowName.Replace("\"", "_x0022_");
            }
            if (strRowName.IndexOf("*") != -1)
            {
                strRowName = strRowName.Replace("*", "_x002A_");
            }
            if (strRowName.IndexOf("%") != -1)
            {
                strRowName = strRowName.Replace("%", "_x0025_");
            }
            if (strRowName.IndexOf("&") != -1)
            {
                strRowName = strRowName.Replace("&", "_x0026_");
            }
            if (strRowName.IndexOf("(") != -1)
            {
                strRowName = strRowName.Replace("(", "_x0028_");
            }
            if (strRowName.IndexOf(")") != -1)
            {
                strRowName = strRowName.Replace(")", "_x0029_") ;
            }
            if (strRowName.IndexOf("=") != -1)
            {
                strRowName = strRowName.Replace("=", "_x003D_");
            }
           
            return strRowName.Replace(" ", "_x0020_")
                  .Replace("%", "_x0025_")
                  .Replace("#", "_x0023_")
                  .Replace("&", "_x0026_")
                  .Replace("（", "_xFF08_")
                  .Replace("）", "_xFF09_")
                  .Replace("/", "_x002F_"); ;
        }

        Dictionary<string,string> EnumMap = new Dictionary<string, string>();

        private StringBuilder EnumConver = new StringBuilder();

        public string BuildConfig(string wszInputFilePath, string wszDefaultNamespace,string dllpath)
        {
            EnumConver = new StringBuilder();
            var tables = getTableName(wszInputFilePath);
            var fi = new FileInfo(wszInputFilePath);
            var config = new List<string>(tables).Find(r => r.ToLower() == "@@config" || r == "Sheet2$" || r == "Sheet2");

            var headmapdic = CsvCodeBuilder.GetTypeNameMapDefine(wszInputFilePath, dllpath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace {0} {".Replace("{0}", wszDefaultNamespace));
            if (config != null)
            {
                var ds = importExcelToDataSet(wszInputFilePath, config).Tables[0];
                for (int i = 0; i < ds.Columns.Count; i++)
                {
                    var colum = ds.Columns[i];
                    if (!string.IsNullOrEmpty(colum.ColumnName) && !colum.ColumnName.StartsWith("Column"))
                    {
;

                        List<string> Configs = new List<string>();
                        List<string> EnConfigs = new List<string>();
                        var finame = fi.Name.Replace(fi.Extension, "");

                        for (int j = 0; j < ds.Rows.Count; j++)
                        {
                            var data = ds.Rows[j][i] + "";
                            if (!string.IsNullOrEmpty(data))
                            {
                                Configs.Add(data);
                                var head = CsvCodeBuilder.HeadFix(data);
                              
                           
                                if (headmapdic.ContainsKey(finame + "." + head))
                                {
                                    head = headmapdic[finame + "." + head];
                                }
                                else if (headmapdic.ContainsKey(head))
                                {
                                    head = headmapdic[head];
                                }
                                else
                                {
                                    if (headmapdic.ContainsKey("#srcurl") && headmapdic.ContainsKey("#dstmath"))
                                    {
                                        head = CsvCodeBuilder.GetTranslateByUrl(
                                            headmapdic["#srcurl"],
                                            headmapdic["#dstmath"],
                                            head,
                                            wszInputFilePath
                                            );
                                    }
                                }
                                EnConfigs.Add(head);
                            }
                        }

                        //buid
                        var classname = FixRowName(colum.ColumnName);


                        if (headmapdic.ContainsKey(finame + "." + classname))
                        {
                            classname = headmapdic[finame + "." + classname];
                        }
                        else if (headmapdic.ContainsKey(classname))
                            classname = headmapdic[classname];
                        else
                        {
                            if (headmapdic.ContainsKey("#srcurl") && headmapdic.ContainsKey("#dstmath"))
                            {
                                classname = CsvCodeBuilder.GetTranslateByUrl(
                                    headmapdic["#srcurl"],
                                    headmapdic["#dstmath"],
                                    classname,
                                    wszInputFilePath
                                    );
                            }
                        }
                        sb.AppendLine(@"
        /// <summary>
        /// {oldhead}
        /// </summary>".Replace("{oldhead}", colum.ColumnName));
                        //添加枚举映射
                        EnumMap[colum.ColumnName] = classname;
                        var classtxt = @"public enum Enum_{class}{";
                        sb.AppendLine(classtxt.Replace("{class}", classname));


                        var bodytpl = @" 
case ""{head}"":
      return Enum_{class}.{new};
                        ";
                        var body = new StringBuilder();
                        for (int j = 0; j < Configs.Count; j++)
                        {
                            var oldhead = Configs[j];
                            var newhead = EnConfigs[j];
                            sb.AppendLine(@"
        /// <summary>
        /// {oldhead}
        /// </summary>".Replace("{oldhead}", oldhead));
                            sb.AppendLine(newhead + ",");

                            body.AppendLine(bodytpl
                                .Replace("{head}", oldhead)
                                .Replace("{new}", newhead)
                                 .Replace("{class}", classname)
                                );
                        }

                        EnumConver.AppendLine(@"
                    ExctionClass.RegEnumConvert<Enum_{class}>(o =>
                    {
                        var str = o.ToString();
                        switch (str)
                        {
                            {body}
                        }
                        return Enum_{class}.{0};
                });
                "
                            .Replace("{class}", classname)
                            .Replace("{body}", body.ToString())
                            .Replace("{0}", EnConfigs[0])

                            );

                        {


                            var bodytplback = @" 
case Enum_{class}.{new}:
      return ""{head}"";
                        ";
                            var bodyback = new StringBuilder();
                            for (int j = 0; j < Configs.Count; j++)
                            {
                                var oldhead = Configs[j];
                                var newhead = EnConfigs[j];

                                bodyback.AppendLine(bodytplback
                                    .Replace("{head}", oldhead)
                                    .Replace("{new}", newhead)
                                     .Replace("{class}", classname)
                                    );
                            }

                            EnumConver.AppendLine(@"
                    ExctionClass.RegEnumConvertBack<Enum_{class}>(o =>
                    {
                        var str = (Enum_{class})o;
                        switch (str)
                        {
                            {body}
                        }
                        return """";
                });
                "
                                .Replace("{class}", classname)
                                .Replace("{body}", bodyback.ToString())
                                .Replace("{0}", EnConfigs[0])

                                );

                        }

                        sb.AppendLine("}");
                    }
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }




        public override void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace,string dllpath)
        {

          

            codes.AppendLine("//Code Gen By ExcelCodeBuilder V1.3 ...");
            codes.AppendLine("//Addin path " + dllpath);
            codes.AppendLine(@"
using System;
using System.Collections.Generic;
using System.Xml;
");
            var tables = getTableName(wszInputFilePath);

            //先创建枚举类型
            var eunmstr = BuildConfig(wszInputFilePath, wszDefaultNamespace,dllpath);
            codes.AppendLine(eunmstr);
            codes.AppendLine("//eunms Gen By ExcelCodeBuilder  ...");

            int tabindex = -1;
            foreach (var table in tables)
            {
                tabindex ++;
                var ds = importExcelToDataSet(wszInputFilePath, table).Tables[0];
                if (table.StartsWith("@@") || table.StartsWith("__"))
                {
                    //不导出此类表
                    continue;
                }
              
                var fileInfo = new FileInfo(wszInputFilePath);
            
                if (!File.Exists(fileInfo.Directory.FullName + "//ConfigHelper.cs"))
                {
                   
                    var cspath = new FileInfo(dllpath).Directory.FullName + "//Tpl//ConfigHelper.cs";

                    if (File.Exists(cspath))
                    {
                        File.Copy(cspath, fileInfo.Directory.FullName + "//ConfigHelper.cs", true);
                    }
                }
                var classname = fileInfo.Name.Replace(fileInfo.Extension, "");

                string key = null;
                StringBuilder body = new StringBuilder();
                StringBuilder method = new StringBuilder();
                StringBuilder setbody = new StringBuilder();
                StringBuilder igetbody = new StringBuilder();
                StringBuilder isetbody = new StringBuilder();
                if (table != "Sheet1$" && table != "Sheet1")
                {
                    classname = table;
                }
                classname = CsvCodeBuilder.HeadFix(classname);
                string oldclassname = classname;
                var headmapdic = CsvCodeBuilder.GetTypeNameMapDefine(wszInputFilePath, dllpath);

                if (headmapdic.ContainsKey(classname))
                    classname = headmapdic[classname];
                else
                {
                    if (headmapdic.ContainsKey("#srcurl") && headmapdic.ContainsKey("#dstmath"))
                    {
                        classname = CsvCodeBuilder.GetTranslateByUrl(
                            headmapdic["#srcurl"],
                            headmapdic["#dstmath"],
                            classname,
                            wszInputFilePath
                            );
                    }
                }

                if (headmapdic.ContainsKey("#xmlout"))
                {
                    var path = fileInfo.Directory.FullName + headmapdic["#xmlout"];
                    path = new FileInfo(path).FullName + "\\";
                    codes.AppendLine("//xmlout " + path);

                    path += classname;
                    MyConvertExcelToXml(wszInputFilePath, path, table);


                }

                //if (!classname.EndsWith("Table"))
                //{
                //    classname = classname + "Table";
                //}
                //建立主体
                {

                    var dic = CsvCodeBuilder.GetTypeDefine(wszInputFilePath, dllpath);



                    string args = "";
                    string IDHead = "ID";
                    for (int i = 0; i < ds.Columns.Count; i++)
                    {
                        string head = ds.Columns[i].ColumnName;
                        string oldhead = head;
                        if (head.StartsWith("@@")) //直接截断
                            break;

                        if (head.StartsWith("__")) //忽略
                            continue;

                        if (head.StartsWith("_"))//移除下划线
                        {
                            head = head.Substring(1);
                        }
                        if (head.StartsWith("__"))//移除下划线
                        {
                            head = head.Substring(2);
                        }
                        if (head == "")
                            head = "h" + i;

                        string type = "string";


                        type = ds.Columns[i].DataType.Name;

                        if (type == "Double")
                        {
                            type = "int";
                        }
                     

                        head = CsvCodeBuilder.HeadFix(head);

                        

                        if (dic.ContainsKey(classname + "." + head))
                        {
                            type = dic[classname + "." + head];
                        }
                        else if (dic.ContainsKey(head))
                        {
                            type = dic[head];
                        }

                        if (headmapdic.ContainsKey(classname + "." + head))
                        {
                            head = headmapdic[classname + "." + head];
                        }
                        else if (headmapdic.ContainsKey(head))
                        {
                            head = headmapdic[head];
                        }
                        else
                        {
                            if (headmapdic.ContainsKey("#srcurl") && headmapdic.ContainsKey("#dstmath"))
                            {
                                head = CsvCodeBuilder.GetTranslateByUrl(
                                    headmapdic["#srcurl"],
                                    headmapdic["#dstmath"],
                                    head,
                                    wszInputFilePath
                                    );
                            }
                        }

                        if (i == 0)
                        {
                            IDHead = head;
                        }


                        if (key == null)
                            key = type;

                        if (type == "枚举" || type == "enum")
                        {
                            if (EnumMap.ContainsKey(ds.Columns[i].ColumnName))
                            {
                                type = "Enum_" +  EnumMap[ds.Columns[i].ColumnName];
                            }
                        }

                        setbody.AppendLine("case \"{0}\":".Replace("{0}", ds.Columns[i].ColumnName));

                        var fixRowName = FixRowName(ds.Columns[i].ColumnName);
                        if (fixRowName != ds.Columns[i].ColumnName)
                        {
                            setbody.AppendLine("case \"{0}\":".Replace("{0}", fixRowName));
                        }

                        //setbody.AppendLine("this._" + head + " =new Func<" + type +  ">(()=> { return  ConfigHelper.ConvertTo<" + type + ">(localnode.InnerText); });");
                        setbody.AppendLine("this._" + head + " =   ConfigHelper.ConvertTo<" + type + ">(localnode.InnerText); ");

                        setbody.AppendLine("break;");

                        body.AppendLine(@"
        /// <summary>
        /// {oldhead}
        /// </summary>".Replace("{oldhead}", oldhead)
                            );

                        //if (i == 0)
                        //    body.AppendLine("\t\t[Config(Index = 0, IsKey = true, Desc = \"" + ds.Columns[i].ColumnName + "\")]");
                        //else
                        //    body.AppendLine("\t\t[Config(Index = " + i + " , Desc = \"" + ds.Columns[i].ColumnName + "\")]");

                        body.AppendLine(@"
 //ConfigValue<{0}> _{1} ;
{0} _{1};

/// <summary>
/// {oldhead}
/// </summary>
[ConfigItemMap(""{oldhead}"")]
public {0} {1} { 
    get{
       return _{1};
}
  set {
    _{1} = value;
}
    
}

"
                            .Replace("{0}", type)
                            .Replace("{1}", head)
                            .Replace("{oldhead}", oldhead)
                            
                            );

                        igetbody.Append(@"
if (memberName == '{1}')
{
    return {1};
}
".Replace('\'', '\"')
 .Replace("{1}", head)

 );

                        isetbody.Append(@"
 if (memberName == '{1}')
{
    obj.{1} = ({0})newValue;
}
".Replace('\'', '\"')
 .Replace("{1}", head)
 .Replace("{0}", type)
 );


                        if (args != "")
                        {
                            args += ",";
                        }
                        args += type + " " + head;
                    }



                    method.AppendLine(
                        @"
#if !RELEASE_CLIENT
        string xmldata = null;

        public override string ToString()
        {
            return xmldata;
        }
#endif
        /// <summary>
        /// ID
        /// </summary>
        //public string xmlcrc { get;set; }

        public bool CheckId()
        {
            if ({ID} > 0)
            {
                return true;
            }
            return false;
        }


        public int GetId()
       {
            return   ConfigHelper.ConvertTo<int>({ID});
        }
        public void Parse(XmlNode xmlnode)
        {
#if !RELEASE_CLIENT
                 xmldata = xmlnode.InnerXml;
#endif
                //xmlcrc = xmlnode.InnerXml.GetHashCode().ToString();
                foreach(XmlNode node in xmlnode)
                {
                    var localnode = node;
                    switch(node.Name)
                    {
                        {body}
                    }
                }
        }


"
                            .Replace("{body}", setbody.ToString())
                            .Replace("{ID}",IDHead)

                        );


                    body.AppendLine(method.ToString());

                }


                //建立类
                codes.AppendLine(@"

namespace {0} 
{
    /// <summary>
    /// {oldclass}
    /// </summary>
    [Serializable]
    //[Config(Index = {index})]
    public partial class {1} 
#if MA
       :IMemberAccessor
#endif
    {

{body}
        public object GetValue(object instance, string memberName)
        {
            {getbody}
            return null;
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
             var obj = instance as {1};
            {setbody}
        }
    }

    
  
    

"
                    .Replace("{0}", wszDefaultNamespace)
                    .Replace("{1}", classname)
                    .Replace("{body}", body.ToString())
                    .Replace("{getbody}", igetbody.ToString())
                    .Replace("{setbody}", isetbody.ToString())
                    .Replace("{ext}", "\"" + Exction + "\"")
                    .Replace("{oldclass}", oldclassname)
                    .Replace("{index}", tabindex.ToString())

                    );

                BuildConfigReadClass(codes, classname,oldclassname);

                codes.AppendLine("}");
                if (table == "Sheet1$" || table == "Sheet1")
                    break;
            }
         
        }

        public void BuildConfigReadClass(StringBuilder codes, string classname, string oldclass)
        {
            codes.AppendLine(@"
    /// <summary>
    /// {oldclass}
    /// </summary>
    public partial class {class}Configs
    {
        public readonly static  string path = ConfigHelper.GetXmlPath + '{class}.xml';

        private static {class}Configs _Configs;
        public static {class}Configs Configs
        {
            get
            {
                if (_Configs == null)
                {
                    var config = Datas;
                }

                return _Configs;
            }
        }

        public static DateTime LastRead { get ; private set;}

        static  Dictionary<int, {class}> DatasMap { get; set; }

        public {class} this[int key]
        {
            get { return DatasMap[key]; }
        }

        public static List<{class}> Datas
        {
            get
            {
                if (_Configs == null)
                {
                     ConvertInit();
                     var cfgs = new {class}Configs();
                    cfgs.Init(null);
                    LastRead = DateTime.Now;
                    _Configs = cfgs;
                   
                }
                return Configs.DataList;
            }
        }

        public static void ConvertInit(){
              {enumbody}
        }

        public static void ReLoad(){
            _Configs = null;
            //var x = Datas;
        }

        private List<{class}> _dateList = new List<{class}>();

        public List<{class}> DataList
        {
            get { return _dateList; }
            set { _dateList = value; }
        }

        public void Init(string inputxml)
        {
            if (string.IsNullOrEmpty(inputxml))
            {
                inputxml = ConfigHelper.GetXml(path, this.Init);
            }
           if (   string.IsNullOrEmpty( inputxml) || inputxml.StartsWith('<'))
           {
                DataList.Clear();
                XmlDocument doc = new XmlDocument();
                XmlReader reader = null;
                if (inputxml != null)
                {
                    doc.LoadXml(inputxml);
                }
                else
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.StreamReader file = null;
                        try
                        {
                            file = System.IO.File.OpenText(path);
                            inputxml = file.ReadToEnd();
                            doc.LoadXml(inputxml);
                        }
                        finally
                        {
                            if (file != null)
                            {
                                file.Close();
                                file.Dispose();
                            }
                        }
                    }
                    else
                    {
                       
                        return;
                        
                    }
                }

                XmlNode xn = doc.ChildNodes[1];

       
                foreach (XmlNode node in xn.ChildNodes)
                {
                    try{
    
                    var item = new {class}();
                    item.Parse(node);
                    if(item.CheckId()){
                        DataList.Add(item);
                    }
                

                    }
                    catch (Exception e)
                    {
                        ConfigHelper.Log('Lost:' + node.InnerXml + ' ' + e.ToString());
                    }
                }
               }
              else
            {
#if !SV
             DataList =  ConfigHelper.ToObject<List<{class}>>(inputxml);
#endif
            }
            DatasMap= new Dictionary<int, {class}>();
            foreach (var item in DataList)
            {
                DatasMap[item.GetId()] = item;
            }

        }
    }".Replace("{class}", classname)
      .Replace("{oldclass}", oldclass)
      .Replace("{enumbody}", EnumConver.ToString())
                .Replace("'", "\""));
        }

        public static DataSet importExcelToDataSet(string FilePath, string TableName)
        {
            return importExcelToDataSet(FilePath, TableName, false);
        }
        // Methods
        public static DataSet importExcelToDataSet(string FilePath, string TableName,bool dooutall)
        {
#if !MONO
            try
            {


                string strConn;
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath +
                          ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FilePath + ";Extended Properties=Excel 8.0;"; 
                OleDbConnection OleDbConn = new OleDbConnection(strConn);
                OleDbConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [" + TableName + "]", OleDbConn);
                DataSet myDataSet = new DataSet();
                try
                {
                    myCommand.Fill(myDataSet);
                }
                catch (Exception ex)
                {
                    throw new Exception("该Excel文件的工作表的名字不正确," + ex.Message);
                }
                OleDbConn.Close();
                return myDataSet;
            }
            catch (Exception ex)
#endif
            {
                var ds = Excel2.importExcelToDataSet(FilePath, TableName);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var table = ds.Tables[0];

                    var newtable = new DataTable(table.TableName);
                    var row = table.Rows[0];
                    foreach (DataColumn column in table.Columns)
                    {
                        var obj = row[column.ColumnName];
                        var newcol = new DataColumn(column.ColumnName);
                        if (obj is int)
                        {
                            newcol.DataType = typeof (int);
                        }
                        else if (obj is float)
                        {
                            newcol.DataType = typeof (float);
                        }
                        else if (obj is double)
                        {
                            newcol.DataType = typeof (double);
                            newcol.DataType = typeof(int);
                            foreach (DataRow row1 in table.Rows)
                            {
                              
                                try
                                {
                                    double value = (double)row1[column.ColumnName];
                                    if (value - (int)value > 0)
                                    {
                                        newcol.DataType = typeof(float);
                                    }
                                }
                                catch 
                                {
                                    
                                   
                                }
                            }
                            
                        }
                        else if (obj is DateTime)
                        {
                            newcol.DataType = typeof (DateTime);
                        }
                        else
                        {
                            newcol.DataType = typeof (string);
                        }
                        if (dooutall)
                        {
                            newcol.DataType = typeof (string);
                        }
                        newtable.Columns.Add(newcol);

                    }
                    var tableclone = newtable.Clone();

                    if (newtable != null)
                    {
                        newtable.BeginLoadData();
                        foreach (DataRow myrow in table.Rows)
                        {

                            try
                            {
                                newtable.ImportRow(myrow);
                            }
                            catch (Exception)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (var o in myrow.ItemArray)
                                {
                                    sb.Append(o.ToString() + ",");
                                }
                                Console.WriteLine("Lost:" + sb.ToString());
                            }
                        }

                        newtable.EndLoadData();


                    }

                    ds.Tables.RemoveAt(0);
                    if (TableName == null)
                    {
                        ds.Tables.Add(newtable);
                    }
                    else  if (table.TableName == TableName)
                    {
                        ds.Tables.Clear();
                        ds.Tables.Add(newtable);
                        break;
                    }
                    else
                    {
                        i--;
                    }
                       
                }



                return ds;
            }
        }

        public static string[] getTableName(string FilePath)
        {
#if !MONO
            try
            {

                string strConn;
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath +
                          ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FilePath + ";Extended Properties=Excel 8.0;";
                OleDbConnection OleDbConn = new OleDbConnection(strConn);
                OleDbConn.Open();
                DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                    new object[] {null, null, null, "TABLE"});
                List<string> sl = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!dt.Rows[i]["TABLE_NAME"].ToString().StartsWith("_xlnm"))
                        sl.Add((String) dt.Rows[i]["TABLE_NAME"]);
                }
                OleDbConn.Close();
                return sl.ToArray();
            }
            catch (Exception ex)
#endif
            {
                var names = Excel2.getTableName(FilePath);


                return names;
            }
        }


        public static void MyConvertExcelToXml(string excelFilePath, string xmlOutFilePath,string tablename)
        {
            var names = getTableName(excelFilePath);
            foreach (var name in names)
            {
                if (tablename == name || name==null)
                {
                    var ds = importExcelToDataSet(excelFilePath, name, true);
                    var ds2 = importExcelToDataSet(excelFilePath, name, false);
                    var table = ds.Tables[0];
                    var table2 = ds2.Tables[0];
                    var removeName = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName.StartsWith("_"))
                        {
                            removeName.Add(column.ColumnName);
                        }
                    }

                    foreach (var colName in removeName)
                    {
                        table.Columns.Remove(colName);
                        table2.Columns.Remove(colName);
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        for (int j = 0; j < row.ItemArray.Length; j++)
                        {
                            var value = row[j];
                            if (value.ToString() == "")
                            {
                                if (table2.Columns[j].DataType == typeof(string) ||
                                    table2.Columns[j].DataType == typeof(DateTime))
                                {
                                    row[j] = "";
                                }
                                else
                                {
                                    row[j] = 0;
                                }
                            }
                        }
                    }



                    ds.WriteXml(xmlOutFilePath + ".xml");
                    ds.Dispose();
                    Console.WriteLine(xmlOutFilePath + ".xml");
                }
               
            }
          
        }


        public static void MyConvertExcelToXmlI(string excelFilePath, string xmlOutFileDic, string tablename)
        {
            var names = getTableName(excelFilePath);
            var dic = new FileInfo(excelFilePath).Directory.FullName;
            var headmapdic = new Dictionary<string, string>();
            try
            {
                Console.WriteLine(dic + "//nameMap.txt");
                headmapdic = CsvCodeBuilder.GetTypeNameMapDefine(dic + "//nameMap.txt", "./");

            }
            catch (Exception)
            {

            }

            var dump = false;
            foreach (var name in names)
            {
                if (tablename == name || tablename == null)
                {
                    if (tablename ==null && name.StartsWith("@@"))
                    {
                        break;
                    }
                    if(name.StartsWith("_"))
                        continue;

                    var classname = CsvCodeBuilder.HeadFix(new FileInfo(excelFilePath).Name.Replace(".xlsx", ""));

                    if (name != "Sheet1")
                    {
                        classname = CsvCodeBuilder.HeadFix(name);

                    }
                    else
                    {
                        dump = true;
                    }
                    if (headmapdic.ContainsKey(classname))
                        classname = headmapdic[classname];
                   
                    var ds = importExcelToDataSet(excelFilePath, name, true);
                    var ds2 = importExcelToDataSet(excelFilePath, name, false);
                    var table = ds.Tables[0];
                    var table2 = ds2.Tables[0];
                    var removeName = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName.StartsWith("_"))
                        {
                            removeName.Add(column.ColumnName);
                        }
                    }

                    foreach (var colName in removeName)
                    {
                        table.Columns.Remove(colName);
                        table2.Columns.Remove(colName);
                    }

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        for (int j = 0; j < row.ItemArray.Length; j++)
                        {
                            var value = row[j];
                            if (value.ToString() == "")
                            {
                                if (table2.Columns[j].DataType == typeof(string) ||
                                    table2.Columns[j].DataType == typeof(DateTime))
                                {
                                    row[j] = "";
                                }
                                else
                                {
                                    row[j] = 0;
                                }
                            }
                        }
                    }


                    if (xmlOutFileDic.EndsWith("\\"))
                    {
                        ds.WriteXml(xmlOutFileDic + classname + ".xml");
                        ds.Dispose();
                        Console.WriteLine(xmlOutFileDic + classname + ".xml");
                    }
                    else
                    {
                        ds.WriteXml(xmlOutFileDic + ".xml");
                        ds.Dispose();
                        Console.WriteLine(xmlOutFileDic + ".xml");
                    }
                
                    if (dump)
                        break;
                }

            }

        }

    }


    public class Excel2
    {
        // Methods
        public static DataSet importExcelToDataSet(string FilePath, string TableName)
        {

            FileStream stream = null;
            try
            {
               // stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                var fileinfo = new FileInfo(FilePath);
                var newpath = fileinfo.Directory.FullName + "//temp" + fileinfo.Extension;
                File.Copy(FilePath, newpath, true);
                FilePath = newpath;
                stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);

            }
            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            Excel.IExcelDataReader excelReader = null;

            if (FilePath.EndsWith("xls"))
            {
                excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            excelReader.IsFirstRowAsColumnNames = true;
       
            DataSet result = excelReader.AsDataSet();
            foreach (DataTable table in result.Tables)
            {
                if (table.TableName == TableName)
                {
                    //table.Rows.RemoveAt(0);
                    return result;
                }
            }
            //result.Tables[0].Rows.RemoveAt(0);
            return result;
        }

        public static string[] getTableName(string FilePath)
        {
            FileStream stream = null;
            try
            {
                
               // stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                var fileinfo = new FileInfo(FilePath);
                var newpath = fileinfo.Directory.FullName + "//temp" + fileinfo.Extension;
                File.Copy(FilePath, newpath,true);

                FilePath = newpath;
                stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);

            }
          

            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            Excel.IExcelDataReader excelReader = null;

            if (FilePath.EndsWith("xls"))
            {
                excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            DataSet result = excelReader.AsDataSet();

            List<string> sl = new List<string>();
            foreach (DataTable table in result.Tables)
            {
                sl.Add(table.TableName);
            }
            return sl.ToArray();
        }

    }

}