using System;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace TSLTestGenerator.DataModel
{
    public enum TSLFieldTypes
    {
        Atom,
        Struct,
        Enum,
        List,
        Array,
        NonField,
    }

    public interface ITSLType
    {
        string Name { get; }
        bool DynamicLengthed { get; }
        TSLFieldTypes FieldType { get; }
        string ClrTypeName { get; }
    }

    public class VoidType : ITSLType
    {
        public string Name => "void";
        public bool DynamicLengthed { get { throw new NotSupportedException(); } }
        public TSLFieldTypes FieldType => TSLFieldTypes.NonField;
        public string ClrTypeName => Name;
        public override string ToString() => Name;

        private VoidType() { }
        public static VoidType Instance { get; } = new VoidType();
    }

    public class StreamType : ITSLType
    {
        public string Name => "stream";
        public bool DynamicLengthed { get { throw new NotSupportedException(); } }
        public TSLFieldTypes FieldType => TSLFieldTypes.NonField;
        public string ClrTypeName => Name;
        public override string ToString() => Name;

        private StreamType() { }
        public static StreamType Instance { get; } = new StreamType();
    }

    public class ArrayType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => false;
        public TSLFieldTypes FieldType => TSLFieldTypes.Array;
        public string ClrTypeName { get; }
        public override string ToString() => Name;

        public ITSLType ElementType { get; }
        public ImmutableArray<int> Dimensions { get; }

        public ArrayType(ITSLType elementType, int[] dimensions)
        {
            ElementType = elementType;
            Dimensions = dimensions.ToImmutableArray();

            if (elementType.DynamicLengthed)
                throw new ArgumentOutOfRangeException(nameof(elementType), "Element type can't be dynamic lengthed!");
            Name = $"{elementType.Name}[{string.Join(", ", dimensions)}]";
            ClrTypeName = $"{elementType.Name}[{string.Join(",", dimensions.Select(d => ""))}]";
        }
    }

    public class ListType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => true;
        public TSLFieldTypes FieldType => TSLFieldTypes.List;
        public string ClrTypeName => Name;
        public override string ToString() => Name;

        public ITSLType ElementType { get; }

        public ListType(ITSLType elementType)
        {
            ElementType = elementType;
            Name = $"List<{elementType.Name}>";
        }
    }

    public class AtomType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }
        public TSLFieldTypes FieldType => TSLFieldTypes.Atom;
        public string ClrTypeName => Name;
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