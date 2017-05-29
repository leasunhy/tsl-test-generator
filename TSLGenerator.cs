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
            InnerTopLevelElementGenerator(typeSelector, this);
            TopLevelElementCount += 1;
            GeneratedElementCount += 1;
        };

        public static readonly Func<double, TSLGeneratorContext, ITSLTopLevelElement> InnerTopLevelElementGenerator =
        (
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Proxy, TopLevelElementGenerators.GenerateProxy) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Server, TopLevelElementGenerators.GenerateServer) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Module, TopLevelElementGenerators.GenerateModule) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Protocol, TopLevelElementGenerators.GenerateProtocol) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Cell, TopLevelElementGenerators.GenerateCell) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Struct, TopLevelElementGenerators.GenerateStruct) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Enum, TopLevelElementGenerators.GenerateEnum)
        ).Generate;

        public static readonly Func<double, TSLGeneratorContext, ITSLType> TypeGenerator =
        (
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Enum, TypeGenerators.GenerateEnumType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Atom, TypeGenerators.GenerateAtomType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.ContainerArray, TypeGenerators.GenerateArrayType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.ContainerList, TypeGenerators.GenerateListType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Struct, TypeGenerators.GenerateStructType)
        ).Generate;
        #endregion
    }

    public class TSLGeneratorCombinator<R>
    {
        public delegate R GenerateDelegate(TSLGeneratorContext context);
        public double Probability { get; }
        public GenerateDelegate GenerateFunc { get; }
        public TSLGeneratorCombinator<R> NextCombinator { get; }

        public TSLGeneratorCombinator(double probability, GenerateDelegate generateFunc, TSLGeneratorCombinator<R> next = null)
        {
            Probability = probability;
            GenerateFunc = generateFunc;
            NextCombinator = next;
        }

        public R Generate(double typeSelector, TSLGeneratorContext context)
        {
            if (typeSelector - Probability > 0)
                return NextCombinator != null ? NextCombinator.Generate(typeSelector - Probability, context) : default(R);
            else
                return GenerateFunc(context);
        }

        public TSLGeneratorCombinator<R> WithNext(TSLGeneratorCombinator<R> next) => new TSLGeneratorCombinator<R>(Probability, GenerateFunc, next);

        //public TSLGeneratorCombinator WithLast(TSLGeneratorCombinator last) => WithNext(NextCombinator?.WithLast(last) ?? last);

        //// if only `|` is right-associative!
        //public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => lhs.WithLast(rhs);

        public static TSLGeneratorCombinator<R> operator |(TSLGeneratorCombinator<R> lhs, TSLGeneratorCombinator<R> rhs) => rhs.WithNext(lhs);
    }
}
