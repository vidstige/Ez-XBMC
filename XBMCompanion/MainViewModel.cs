using System;
using System.Xml;

namespace XBMCompanion
{
    public class MainViewModel: ViewModel
    {
        private string _searchQuery;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                RaisePropertyChanged("SearchQuery");
            }
        }

        internal void Search()
        {
            var rssUrl = string.Format("http://www.ezrss.it/search/index.php?simple&show_name={0}&mode=rss", SearchQuery);
            var xmlReader = XmlReader.Create(rssUrl);
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            using (XmlReader reader = XmlReader.Create(rssUrl, settings))
            {
                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //writer.WriteStartElement(reader.Name);
                            Console.WriteLine(reader.Name);
                            break;
                        case XmlNodeType.Text:
                            //writer.WriteString(reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:
                            //writer.WriteProcessingInstruction(reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            //writer.WriteComment(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            //writer.WriteFullEndElement();
                            break;
                    }
                }
            }
        }
    }
}
