using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = HeroWatchConfigs.Configs[1000];

            //从枚举 返回中文
            //Enum_TheFiveLine.Fire -> 火
            var str = ExctionClass.GetEnumConvertBack<Enum_TheFiveLine>(Enum_TheFiveLine.Fire);

        }
    }
}
