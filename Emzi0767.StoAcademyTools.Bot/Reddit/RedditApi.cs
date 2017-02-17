using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Emzi0767.StoAcademyTools.Bot.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Emzi0767.StoAcademyTools.Bot.Reddit
{
    public class RedditApi
    {
        // IT's A LIVING THING! PROOF:
        // https://www.reddit.com/r/shittystoquotes/comments/55u5a1/this_is_a_post_and_its_really_a_great_post_it/d8do0d0
        private const string OAUTH_TOKEN_REFRESH_URI = "https://www.reddit.com/api/v1/access_token";

        private HttpRequestBuilder RequestBuilder { get; set; }

        private string OAuthToken { get; set; }
        private string RefreshToken { get; set; }

        private string AppId { get; set; }
        private List<string> LastThings { get; set; }
        private List<string> LastMiniThings { get; set; }

        public RedditApi(HttpRequestBuilder request_builder)
        {
            this.RequestBuilder = request_builder;
        }

        public void Initialize()
        {
            this.LoadSettings();
        }

        public IEnumerable<RedditPost> QueryPosts()
        {
            return this.QueryPosts(null);
        }

        public IEnumerable<RedditPost> QueryPosts(string last)
        {
            string suri = "https://oauth.reddit.com/r/stobuilds/new";
            var uri = new Uri(string.IsNullOrWhiteSpace(last) ? suri : string.Concat(suri, "?after=", last));

            var dat = (JObject)null;
            try
            {
                var req = this.RequestBuilder.Build(uri, null, this.MakeBearerHeader());
                var res = this.RequestBuilder.GetResponse(req);
                dat = this.RequestBuilder.GetResponseJson(res);
            }
            catch (WebException)
            {
                this.RefreshOAuthToken();
                var req = this.RequestBuilder.Build(uri, null, this.MakeBearerHeader());
                var res = this.RequestBuilder.GetResponse(req);
                dat = this.RequestBuilder.GetResponseJson(res);
            }

            var data = (JObject)dat["data"];
            var chdn = (JArray)data["children"];
            var psts = new List<RedditPost>();
            var twas = false;
            foreach (var xchd in chdn)
            {
                var xjo = (JObject)xchd;
                var xdata = (JObject)xjo["data"];

                var xname = (string)xdata["name"];
                var xprev = this.LastThings.Contains(xname);
                twas |= xprev;
                if (!xprev)
                    this.LastThings.Add(xname);

                var xtext = (string)xdata["selftext"];
                var xlink = (string)xdata["url"];
                var xcont = (string)null;
                if (xlink != null)
                    xcont = xlink;
                if (!string.IsNullOrWhiteSpace(xtext))
                    xcont = xtext;

                var indx1 = xcont.IndexOf("http://skillplanner.stoacademy.com/");
                var indx2 = xcont.IndexOf("Above was translated automatically ");
                var xconv = false;
                if (indx1 != -1 && indx2 == -1)
                    xconv = true;

                var xblid = (string)null;
                if (xconv)
                {
                    xlink = xcont.Substring(indx1, 67);

                    var xuri = new Uri(xlink);

                    xblid = xuri.AbsolutePath.Substring(1);
                }

                if (!xprev) psts.Add(new RedditPost { Content = xcont, Convert = xconv, Fullname = xname, BuildId = xblid });
            }

            if (!twas)
                psts.AddRange(this.QueryPosts(psts[psts.Count - 1].Fullname));

            return psts;
        }

        public IEnumerable<RedditPost> QueryComments()
        {
            return this.QueryComments(null);
        }

        public IEnumerable<RedditPost> QueryComments(string last)
        {
            string suri = "https://oauth.reddit.com/r/stobuilds/comments?sort=new";
            var uri = new Uri(string.IsNullOrWhiteSpace(last) ? suri : string.Concat(suri, "&after=", last));

            var dat = (JObject)null;
            try
            {
                var req = this.RequestBuilder.Build(uri, null, this.MakeBearerHeader());
                var res = this.RequestBuilder.GetResponse(req);
                dat = this.RequestBuilder.GetResponseJson(res);
            }
            catch (WebException)
            {
                this.RefreshOAuthToken();
                var req = this.RequestBuilder.Build(uri, null, this.MakeBearerHeader());
                var res = this.RequestBuilder.GetResponse(req);
                dat = this.RequestBuilder.GetResponseJson(res);
            }

            var data = (JObject)dat["data"];
            var chdn = (JArray)data["children"];
            var cmts = new List<RedditPost>();
            var twas = false;
            foreach (var xchd in chdn)
            {
                var xjo = (JObject)xchd;
                var xdata = (JObject)xjo["data"];

                var xname = (string)xdata["name"];
                var xprev = this.LastMiniThings.Contains(xname);
                twas |= xprev;
                if (!xprev)
                    this.LastMiniThings.Add(xname);

                var xtext = (string)xdata["body"];

                var indx1 = xtext.IndexOf("http://skillplanner.stoacademy.com/");
                var indx2 = xtext.IndexOf("Above was translated automatically ");
                var xconv = false;
                if (indx1 != -1 && indx2 == -1)
                    xconv = true;

                var xuser = (string)xdata["author"];
                if (xuser.ToLowerInvariant() == "stoaproxybot")
                    xconv = false;

                var xblid = (string)null;
                if (xconv)
                {
                    var xlink = xtext.Substring(indx1, 67);

                    var xuri = new Uri(xlink);

                    xblid = xuri.AbsolutePath.Substring(1);
                    if (xblid.Length != 32)
                        xconv = false;
                }

                if (!xprev) cmts.Add(new RedditPost { Content = xtext, Convert = xconv, Fullname = xname, BuildId = xblid });
            }

            if (!twas)
                cmts.AddRange(this.QueryComments(cmts[cmts.Count - 1].Fullname));

            return cmts;
        }

        public string Comment(string comment, string parent)
        {
            var auth = MakeBearerHeader();
            var parameters = new Dictionary<string, string>();
            parameters.Add("api_type", "json");
            parameters.Add("thing_id", parent);
            parameters.Add("text", comment);

            var req = this.RequestBuilder.Build(new Uri("https://oauth.reddit.com/api/comment"), parameters, auth);
            var res = this.RequestBuilder.GetResponse(req);
            var dat = this.RequestBuilder.GetResponseJson(res);
            return (string)dat.SelectToken("json.data.things[0].data.name");
        }

        public void UnregisterThing(string thing)
        {
            this.LastThings.Remove(thing);
        }

        public void UnregisterMiniThing(string thing)
        {
            this.LastMiniThings.Remove(thing);
        }

        public void WriteLast()
        {
            var l = AcademyLib.Location;
            l = Path.Combine(l, "v2_settings");
            var utf8 = new UTF8Encoding(false);

            // trim
            while (this.LastThings.Count > 100)
                this.LastThings.RemoveAt(0);
            while (this.LastMiniThings.Count > 1000)
                this.LastMiniThings.RemoveAt(0);

            // settings
            var sp = Path.Combine(l, "settings.json");
            var sjson = File.ReadAllText(sp, utf8);
            var sjo = JObject.Parse(sjson);
            sjo["last_things"] = new JArray(this.LastThings);
            sjo["last_minithings"] = new JArray(this.LastMiniThings);

            // write
            var settings = sjo.ToString(Formatting.None);
            File.WriteAllText(sp, settings, utf8);
        }

        private void LoadSettings()
        {
            var l = AcademyLib.Location;
            l = Path.Combine(l, "v2_settings");
            var utf8 = new UTF8Encoding(false);

            // settings
            var sp = Path.Combine(l, "settings.json");
            var sjson = File.ReadAllText(sp, utf8);
            var sjo = JObject.Parse(sjson);
            this.AppId = (string)sjo["app_id"];
            this.LastThings = new List<string>();
            this.LastMiniThings = new List<string>();
            foreach (var lt in (JArray)sjo["last_things"])
                this.LastThings.Add((string)lt);
            foreach (var lt in (JArray)sjo["last_minithings"])
                this.LastMiniThings.Add((string)lt);

            // oauth
            var op = Path.Combine(l, "oauth.json");
            var ojson = File.ReadAllText(op, utf8);
            var ojo = JObject.Parse(ojson);
            this.OAuthToken = (string)ojo["access_token"];
            this.RefreshToken = (string)ojo["refresh_token"];
        }

        private void RefreshOAuthToken()
        {
            // make data
            var token_data = new Dictionary<string, string>();
            token_data.Add("grant_type", "refresh_token");
            token_data.Add("refresh_token", this.RefreshToken);

            // make auth
            var headers = new Dictionary<HttpRequestHeader, string>();
            headers.Add(HttpRequestHeader.Authorization, MakeAuthBasic());

            // make request
            var req = this.RequestBuilder.Build(new Uri(OAUTH_TOKEN_REFRESH_URI), token_data, headers);
            var res = this.RequestBuilder.GetResponse(req);
            var json = this.RequestBuilder.GetResponseJson(res);

            // read current token
            var l = AcademyLib.Location;
            l = Path.Combine(l, "v2_settings");
            var utf8 = new UTF8Encoding(false);

            var op = Path.Combine(l, "oauth.json");
            var ojson = File.ReadAllText(op, utf8);
            var token = JObject.Parse(ojson);

            // set new token data
            this.OAuthToken = (string)json["access_token"];

            token["access_token"] = this.OAuthToken;
            token["token_type"] = (string)json["token_type"];
            token["expires_in"] = (int)json["expires_in"];
            token["scope"] = (string)json["scope"];

            // write new token
            var newtoken = token.ToString();
            File.WriteAllText(op, newtoken, utf8);
        }

        private string MakeAuthBearer()
        {
            return string.Concat("Bearer ", this.OAuthToken);
        }

        private string MakeAuthBasic()
        {
            var str = string.Concat(this.AppId, ":");
            var utf = new UTF8Encoding(false);
            var dat = utf.GetBytes(str);
            var b64 = Convert.ToBase64String(dat);

            return string.Concat("Basic ", b64);
        }

        private Dictionary<HttpRequestHeader, string> MakeBearerHeader()
        {
            return new Dictionary<HttpRequestHeader, string>()
            {
                { HttpRequestHeader.Authorization, this.MakeAuthBearer() }
            };
        }
    }
}
