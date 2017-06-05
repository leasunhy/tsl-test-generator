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
                for (int i = 0; i < GeneralSettings.NonNullRetries; ++i)
                {
                    ret = generator.DefaultGenerate(context);
                    if (ret != null) return ret;
                }
                throw new Exception("Too many retries before getting a non null type!");
            };
        }
        #endregion

        #region Total Generator
        public static readonly TSLGeneratorCombinator<ITSLType> TypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Enum, GenerateEnumType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Atom, GenerateAtomType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.ContainerArray, GenerateArrayType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.ContainerList, GenerateListType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Struct, GenerateStructType);

        public static readonly TSLGeneratorCombinator<ITSLType> ListElementTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementEnum, GenerateEnumType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementAtom, GenerateAtomType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementArray, GenerateArrayType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementList, GenerateListType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.List.ElementStruct, GenerateStructType);

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomTypeDelegate = GenerateNonNullType(TypeGenerator);
        public static ITSLType GenerateRandomFieldType(this TSLGeneratorContext context) => GenerateRandomTypeDelegate(context);

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateListElementTypeDelegate = GenerateNonNullType(ListElementTypeGenerator);
        public static ITSLType GenerateListElementType(this TSLGeneratorContext context) => GenerateListElementTypeDelegate(context);
        #endregion

        #region Generators
        public static ITSLType GenerateAtomType(this TSLGeneratorContext context)
        {
            // upper bound for DiscreteUniform.Sample() is inclusive
            return context.Enums.Choice(context.MasterRandom);
        }

        public static ITSLType GenerateEnumType(this TSLGeneratorContext context)
        {
            if (context.Enums.Count == 0)
                return null;
            return context.Enums.Choice(context.MasterRandom);
        }

        public static ITSLType GenerateListType(this TSLGeneratorContext context)
        {
            var elementType = context.GenerateListElementType();
            return new ListType(elementType);
        }

        public static ITSLType GenerateArrayType(this TSLGeneratorContext context)
        {
            var elementType = context.GenerateRandomArrayElementType();
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
            if (context.StructsBeforeMaxDepth.Count == 0)
                return null;
            return context.StructsBeforeMaxDepth.Choice(context.MasterRandom);
        }
        #endregion
    }

    public static class FixedLengthTypeGenerators
    {
        #region Total Generator
        public static readonly TSLGeneratorCombinator<ITSLType> FixedLengthTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Enum, GenerateFixedLengthEnumType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Atom, GenerateFixedLengthAtomType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.ContainerArray, GenerateFixedLengthArrayType) |
            new TSLGeneratorCombinator<ITSLType>(StructSettings.FieldProbabilities.Struct, GenerateFixedLengthStructType);

        public static readonly TSLGeneratorCombinator<ITSLType> ArrayElementTypeGenerator =
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementEnum, GenerateFixedLengthEnumType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementAtom, GenerateFixedLengthAtomType) |
            new TSLGeneratorCombinator<ITSLType>(ContainerProbabilities.Array.ElementStruct, GenerateFixedLengthStructType);

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomFixedLengthTypeDelegate =
            TypeGenerators.GenerateNonNullType(FixedLengthTypeGenerator);

        public static ITSLType GenerateRandomFixedLengthType(this TSLGeneratorContext context)
            => GenerateRandomFixedLengthTypeDelegate(context);

        public static readonly Func<TSLGeneratorContext, ITSLType> GenerateRandomArrayElementTypeDelegate =
            TypeGenerators.GenerateNonNullType(ArrayElementTypeGenerator);
        public static ITSLType GenerateRandomArrayElementType(this TSLGeneratorContext context)
            => GenerateRandomArrayElementTypeDelegate(context);

        #endregion

        #region Generators
        public static ITSLType GenerateFixedLengthAtomType(this TSLGeneratorContext context)
        {
            // upper bound for DiscreteUniform.Sample() is inclusive
            return AtomType.FixedLengthAtomTypes.Choice(context.MasterRandom);
        }

        public static ITSLType GenerateFixedLengthEnumType(this TSLGeneratorContext context)
            => context.GenerateEnumType();

        public static ITSLType GenerateFixedLengthArrayType(this TSLGeneratorContext context)
            => context.GenerateArrayType();

        public static ITSLType GenerateFixedLengthStructType(this TSLGeneratorContext context)
        {
            if (context.FixedLengthStructsBeforeMaxDepth.Count == 0)
                return null;
            return context.FixedLengthStructsBeforeMaxDepth.Choice(context.MasterRandom);
        }
        #endregion
    }
}