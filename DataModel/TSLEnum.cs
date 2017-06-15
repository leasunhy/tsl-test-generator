using System;
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
        public string ClrTypeName => Name;
        public Func<Random, string> GetRandomValue { get; }
        public ImmutableArray<(string, int?)> Members { get; }
        // TODO(leasunhy): implement `starting value`

        public TSLEnum(string name, IEnumerable<string> members, IDictionary<string, int> values = null)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));
            Name = name;
            Members =
                members
                    .Select(m => (m, values?.GetValueOrNull(m)))
                    .ToImmutableArray();
            GetRandomValue = random => Members.Choice(random).Item1;
        }

        public override string ToString()
        {
            var members = Members.Select((key, value) => $"{key}{FormatValue(value)}");
            return $"enum {Name}\n{{\n  {string.Join(",\n  ", members)}\n}}";
        }

        private static string FormatValue(int? value) =>
            value == null ? "" : $" = {value}";
    }
}
