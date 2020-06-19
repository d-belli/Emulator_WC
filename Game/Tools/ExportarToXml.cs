using System;
using System.Xml;
using System.Xml.Serialization;

namespace Emulator
{
    public static class ExportarToXml
    {
        public static void ExportaToXml<T>(T obj, string path)
        {
            if (obj == null)
            {
                throw new Exception("");
            }
            else
            {
                try
                {
                    XmlSerializer xsSubmit = new XmlSerializer(obj.GetType());
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = false;

                    using (XmlWriter xw = XmlWriter.Create(path, settings))
                        xsSubmit.Serialize(xw, obj);

                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
