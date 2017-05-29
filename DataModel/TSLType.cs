using System;
using System.Collections.Immutable;

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

        public static readonly ImmutableArray<AtomType> AtomTypes = new []
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
        }.ToImmutableArray();
    }
}