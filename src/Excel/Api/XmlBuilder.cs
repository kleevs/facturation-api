using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace Excel
{
    public class XmlBuilder
    {
        private readonly XmlDocument _doc;
        private readonly XmlElement _xmlElement;

        public XmlBuilder() 
        {
            _doc = new XmlDocument();
            _xmlElement = _doc.DocumentElement;
        }

        private XmlBuilder(XmlDocument doc, XmlElement xmlElement)
        {
            _doc = doc;
            _xmlElement = xmlElement;
        }

        public void ToBuild(Stream stream) 
        {
            _doc.Save(stream);
        }

        public XmlBuilder SetDeclaration(string version, string encoding, bool standalone) 
        {
            var xmlDeclaration = _doc.CreateXmlDeclaration(version, encoding, standalone ? "yes" : "no");
            _doc.InsertBefore(xmlDeclaration, _xmlElement);
            return this;
        }

        public XmlBuilder Append(string text)
        {
            if (_xmlElement != null)
            {
                _xmlElement.InnerText += text;
            }
            else
            {
                _doc.InnerText += text;
            }

            return this;
        }

        public XmlBuilder Append(string typeNode, string namespaceuri, params string[] attributes) 
        {
            var prefix = typeNode.Contains(":") ? typeNode.Substring(0, typeNode.IndexOf(":")) : string.Empty;
            if (!string.IsNullOrEmpty(prefix))
            {
                typeNode = typeNode.Substring(typeNode.IndexOf(":") + 1, typeNode.Length - prefix.Length - 1);
            }

            var attrs = new List<KeyValuePair<string, string>>();
            if (attributes != null)
            {
                for (var i = 0; i < attributes.Length / 2; i++)
                {
                    attrs.Add(new KeyValuePair<string, string>(attributes[i*2], attributes[i*2 + 1]));
                }
            }

            var node = _doc.CreateElement(prefix, typeNode, namespaceuri ?? string.Empty);
            if (string.IsNullOrEmpty(namespaceuri)) 
            {
                node.Attributes.RemoveNamedItem("xmlns");
            }

            var list = attrs.Select(attr =>
            {
                var res = _doc.CreateNode(XmlNodeType.Attribute, attr.Key, attr.Key.StartsWith("xmlns") ? "http://www.w3.org/2000/xmlns/" : null);
                res.Value = attr.Value;
                return res;
            }).ToList();

            foreach (var l in list)
            {
                node.Attributes.SetNamedItem(l);
            }

            if (_xmlElement != null)
            {
                _xmlElement.AppendChild(node);
            }
            else 
            {
                _doc.AppendChild(node);
            }

            return new XmlBuilder(_doc, node);
        }
    }
}
