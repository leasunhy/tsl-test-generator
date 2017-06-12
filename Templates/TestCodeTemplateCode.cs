using System;
using System.Collections.Immutable;
using System.Linq;
using TSLTestGenerator.DataModel;

namespace TSLTestGenerator.Templates
{
    public partial class TestCodeTemplate
    {
        public TSLScript Script { get; }
        public TestCodeGeneratorContext Context { get; }
        protected ImmutableHashSet<TSLStruct> StructsUsedInProtocols { get; }

        public TestCodeTemplate(TSLScript script, Random masterRandom)
        {
            Script = script;
            Context = new TestCodeGeneratorContext(masterRandom);
            StructsUsedInProtocols =
                script.Protocols
                    .SelectMany(p => new[] {p.RequestType, p.ResponseType})
                    .OfType<TSLStruct>()
                    .ToImmutableHashSet();
        }
    }

    public class TestCodeGeneratorContext
    {
        public TestCodeGeneratorContext(Random masterRandom)
        {
            MasterRandom = masterRandom;
        }

        public Random MasterRandom { get; }
        public long GeneratedCount { get; set; }
    }
}