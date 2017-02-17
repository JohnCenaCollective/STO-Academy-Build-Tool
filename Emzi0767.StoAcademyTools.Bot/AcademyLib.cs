using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Emzi0767.StoAcademyTools.Bot.Reddit;
using Emzi0767.StoAcademyTools.Bot.Utilities;
using Emzi0767.StoAcademyTools.Library;
using Emzi0767.Tools.MicroLogger;
using Newtonsoft.Json.Linq;

namespace Emzi0767.StoAcademyTools.Bot
{
    public static class AcademyLib
    {
        public static string Location { get; private set; }
        public static Version LibAcademyVersion { get; private set; }
        public static Version LibBotVersion { get; private set; }

        private static bool UseDiscordLog { get; set; }
        private static IPEndPoint DiscordLogEndpoint { get; set; }
        private static StringBuilder DiscordLog { get; set; }

        public static void RunBot(string[] args)
        {
            LoadConf();

            L.D(Debugger.IsAttached);
            L.R(Console.Out);

            // this is for discord logger
            if (UseDiscordLog)
            {
                var dsb = new StringBuilder();
                var dsw = new StringWriter(dsb);
                L.R(dsw);
                DiscordLog = dsb;
            }

            L.W("ACB v2", "Initializing");
            L.W("ACB v2", "Running from \"{0}\"", Location);
            L.W("ACB v2", "Bot Version: {0}", LibBotVersion);
            L.W("ACB v2", "LibStoa Version: {0}", LibAcademyVersion);

            L.W("ACB v2", "Parsing commandline");
            var cmdl = ParseCommandline(args);
            L.W("ACB v2", "Options:");
            L.W("ACB v2", "  Override: {0}", cmdl.Target);
            L.W("ACB v2", "  Single: {0}", cmdl.SingleId);

            L.W("STOA Lib", "Initializing STO Academy tools");
            var tools = new StoAcademyInterface();
            tools.Initialize();

            L.W("ACB-HTTP", "Initializing HTTP component");
            var http = new HttpRequestBuilder();
            http.UserAgent = "linux:stoap:2 (by /u/eMZi0767)";

            L.W("ACB-REDDIT", "Initializing Reddit component");
            var reddit = new RedditApi(http);
            reddit.Initialize();

            L.W("ACB-REDDIT", "Querying STOBuilds");
            var posts = reddit.QueryPosts();
            var comments = reddit.QueryComments();

            L.W("ACB v2", "Checking posts");
            foreach (var post in posts)
            {
                try
                {
                    if (!post.Convert)
                        continue;

                    L.W("ACB-STOA", "Post {0} requires conversion", post.Fullname);

                    L.W("ACB-STOA", "Loading build");
                    var xbld = tools.GetBuild(post.BuildId);

                    L.W("ACB-BLDW", "Writing markdown");
                    var xbsb = new StringBuilder();
                    using (var sw = new StringWriter(xbsb))
                    using (var md = new MarkdownWriter(sw))
                    using (var bw = new BuildWriter(md))
                        bw.WriteBuild(xbld, tools);

                    L.W("ACB-MD", "Making comments");
                    var xblc = xbsb.ToString().Replace(Environment.NewLine, "\n");
                    var xbps = new List<string>();
                    if (xblc.Length > 10000)
                    {
                        L.W("ACB-MD", "Comment too long, splitting");

                        var xscs = xblc.Split(new string[] { "\n\n---\n\n" }, StringSplitOptions.None);
                        var xcpr = "";
                        foreach (var xsec in xscs)
                        {
                            if (xsec.Length > 10000)
                                throw new Exception("Section too long");

                            if (xcpr.Length + xsec.Length + 7 < 10000)
                            {
                                if (!string.IsNullOrWhiteSpace(xcpr))
                                    xcpr += "\n\n---\n\n" + xsec;
                                else
                                    xcpr += xsec;
                            }
                            else
                            {
                                xbps.Add(xcpr);
                                xcpr = xsec;
                            }
                        }
                        xbps.Add(xcpr);
                    }
                    else
                    {
                        xbps.Add(xblc);
                    }

                    var xcpn = string.IsNullOrWhiteSpace(cmdl.Target) ? post.Fullname : cmdl.Target;
                    foreach (var xbpt in xbps)
                    {
                        L.W("ACB-REDDIT", "Writing comment");
                        xcpn = reddit.Comment(xbpt, xcpn);
                    }
                }
                catch (Exception ex)
                {
                    L.X("ACB v2 ERR", ex);
                    reddit.UnregisterThing(post.Fullname);
                }
            }

            L.W("ACB v2", "Checking comments");
            foreach (var comment in comments)
            {
                try
                {
                    if (!comment.Convert)
                        continue;

                    L.W("ACB-STOA", "Comment {0} requires conversion", comment.Fullname);

                    L.W("ACB-STOA", "Loading build");
                    var xbld = tools.GetBuild(comment.BuildId);

                    L.W("ACB-BLDW", "Writing markdown");
                    var xbsb = new StringBuilder();
                    using (var sw = new StringWriter(xbsb))
                    using (var md = new MarkdownWriter(sw))
                    using (var bw = new BuildWriter(md))
                        bw.WriteBuild(xbld, tools);

                    L.W("ACB-MD", "Making comments");
                    var xblc = xbsb.ToString().Replace(Environment.NewLine, "\n");
                    var xbps = new List<string>();
                    if (xblc.Length > 10000)
                    {
                        L.W("ACB-MD", "Comment too long, splitting");

                        var xscs = xblc.Split(new string[] { "\n\n---\n\n" }, StringSplitOptions.None);
                        var xcpr = "";
                        foreach (var xsec in xscs)
                        {
                            if (xsec.Length > 10000)
                                throw new Exception("Section too long");

                            if (xcpr.Length + xsec.Length + 7 < 10000)
                            {
                                if (!string.IsNullOrWhiteSpace(xcpr))
                                    xcpr += "\n\n---\n\n" + xsec;
                                else
                                    xcpr += xsec;
                            }
                            else
                            {
                                xbps.Add(xcpr);
                                xcpr = xsec;
                            }
                        }
                        xbps.Add(xcpr);
                    }
                    else
                    {
                        xbps.Add(xblc);
                    }

                    var xcpn = string.IsNullOrWhiteSpace(cmdl.Target) ? comment.Fullname : cmdl.Target;
                    foreach (var xbpt in xbps)
                    {
                        L.W("ACB-REDDIT", "Writing comment");
                        xcpn = reddit.Comment(xbpt, xcpn);
                    }
                }
                catch (Exception ex)
                {
                    L.X("ACB v2 ERR", ex);
                    reddit.UnregisterMiniThing(comment.Fullname);
                }
            }

            L.W("ACB v2", "Writing settings");
            reddit.WriteLast();

            L.W("ACB v2", "All operations completed");
            L.Q();

            // write the discord log
            if (UseDiscordLog)
            {
                var sender = new Socket(DiscordLogEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(DiscordLogEndpoint);
                var msgd = new UTF8Encoding(false).GetBytes(DiscordLog.ToString());
                var msgl = BitConverter.GetBytes((ulong)msgd.Length);
                var msg = new byte[msgd.Length + msgl.Length];
                Array.Copy(msgl, 0, msg, 0, msgl.Length);
                Array.Copy(msgd, 0, msg, msgl.Length, msgd.Length);
                sender.Send(msg);
                sender.Close();
            }
        }

        private static void LoadConf()
        {
            var l = Location;
            l = Path.Combine(l, "v2_settings");
            var utf8 = new UTF8Encoding(false);

            // settings
            var sp = Path.Combine(l, "config.json");
            var sjson = File.ReadAllText(sp, utf8);
            var sjo = JObject.Parse(sjson);
            UseDiscordLog = (bool)sjo["use_discord"];
            DiscordLogEndpoint = new IPEndPoint(IPAddress.Parse((string)sjo["discord_ip"]), (int)sjo["discord_port"]);
        }

        private static CommandLine ParseCommandline(string[] args)
        {
            var t = typeof(CommandLine);
            var ps = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var pd = new Dictionary<string, PropertyInfo>();
            var cd = new CommandLine();
            foreach (var p in ps)
            {
                var attr = (CommandLineParameterAttribute)Attribute.GetCustomAttribute(p, typeof(CommandLineParameterAttribute));
                if (attr == null)
                    continue;

                pd.Add(attr.Name, p);
            }

            foreach (var arg in args)
            {
                if (!arg.StartsWith("--"))
                    continue;

                var eqi = arg.IndexOf('=');
                if (eqi == -1)
                    eqi = arg.Length;

                var argn = arg.Substring(2, eqi - 2);
                var argv = eqi + 1 > arg.Length ? null : arg.Substring(eqi + 1);

                if (!pd.ContainsKey(argn))
                    continue;

                pd[argn].SetValue(cd, argv);
            }

            return cd;
        }
    }
}
