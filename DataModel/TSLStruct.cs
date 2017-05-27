using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public class TSLType
    {
        public string Name { get; private set; }
        public bool DynamicLengthed { get; private set; }
        public TSLType(string name, bool dynamicLengthed)
        {
            this.Name = name;
            DynamicLengthed = dynamicLengthed;
        }

        public override string ToString() => Name;

        public static ImmutableArray<TSLType> AtomTypes = new ImmutableArray<TSLType>
        {
            new TSLType("byte", false),
            new TSLType("sbyte", false),
            new TSLType("bool", false),
            new TSLType("char", false),
            new TSLType("short", false),
            new TSLType("ushort", false),
            new TSLType("int", false),
            new TSLType("uint", false),
            new TSLType("long", false),
            new TSLType("ulong", false),
            new TSLType("float", false),
            new TSLType("double", false),
            new TSLType("decimal", false),
            new TSLType("DateTime", false),
            new TSLType("Guid", false),
            new TSLType("string", true),
            new TSLType("u8string", true),
        };
    }

    public class TSLArrayType : TSLType
    {
        public TSLArrayType(TSLType elementType, int[] dimensions)
            : base($"{elementType}[{string.Join(", ", dimensions)}]", false)
        {
            if (elementType.DynamicLengthed)
                throw new ArgumentOutOfRangeException(nameof(elementType), "Element type can't be dynamic lengthed!");
        }
    }

    public class TSLListType : TSLType
    {
        public TSLListType(TSLType elementType) : base($"List<{elementType}>", true)
        {
        }
    }

    public class TSLAttribute
    {
        public string AttributeName { get; private set; }

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
        public TSLField(TSLType type, string name, IEnumerable<TSLAttribute> attributes, bool optional)
        {
            Type = type;
            Name = name;
            Optional = optional;
            Attributes = attributes.ToImmutableArray();
        }

        public bool Optional { get; private set; }
        public TSLType Type { get; private set; }
        public string Name { get; private set; }
        public ImmutableArray<TSLAttribute> Attributes { get; private set; }

        public override string ToString()
        {
            string attributes = Attributes.Any() ? $"[{string.Join(",", Attributes)}]\n" : "";
            string optional = Optional ? "optional" : "";
            return $"{Attributes}{optional} {Type} {Name};";
        }
    }

    public class TSLStruct : ITSLTopLevelElement
    {
        public TSLStruct(string name, IEnumerable<TSLField> fields, IEnumerable<TSLAttribute> attributes = null)
        {
            Name = name;
            Fields = fields.ToImmutableArray();
            Attributes = (attributes?.ToImmutableArray()).GetValueOrDefault();
        }

        public string Name { get; }
        public ImmutableArray<TSLAttribute> Attributes { get; }
        public ImmutableArray<TSLField> Fields { get; }

        public override string ToString()
        {
            string attributes = Attributes == null ? "" : $"[{string.Join(", ", Attributes)}]";
            string fields = string.Join("\n", Fields);
            return $"{attributes}\nstruct {Name}\n{{\n{fields}}}";
        }
    }

    public class TSLCell : TSLStruct, ITSLTopLevelElement
    {
        public TSLCell(string name, IEnumerable<TSLField> fields, IEnumerable<TSLAttribute> attributes = null)
            : base(name, fields, attributes)
        {
        }

        public override string ToString()
        {
            string attributes = $"[{string.Join(", ", Attributes)}]";
            string fields = string.Join("\n", Fields);
            return $"{attributes}\ncell struct {Name}\n{{\n{fields}}}";
        }
    }
}
