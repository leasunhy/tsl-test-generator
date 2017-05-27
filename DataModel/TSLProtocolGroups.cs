using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLCommunicationInstance
    {
        protected TSLCommunicationInstance(string name, IEnumerable<TSLProtocol> protocols)
        {
            Name = name;
            Protocols = protocols.ToImmutableArray();
        }

        public ImmutableArray<TSLProtocol> Protocols { get; private set; }
        public string Name { get; private set; }

        public override string ToString() => $"{Name}\n{{ {string.Join("\n", Protocols.Select(p => $"protocol {p};"))} }}";
    }

    public class TSLServer : TSLCommunicationInstance, ITSLTopLevelElement
    {
        public TSLServer(string name, IEnumerable<TSLProtocol> protocols) : base(name, protocols)
        {
        }

        public override string ToString() => $"server {base.ToString()}";
    }

    public class TSLProxy : TSLCommunicationInstance, ITSLTopLevelElement
    {
        public TSLProxy(string name, IEnumerable<TSLProtocol> protocols) : base(name, protocols)
        {
        }

        public override string ToString() => $"proxy {base.ToString()}";
    }

    public class TSLModule : TSLCommunicationInstance, ITSLTopLevelElement
    {
        public TSLModule(string name, IEnumerable<TSLProtocol> protocols) : base(name, protocols)
        {
        }

        public override string ToString() => $"module {base.ToString()}";
    }
}