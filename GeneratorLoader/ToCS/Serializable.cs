using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace FileGenLoaderDomain
{
    public class Serializable
    {
        // Fields
        private static Dictionary<Type, XmlSerializer> _SerializationDic = new Dictionary<Type, XmlSerializer>();
        public static bool UserCache = true;

        // Methods
        public static Stream ConverBytesToStream(byte[] bData)
        {
            Stream ss2 = new MemoryStream(bData, true);
            ss2.Read(bData, 0, bData.Length);
            return ss2;
        }

        public static string ConverBytesToString(byte[] bData)
        {
            return Encoding.Unicode.GetString(bData);
        }

        public static byte[] ConverStreamToByte(Stream stream)
        {
            Encoding encode = Encoding.GetEncoding("utf-8");
            byte[] buffer = new byte[stream.Length];
            stream.Seek(0L, SeekOrigin.Begin);
            stream.Read(buffer, 0, int.Parse(stream.Length.ToString()));
            stream.Close();
            return buffer;
        }

        public static string ConverStreamToStr(Stream stream)
        {
            return Encoding.Unicode.GetString(ConverStreamToByte(stream));
        }

        public static Stream ConverStrToStream(string Str)
        {
            return ConverBytesToStream(Encoding.Unicode.GetBytes(Str));
        }

        public static string DecodeBase64(string code_type, string code)
        {
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                return Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static byte[] DeDeflate(byte[] data)
        {
            MemoryStream source = new MemoryStream(data);
            using (MemoryStream destination = new MemoryStream())
            {
                using (GZipStream input = new GZipStream(source, CompressionMode.Decompress, true))
                {
                    int n;
                    byte[] bytes = new byte[0x1000];
                    while ((n = input.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        destination.Write(bytes, 0, n);
                    }
                }
                destination.Flush();
                destination.Position = 0L;
                return destination.ToArray();
            }
        }

        public static object DeepDeserialize(string bytesstr)
        {
            return DeepDeserializeStream(ConverBytesToStream(Deserialize(new byte[0].GetType(), bytesstr) as byte[]));
        }

        public static object DeepDeserializeByte(byte[] buff)
        {
            return DeepDeserializeStream(ConverBytesToStream(buff));
        }

        public static object DeepDeserializeStream(Stream stream)
        {
            stream.Seek(0L, SeekOrigin.Begin);
            IFormatter myformater = new BinaryFormatter();
            return myformater.Deserialize(stream);
        }

        public static string DeepSerialization(object mysoap)
        {
            return Serialiaze(ConverStreamToByte(DeepSerializationStream(mysoap)));
        }

        public static byte[] DeepSerializationByte(object mysoap)
        {
            return ConverStreamToByte(DeepSerializationStream(mysoap));
        }

        public static Stream DeepSerializationStream(object mysoap)
        {
            IFormatter myformater = new BinaryFormatter();
            Stream stream = new MemoryStream();
            myformater.Serialize(stream, mysoap);
            return stream;
        }

        public static byte[] Deflate(byte[] data)
        {
            MemoryStream source = new MemoryStream(data);
            using (MemoryStream destination = new MemoryStream())
            {
                using (GZipStream output = new GZipStream(destination, CompressionMode.Compress))
                {
                    int n;
                    byte[] bytes = new byte[0x1000];
                    while ((n = source.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        output.Write(bytes, 0, n);
                    }
                }
                data = destination.ToArray();
                return data;
            }
        }

        public static object Deserialize(Type returnType, string data)
        {
            if (data == null)
            {
                return null;
            }
            XmlSerializer serializer = XmlSerializerGet(returnType);
            using (StringReader sr = new StringReader(data))
            {
                return serializer.Deserialize(sr);
            }
        }

        public static object Deserialize4File(Type type, string file)
        {
            if (File.Exists(file))
            {
                return Deserialize(type, File.ReadAllText(file));
            }
            return null;
        }

        public static object DeserializeByte(Type returnType, byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            XmlSerializer serializer = null;
            serializer = XmlSerializerGet(returnType);
            using (Stream sw = new MemoryStream(data))
            {
                return serializer.Deserialize(sw);
            }
        }

        public static string EncodeBase64(string code_type, string code)
        {
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static string Serialiaze(object obj)
        {
            XmlSerializer serializer = null;
            serializer = XmlSerializerGet(obj.GetType());
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize((TextWriter) sw, obj);
                sw.Flush();
                return sw.ToString();
            }
        }

        public static void Serialiaze2File(object obj, string file)
        {
            File.WriteAllText(file, Serialiaze(obj));
        }

        public static byte[] SerialiazeByte(object obj)
        {
            XmlSerializer serializer = null;
            serializer = XmlSerializerGet(obj.GetType());
            using (Stream sw = new MemoryStream())
            {
                serializer.Serialize(sw, obj);
                sw.Flush();
                return ConverStreamToByte(sw);
            }
        }

        public static byte[] SerialiazeToByte(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        public static XmlSerializer XmlSerializerGet(Type type)
        {
            lock (_SerializationDic)
            {
                if (!UserCache)
                {
                    return new XmlSerializer(type);
                }
                if (_SerializationDic.ContainsKey(type))
                {
                    return _SerializationDic[type];
                }
                XmlSerializer xmls = new XmlSerializer(type);
                _SerializationDic.Add(type, xmls);
                return xmls;
            }
        }
    }
}