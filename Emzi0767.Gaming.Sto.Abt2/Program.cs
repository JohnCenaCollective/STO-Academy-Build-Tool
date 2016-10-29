using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Emzi0767.Gaming.Sto.Abt2
{
    public static class Program
    {
        internal static string buildid = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAbtMain());

            Application.ThreadException -= Application_ThreadException;
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MakeErrorReport(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MakeErrorReport(e.ExceptionObject as Exception);
        }

        private static void MakeErrorReport(Exception e)
        {
            var frm = Form.ActiveForm;
            var now = DateTime.UtcNow;
            var a = Assembly.GetExecutingAssembly();
            var al = a.Location;
            var ap = Path.GetDirectoryName(al);
            var fi = (FileInfo)null;

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var proc = Process.GetCurrentProcess();
                var pmod = proc.MainModule;
                var os = Environment.OSVersion;
                var os64 = Environment.Is64BitOperatingSystem;
                var p64 = Environment.Is64BitProcess;
                var apd = AppDomain.CurrentDomain;
                var apa = apd.GetAssemblies();

                sw.WriteLine("ERROR REPORT");
                sw.WriteLine("Timestamp (localtime):       {0:yyyy-MM-ddTHH:mm:ss zzz}", now.ToLocalTime());
                sw.WriteLine("Timestamp (UTC):             {0:yyyy-MM-ddTHH:mm:ss}", now);
                sw.WriteLine();
                sw.WriteLine("ENVIRONMENT INFO");
                sw.WriteLine("OS Platform:                 {0}", os.Platform.ToString());
                sw.WriteLine("OS Version:                  {0} ({1}); Service Pack: {2}", os.Version, os.VersionString, os.ServicePack);
                sw.WriteLine("OS 64-bit:                   {0}", os64 ? "Yes" : "No");
                sw.WriteLine();
                sw.WriteLine("PROCESS INFO");
                sw.WriteLine("PID:                         {0}", proc.Id);
                sw.WriteLine("Process 64-bit:              {0}", p64 ? "Yes" : "No");
                sw.WriteLine("Main Module Base Address:    {0}", pmod.BaseAddress.ToPtrString(os64));
                sw.WriteLine("Main Module Entry Point:     {0}", pmod.EntryPointAddress.ToPtrString(os64));
                sw.WriteLine("Main Module Name:            {0}", pmod.ModuleName);
                sw.WriteLine("Main Module File Name:       {0}", pmod.FileName);
                sw.WriteLine("Main Module File Version:    {0}/{1}", pmod.FileVersionInfo.FileVersion, pmod.FileVersionInfo.ProductVersion);
                sw.WriteLine("Process Module Count:        {0:#,##0}", proc.Modules.Count);
                sw.WriteLine("Process Modules:");
                foreach (ProcessModule xpmod in proc.Modules)
                {
                    sw.WriteLine(" - {0}: {1}", xpmod.BaseAddress.ToPtrString(os64), xpmod.ModuleName);
                    sw.WriteLine("   Loaded from: {0}", xpmod.FileName);
                    sw.WriteLine("   Entry point: {0}", xpmod.EntryPointAddress.ToPtrString(os64));
                    sw.WriteLine("   Version:     {0}/{1}", xpmod.FileVersionInfo.FileVersion, xpmod.FileVersionInfo.ProductVersion);
                    sw.WriteLine("   Description: {0}", xpmod.FileVersionInfo.FileDescription);
                }
                sw.WriteLine("Process Working Set:         {0} ({1:#,##0}B)", proc.WorkingSet64.ToSize(), proc.WorkingSet64);
                sw.WriteLine("Process Virtual Memory Size: {0} ({1:#,##0}B)", proc.VirtualMemorySize64.ToSize(), proc.VirtualMemorySize64);
                sw.WriteLine("Process Main Window Title:   {0}", proc.MainWindowTitle);
                sw.WriteLine("Process Command Line:        {0} {1}", proc.StartInfo.FileName, proc.StartInfo.Arguments);
                sw.WriteLine("Process Start Time:          {0:yyyy-MM-ddTHH:mm:ss zzz}", proc.StartTime);
                sw.WriteLine("Process Thread Count:        {0:#,##0}", proc.Threads.Count);
                sw.WriteLine("Process Processor Time:      {0:c}", proc.TotalProcessorTime);
                sw.WriteLine("Process User Processor Time: {0:c}", proc.UserProcessorTime);
                sw.WriteLine();
                sw.WriteLine(".NET INFORMATION");
                sw.WriteLine(".NET Environment Version:    {0}", Environment.Version);
                sw.WriteLine("Is Mono:                     {0}", Type.GetType("Mono.Runtime") != null ? "Yes" : "No");
                sw.WriteLine();
                sw.WriteLine("APP DOMAIN INFO");
                sw.WriteLine("ID:                          {0}", apd.Id);
                sw.WriteLine("Name:                        {0}", apd.FriendlyName);
                sw.WriteLine("Resolver Base Directory:     {0}", apd.BaseDirectory);
                sw.WriteLine("Dynamic Assembly Directory:  {0}", apd.DynamicDirectory);
                sw.WriteLine("Relative Search Directory:   {0}", apd.RelativeSearchPath);
                sw.WriteLine("Loaded Assmebly Count:       {0:#,##0}", apa.Length);
                sw.WriteLine("Loaded Assemblies:");
                foreach (var xa in apa)
                {
                    sw.WriteLine(" - {0}", xa.FullName);
                    sw.WriteLine("   Version:     {0}", xa.GetName().Version);
                    sw.WriteLine("   Loaded From: {0}", xa.Location);
                    sw.WriteLine("   Code Base:   {0}", xa.CodeBase);
                    sw.WriteLine("   Entry Point: {0}", xa.EntryPoint != null ? xa.EntryPoint.ToString() : "<none>");
                }
                sw.WriteLine();
                sw.WriteLine("FORM STATE INFO");
                sw.WriteLine("Active Build ID:             {0}", buildid);
                sw.WriteLine();

                sw.WriteLine("EXCEPTION INFO");
                var ex = e;
                while (ex != null)
                {
                    sw.WriteLine("Type:                        {0}", ex.GetType());
                    sw.WriteLine("Message:                     {0}", ex.Message);
                    sw.WriteLine("Source:                      {0}", ex.Source);
                    sw.WriteLine("Target Site:                 {0}", ex.TargetSite.ToString());
                    sw.WriteLine("HRESULT:                     {0}", ex.HResult);
                    sw.WriteLine("Data:");
                    foreach (DictionaryEntry datum in ex.Data)
                    {
                        sw.WriteLine(" - {0}", datum.Key);
                        sw.WriteLine("   Type: {0}", datum.Value.GetType());
                        sw.WriteLine("   Data: {0}", datum.Value.ToString());
                    }
                    sw.WriteLine("Stack Trace:");
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine();
                    ex = ex.InnerException;
                    if (ex != null)
                    {
                        sw.WriteLine("INNER EXCEPTION INFO");
                    }
                }
            }

            var exdatum = sb.ToString();
            var utf = new UTF8Encoding(false);
            var exdat = utf.GetBytes(exdatum);

            var fn = string.Concat("bugreport-", now.ToString("yyyyMMddHHmmss"), ".bug");
            fi = new FileInfo(Path.Combine(ap, fn));
            try
            {
                using (var fs = fi.Create())
                using (var gz = new GZipStream(fs, CompressionLevel.Optimal))
                    gz.Write(exdat, 0, exdat.Length);

                Process.Start(Path.Combine(ap, "errorview.exe"), fi.FullName);
            }
            catch (Exception)
            {
                fi = null;
                MessageBox.Show("An error occured within the application, and error log could not be created. Application will now exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.ThreadException -= Application_ThreadException;
            Application.Exit();
        }

        private static string ToPtrString(this IntPtr p, bool is64)
        {
            return string.Concat("0x", is64 ? p.ToInt64().ToString("X16") : p.ToInt32().ToString("X8"));
        }

        private static string ToSize(this long l)
        {
            var d = (double)l;
            int i = 0;
            var p = new string[] { "", "k", "M", "G", "T" };
            while (d >= 900D)
            {
                d /= 1024D;
                i++;
            }
            return string.Concat(d.ToString("#,##0.00"), " ", p[i], "B");
        }
    }
}
