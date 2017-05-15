
## excel文件代码配置生成器

>由于xls版本采用旧格式，读取的数据会导致部分异常，仅支持xlsx后缀格式（office 2007 及以后版本）

 安装插件后，把xlsx文件放入工程，并且右键属性，在自定义工具中输入，ToCS（注意大小写）
在弹出对话框中，找到插件目录，输入（注意最后有下划线），并刷新，如果刷新出有数据则为正确路径。
关闭，右键运行自定义工具，即可生成代码。

### 注意：
  第一列为ID字段，默认仅支持int，类型，所以填0，（**仅关心ID>0**）的列，任何小于1的数据都被忽略。
  ID不可重复，如果重复，会被后面的相同ID覆盖。


### 类型：

第一列示例数据，可定义当前类型，
空，默认为 string
0，定义为  int
0.1 定义为 float

可在 **typeMap.txt** 中强行定义


``` 

#为忽略当前行
#针对某个表的特定可进行
#表翻译.字段
#Name|type
ID|int
字段名称|枚举
#字段名称|enum
#怪物表中的五行字段|强制为枚举类型
MonsterTable.五行|Enum_TheFiveLine

```                     




#### 强制类型

枚举，通过寻找当前同表枚举表进行搜索，如果搜索到，则进行匹配。
如果是其他表，则无法自动匹配，需要手动指定类型。

**nameMap.txt**

```
#srcurl|http://fanyi.baidu.com/v2transapi?from=zh&to=en&query={src}
#这是百度翻译接口
#dstmath|"dst":"(?<dst>([A-Z0-9a-z\s]+))"
#这是结果获取
#xmlout|./
#这是xml输出路径，默认可以不用去改

```

#### 截断支持

 1. 表名，或字段 前增加 "__" 为忽略字段
 2. 字段 之前增加 "@@" 为后续字段全部忽略

#### 枚举类型支持

枚举表格为默认名称 Sheet2 或手动修改成  @@config

默认第一个表格的字段与 配置表格字段相同，则为配置表格生成枚举类型

示例：
表格1 Sheet1

|ID	|名字	|头像	|形象	|战斗头像	|品质	|五行
| ----- |:-------:|:-------:|:-------:|:-------:|:-------:|:-------:|
|0	|str	|str	|str	|str	|白	|金
|1	|东方不败			|ZD_TX_dongfangbubai | |	|蓝	|金
|2	|令狐冲			|ZD_TX_dongfangbubai | |	|蓝	|金
|3	|慕容复			|ZD_TX_dongfangbubai | |	|紫	|金
|4	|段誉			|ZD_TX_dongfangbubai | |	|紫	|金
|5	|郭靖			|ZD_TX_dongfangbubai | |	|橙	|金
|6	|小龙女			|ZD_TX_dongfangbubai | |	|橙	|金

表格2 Sheet2


|五行	|品质
|--------|:------:|
|金	|白
|木	|蓝
|水	|紫
|火	|橙
|土	


生成
```CS
  /// <summary>
    /// 五行
    /// </summary>
    public enum Enum_TheFiveLine
    {

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
    public enum Enum_Quality
    {

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
```
