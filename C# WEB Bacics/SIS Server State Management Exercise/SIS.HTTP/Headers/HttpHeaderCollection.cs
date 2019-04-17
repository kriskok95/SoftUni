using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Headers.Contracts;

namespace SIS.HTTP.Headers
{
    class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (!this.ContainsHeader(header.Key))
            {
                this.headers.Add(header.Key, header);
            }
        }

        public bool ContainsHeader(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }

            return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, headers.Values);
        }
    }
}
