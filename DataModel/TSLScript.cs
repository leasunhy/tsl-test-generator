using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLScript
    {
        public TSLScript(IEnumerable<ITSLTopLevelElement> topLevelElements)
        {
            TopLevelElements = topLevelElements.ToImmutableArray();
        }

        public ImmutableArray<ITSLTopLevelElement> TopLevelElements { get; private set; }

        public override string ToString() => string.Join("\n", TopLevelElements);
    }
}