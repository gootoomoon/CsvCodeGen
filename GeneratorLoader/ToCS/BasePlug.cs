using System.Text;

namespace FileGenLoaderDomain
{
    public interface IPlug
    {
        void GenerateOnFile(StringBuilder codes, string wszInputFilePath, string bstrInputFileContents,
                            string wszDefaultNamespace, string dllpath);

    }

    public abstract class BasePlug : IPlug
    {

        public abstract void GenerateOnFile(StringBuilder codes,
                                            string wszInputFilePath,
                                            string bstrInputFileContents,
                                            string wszDefaultNamespace,string dllpath);

        /// <summary>
        /// 扩展名
        /// </summary>
        public abstract string Exction { get; }


    }
}