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
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Proxy, GenerateProxy) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Server, GenerateServer) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Module, GenerateModule) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Protocol, GenerateProtocol) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Cell, GenerateCell) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Struct, GenerateStruct) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(DefaultSettings.TopLevelElementProbabilities.Enum, GenerateEnum)
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
            var name = $"Server_{context.GeneratedElementCount + 1}";
            var protocols = context.GetDefaultNumberOfDistinctProtocols();
            var server = new TSLServer(name, protocols);
            context.Servers.Add(server);
            return server;
        }

        public static ITSLTopLevelElement GenerateProxy(this TSLGeneratorContext context)
        {
            var name = $"Proxy_{context.GeneratedElementCount + 1}";
            var protocols = context.GetDefaultNumberOfDistinctProtocols();
            var proxy = new TSLProxy(name, protocols);
            context.Proxies.Add(proxy);
            return proxy;
        }

        public static ITSLTopLevelElement GenerateModule(this TSLGeneratorContext context)
        {
            var name = $"Module_{context.GeneratedElementCount + 1}";
            var protocols = context.GetDefaultNumberOfDistinctProtocols();
            var module = new TSLModule(name, protocols);
            context.Modules.Add(module);
            return module;
        }

        public static ITSLTopLevelElement GenerateProtocol(this TSLGeneratorContext context)
        {
            var protocol = context.GenerateRandomProtocol();
            context.Protocols.Add(protocol);
            return protocol;
        }

        public static ITSLTopLevelElement GenerateEnum(this TSLGeneratorContext context)
        {
            var name = $"Enum{context.TopLevelElementCount + 1}";
            var memberNumber = DiscreteUniform.Sample(context.MasterRandom,
                DefaultSettings.MinEnumMemberNumber, DefaultSettings.MaxEnumMemberNumber);
            var members = Enumerable.Range(0, memberNumber).Select(i => $"{name}_{i}");
            var result = new TSLEnum(name, members);
            context.Enums.Add(result);
            return result;
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

        public static IEnumerable<TSLProtocol> GetRandomDistinctProtocols(this TSLGeneratorContext context, int number)
        {
            var protocols = DiscreteUniform.Samples(context.MasterRandom, 0, context.Protocols.Count - 1)
                                           .Distinct()
                                           .Take(number)
                                           .Select(i => context.Protocols[i]);
            return protocols;
        }

        public static IEnumerable<TSLProtocol> GetDefaultNumberOfDistinctProtocols(this TSLGeneratorContext context)
        {
            var protocolNumber = DiscreteUniform.Sample(context.MasterRandom,
                                                 DefaultSettings.MinProtocolNumber, DefaultSettings.MaxProtocolNumber);
            return context.GetRandomDistinctProtocols(protocolNumber);
        }
        #endregion
    }
}