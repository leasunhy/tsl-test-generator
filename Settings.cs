using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TSLTestGenerator
{
    public static class DefaultSettings
    {

        public const int MinFieldNumber = 0;
        public const int MaxFieldNumber = 100;

        public const int MinTopLevelElementNumber = 20;
        public const int MaxTopLevelElementNumber = 100;

        public static class TopLevelElementProbabilities
        {
            static TopLevelElementProbabilities()
            {
                Debug.Assert(Math.Abs(Struct + Cell + Protocol +
                                      Server + Proxy + Module +
                                      Enum - 1.00) < 1e-6);
            }

            // element probabilities; must sum to 1.00
            public const double Struct = 0.20;
            public const double Cell = 0.10;
            public const double Protocol = 0.20;
            public const double Server = 0.20;
            public const double Proxy = 0.05;
            public const double Enum = 0.05;
            public const double Module = 0.20;
        }

        public static class FieldProbabilities
        {
            public const double OptionalFieldProbability = 0.30;
        }

        public static class TypeProbabilities
        {
            static TypeProbabilities()
            {
                Debug.Assert(Math.Abs(Atom + Struct + Enum + ContainerList + ContainerArray - 1.0) < 1e-6);
            }

            // type probabilities; must sum to 1.00
            public const double Atom = 0.30;
            public const double Struct = 0.30;
            public const double Enum = 0.10;
            public const double ContainerList = 0.15;
            public const double ContainerArray = 0.15;
        }

        public static class ContainerProbabilities
        {
            static ContainerProbabilities()
            {
                Debug.Assert(Math.Abs(ElementAtom + ElementStruct + ElementEnum - 1.0) < 1e-6);
            }

            public const double ElementAtom = 0.45;
            public const double ElementStruct = 0.45;
            public const double ElementEnum = 0.10;

            // to test against lists/arrays of diverse lengths, we use uniform distribution instead of normal distributions
            public const int ListMinDepth = 1;
            public const int ListMaxDepth = 10;

            public const int ArrayMinDim = 1;
            public const int ArrayMaxDim = 10;
        }
    }
}
