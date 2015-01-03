using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ExpassistLib.Extension
{
    public static class XmlExtension
    {
        public static XmlElement AddXmlElement(this XmlElement root, string tag, string innerText = "")
        {
            var element = root.OwnerDocument.CreateElement(tag);
            element.InnerText = innerText;
            root.AppendChild(element);
            return element;
        }

        public static XmlElement AddXmlElement(this XmlDocument root, string tag, string innerText = "")
        {
            var element = root.CreateElement(tag);
            element.InnerText = innerText;
            root.AppendChild(element);
            return element;
        }
    }
}
