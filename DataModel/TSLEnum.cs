using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLEnum : ITSLType, ITSLTopLevelElement
    {
        public string Name { get; }
        public bool DynamicLengthed => false;
        public TSLFieldTypes FieldType => TSLFieldTypes.Enum;
        public ImmutableArray<string> Members { get; }
        // TODO(leasunhy): implement `starting value`

        public TSLEnum(string name, IEnumerable<string> values = null)
        {
            Name = name;
            Members = (values ?? Enumerable.Range(0, 2).Select(i => $"{name}_{i}")).ToImmutableArray();
        }

        public override string ToString() =>
            $"enum {Name}\n{{\n  {string.Join(",\n  ", Members)}\n}}";
    }
}
