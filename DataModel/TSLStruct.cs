using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TSLTestGenerator.DataModel
{
    public interface ITSLType
    {
        string Name { get; }
        bool DynamicLengthed { get; }
    }

    public class ArrayType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => false;

        public ArrayType(ITSLType elementType, int[] dimensions)
        {
            if (elementType.DynamicLengthed)
                throw new ArgumentOutOfRangeException(nameof(elementType), "Element type can't be dynamic lengthed!");
            Name = $"{elementType}[{string.Join(", ", dimensions)}]";
        }
    }

    public class ListType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => true;

        public ListType(ITSLType elementType)
        {
            Name = $"List<{elementType}>";
        }
    }

    public class AtomType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }

        public AtomType(string name, bool dynamicLengthed)
        {
            this.Name = name;
            DynamicLengthed = dynamicLengthed;
        }

        public override string ToString() => Name;

        public static ImmutableArray<AtomType> AtomTypes = new ImmutableArray<AtomType>
        {
            new AtomType("byte", false),
            new AtomType("sbyte", false),
            new AtomType("bool", false),
            new AtomType("char", false),
            new AtomType("short", false),
            new AtomType("ushort", false),
            new AtomType("int", false),
            new AtomType("uint", false),
            new AtomType("long", false),
            new AtomType("ulong", false),
            new AtomType("float", false),
            new AtomType("double", false),
            new AtomType("decimal", false),
            new AtomType("DateTime", false),
            new AtomType("Guid", false),
            new AtomType("string", true),
            new AtomType("u8string", true),
        };
    }

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
        public TSLField(ITSLType type, string name, IEnumerable<TSLAttribute> attributes, bool optional)
        {
            Type = type;
            Name = name;
            Optional = optional;
            Attributes = attributes.ToImmutableArray();
        }

        public bool Optional { get; }
        public ITSLType Type { get; }
        public string Name { get; }
        public ImmutableArray<TSLAttribute> Attributes { get; private set; }

        public override string ToString()
        {
            string attributes = Attributes.Any() ? $"[{string.Join(",", Attributes)}]\n" : "";
            string optional = Optional ? "optional" : "";
            return $"{Attributes}{optional} {Type} {Name};";
        }
    }

    public class TSLStruct : ITSLType, ITSLTopLevelElement
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }
        public ImmutableArray<TSLAttribute> Attributes { get; }
        public ImmutableArray<TSLField> Fields { get; }

        public TSLStruct(string name, IEnumerable<TSLField> fields, IEnumerable<TSLAttribute> attributes = null)
        {
            Name = name;
            Fields = fields.ToImmutableArray();
            DynamicLengthed = Fields.Any(f => f.Type.DynamicLengthed);
            Attributes = (attributes?.ToImmutableArray()).GetValueOrDefault();
        }

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
            string attributes = Attributes == null ? "" : $"[{string.Join(", ", Attributes)}]";
            string fields = string.Join("\n", Fields);
            return $"{attributes}\ncell struct {Name}\n{{\n{fields}}}";
        }
    }
}
