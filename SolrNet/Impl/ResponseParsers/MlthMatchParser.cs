﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SolrNet.Impl.ResponseParsers
{
    public class MlthMatchParser<T> : ISolrMoreLikeThisHandlerResponseParser<T>
    {
        private readonly ISolrDocumentResponseParser<T> docParser;

        public MlthMatchParser(ISolrDocumentResponseParser<T> docParser)
        {
            this.docParser = docParser;
        }

        public void Parse(XDocument xml, IAbstractSolrQueryResults<T> results)
        {
            if (results is IMoreLikeThisQueryResults<T>)
            {
                this.Parse(xml, (IMoreLikeThisQueryResults<T>)results);
            }
        }

        public void Parse(XDocument xml, IMoreLikeThisQueryResults<T> results)
        {
            var resultNode = xml.Element("response").Elements("result").FirstOrDefault(e => (string)e.Attribute("name") == "match");

            if (resultNode == null)
            {
                results.Match = default(T);
                return;
            }

            results.Match = docParser.ParseResults(resultNode).FirstOrDefault();
        }
    }
}
