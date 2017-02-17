using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Emzi0767.Gaming.Sto.AcademyConverterBot.Reddit;
using Emzi0767.Gaming.Sto.AcademyConverterBot.Utilities;
using Emzi0767.Gaming.Sto.StoaLib;
using Emzi0767.Tools.MicroLogger;

namespace Emzi0767.Gaming.Sto.AcademyConverterBot
{
    internal static class Program
    {
        internal static string Location { get; private set; }
        private static Dictionary<string, Assembly> Assemblies { get; set; }

        internal static void Main(string[] args)
        {
            var a = Assembly.GetExecutingAssembly();
            var l = a.Location;
            Location = Path.GetDirectoryName(l);
            Assemblies = new Dictionary<string, Assembly>();
            
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            l = Path.Combine(Location, "v2_lib");
            if (Directory.Exists(l))
            {
                var ls = Directory.GetFiles(l, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (var xl in ls)
                {
                    try
                    {
                        var xa = Assembly.Load(File.ReadAllBytes(xl));
                        Assemblies.Add(xa.FullName, xa);
                    }
                    catch (Exception ex) { Console.WriteLine("CRITICAL: Could not load {0}: {2}/{1}", xl, ex.Message, ex.GetType().ToString()); }
                }
            }
            else
                Console.WriteLine("CRITICAL: v2_lib does not exist");

            L.D(Debugger.IsAttached);
            L.R(Console.Out);
            L.W("ACB v2", "Initializing");
            L.W("ACB v2", "Running from \"{0}\"", Location);

            L.W("ACB v2", "Parsing commandline");
            var cmdl = ParseCommandline(args);
            L.W("ACB v2", "Options:");
            L.W("ACB v2", "  Override: {0}", cmdl.Target);
            L.W("ACB v2", "  Single: {0}", cmdl.SingleId);

            L.W("STOA Lib", "Initializing STO Academy tools");
            var tools = new StoAcademyTools();
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
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (Assemblies.ContainsKey(args.Name))
                return Assemblies[args.Name];
            return null;
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
