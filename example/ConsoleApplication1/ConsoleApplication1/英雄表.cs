//Code Gen By GeneratorSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//Start Run:
//CodeBase file:///C:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/Extensions/0kdo2r54.zzi/GeneratorSample.dll
//wszInputFilePath:C:\Users\fj\Documents\Visual Studio 2015\Projects\ConsoleApplication1\ConsoleApplication1\英雄表.xlsx
//wszDefaultNamespace:ConsoleApplication1
//BasePath:C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\0kdo2r54.zzi

//Code Gen By ExcelCodeBuilder V1.3 ...
//Addin path E:\VSAddIn\Addin\CsvCodeGen.exe

using System;
using System.Collections.Generic;
using System.Xml;

namespace ConsoleApplication1 {

        /// <summary>
        /// 五行
        /// </summary>
public enum Enum_TheFiveLine{

        /// <summary>
        /// 金
        /// </summary>
Gold,

        /// <summary>
        /// 木
        /// </summary>
Wood,

        /// <summary>
        /// 水
        /// </summary>
Water,

        /// <summary>
        /// 火
        /// </summary>
Fire,

        /// <summary>
        /// 土
        /// </summary>
Soil,
}

        /// <summary>
        /// 品质
        /// </summary>
public enum Enum_Quality{

        /// <summary>
        /// 白
        /// </summary>
White,

        /// <summary>
        /// 蓝
        /// </summary>
Blue,

        /// <summary>
        /// 紫
        /// </summary>
Purple,

        /// <summary>
        /// 橙
        /// </summary>
Orange,
}
}

//eunms Gen By ExcelCodeBuilder  ...
//xmlout C:\Users\fj\Documents\Visual Studio 2015\Projects\ConsoleApplication1\ConsoleApplication1\\


