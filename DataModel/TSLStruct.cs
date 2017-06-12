using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using static TSLTestGenerator.DefaultSettings;

namespace TSLTestGenerator.DataModel
{
    public class TSLAttribute
    {
        public string AttributeName { get; }

        private TSLAttribute(string attributeName)
        {
            this.AttributeName = attributeName;
        }

        private static readonly Dictionary<string, TSLAttribute> InitializedAttributes = new Dictionary<string, TSLAttribute>();
        public static TSLAttribute GetAttribute(string attributeName)
        {
            if (InitializedAttributes.TryGetValue(attributeName, out TSLAttribute result))
                return result;
            return InitializedAttributes[attributeName] = new TSLAttribute(attributeName);
        }

        public override string ToString() => AttributeName;
    }

    public class TSLField
    {
        public bool Optional { get; }
        public ITSLType Type { get; }
        public string Name { get; }
        public ImmutableArray<TSLAttribute> Attributes { get; }
        public bool DynamicLengthed => Optional || Type.DynamicLengthed;

        public TSLField(ITSLType type, string name, bool optional, IEnumerable<TSLAttribute> attributes = null)
        {
            Type = type;
            Name = name;
            Optional = optional;
            Attributes = (attributes?.ToImmutableArray()).GetValueOrDefault();
        }

        public override string ToString()
        {
            string attributes = Attributes != null && !Attributes.IsEmpty ? $"[{string.Join(",", Attributes)}]\n" : "";
            string optional = Optional ? "optional " : "";
            return $"  {attributes}{optional}{Type.Name} {Name};";
        }
    }

    public class TSLStruct : ITSLType, ITSLTopLevelElement
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }
        public TSLFieldTypes FieldType => TSLFieldTypes.Struct;
        public Func<Random, string> GetRandomValue { get; }
        public string ClrTypeName => Name;
        public ImmutableArray<TSLAttribute> Attributes { get; }
        public ImmutableArray<TSLField> Fields { get; }
        public int Depth { get; }

        public TSLStruct(string name, IEnumerable<TSLField> fields, IEnumerable<TSLAttribute> attributes = null)
        {
            Name = name;
            Fields = fields.ToImmutableArray();
            DynamicLengthed = Fields.Any(f => f.DynamicLengthed);
            Attributes = (attributes?.ToImmutableArray()).GetValueOrDefault();
            Depth = Fields.Select(f => f.Type)
                          .Where(t => t.FieldType == TSLFieldTypes.Struct)
                          .Cast<TSLStruct>()
                          .Aggregate(0, (max, s) => Math.Max(max, s.Depth)) + 1;
            GetRandomValue = RandomStruct;
            Debug.Assert(Depth <= StructSettings.MaxDepth);
        }

        private string RandomStruct(Random random)
        {
            var fieldInititializers =
                Fields.Select(f => $"{f.Name} = {f.Type.GetRandomValue(random)}");
            var ret = $"new {Name} {string.Join(", ", fieldInititializers)}";
            return ret;
        }

        public override string ToString()
        {
            string attributes = Attributes == null || Attributes.IsEmpty ? "" : $"[{string.Join(", ", Attributes)}]\n";
            string fields = string.Join("\n", Fields);
            return $"{attributes}struct {Name}\n{{\n{fields}\n}}";
        }
    }

    public class TSLCell : ITSLTopLevelElement
    {
        public string Name { get; }
        public ImmutableArray<TSLAttribute> Attributes { get; }
        public ImmutableArray<TSLField> Fields { get; }

        public TSLCell(string name, IEnumerable<TSLField> fields, IEnumerable<TSLAttribute> attributes = null)
        {
            Name = name;
            Fields = fields.ToImmutableArray();
            Attributes = (attributes?.ToImmutableArray()).GetValueOrDefault();
        }

        public override string ToString()
        {
            string attributes = Attributes == null || Attributes.IsEmpty ? "" : $"[{string.Join(", ", Attributes)}]\n";
            string fields = string.Join("\n", Fields);
            return $"{attributes}cell {Name}\n{{\n{fields}\n}}";
        }
    }
}
