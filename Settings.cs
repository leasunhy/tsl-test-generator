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
        static DefaultSettings()
        {
            Debug.Assert(Math.Abs(StructProbability + CellProbability + ProtocolProbability +
                                  ServerProbability + ProxyProbability + ModuleProbability +
                                  EnumProbability - 1.00) < 1e-6);
        }

        public const int MinFieldNumber = 0;
        public const int MaxFieldNumber = 100;

        public const int MinTopLevelElementNumber = 20;
        public const int MaxTopLevelElementNumber = 100;

        // element probabilities; must sum to 1.00
        public const double StructProbability = 0.20;
        public const double CellProbability = 0.10;
        public const double ProtocolProbability = 0.20;
        public const double ServerProbability = 0.20;
        public const double ProxyProbability = 0.05;
        public const double EnumProbability = 0.05;
        public const double ModuleProbability = 0.20;
    }
}
