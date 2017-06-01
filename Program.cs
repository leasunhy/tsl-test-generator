using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using TSLTestGenerator.Templates;

namespace TSLTestGenerator
{
    static class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
                Environment.Exit(-1);
            var generatedScript = TSLGenerator.GetRandomTSLScript(options.Seed);
            var template = new TSLTemplate(generatedScript);
            Console.WriteLine(template.TransformText());
        }
    }

    internal class CommandLineOptions
    {
        [Option('s', "seed", DefaultValue = null, HelpText = "The seed for initializing the Random instance.")]
        public int? Seed { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
