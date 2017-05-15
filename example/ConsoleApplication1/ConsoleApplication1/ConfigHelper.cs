using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



public static class ExctionClass
{
    public static string[] SplitX(this string str)
    {
        return str.Split(new char[] { ';', ',', '；', '，', '|', '=' });
    }


    static Dictionary<Type, Func<object, object>> ConvertMap = new Dictionary<Type, Func<object, object>>();

   

    public static void RegConvert<T>(Func<object, object> convert)
    {
        ConvertMap[typeof(T)] = convert;
    }


    static Dictionary<Type, Func<object, string>> ConvertBackMap = new Dictionary<Type, Func<object, string>>();
    public static void RegEnumConvertBack<T>(Func<object, string> convert)
    {
        ConvertBackMap[typeof(T)] = convert;
    }

    public static string GetEnumConvertBack<T>(T value)
    {
        return ConvertBackMap[typeof(T)](value);
    }

    public static void RegEnumConvert<T>(Func<object, object> convert)
    {
        ConvertMap[typeof(T)] = convert;
    }

    public static T ConvertTo<T>(object input, bool userMap = true)
    {
        if (ConvertMap.ContainsKey(typeof(T)))
        {
            return (T)ConvertMap[typeof(T)](input);
        }
        if (input is T)
        {
            return (T)input;
        }
        try
        {
            if (!(input is string))
            {
                input = input + "";
            }
            var str = input as string;
            object obj = default(T);
            if (typeof(T) == typeof(string))
            {
                obj = input;
            }
            else if (typeof(T) == typeof(int))
            {
                int value = 0;
                if (str != null)
                {
                    var istr = str.Replace("%", "");
                    int.TryParse(istr, out value);
                }
                obj = value;

            }
            else if (typeof(T).IsEnum)
            {
                var names = Enum.GetNames(typeof(T));
                if (names.Contains(str))
                {
                    obj = Enum.Parse(typeof(T), str);
                }


            }
            else if (typeof(T) == typeof(float))
            {
                float value = 0f;
                float.TryParse(str, out value);

                obj = value;
            }
            else if (typeof(T) == typeof(List<string>))
            {
                var list = new List<string>();

                var values = str.SplitX();
                foreach (var subvalue in values)
                {
                    if (!string.IsNullOrEmpty(subvalue))
                    {
                        list.Add(subvalue);
                    }
                }

                obj = list;
            }
            else if (typeof(T) == typeof(List<int>))
            {
                var list = new List<int>();

                var values = str.SplitX();
                foreach (var subvalue in values)
                {
                    if (!string.IsNullOrEmpty(subvalue))
                    {
                        int value = 0;
                        if (int.TryParse(subvalue, out value))
                        {
                            list.Add(value);
                        }
                    }
                }

                obj = list;

            }
            else if (typeof(T) == typeof(List<float>))
            {
                var list = new List<float>();

                var values = str.SplitX();
                foreach (var subvalue in values)
                {
                    if (!string.IsNullOrEmpty(subvalue))
                    {
                        float value = 0;
                        if (float.TryParse(subvalue, out value))
                        {
                            list.Add(value);
                        }
                    }
                }

                obj = list;

            }

            else if (typeof(T) == typeof(bool))
            {
                if (str == "是" || str == "1" || str == "Y" || str == "y")
                {
                    obj = true;
                }
                else
                {
                    obj = false;
                }
            }

            else if (typeof(T) == typeof(DateTime))
            {

                obj = DateTime.Parse(input as string);
            }

            return (T)obj;
        }

        catch (Exception e)
        {
            throw e;
        }
    }

}

public class ConfigItemMapAttribute : Attribute
{
    /// <summary>
    /// 旧名称
    /// </summary>
    public string SrcName { get; set; }

    public ConfigItemMapAttribute()
    {


    }

    public ConfigItemMapAttribute(string srcname)
    {
        SrcName = srcname;
    }
}
public static class ConfigHelper
{

