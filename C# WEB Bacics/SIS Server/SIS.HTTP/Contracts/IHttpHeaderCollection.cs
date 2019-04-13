using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Contracts
{
    public interface IHttpHeaderCollection
    {
        void Add(HttpHeader header);

        bool ContainsHeader(string key);

        HttpHeader GetHeader(string key);
    }
}
