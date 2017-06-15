using System;
using System.Collections.Generic;
using System.IO;
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

            var outputFolder = options.OutputFolder ?? ".";
            var testName = options.TestName ?? $"GeneratedTSLTest{randomSeed}";
            var outputPath = Path.Combine(outputFolder, testName);
            Directory.CreateDirectory(outputPath);

            var generatorContext = new TestCodeGeneratorContext(testName, generatedScript, masterRandom);

            Console.WriteLine("Generating files...");

            Console.WriteLine("Generating tsl script...");
            var tslTemplate = new TSLTemplate(generatedScript);
            File.WriteAllText(Path.Combine(outputPath, testName + ".tsl"), tslTemplate.TransformText());

            Console.WriteLine("Generating C# test code...");
            var csFileList = new List<string>();
            foreach (var element in generatedScript.TopLevelElements)
            {
                var fileName = $"{testName}_{element.Name}.cs";
                csFileList.Add(fileName);
                Console.WriteLine($"\tGenerating {fileName}...");
                var testCodeTemplate = new TestCodeTemplate(generatorContext, element);
                File.WriteAllText(Path.Combine(outputPath, fileName), testCodeTemplate.TransformText());
            }

            Console.WriteLine("Generating Utils.cs...");
            var utilsTemplate = new UtilsTemplate(testName);
            File.WriteAllText(Path.Combine(outputPath, "Utils.cs"), utilsTemplate.TransformText());

            Console.WriteLine("Generating packages.config...");
            var packagesConfigTemplate = new PackagesConfigTemplate();
            File.WriteAllText(Path.Combine(outputPath, "packages.config"), packagesConfigTemplate.TransformText());

            Console.WriteLine("Generating netfx project file...");
            var netfxProjectFileTemplate = new NetfxProjectFileTemplate(generatorContext, csFileList);
            File.WriteAllText(Path.Combine(outputPath, testName + ".csproj"), netfxProjectFileTemplate.TransformText());

            Console.WriteLine("Generating netcore project file...");
            var netcoreProjectFileTemplate = new NetcoreProjectFileTemplate(generatorContext);
            File.WriteAllText(Path.Combine(outputPath, testName + "_coreclr.csproj"), netcoreProjectFileTemplate.TransformText());

            Console.WriteLine("Done.");
        }
    }

    internal class CommandLineOptions
    {
        [Option('s', "seed", DefaultValue = null, HelpText = "The seed for initializing the Random instance.")]
        public int? Seed { get; set; }

        [Option('i', "testName", DefaultValue = null, HelpText = "The name for the generated test.")]
        public string TestName { get; set; }

        [Option('o', "outputFolder", DefaultValue = null, HelpText = "The path to the folder to hold the generated test project.")]
        public string OutputFolder { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