    public delegate string GetAssetXMLHandle(string filename);

    public delegate string GetAssetXMLAsyncHandle(string filename, Action<string> action);

    public static event GetAssetXMLHandle GetAssetXML;

    public static event GetAssetXMLAsyncHandle GetAssetXMLAsync;
    public static List<string> GetXmlPaths { get; set; } = new List<string>();

    public static List<Type> RegTypes { get; set; } = new List<Type>();

    public static string GetXmlPath = "";

    public static event Func<string, string> OnChangeToken;

    static ConfigHelper()
    {
        var dirs = new string[]
        {
                "../../XMLDatas/",
                "../XMLDatas/",
                "/XMLDatas/",
                "../../Configs/",
                "../Configs/",
                "/Configs/",
                ""
        };
#if !JS
        foreach (var dir in dirs)
        {
            GetXmlPaths.Add(AppDomain.CurrentDomain.BaseDirectory + "/" + dir);
            GetXmlPaths.Add(System.Environment.CurrentDirectory + "/" + dir);
        }

#endif
        RegTypes.Add(typeof(ConfigHelper));
    }

    public static DateTime GetNowTime(Action<DateTime> callback = null)
    {
        return DateTime.Now;
    }

    public static string LoadConfig(string filename, Action<string> callback = null)
    {

#if SV
            foreach (var regType in RegTypes)
            {
                var types = regType.Assembly.GetTypes();
                foreach (var type1 in types)
                {
                    if (type1.FullName.IndexOf(".Configs.") >= 0 || type1.FullName.IndexOf(".ExcelConfigs.")>=0)
                    {
                        try
                        {
                            var fi = type1.GetField("path");
                            if (fi != null)
                            {
                                if ((fi.GetValue(null) as string).EndsWith(filename))
                                {
                                    var pi = type1.GetProperty("Datas");
                                    var data = pi.GetValue(null);
                                    return data.ToJson();
                                }
                            }
                        }
                        catch
                        {


                        }
                    }
                }
            }
#endif
        return null;
    }

    public static T ConvertTo<T>(object input)
    {
        return ExctionClass.ConvertTo<T>(input);
    }



    public static string GetXml(string path, Action<string> action, bool throwex = true)
    {
#if JS
            throwex = false;
#endif
        var xml = GetXml(path, throwex);

        if (GetAssetXMLAsync != null)
        {
            GetAssetXMLAsync(path, action);
        }
        return xml;
    }

    public static void Log(string logmessage)
    {
        Console.WriteLine(logmessage);
    }

    public static string ChangeToken(string token, Action<string> callback = null)
    {
        if (OnChangeToken != null)
        {
            return OnChangeToken(token);
        }
        return null;
    }

    public static string GetXml(string path, bool throweex = true)
    {
        foreach (var xmlPath in GetXmlPaths)
        {
#if !JS
            var newpath = Path.Combine(xmlPath, path);
            var fionfo = new FileInfo(newpath);

            //本地检查
            if (fionfo.Exists)
            {
                var fr = System.IO.File.OpenText(newpath);
                var xml = fr.ReadToEnd();
                fr.Close();
                return xml;
            }
            if (System.IO.File.Exists(path))
            {
                var fr = System.IO.File.OpenText(path);
                var xml = fr.ReadToEnd();
                fr.Close();
                return xml;
            }
#endif
        }

        var values = path.Split('\\');
        var filename = values.Last();

        try
        {
            if (GetAssetXML != null)
            {
                return GetAssetXML(filename);
            }
        }
        catch (Exception ex)
        {
            if (!throweex)
            {
                return null;
            }
            throw;
        }
        if (throweex)
            throw new NotImplementedException("请初始化Helper.GetAssetXML " + path);

        return null;
    }

    public static T ToObject<T>(string inputxml)
    {
        throw new NotImplementedException();
    }
}
