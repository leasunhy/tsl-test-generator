using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MathNet.Numerics.Distributions;
using TSLTestGenerator.DataModel;
using static TSLTestGenerator.DefaultSettings;

namespace TSLTestGenerator
{
    public static class TopLevelElementGenerators
    {
        #region Total Generator
        // NOTE(leasunhy): the `typeSelector` for this generator is the ratio of elements generated,
        //                 so enums will be generated first, and then struct, and then cell, etc.
        public static readonly Func<double, TSLGeneratorContext, ITSLTopLevelElement> StaticTopLevelElementGenerator =
        (
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Proxy, GenerateProxy) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Server, GenerateServer) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Module, GenerateModule) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Protocol, GenerateProtocol) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Cell, GenerateCell) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Struct, GenerateStruct) |
            new TSLGeneratorCombinator<ITSLTopLevelElement>(TopLevelElementProbabilities.Enum, GenerateEnum)
        ).Generate;
        #endregion

        #region Generators
        public static ITSLTopLevelElement GenerateStruct(this TSLGeneratorContext context)
        {
            var name = $"Struct_{context.TopLevelElementCount + 1}";
            var numberOfFields = DiscreteUniform.Sample(context.MasterRandom, StructSettings.MinFieldNumber, StructSettings.MaxFieldNumber);
            var fields = context.RandomFields().Take(numberOfFields);
            // TODO(leasunhy): generate attributes
            var result = new TSLStruct(name, fields);
            context.AddStruct(result);
            return result;
        }

        public static ITSLTopLevelElement GenerateCell(this TSLGeneratorContext context)
        {
            var name = $"CellStruct_{context.TopLevelElementCount + 1}";
            var numberOfFields = DiscreteUniform.Sample(context.MasterRandom, StructSettings.MinFieldNumber, StructSettings.MaxFieldNumber);
            var fields = context.RandomFields().Take(numberOfFields).ToArray();
            var result = new TSLCell(name, fields);
            context.Cells.Add(result);
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
                EnumSettings.MinEnumMemberNumber, EnumSettings.MaxEnumMemberNumber);
            var members = Enumerable.Range(0, memberNumber).Select(i => $"{name}_{i}").ToList();
            var values =
                members
                    .WithProbabilityThreshold(
                        EnumSettings.ValueSpecifiedProbability,
                        n => new KeyValuePair<string, int>(n, DiscreteUniform.Sample(context.MasterRandom, 0, byte.MaxValue)),
                        _ => ContinuousUniform.Sample(context.MasterRandom, 0.0, 1.0))
                    .Where(p => p.Key != null)
                    .ToImmutableDictionary();
            var result = new TSLEnum(name, members, values);
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
                               StructSettings.FieldProbabilities.OptionalFieldProbability;
                var name = $"field{context.GeneratedElementCount}";
                var type = context.GenerateRandomFieldType();
                context.GeneratedElementCount += 1;
                // TODO(leasunhy): generate attributes
                var field = new TSLField(type, name, optional, attributes: null);
                yield return field;
            }
        }

        public static IEnumerable<TSLProtocol> GetRandomDistinctProtocols(this TSLGeneratorContext context, int number)
        {
            if (context.Protocols.Count == 0) return Enumerable.Empty<TSLProtocol>();
            if (number > context.Protocols.Count) number = context.Protocols.Count;
            var protocols = DiscreteUniform.Samples(context.MasterRandom, 0, context.Protocols.Count - 1)
                                           .Take(number)
                                           .Distinct()  // may return fewer than number of protocols
                                           .Select(i => context.Protocols[i]);
            return protocols;
        }

        public static IEnumerable<TSLProtocol> GetDefaultNumberOfDistinctProtocols(this TSLGeneratorContext context)
        {
            var protocolNumber = DiscreteUniform.Sample(context.MasterRandom,
                                                 CommunicationInstanceSettings.MinProtocolNumber, CommunicationInstanceSettings.MaxProtocolNumber);
            return context.GetRandomDistinctProtocols(protocolNumber);
        }
        #endregion
    }
}