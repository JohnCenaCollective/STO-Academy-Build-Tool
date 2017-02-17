using System;

namespace Emzi0767.StoAcademyTools.Bot
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class CommandLineParameterAttribute : Attribute
    {
        public string Name { get; private set; }

        public CommandLineParameterAttribute(string param_name)
        {
            this.Name = param_name;
        }
    }
}