namespace ConsoleApplication1 
{
    /// <summary>
    /// 英雄表
    /// </summary>
    [Serializable]
    //[Config(Index = 0)]
    public partial class HeroWatch 
#if MA
       :IMemberAccessor
#endif
    {


        /// <summary>
        /// ID
        /// </summary>

 //ConfigValue<int> _ID ;
int _ID;

/// <summary>
/// ID
/// </summary>
[ConfigItemMap("ID")]
public int ID { 
    get{
       return _ID;
}
  set {
    _ID = value;
}
    
}



        /// <summary>
        /// 名字
        /// </summary>

 //ConfigValue<String> _Name ;
String _Name;

/// <summary>
/// 名字
/// </summary>
[ConfigItemMap("名字")]
public String Name { 
    get{
       return _Name;
}
  set {
    _Name = value;
}
    
}



        /// <summary>
        /// 头像
        /// </summary>

 //ConfigValue<String> _HeadPortrait ;
String _HeadPortrait;

/// <summary>
/// 头像
/// </summary>
[ConfigItemMap("头像")]
public String HeadPortrait { 
    get{
       return _HeadPortrait;
}
  set {
    _HeadPortrait = value;
}
    
}



        /// <summary>
        /// 形象
        /// </summary>

 //ConfigValue<String> _Image ;
String _Image;

/// <summary>
/// 形象
/// </summary>
[ConfigItemMap("形象")]
public String Image { 
    get{
       return _Image;
}
  set {
    _Image = value;
}
    
}



        /// <summary>
        /// 战斗头像
        /// </summary>

 //ConfigValue<String> _BattleAvatar ;
String _BattleAvatar;

/// <summary>
/// 战斗头像
/// </summary>
[ConfigItemMap("战斗头像")]
public String BattleAvatar { 
    get{
       return _BattleAvatar;
}
  set {
    _BattleAvatar = value;
}
    
}



        /// <summary>
        /// 品质
        /// </summary>

 //ConfigValue<String> _Quality ;
String _Quality;

/// <summary>
/// 品质
/// </summary>
[ConfigItemMap("品质")]
public String Quality { 
    get{
       return _Quality;
}
  set {
    _Quality = value;
}
    
}



        /// <summary>
        /// 五行
        /// </summary>

 //ConfigValue<Enum_TheFiveLine> _TheFiveLine ;
Enum_TheFiveLine _TheFiveLine;

/// <summary>
/// 五行
/// </summary>
[ConfigItemMap("五行")]
public Enum_TheFiveLine TheFiveLine { 
    get{
       return _TheFiveLine;
}
  set {
    _TheFiveLine = value;
}
    
}



        /// <summary>
        /// 兵力
        /// </summary>

 //ConfigValue<Int32> _Troops ;
Int32 _Troops;

/// <summary>
/// 兵力
/// </summary>
[ConfigItemMap("兵力")]
public Int32 Troops { 
    get{
       return _Troops;
}
  set {
    _Troops = value;
}
    
}



        /// <summary>
        /// 攻击
        /// </summary>

 //ConfigValue<Int32> _Attack ;
Int32 _Attack;

/// <summary>
/// 攻击
/// </summary>
[ConfigItemMap("攻击")]
public Int32 Attack { 
    get{
       return _Attack;
}
  set {
    _Attack = value;
}
    
}



        /// <summary>
        /// 防御
        /// </summary>

 //ConfigValue<Int32> _Defense ;
Int32 _Defense;

/// <summary>
/// 防御
/// </summary>
[ConfigItemMap("防御")]
public Int32 Defense { 
    get{
       return _Defense;
}
  set {
    _Defense = value;
}
    
}



        /// <summary>
        /// 智力
        /// </summary>

 //ConfigValue<Int32> _Intelligence ;
Int32 _Intelligence;

/// <summary>
/// 智力
/// </summary>
[ConfigItemMap("智力")]
public Int32 Intelligence { 
    get{
       return _Intelligence;
}
  set {
    _Intelligence = value;
}
    
}



        /// <summary>
        /// 速度
        /// </summary>

 //ConfigValue<Int32> _Speed ;
Int32 _Speed;

/// <summary>
/// 速度
/// </summary>
[ConfigItemMap("速度")]
public Int32 Speed { 
    get{
       return _Speed;
}
  set {
    _Speed = value;
}
    
}



        /// <summary>
        /// 暴击
        /// </summary>

 //ConfigValue<String> _Crit ;
String _Crit;

/// <summary>
/// 暴击
/// </summary>
[ConfigItemMap("暴击")]
public String Crit { 
    get{
       return _Crit;
}
  set {
    _Crit = value;
}
    
}



        /// <summary>
        /// 韧性
        /// </summary>

 //ConfigValue<String> _Toughness ;
String _Toughness;

/// <summary>
/// 韧性
/// </summary>
[ConfigItemMap("韧性")]
public String Toughness { 
    get{
       return _Toughness;
}
  set {
    _Toughness = value;
}
    
}



        /// <summary>
        /// 兵力成长
        /// </summary>

 //ConfigValue<Int32> _TroopGrowth ;
Int32 _TroopGrowth;

/// <summary>
/// 兵力成长
/// </summary>
[ConfigItemMap("兵力成长")]
public Int32 TroopGrowth { 
    get{
       return _TroopGrowth;
}
  set {
    _TroopGrowth = value;
}
    
}



        /// <summary>
        /// 攻击成长
        /// </summary>

 //ConfigValue<Single> _AttackGrowth ;
Single _AttackGrowth;

/// <summary>
/// 攻击成长
/// </summary>
[ConfigItemMap("攻击成长")]
public Single AttackGrowth { 
    get{
       return _AttackGrowth;
}
  set {
    _AttackGrowth = value;
}
    
}



        /// <summary>
        /// 防御成长
        /// </summary>

 //ConfigValue<Single> _DefenseGrowth ;
Single _DefenseGrowth;

/// <summary>
/// 防御成长
/// </summary>
[ConfigItemMap("防御成长")]
public Single DefenseGrowth { 
    get{
       return _DefenseGrowth;
}
  set {
    _DefenseGrowth = value;
}
    
}



        /// <summary>
        /// 智力成长
        /// </summary>

 //ConfigValue<Single> _IntellectualGrowth ;
Single _IntellectualGrowth;

/// <summary>
/// 智力成长
/// </summary>
[ConfigItemMap("智力成长")]
public Single IntellectualGrowth { 
    get{
       return _IntellectualGrowth;
}
  set {
    _IntellectualGrowth = value;
}
    
}



        /// <summary>
        /// 速度成长
        /// </summary>

 //ConfigValue<Single> _SpeedGrowth ;
Single _SpeedGrowth;

/// <summary>
/// 速度成长
/// </summary>
[ConfigItemMap("速度成长")]
public Single SpeedGrowth { 
    get{
       return _SpeedGrowth;
}
  set {
    _SpeedGrowth = value;
}
    
}



        /// <summary>
        /// 战斗技能
        /// </summary>

 //ConfigValue<Int32> _CombatSkills ;
Int32 _CombatSkills;

/// <summary>
/// 战斗技能
/// </summary>
[ConfigItemMap("战斗技能")]
public Int32 CombatSkills { 
    get{
       return _CombatSkills;
}
  set {
    _CombatSkills = value;
}
    
}



        /// <summary>
        /// 内政技能1
        /// </summary>

 //ConfigValue<Int32> _HomeSkills1 ;
Int32 _HomeSkills1;

/// <summary>
/// 内政技能1
/// </summary>
[ConfigItemMap("内政技能1")]
public Int32 HomeSkills1 { 
    get{
       return _HomeSkills1;
}
  set {
    _HomeSkills1 = value;
}
    
}



        /// <summary>
        /// 内政技能2
        /// </summary>

 //ConfigValue<Int32> _HomeSkills2 ;
Int32 _HomeSkills2;

/// <summary>
/// 内政技能2
/// </summary>
[ConfigItemMap("内政技能2")]
public Int32 HomeSkills2 { 
    get{
       return _HomeSkills2;
}
  set {
    _HomeSkills2 = value;
}
    
}



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
            if (ID > 0)
            {
                return true;
            }
            return false;
        }


