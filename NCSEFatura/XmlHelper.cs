using System;
using System.IO;
using System.Text;
using System.Xml;

namespace NCSEFatura
{
    internal static class XmlHelper
    {
        internal static void Update(string fileFullPath, string profileId, Guid guid,ServiceProvider serviceProvider)
        {
            TextReader textReader = new StreamReader(fileFullPath, Encoding.Default);
            StringBuilder sb = new StringBuilder(textReader.ReadToEnd());
            textReader.Close();
            textReader.Dispose();

            File.Delete(fileFullPath);

            var format = "<cbc:ProfileID>{0}</cbc:ProfileID>";
            sb.Replace(string.Format(format, string.Empty), string.Format(format, profileId));
            format = "<cbc:UUID>{0}</cbc:UUID>";
            sb.Replace(string.Format(format, string.Empty), string.Format(format, guid));

            TextWriter textWriter = new StreamWriter(fileFullPath, false, Encoding.UTF8);
            textWriter.Write(sb.ToString());
            textWriter.Close();
            textWriter.Dispose();

            //Xml dosyasını diskten okumak için
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileFullPath);

            var nodes = xmlDocument.ChildNodes;

            var b = true;
            foreach (XmlNode node in nodes)
            {
                TrimLeadingOrTrailingSpacesFromNodeValue(node);

                foreach (XmlNode subNode in node.ChildNodes)
                {
                    var name = subNode.Name;
                    if (name == "cac:AdditionalDocumentReference")
                    {
                        if (b)
                        {
                            var attachment = subNode.ChildNodes[2];
                            var embeddedDocumentBinaryObject = attachment.ChildNodes[0];
                            embeddedDocumentBinaryObject.InnerXml = GetXsltBase64String(profileId,serviceProvider, fileFullPath);
                            b = false;
                        }
                    }
                }
            }

            File.Delete(fileFullPath);
            xmlDocument.Save(fileFullPath);
        }

        private static void TrimLeadingOrTrailingSpacesFromNodeValue(XmlNode node)
        {
            if (node.ChildNodes.Count == 0)
                node.InnerText = node.InnerText.Trim();
            else
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    TrimLeadingOrTrailingSpacesFromNodeValue(node.ChildNodes[i]);
                }
            }
        }

        private static string GetXsltBase64String(string profileId,ServiceProvider serviceProvider, string fileFullPath)
        {
            if (serviceProvider == ServiceProvider.Veriban)
            {
                var filePath = string.Empty;

                if (profileId == "TEMELFATURA" || profileId == "TICARIFATURA")
                {

                    filePath = Path.GetDirectoryName(fileFullPath) + "\\efatura.xslt";
                }
                else
                {
                    filePath = Path.GetDirectoryName(fileFullPath) + "\\earsiv.xslt";
                }

                if (File.Exists(filePath))
                {
                    var plainTextBytes = File.ReadAllBytes(filePath);
                    return Convert.ToBase64String(plainTextBytes);
                }
                else
                {
                    throw new Exception(string.Format("{0} dosyası bulunamadı.", filePath));
                }
            }
            //else if (serviceProvider == ServiceProvider.Uyumsoft)
            //{
            //    return string.Empty;
            //}

            return string.Empty;
        }
    }
}