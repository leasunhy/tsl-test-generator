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
    }
}
