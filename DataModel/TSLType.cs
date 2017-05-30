using System;
using System.Collections.Immutable;
using NUnit.Framework;

namespace TSLTestGenerator.DataModel
{
    public interface ITSLType
    {
        string Name { get; }
        bool DynamicLengthed { get; }
    }

    public class VoidType : ITSLType
    {
        public string Name => "void";
        public bool DynamicLengthed { get { throw new NotSupportedException(); } }
        public override string ToString() => Name;

        private VoidType() { }
        public static VoidType Instance { get; } = new VoidType();
    }

    public class StreamType : ITSLType
    {
        public string Name => "stream";
        public bool DynamicLengthed { get { throw new NotSupportedException(); } }
        public override string ToString() => Name;

        private StreamType() { }
        public static StreamType Instance { get; } = new StreamType();
    }

    public class ArrayType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => false;
        public override string ToString() => Name;

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
        public override string ToString() => Name;

        public ListType(ITSLType elementType)
        {
            Name = $"List<{elementType}>";
        }
    }

    public class AtomType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }
        public override string ToString() => Name;

        public AtomType(string name, bool dynamicLengthed)
        {
            this.Name = name;
            DynamicLengthed = dynamicLengthed;
        }

        public static readonly ImmutableArray<AtomType> FixedLengthAtomTypes = new[]
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
        }.ToImmutableArray();

        public static readonly ImmutableArray<AtomType> AtomTypes = new[]
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