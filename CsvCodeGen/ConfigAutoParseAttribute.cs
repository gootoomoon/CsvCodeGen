using System;

namespace CsvCodeGen
{
    /// <summary>
    /// ��������
    /// </summary>
    public class ConfigAutoParseAttribute : Attribute
    {
        public string File { get; set; }
        public string ParseType { get; set; }
    }
}