        public int GetId()
       {
            return   ConfigHelper.ConvertTo<int>(ID);
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
                        case "ID":
this._ID =   ConfigHelper.ConvertTo<int>(localnode.InnerText); 
break;
case "名字":
this._Name =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "头像":
this._HeadPortrait =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "形象":
this._Image =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "战斗头像":
this._BattleAvatar =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "品质":
this._Quality =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "五行":
this._TheFiveLine =   ConfigHelper.ConvertTo<Enum_TheFiveLine>(localnode.InnerText); 
break;
case "兵力":
this._Troops =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "攻击":
this._Attack =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "防御":
this._Defense =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "智力":
this._Intelligence =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "速度":
this._Speed =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "暴击":
this._Crit =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "韧性":
this._Toughness =   ConfigHelper.ConvertTo<String>(localnode.InnerText); 
break;
case "兵力成长":
this._TroopGrowth =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "攻击成长":
this._AttackGrowth =   ConfigHelper.ConvertTo<Single>(localnode.InnerText); 
break;
case "防御成长":
this._DefenseGrowth =   ConfigHelper.ConvertTo<Single>(localnode.InnerText); 
break;
case "智力成长":
this._IntellectualGrowth =   ConfigHelper.ConvertTo<Single>(localnode.InnerText); 
break;
case "速度成长":
this._SpeedGrowth =   ConfigHelper.ConvertTo<Single>(localnode.InnerText); 
break;
case "战斗技能":
this._CombatSkills =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "内政技能1":
this._HomeSkills1 =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;
case "内政技能2":
this._HomeSkills2 =   ConfigHelper.ConvertTo<Int32>(localnode.InnerText); 
break;

                    }
                }
        }





        public object GetValue(object instance, string memberName)
        {
            
if (memberName == "ID")
{
    return ID;
}

if (memberName == "Name")
{
    return Name;
}

if (memberName == "HeadPortrait")
{
    return HeadPortrait;
}

if (memberName == "Image")
{
    return Image;
}

if (memberName == "BattleAvatar")
{
    return BattleAvatar;
}

if (memberName == "Quality")
{
    return Quality;
}

if (memberName == "TheFiveLine")
{
    return TheFiveLine;
}

if (memberName == "Troops")
{
    return Troops;
}

if (memberName == "Attack")
{
    return Attack;
}

if (memberName == "Defense")
{
    return Defense;
}

if (memberName == "Intelligence")
{
    return Intelligence;
}

if (memberName == "Speed")
{
    return Speed;
}

if (memberName == "Crit")
{
    return Crit;
}

if (memberName == "Toughness")
{
    return Toughness;
}

if (memberName == "TroopGrowth")
{
    return TroopGrowth;
}

if (memberName == "AttackGrowth")
{
    return AttackGrowth;
}

if (memberName == "DefenseGrowth")
{
    return DefenseGrowth;
}

if (memberName == "IntellectualGrowth")
{
    return IntellectualGrowth;
}

if (memberName == "SpeedGrowth")
{
    return SpeedGrowth;
}

if (memberName == "CombatSkills")
{
    return CombatSkills;
}

if (memberName == "HomeSkills1")
{
    return HomeSkills1;
}

if (memberName == "HomeSkills2")
{
    return HomeSkills2;
}

            return null;
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
             var obj = instance as HeroWatch;
            
 if (memberName == "ID")
{
    obj.ID = (int)newValue;
}

 if (memberName == "Name")
{
    obj.Name = (String)newValue;
}

 if (memberName == "HeadPortrait")
{
    obj.HeadPortrait = (String)newValue;
}

 if (memberName == "Image")
{
    obj.Image = (String)newValue;
}

 if (memberName == "BattleAvatar")
{
    obj.BattleAvatar = (String)newValue;
}

 if (memberName == "Quality")
{
    obj.Quality = (String)newValue;
}

 if (memberName == "TheFiveLine")
{
    obj.TheFiveLine = (Enum_TheFiveLine)newValue;
}

 if (memberName == "Troops")
{
    obj.Troops = (Int32)newValue;
}

 if (memberName == "Attack")
{
    obj.Attack = (Int32)newValue;
}

 if (memberName == "Defense")
{
    obj.Defense = (Int32)newValue;
}

 if (memberName == "Intelligence")
{
    obj.Intelligence = (Int32)newValue;
}

 if (memberName == "Speed")
{
    obj.Speed = (Int32)newValue;
}

 if (memberName == "Crit")
{
    obj.Crit = (String)newValue;
}

 if (memberName == "Toughness")
{
    obj.Toughness = (String)newValue;
}

 if (memberName == "TroopGrowth")
{
    obj.TroopGrowth = (Int32)newValue;
}

 if (memberName == "AttackGrowth")
{
    obj.AttackGrowth = (Single)newValue;
}

 if (memberName == "DefenseGrowth")
{
    obj.DefenseGrowth = (Single)newValue;
}

 if (memberName == "IntellectualGrowth")
{
    obj.IntellectualGrowth = (Single)newValue;
}

 if (memberName == "SpeedGrowth")
{
    obj.SpeedGrowth = (Single)newValue;
}

 if (memberName == "CombatSkills")
{
    obj.CombatSkills = (Int32)newValue;
}

 if (memberName == "HomeSkills1")
{
    obj.HomeSkills1 = (Int32)newValue;
}

 if (memberName == "HomeSkills2")
{
    obj.HomeSkills2 = (Int32)newValue;
}

        }
    }

    
  
    



    /// <summary>
    /// 英雄表
    /// </summary>
    public partial class HeroWatchConfigs
    {
        public readonly static  string path = ConfigHelper.GetXmlPath + "HeroWatch.xml";

        private static HeroWatchConfigs _Configs;
        public static HeroWatchConfigs Configs
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

        static  Dictionary<int, HeroWatch> DatasMap { get; set; }

        public HeroWatch this[int key]
        {
            get { return DatasMap[key]; }
        }

        public static List<HeroWatch> Datas
        {
            get
            {
                if (_Configs == null)
                {
                     ConvertInit();
                     var cfgs = new HeroWatchConfigs();
                    cfgs.Init(null);
                    LastRead = DateTime.Now;
                    _Configs = cfgs;
                   
                }
                return Configs.DataList;
            }
        }

        public static void ConvertInit(){
              
                    ExctionClass.RegEnumConvert<Enum_TheFiveLine>(o =>
                    {
                        var str = o.ToString();
                        switch (str)
                        {
                             
case "金":
      return Enum_TheFiveLine.Gold;
                        
 
case "木":
      return Enum_TheFiveLine.Wood;
                        
 
case "水":
      return Enum_TheFiveLine.Water;
                        
 
case "火":
      return Enum_TheFiveLine.Fire;
                        
 
case "土":
      return Enum_TheFiveLine.Soil;
                        

                        }
                        return Enum_TheFiveLine.Gold;
                });
                

                    ExctionClass.RegEnumConvertBack<Enum_TheFiveLine>(o =>
                    {
                        var str = (Enum_TheFiveLine)o;
                        switch (str)
                        {
                             
case Enum_TheFiveLine.Gold:
      return "金";
                        
 
case Enum_TheFiveLine.Wood:
      return "木";
                        
 
case Enum_TheFiveLine.Water:
      return "水";
                        
 
case Enum_TheFiveLine.Fire:
      return "火";
                        
 
case Enum_TheFiveLine.Soil:
      return "土";
                        

                        }
                        return "";
                });
                

                    ExctionClass.RegEnumConvert<Enum_Quality>(o =>
                    {
                        var str = o.ToString();
                        switch (str)
                        {
                             
case "白":
      return Enum_Quality.White;
                        
 
case "蓝":
      return Enum_Quality.Blue;
                        
 
case "紫":
      return Enum_Quality.Purple;
                        
 
case "橙":
      return Enum_Quality.Orange;
                        

                        }
                        return Enum_Quality.White;
                });
                

                    ExctionClass.RegEnumConvertBack<Enum_Quality>(o =>
                    {
                        var str = (Enum_Quality)o;
                        switch (str)
                        {
                             
case Enum_Quality.White:
      return "白";
                        
 
case Enum_Quality.Blue:
      return "蓝";
                        
 
case Enum_Quality.Purple:
      return "紫";
                        
 
case Enum_Quality.Orange:
      return "橙";
                        

                        }
                        return "";
                });
                

        }

        public static void ReLoad(){
            _Configs = null;
            //var x = Datas;
        }

        private List<HeroWatch> _dateList = new List<HeroWatch>();

        public List<HeroWatch> DataList
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
           if (   string.IsNullOrEmpty( inputxml) || inputxml.StartsWith("<"))
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
    
                    var item = new HeroWatch();
                    item.Parse(node);
                    if(item.CheckId()){
                        DataList.Add(item);
                    }
                

                    }
                    catch (Exception e)
                    {
                        ConfigHelper.Log("Lost:" + node.InnerXml + " " + e.ToString());
                    }
                }
               }
              else
            {
#if !SV
             DataList =  ConfigHelper.ToObject<List<HeroWatch>>(inputxml);
#endif
            }
            DatasMap= new Dictionary<int, HeroWatch>();
            foreach (var item in DataList)
            {
                DatasMap[item.GetId()] = item;
            }

        }
    }
}
//User Plug:CsvCodeGen.exe Type:CsvCodeGen.ExcelCodeBuilder

