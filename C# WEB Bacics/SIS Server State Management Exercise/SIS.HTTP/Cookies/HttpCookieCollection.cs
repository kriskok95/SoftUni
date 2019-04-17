using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Cookies.Contracts;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentException();
            }

            if (this.ContainsCookie(cookie.Key))
            {
                throw new ArgumentException();
            }

            this.cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException();
            }

            return this.cookies[key];
        }

        public bool HasCookies()
        {
            return this.cookies.Count > 0 ? true : false;
        }

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }
    }
}
