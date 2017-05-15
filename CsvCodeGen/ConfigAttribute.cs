using System;

namespace CsvCodeGen
{
    /// <summary>
    /// 配置属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// 数据所在节点(必须)
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 默认值(非必须)
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 是否是关键字段(必须有一个)
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// 是否是列表(非必须)
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public  string Desc { get; set; }
      
    }
}