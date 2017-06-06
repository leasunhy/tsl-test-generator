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
            int randomSeed = options.Seed ?? new Random().Next();
            var masterRandom = new Random(randomSeed);
            var generatedScript = TSLGenerator.GetRandomTSLScript(randomSeed, masterRandom);

            var tslTemplate = new TSLTemplate(generatedScript);
            Console.WriteLine(tslTemplate.TransformText());

            Console.WriteLine("                        ");
            Console.WriteLine("------------------------");
            Console.WriteLine("                        ");

            var testCodeTemplate = new TestCodeTemplate(generatedScript, masterRandom);
            Console.WriteLine(testCodeTemplate.TransformText());
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
