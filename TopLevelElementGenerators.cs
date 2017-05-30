using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using TSLTestGenerator.DataModel;

namespace TSLTestGenerator
{
    public static class TopLevelElementGenerators
    {
        #region Total Generator
        public static readonly Func<double, TSLGeneratorContext, ITSLTopLevelElement> StaticTopLevelElementGenerator =
        (
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Proxy, TopLevelElementGenerators.GenerateProxy) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Server, TopLevelElementGenerators.GenerateServer) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Module, TopLevelElementGenerators.GenerateModule) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Protocol, TopLevelElementGenerators.GenerateProtocol) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Cell, TopLevelElementGenerators.GenerateCell) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Struct, TopLevelElementGenerators.GenerateStruct) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Enum, TopLevelElementGenerators.GenerateEnum)
        ).Generate;
        #endregion

        #region Generators
        public static ITSLTopLevelElement GenerateStruct(this TSLGeneratorContext context)
        {
            var name = $"Struct_{context.TopLevelElementCount + 1}";
            var numberOfFields = DiscreteUniform.Sample(context.MasterRandom, DefaultSettings.MinFieldNumber, DefaultSettings.MaxFieldNumber);
            var fields = context.RandomFields().Take(numberOfFields);
            var result = new TSLStruct(name, fields);
            context.Structs.Add(result);
            if (!result.DynamicLengthed)
                context.FixedLengthStructs.Add(result);
            return result;
        }

        public static ITSLTopLevelElement GenerateCell(this TSLGeneratorContext context)
        {
            var name = $"CellStruct_{context.TopLevelElementCount + 1}";
            var numberOfFields = DiscreteUniform.Sample(context.MasterRandom, DefaultSettings.MinFieldNumber, DefaultSettings.MaxFieldNumber);
            var fields = context.RandomFields().Take(numberOfFields);
            var result = new TSLCell(name, fields);
            context.Cells.Add(result);
            // also treat cells as structs
            context.Structs.Add(result);
            if (!result.DynamicLengthed)
                context.FixedLengthStructs.Add(result);
            return result;
        }

        public static ITSLTopLevelElement GenerateServer(this TSLGeneratorContext context)
        {
            // TODO
            return null;
        }

        public static ITSLTopLevelElement GenerateProxy(this TSLGeneratorContext context)
        {
            // TODO
            return null;
        }

        public static ITSLTopLevelElement GenerateModule(this TSLGeneratorContext context)
        {
            // TODO
            return null;
        }

        public static ITSLTopLevelElement GenerateProtocol(this TSLGeneratorContext context)
        {
            // TODO
            return null;
        }

        public static ITSLTopLevelElement GenerateEnum(this TSLGeneratorContext context)
        {
            var name = $"Enum{context.TopLevelElementCount + 1}";
            var memberNumber = DiscreteUniform.Sample(context.MasterRandom,
                DefaultSettings.MinEnumMemberNumber, DefaultSettings.MaxEnumMemberNumber);
            var members = Enumerable.Range(0, memberNumber).Select(i => name + i);
            return new TSLEnum(name, members);
        }
        #endregion

        #region Helpers
        public static IEnumerable<TSLField> RandomFields(this TSLGeneratorContext context)
        {
            while (true)
            {
                var optional = ContinuousUniform.Sample(context.MasterRandom, 0.0, 1.0) <
                               DefaultSettings.FieldProbabilities.OptionalFieldProbability;
                var name = $"field{context.GeneratedElementCount}";
                var type = context.GenerateRandomType();
                context.GeneratedElementCount += 1;
                // TODO(leasunhy): generate attributes
                var field = new TSLField(type, name, optional, attributes: null);
                yield return field;
            }
        }
        #endregion
    }
}