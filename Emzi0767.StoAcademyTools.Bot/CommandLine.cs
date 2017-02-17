namespace Emzi0767.StoAcademyTools.Bot
{
    internal class CommandLine
    {
        [CommandLineParameter("single")]
        public string SingleId { get; set; }

        [CommandLineParameter("target")]
        public string Target { get; set; }
    }
}
