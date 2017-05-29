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

        public static TSLScript GetRandomTSLScript(int? randomSeed = null)
        {
            // initialize master random
            int seed = randomSeed ?? Random.Next();
            var masterRandom = new Random(seed);

            // generate the top level elements = Struct | Cell | Enum | Protocol | Server | Proxy | Module
            var context = new TSLGeneratorContext(masterRandom);
            var topLevelElementNumber = DiscreteUniform.Sample(masterRandom, MinTopLevelElementNumber, MaxTopLevelElementNumber);

            // NOTE(leasunhy): to ensure the order of the types of the generated elements, we use (i/total) as typeSelector
            //var topLevelElementTypeDist = new ContinuousUniform(0.0, 1.0, new Random(masterRandom.Next()));
            //foreach (var typeSelector in topLevelElementTypeDist.Samples().Take(topLevelElementNumber))
            //    TopLevelElementGenerator(typeSelector, context, masterRandom);
            var topLevelElementGenerator = context.GetTopLevelElementGenerator();
            for (int i = 0; i < topLevelElementNumber; ++i)
                topLevelElementGenerator((double)i / topLevelElementNumber);

            Debug.Assert(topLevelElementNumber == context.TopLevelElementCount);
            var topLevelElements = context.GetAllTopLevelElements().ToImmutableArray();
            return new TSLScript(topLevelElements);
        }
    }

    public static class TSLGeneratorContextExtensions
    {
        #region Generators
        public static void GenerateStruct(this TSLGeneratorContext context)
        {
            var name = $"Struct_{context.TopLevelElementCount + 1}";
            var numberOfFields = DiscreteUniform.Sample(context.MasterRandom, MinFieldNumber, MaxFieldNumber);
            var fields = new List<TSLField>(numberOfFields);
            for (int i = 0; i < numberOfFields; ++i)
                fields.Add(context.GenerateRandomField());
            var result = new TSLStruct(name, fields);
            context.Structs.Add(result);
        }

        public static void GenerateServer(this TSLGeneratorContext context)
        {
            // TODO
        }

        public static void GenerateProxy(this TSLGeneratorContext context)
        {
            // TODO
        }

        public static void GenerateModule(this TSLGeneratorContext context)
        {
            // TODO
        }

        public static void GenerateProtocol(this TSLGeneratorContext context)
        {
            // TODO
        }

        public static void GenerateEnum(this TSLGeneratorContext context)
        {
            // TODO
        }

        public static void GenerateCell(this TSLGeneratorContext context)
        {
            // TODO
        }
        #endregion

        #region Helpers
        public static TSLField GenerateRandomField(this TSLGeneratorContext context)
        {
            var optional = ContinuousUniform.Sample(context.MasterRandom, 0.0, 1.0) <
                FieldProbabilities.OptionalFieldProbability;
            var name = $"field{context.GeneratedElementCount}";
            var type = context.GenerateRandomType();
            context.GeneratedElementCount += 1;
            // TODO(leasunhy): generate attributes
            var field = new TSLField(type, name, optional, attributes: null);
            return field;
        }

        public static ITSLType GenerateRandomType(this TSLGeneratorContext context)
        {
            // TODO
            return AtomType.AtomTypes[0];
        }
        #endregion
    }

    public class TSLGeneratorContext
    {
        public TSLGeneratorContext(Random masterRandom)
        {
            MasterRandom = masterRandom;
        }

        public Random MasterRandom { get; }
        public int TopLevelElementCount { get; private set; } = 0;
        public int GeneratedElementCount { get; set; } = 0;

        public List<TSLStruct> Structs { get; } = new List<TSLStruct>();
        public List<TSLCell> Cells { get; } = new List<TSLCell>();
        public List<TSLEnum> Enums { get; } = new List<TSLEnum>();
        public List<TSLProtocol> Protocols { get; } = new List<TSLProtocol>();
        public List<TSLServer> Servers { get; } = new List<TSLServer>();
        public List<TSLProxy> Proxies { get; } = new List<TSLProxy>();
        public List<TSLModule> Modules { get; } = new List<TSLModule>();

        public IEnumerable<ITSLTopLevelElement> GetAllTopLevelElements() =>
            new IEnumerable<ITSLTopLevelElement>[] { Structs, Cells, Enums, Protocols, Servers, Proxies, Modules }.Aggregate(Enumerable.Concat);

        #region Total Generator
        // NOTE(leasunhy): the bottom-most combinator is evaluated first due to implementation of operator |
        // top level elements = Struct | Cell | Enum | Protocol | Server | Proxy | Module
        public Action<double> GetTopLevelElementGenerator() => typeSelector =>
        {
            InnerGenerator(typeSelector, this);
            TopLevelElementCount += 1;
            GeneratedElementCount += 1;
        };

        public static readonly Action<double, TSLGeneratorContext> InnerGenerator =
        (
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Proxy, TSLGeneratorContextExtensions.GenerateProxy) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Server, TSLGeneratorContextExtensions.GenerateServer) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Module, TSLGeneratorContextExtensions.GenerateModule) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Protocol, TSLGeneratorContextExtensions.GenerateProtocol) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Cell, TSLGeneratorContextExtensions.GenerateCell) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Struct, TSLGeneratorContextExtensions.GenerateStruct) |
            new TSLGeneratorCombinator(TopLevelElementProbabilities.Enum, TSLGeneratorContextExtensions.GenerateEnum)
        ).Generate;
        #endregion
    }

    public class TSLGeneratorCombinator
    {
        public delegate void GenerateDelegate(TSLGeneratorContext context);
        public double Probability { get; }
        public GenerateDelegate GenerateFunc { get; }
        public TSLGeneratorCombinator NextCombinator { get; }

        public TSLGeneratorCombinator(double probability, GenerateDelegate generateFunc, TSLGeneratorCombinator next = null)
        {
            Probability = probability;
            GenerateFunc = generateFunc;
            NextCombinator = next;
        }

        public void Generate(double typeSelector, TSLGeneratorContext context)
        {
            if (typeSelector - Probability > 0)
            {
                NextCombinator?.Generate(typeSelector - Probability, context);
            }
            else
            {
                GenerateFunc(context);
            }
        }

        public TSLGeneratorCombinator WithNext(TSLGeneratorCombinator next) => new TSLGeneratorCombinator(Probability, GenerateFunc, next);

        //public TSLGeneratorCombinator WithLast(TSLGeneratorCombinator last) => WithNext(NextCombinator?.WithLast(last) ?? last);

        //// if only `|` is right-associative!
        //public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => lhs.WithLast(rhs);

        public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => rhs.WithNext(lhs);
    }
}
