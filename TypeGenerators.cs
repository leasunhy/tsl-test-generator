using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using TSLTestGenerator.DataModel;
using static TSLTestGenerator.DefaultSettings;

namespace TSLTestGenerator
{
    public static class TypeGenerators
    {
        #region Helper
        public static Func<TSLGeneratorContext, ITSLType> GenerateNonNullType(TSLGeneratorCombinator<ITSLType> generator)
        {
            return context =>
            {
                ITSLType ret = null;
                while (ret == null)
                {
                    var typeSelector = ContinuousUniform.Sample(context.MasterRandom, 0.0,
                        TypeGenerator.CumulativeProbability);
                    ret = context.TypeGenerator(typeSelector);
                }
                return ret;
            };
        }
        #endregion

        #region Total Generator
        public static readonly TSLGeneratorCombinator<ITSLType> TypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Enum, GenerateEnumType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Atom, GenerateAtomType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.ContainerArray, GenerateArrayType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.ContainerList, GenerateListType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Struct, GenerateStructType);

        public static readonly TSLGeneratorCombinator<ITSLType> ListElementTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementEnum, GenerateEnumType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementAtom, GenerateAtomType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementArray, GenerateArrayType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementList, GenerateListType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementStruct, GenerateStructType);
        #endregion

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomType = GenerateNonNullType(TypeGenerator);
        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateListElementType = GenerateNonNullType(ListElementTypeGenerator);

        public static ITSLType GenerateAtomType(this TSLGeneratorContext context)
        {
            // upper bound for DiscreteUniform.Sample() is inclusive
            var selector = DiscreteUniform.Sample(context.MasterRandom, 0, AtomType.AtomTypes.Length - 1);
            return AtomType.AtomTypes[selector];
        }

        public static ITSLType GenerateEnumType(this TSLGeneratorContext context)
        {
            if (context.Enums.Count == 0)
                return null;
            var selector = DiscreteUniform.Sample(context.MasterRandom, 0, context.Enums.Count - 1);
            return context.Enums[selector];
        }

        public static ITSLType GenerateListType(this TSLGeneratorContext context)
        {
            var elementType = GenerateListElementType(context);
            return new ListType(elementType);
        }

        public static ITSLType GenerateArrayType(this TSLGeneratorContext context)
        {
            var elementType = FixedLengthTypeGenerators.GenerateRandomArrayElementType(context);
            var dimension = DiscreteUniform.Sample(
                context.MasterRandom,
                ContainerProbabilities.Array.MinDimension,
                ContainerProbabilities.Array.MaxDimension);
            var dimensions =
                DiscreteUniform.Samples(context.MasterRandom,
                                        ContainerProbabilities.Array.MinDimensionLength,
                                        ContainerProbabilities.Array.MaxDimensionLength)
                               .Take(dimension);
            return new ArrayType(elementType, dimensions.ToArray());
        }

        public static ITSLType GenerateStructType(this TSLGeneratorContext context)
        {
            if (context.Structs.Count == 0)
                return null;
            var selector = DiscreteUniform.Sample(context.MasterRandom, 0, context.Structs.Count - 1);
            return context.Structs[selector];
        }
    }

    public static class FixedLengthTypeGenerators
    {
        #region Total Generator

        public static readonly TSLGeneratorCombinator<ITSLType> FixedLengthTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Enum, GenerateFixedLengthEnumType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Atom, GenerateFixedLengthAtomType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.ContainerArray, GenerateFixedLengthArrayType) |
            new TSLGeneratorCombinator<ITSLType>(TypeProbabilities.Struct, GenerateFixedLengthStructType);

        public static readonly TSLGeneratorCombinator<ITSLType> ArrayElementTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementEnum, GenerateFixedLengthEnumType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementAtom, GenerateFixedLengthAtomType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementStruct, GenerateFixedLengthStructType);
        #endregion

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomFixedLengthType =
            TypeGenerators.GenerateNonNullType(FixedLengthTypeGenerator);

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomArrayElementType =
            TypeGenerators.GenerateNonNullType(ArrayElementTypeGenerator);

        public static ITSLType GenerateFixedLengthAtomType(this TSLGeneratorContext context)
        {
            // upper bound for DiscreteUniform.Sample() is inclusive
            var selector = DiscreteUniform.Sample(context.MasterRandom, 0, AtomType.AtomTypes.Length - 1);
            return AtomType.AtomTypes[selector];
        }

        public static ITSLType GenerateFixedLengthEnumType(this TSLGeneratorContext context)
            => context.GenerateEnumType();

        public static ITSLType GenerateFixedLengthArrayType(this TSLGeneratorContext context)
            => context.GenerateArrayType();

        public static ITSLType GenerateFixedLengthStructType(this TSLGeneratorContext context)
        {
            if (context.FixedLengthStructs.Count == 0)
                return null;
            var selector = DiscreteUniform.Sample(context.MasterRandom, 0, context.FixedLengthStructs.Count - 1);
            return context.FixedLengthStructs[selector];
        }
    }
}