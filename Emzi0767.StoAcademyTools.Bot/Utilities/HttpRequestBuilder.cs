using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Emzi0767.StoAcademyTools.Bot.Utilities
{
    public class HttpRequestBuilder
    {
        public string UserAgent { get; set; }
        public Encoding Encoding { get; protected set; }

        public HttpRequestBuilder()
        {
            this.Encoding = new UTF8Encoding(false);
        }

        public HttpWebRequest Build(Uri uri)
        {
            return this.Build(uri, null);
        }

        public HttpWebRequest Build(Uri uri, IDictionary<string, string> post_data)
        {
            return this.Build(uri, post_data, null);
        }

        public HttpWebRequest Build(Uri uri, IDictionary<string, string> post_data, IDictionary<HttpRequestHeader, string> headers)
        {
            var req = WebRequest.CreateHttp(uri);

            if (!string.IsNullOrWhiteSpace(this.UserAgent))
                req.UserAgent = this.UserAgent;

            if (headers != null && headers.Count > 0)
                foreach (var header in headers)
                    req.Headers.Add(header.Key, header.Value);

            if (post_data != null && post_data.Count > 0)
            {
                var data = this.BuildQuery(post_data);
                var rdat = this.Encoding.GetBytes(data);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                using (var rqs = req.GetRequestStream())
                    rqs.Write(rdat, 0, rdat.Length);
            }

            return req;
        }

        public HttpWebResponse GetResponse(HttpWebRequest request)
        {
            return (HttpWebResponse)request.GetResponse();
        }

        public JObject GetResponseJson(HttpWebResponse response)
        {
            var jo = (JObject)null;

            using (var rps = response.GetResponseStream())
            {
                var len = response.ContentLength;
                var dat = new byte[len];
                while (len > 0)
                    len -= rps.Read(dat, (int)(response.ContentLength - len), (int)len);

                var jsr = this.Encoding.GetString(dat);
                jo = JObject.Parse(jsr);
            }

            return jo;
        }

        private string BuildQuery(IDictionary<string, string> data)
        {
            var prms = new string[data.Count];
            var i = 0;
            foreach (var nv in data)
                prms[i++] = string.Format("{0}={1}", HttpUtility.UrlEncode(nv.Key), HttpUtility.UrlEncode(nv.Value));

            return string.Join("&", prms);
        }
    }
}
