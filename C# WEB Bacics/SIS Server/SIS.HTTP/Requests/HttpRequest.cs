using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extenstions;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private bool IsValidRequestQueryString(string queryString)
        {
            if (queryString == null)
            {
                return false;
            }

            var queryLength = queryString.Split('&').ToArray();

            if (queryLength.Length < 1)
            {
                return false;
            }

            return true;
        }

        private void ParseRequestParameters(string formData)
        {
            ParseQueryParameters();
            ParseFormDataParameters(formData);
        }

        private void ParseFormDataParameters(string formData)
        {
            var keyQueryPairs = formData
                .Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            foreach (var keyQueryPair in keyQueryPairs)
            {
                var keyValuePair = keyQueryPair
                    .Split(new []{'='}, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }

                string key = keyValuePair[0];
                string value = keyValuePair[1];

                this.FormData[key] = value;
            }
        }

        private void ParseQueryParameters()
        {
            string queryString = this.Url.Split(new[] {'?', '#'})
                .Skip(1)
                .Take(1)
                .FirstOrDefault();

            if (IsValidRequestQueryString(queryString))
            {
                var parametersArr = queryString
                    .Split('&')
                    .ToArray();

                foreach (var parameter in parametersArr)
                {
                    string[] parametersSplit = parameter.Split('=');
                    if (parametersSplit.Length != 2)
                    {
                        throw new BadRequestException();
                    }
                    string key = parametersSplit[0];
                    string value = parametersSplit[1];

                    this.QueryData[key] = value;

                }
            }

            
        }

        private void ParseHeaders(string[] headersArr)
        {
            int counter = 0;
            string headerLine = headersArr[counter++];

            while (headerLine != string.Empty)
            {
                string[] headerArgs = headerLine.Split(new[]{": "}, StringSplitOptions.None);

                if (!this.Headers.ContainsHeader(headerArgs[0]))
                {
                    HttpHeader header = new HttpHeader(headerArgs[0], headerArgs[1]);
                    this.Headers.Add(header);
                }

                if (!this.Headers.ContainsHeader("Host"))
                {
                    throw new BadRequestException();
                }
                headerLine = headersArr[counter++];
            }
        }

        private void ParseRequestPath()
        {
            ////TODO: Maybe there is problems with the length of the path
            if (!this.Url.Contains('?'))
            {
                this.Path = Url;
            }
            else
            {
                int theLastIndexOfPath = this.Url.IndexOf('?') - 1;
                string path = this.Url.Substring(0, theLastIndexOfPath + 1);
                this.Path = path;
            }
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            string requestMethod = StringExtensions.Capitalize(requestLine[0]);
            Enum.TryParse(requestMethod, true, out HttpRequestMethod parsedRequestMethod);
            this.RequestMethod = parsedRequestMethod;
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2] == "HTTP/1.1";
        }
    }
}
