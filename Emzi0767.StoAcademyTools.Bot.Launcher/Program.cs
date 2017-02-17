using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Emzi0767.StoAcademyTools.Bot.Launcher
{
    internal static class Program
    {
        internal static string Location { get; private set; }
        private static Dictionary<string, Assembly> Assemblies { get; set; }

        internal static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var a = Assembly.GetExecutingAssembly();
            var l = a.Location;
            Location = Path.GetDirectoryName(l);
            Assemblies = new Dictionary<string, Assembly>();

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

            var bot = Assemblies.First(kvp => kvp.Value.FullName.Contains("Emzi0767.StoAcademyTools.AcademyConverterBot")).Value;
            var lib = Assemblies.First(kvp => kvp.Value.FullName.Contains("Emzi0767.StoAcademyTools.StoaLib")).Value;

            var bot_cls = bot.GetType("Emzi0767.StoAcademyTools.AcademyConverterBot.AcademyLib");
            var bot_mtd = bot_cls.GetMethod("RunBot", BindingFlags.Public | BindingFlags.Static);

            var bot_lcf = bot_cls.GetProperty("Location", BindingFlags.Public | BindingFlags.Static);
            var bot_lcm = bot_lcf.SetMethod;

            var bot_lvf = bot_cls.GetProperty("LibAcademyVersion", BindingFlags.Public | BindingFlags.Static);
            var bot_lvm = bot_lvf.SetMethod;

            var bot_bvf = bot_cls.GetProperty("LibBotVersion", BindingFlags.Public | BindingFlags.Static);
            var bot_bvm = bot_bvf.SetMethod;

            bot_lcm.Invoke(null, new object[] { Location });
            bot_lvm.Invoke(null, new object[] { lib.GetName().Version });
            bot_bvm.Invoke(null, new object[] { bot.GetName().Version });
            bot_mtd.Invoke(null, new object[] { args });
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (Assemblies.ContainsKey(args.Name))
                return Assemblies[args.Name];
            return null;
        }
    }
}
