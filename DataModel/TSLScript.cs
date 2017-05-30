using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLScript
    {
        public int RandomSeedForGeneration { get; set; }

        public TSLScript(IEnumerable<ITSLTopLevelElement> topLevelElements)
        {
            TopLevelElements = topLevelElements.ToImmutableArray();
        }

        public ImmutableArray<ITSLTopLevelElement> TopLevelElements { get; }

        public override string ToString() => string.Join("\n\n", TopLevelElements);
    }
}