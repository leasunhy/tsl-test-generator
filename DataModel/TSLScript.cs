using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLScript
    {
        public int RandomSeedForGeneration { get; set; }
        public Random MasterRandom { get; set; }

        public TSLScript(IEnumerable<ITSLTopLevelElement> topLevelElements)
        {
            TopLevelElements = topLevelElements.ToImmutableArray();

            Cells = TopLevelElements.OfType<TSLCell>().ToImmutableArray();
            Structs = TopLevelElements.Where(e => e.GetType() == typeof(TSLStruct)).Cast<TSLStruct>().ToImmutableArray();
            Enums = TopLevelElements.OfType<TSLEnum>().ToImmutableArray();
            Protocols = TopLevelElements.OfType<TSLProtocol>().ToImmutableArray();
            Servers = TopLevelElements.OfType<TSLServer>().ToImmutableArray();
            Proxies = TopLevelElements.OfType<TSLProxy>().ToImmutableArray();
            Modules = TopLevelElements.OfType<TSLModule>().ToImmutableArray();
        }

        public TSLScript(TSLGeneratorContext context)
        {
            TopLevelElements = context.GetAllTopLevelElements().ToImmutableArray();

            Cells = context.Cells.ToImmutableArray();
            Structs = context.Structs.ToImmutableArray();
            Enums = context.Enums.ToImmutableArray();
            Protocols = context.Protocols.ToImmutableArray();
            Servers = context.Servers.ToImmutableArray();
            Proxies = context.Proxies.ToImmutableArray();
            Modules = context.Modules.ToImmutableArray();
        }

        public ImmutableArray<ITSLTopLevelElement> TopLevelElements { get; }
        public ImmutableArray<TSLCell> Cells { get; }
        public ImmutableArray<TSLStruct> Structs { get; }
        public ImmutableArray<TSLEnum> Enums { get; }
        public ImmutableArray<TSLProtocol> Protocols { get; }
        public ImmutableArray<TSLServer> Servers { get; }
        public ImmutableArray<TSLProxy> Proxies { get; }
        public ImmutableArray<TSLModule> Modules { get; }

        public override string ToString()
        {
            var premble = $"// Generated with Random seed = {RandomSeedForGeneration}\n\n";
            var body = string.Join("\n\n", TopLevelElements);
            return premble + body;
        }
    }
}