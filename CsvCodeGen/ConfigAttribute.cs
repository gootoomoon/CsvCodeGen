using System;

namespace CsvCodeGen
{
    /// <summary>
    /// ��������
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// �������ڽڵ�(����)
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Ĭ��ֵ(�Ǳ���)
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// �Ƿ��ǹؼ��ֶ�(������һ��)
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// �Ƿ����б�(�Ǳ���)
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public  string Desc { get; set; }
      
    }
}