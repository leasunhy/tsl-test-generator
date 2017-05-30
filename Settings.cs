using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public const int MinEnumMemberNumber = 5;
        public const int MaxEnumMemberNumber = 20;

        public const int MinProtocolNumber = 1;
        public const int MaxProtocolNumber = 100;

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

            public static class Array
            {
                static Array()
                {
                    Debug.Assert(Math.Abs(ElementAtom + ElementStruct + ElementEnum - 1.0) < 1e-6);
                }
                public const double ElementAtom = 0.45;
                public const double ElementStruct = 0.45;
                public const double ElementEnum = 0.10;

                public const int MinDimension = 1;
                public const int MaxDimension = 10;

                public const int MinDimensionLength = 1;
                public const int MaxDimensionLength = 100;
            }

            public static class List
            {
                static List()
                {
                    Debug.Assert(Math.Abs(ElementAtom + ElementStruct + ElementEnum + ElementArray + ElementList - 1.0) < 1e-6);
                }
                public const double ElementAtom = 0.30;
                public const double ElementStruct = 0.30;
                public const double ElementEnum = 0.10;
                public const double ElementArray = 0.15;
                public const double ElementList = 0.15;

                //public const int MinDepth = 1;
                //public const int MaxDepth = 10;
            }
        }

        public static class ProtocolProbabilities
        {
            public static class Type
            {
                public const double SynProtocol = 0.40;
                public const double AsynProtocol = 0.40;
                public const double HttpProtocol = 0.40;
            }

            public static class Syn
            {
                public class Request : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.30;
                    public override double Struct { get; } = 0.50;
                    public override double Void { get; } = 0.20;
                    public override double Stream { get; } = 0.00;
                    public Request() { Check(); }
                }

                public class Response : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.30;
                    public override double Struct { get; } = 0.50;
                    public override double Void { get; } = 0.20;
                    public override double Stream { get; } = 0.00;
                    public Response() { Check(); }
                }
            }

            public static class Asyn
            {
                public class Request : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.30;
                    public override double Struct { get; } = 0.50;
                    public override double Void { get; } = 0.20;
                    public override double Stream { get; } = 0.00;
                    public Request() { Check(); }
                }

                public class Response : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.00;
                    public override double Struct { get; } = 0.00;
                    public override double Void { get; } = 1.00;
                    public override double Stream { get; } = 0.00;
                    public Response() { Check(); }
                }
            }

            public static class Http
            {
                public class Request : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.20;
                    public override double Struct { get; } = 0.30;
                    public override double Void { get; } = 0.20;
                    public override double Stream { get; } = 0.30;
                    public Request() { Check(); }
                }

                public class Response : ReqRspProbabilitySpecifier
                {
                    public override double Atom { get; } = 0.20;
                    public override double Struct { get; } = 0.30;
                    public override double Void { get; } = 0.20;
                    public override double Stream { get; } = 0.30;
                    public Response() { Check(); }
                }
            }
        }
    }

    public abstract class ReqRspProbabilitySpecifier
    {
        public abstract double Atom { get; }
        public abstract double Struct { get; }
        public abstract double Void { get; }
        public abstract double Stream { get; }

        protected void Check() => Debug.Assert(Math.Abs(Atom + Struct + Void + Stream - 1.0) < 1e-6);
    }
}
