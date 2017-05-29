using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using NUnit.Framework.Constraints;
using TSLTestGenerator.DataModel;

using static TSLTestGenerator.DefaultSettings;

namespace TSLTestGenerator
{
    public class TSLGenerator
    {
        private static readonly Random Random = new Random();

        // NOTE(leasunhy): the bottom-most combinator is evaluated first due to implementation of operator |
        private static readonly Action<double, TSLGeneratorContext, Random> TopLevelElementGenerator =
        (
            new TSLGeneratorCombinator(ProxyProbability, GenerateProxy) |
            new TSLGeneratorCombinator(ServerProbability, GenerateServer) |
            new TSLGeneratorCombinator(ModuleProbability, GenerateModule) |
            new TSLGeneratorCombinator(ProtocolProbability, GenerateProtocol) |
            new TSLGeneratorCombinator(CellProbability, GenerateCell) |
            new TSLGeneratorCombinator(StructProbability, GenerateStruct) |
            new TSLGeneratorCombinator(EnumProbability, GenerateEnum)
        ).Generate;

        public static TSLScript GetRandomTSLScript(int? randomSeed = null)
        {
            // initialize master random
            int seed = randomSeed ?? Random.Next();
            var masterRandom = new Random(seed);

            // initialize distributions
            var topLevelElementNumberDist = new DiscreteUniform(MinTopLevelElementNumber, MaxTopLevelElementNumber, new Random(masterRandom.Next()));
            //var topLevelElementTypeDist = new ContinuousUniform(0.0, 1.0, new Random(masterRandom.Next()));

            // generate the top level elements
            // Top Level Element = Struct | Cell | Enum | Protocol | Server | Proxy | Module
            var context = new TSLGeneratorContext();
            var topLevelElementNumber = topLevelElementNumberDist.Sample();

            // NOTE(leasunhy): to ensure the order of the types of the generated elements, we use (i/total) as typeSelector
            //foreach (var typeSelector in topLevelElementTypeDist.Samples().Take(topLevelElementNumber))
            //    TopLevelElementGenerator(typeSelector, context, masterRandom);
            for (int i = 0; i < topLevelElementNumber; ++i)
                TopLevelElementGenerator((double) i / topLevelElementNumber, context, masterRandom);

            Debug.Assert(topLevelElementNumber == context.ElementCount);
            var topLevelElements = context.GetAllTopLevelElements().ToImmutableArray();
            return new TSLScript(topLevelElements);
        }

        public static void GenerateStruct(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
            var name = $"Struct_{context.ElementCount + 1}";
            var result = new TSLStruct(name, Enumerable.Empty<TSLField>());
            context.Structs.Add(result);
        }

        public static void GenerateServer(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        public static void GenerateProxy(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        public static void GenerateModule(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        public static void GenerateProtocol(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        public static void GenerateEnum(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        public static void GenerateCell(TSLGeneratorContext context, Random masterRandom)
        {
            // TODO
        }

        private class TSLGeneratorCombinator
        {
            public delegate void GenerateDelegate(TSLGeneratorContext context, Random masterRandom);
            public double Probability { get; }
            public GenerateDelegate GenerateFunc { get; }
            public TSLGeneratorCombinator NextCombinator { get; }

            public TSLGeneratorCombinator(double probability, GenerateDelegate generateFunc, TSLGeneratorCombinator next = null)
            {
                Probability = probability;
                GenerateFunc = generateFunc;
                NextCombinator = next;
            }

            public void Generate(double typeSelector, TSLGeneratorContext context, Random masterRandom)
            {
                if (typeSelector - Probability > 0)
                {
                    NextCombinator?.Generate(typeSelector - Probability, context, masterRandom);
                }
                else
                {
                    GenerateFunc(context, masterRandom);
                    context.ElementCount += 1;
                }
            }

            public TSLGeneratorCombinator WithNext(TSLGeneratorCombinator next) => new TSLGeneratorCombinator(Probability, GenerateFunc, next);

            //public TSLGeneratorCombinator WithLast(TSLGeneratorCombinator last) => WithNext(NextCombinator?.WithLast(last) ?? last);

            //// if only `|` is right-associative!
            //public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => lhs.WithLast(rhs);

            public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => rhs.WithNext(lhs);
        }
    }

    public class TSLGeneratorContext
    {
        public int ElementCount { get; set; } = 0;

        public List<TSLStruct> Structs { get; } = new List<TSLStruct>();
        public List<TSLCell> Cells { get; } = new List<TSLCell>();
        public List<TSLEnum> Enums { get; } = new List<TSLEnum>();
        public List<TSLProtocol> Protocols { get; } = new List<TSLProtocol>();
        public List<TSLServer> Servers { get; } = new List<TSLServer>();
        public List<TSLProxy> Proxies { get; } = new List<TSLProxy>();
        public List<TSLModule> Modules { get; } = new List<TSLModule>();

        public IEnumerable<ITSLTopLevelElement> GetAllTopLevelElements() =>
            new IEnumerable<ITSLTopLevelElement>[] { Structs, Cells, Enums, Protocols, Servers, Proxies, Modules }.Aggregate(Enumerable.Concat);
    }
}
