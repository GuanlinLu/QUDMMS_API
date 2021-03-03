using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace QUDMMSAPI.Common
{
    public class XMLHelper
    {
        public static string GetSql(string SqlNode)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(@"XML/SQL.xml");

            XmlNodeList NodeList = xmlDoc.SelectNodes(string.Format("/XML/QUDMMS_DB/{0}", SqlNode));

            if (NodeList.Count == 0)
                return null;

            return NodeList[0].InnerText;
        }
    }
}
