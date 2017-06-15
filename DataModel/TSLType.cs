using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

using static TSLTestGenerator.DefaultSettings;

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
        Func<Random, string> GetRandomValue { get; }
    }

    public class VoidType : ITSLType
    {
        public string Name => "void";
        public bool DynamicLengthed { get { throw new NotSupportedException(); } }
        public TSLFieldTypes FieldType => TSLFieldTypes.NonField;
        public string ClrTypeName => Name;
        public Func<Random, string> GetRandomValue { get { throw new NotImplementedException(); } }
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
        public Func<Random, string> GetRandomValue { get { throw new NotImplementedException(); } }
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
        public Func<Random, string> GetRandomValue { get; }
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
            GetRandomValue = random => GetRandomArrayValue(random);
        }

        private string GetRandomArrayValue(Random random, int currentDim = 0)
        {
            var d = Dimensions[currentDim];
            IEnumerable<string> elements;
            if (currentDim == Dimensions.Length - 1)
            {
                elements = Enumerable.Range(0, d)
                                     .Select(_ => ElementType.GetRandomValue(random));
            }
            else
            {
                elements = Enumerable.Range(0, d)
                                     .Select(_ => GetRandomArrayValue(random, currentDim + 1));
            }
            return $"{{ {string.Join(", ", elements)} }}";
        }
    }

    public class ListType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed => true;
        public TSLFieldTypes FieldType => TSLFieldTypes.List;
        public string ClrTypeName { get; }
        public Func<Random, string> GetRandomValue { get; }
        public override string ToString() => Name;

        public ITSLType ElementType { get; }

        public ListType(ITSLType elementType)
        {
            ElementType = elementType;
            Name = $"List<{elementType.Name}>";
            ClrTypeName = $"List<{elementType.ClrTypeName}>";
            GetRandomValue = GetRandomListValue;
        }

        private string GetRandomListValue(Random random)
        {
            var count = DiscreteUniform.Sample(random,
                0, ContainerProbabilities.List.MaxRandomElementCount);
            var elems = Enumerable.Range(0, count).Select(_ => ElementType.GetRandomValue(random));
            return $"new {Name}{{ {string.Join(", ", elems)} }}";
        }
    }

    public class AtomType : ITSLType
    {
        public string Name { get; }
        public bool DynamicLengthed { get; }
        public TSLFieldTypes FieldType => TSLFieldTypes.Atom;
        public string ClrTypeName { get; }
        public Func<Random, string> GetRandomValue { get; }
        public override string ToString() => Name;

        private AtomType(string name, bool dynamicLengthed, Func<Random, string> randomValueProvider, string clrTypeName = null)
        {
            Name = name;
            DynamicLengthed = dynamicLengthed;
            GetRandomValue = randomValueProvider;
            ClrTypeName = clrTypeName ?? name;
        }

        public static readonly ImmutableArray<AtomType> FixedLengthAtomTypes = new[]
        {
            IntegralType("byte", byte.MinValue, byte.MaxValue),
            IntegralType("sbyte", sbyte.MinValue, sbyte.MaxValue),
            new AtomType("bool", false, random => DiscreteUniform.Sample(random, 0 ,1) == 0 ? "false" : "true"),
            IntegralType("char", char.MinValue, char.MaxValue),
            IntegralType("short", short.MinValue, short.MaxValue),
            IntegralType("ushort", ushort.MinValue, ushort.MaxValue),
            IntegralType("int", int.MinValue, int.MaxValue),
            new AtomType("uint", false, random => ((uint)RandomInt(random)).ToString()),
            new AtomType("long", false, random => RandomLong(random).ToString()),
            new AtomType("ulong", false, random => ((ulong)RandomLong(random)).ToString()), 
            new AtomType("float", false, random => RandomFloat(random).ToString(CultureInfo.InvariantCulture)),
            new AtomType("double", false, random => RandomDouble(random).ToString(CultureInfo.InvariantCulture)),
            new AtomType("decimal", false, random => RandomDecimal(random).ToString(CultureInfo.InvariantCulture)),
            new AtomType("DateTime", false, random => $"new DateTime({RandomLong(random)})"),
            new AtomType("Guid", false, random => $"new Guid(new byte[] {{ {string.Join(", ", RandomBuffer(random, 128))} }})"),
        }.ToImmutableArray();

        public static readonly ImmutableArray<AtomType> AtomTypes = FixedLengthAtomTypes.AddRange(new[]
        {
            new AtomType("string", true, random => $"\"{RandomString(random)}\""),
            new AtomType("u8string", true, random => $"\"{RandomString(random)}\"", "string"),
        }).ToImmutableArray();

        private static AtomType IntegralType(string name, int lower, int upper, string suffix = "")
        {
            Func<Random, string> provider =
                random => DiscreteUniform.Sample(random, lower, upper).ToString() + suffix;
            return new AtomType(name, false, provider);
        }

        private static int RandomInt(Random random) =>
            DiscreteUniform.Sample(random, int.MinValue, int.MaxValue);

        private static long RandomLong(Random random) =>
            (long)RandomInt(random) << 32 | (long)RandomInt(random);

        private static float RandomFloat(Random random) =>
            BitConverter.ToSingle(BitConverter.GetBytes(RandomInt(random)), 0);

        private static double RandomDouble(Random random) =>
            BitConverter.ToDouble(BitConverter.GetBytes(RandomLong(random)), 0);

        private static unsafe decimal RandomDecimal(Random random)
        {
            fixed (byte* p = RandomBuffer(random, sizeof(decimal)))
                return *(decimal*)p;
        }

        private static string RandomString(Random random)
        {
            // TODO(leasunhy): extend alphabet or generate random chars
            var builder = new StringBuilder();
            var length = DiscreteUniform.Sample(random, 0, RandomValueSettings.MaxStringLength);
            for (int i = 0; i < length; ++i)
                builder.Append(RandomValueSettings.StringAlphabet.Choice(random));
            return builder.ToString();
        }

        private static unsafe byte[] RandomBuffer(Random random, int bytes)
        {
            byte[] ret = new byte[bytes];
            for (int i = 0; i < bytes; ++i)
            {
                var randomLong = RandomLong(random);
                var pointer = (byte*) &randomLong;
                for (int j = 0; j < 8 && i + j < bytes; ++j)
                    ret[i + j] = *pointer++;
            }
            return ret;
        }
    }
}