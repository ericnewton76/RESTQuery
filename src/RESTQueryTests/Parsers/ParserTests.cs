using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTQueryTests.Parsers
{
    public abstract class ParserTests
    {

        protected IEnumerable<KeyValuePair<string, string>> GetQueryNameValuePairs(string querystring)
        {
            string[] split = querystring.Split('&');

            foreach (string splitX in split)
            {
                int indexOfEq = splitX.IndexOf("=");

                if (indexOfEq == -1)
                    //old querystring parsers put any un-keyed values as null key
                    yield return new KeyValuePair<string, string>(null, splitX);
                else
                {
                    string key = splitX.Substring(0, indexOfEq);
                    string value = splitX.Substring(indexOfEq);
                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }

    }
}
