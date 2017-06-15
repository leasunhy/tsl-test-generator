using System;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework.Internal.Filters;
using TSLTestGenerator.DataModel;

namespace TSLTestGenerator.Templates
{
    public class TestCodeGeneratorContext
    {
        public TestCodeGeneratorContext(string testName, TSLScript script, Random masterRandom)
        {
            TestName = testName;
            Script = script;
            MasterRandom = masterRandom;
            StructsUsedInProtocols =
                script.Protocols
                    .Where(p => p.Type != TSLProtocolType.Http)
                    .SelectMany(p => new[] {p.RequestType, p.ResponseType})
                    .OfType<TSLStruct>()
                    .ToImmutableHashSet();
        }

        public string TestName { get; }
        public TSLScript Script { get; }
        public Random MasterRandom { get; }
        public ImmutableHashSet<TSLStruct> StructsUsedInProtocols { get; }
        public long GeneratedCount { get; set; }
    }
}