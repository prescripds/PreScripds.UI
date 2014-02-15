using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PreScripds.Infrastructure
{
    public static class Serialization
    {
        public static T Deserialize<T>(StringReader dataIn)
        {
            return (T)(new XmlSerializer(typeof(T), String.Empty)).Deserialize(dataIn);
        }
        public static XDocument Serialize(object obj)
        {
            // Serialise to the XML document
            var objectDocument = new XmlDocument();
            using (XmlWriter writer = (objectDocument.CreateNavigator()).AppendChild()){
                (new XmlSerializer(obj.GetType())).Serialize(writer, obj);
                writer.Close();
            }
            return XDocument.Load(new XmlNodeReader(objectDocument));
        }
        public static T Open<T>(string fileName){
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The provided file does not exist.", fileName);
            return (Serialization.Deserialize<T>(new StringReader(fileName)));
        }
        public static void Save(object obj, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName) && !Directory.Exists(Path.GetFullPath(fileName)) &&
                !(Directory.CreateDirectory(fileName)).Exists)
                throw new DirectoryNotFoundException(
                    String.Format("The provided Directory does not exist or cannot be created."));
            (Serialization.Serialize(obj)).Save(fileName);
        }
    }
}
