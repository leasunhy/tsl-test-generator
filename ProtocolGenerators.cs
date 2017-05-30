using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using TSLTestGenerator.DataModel;

using static TSLTestGenerator.DefaultSettings;

namespace TSLTestGenerator
{
    public static class ProtocolGenerators
    {
        public static readonly TSLGeneratorCombinator<TSLProtocol> ProtocolGenerator =
            new TSLGeneratorCombinator<TSLProtocol>(ProtocolProbabilities.Type.SynProtocol, GenerateSynProtocol) |
            new TSLGeneratorCombinator<TSLProtocol>(ProtocolProbabilities.Type.AsynProtocol, GenerateAsynProtocol) |
            new TSLGeneratorCombinator<TSLProtocol>(ProtocolProbabilities.Type.HttpProtocol, GenerateHttpProtocol);

        public static TSLProtocol GenerateRandomProtocol(this TSLGeneratorContext context)
        {
            double typeSelector = ContinuousUniform.Sample(context.MasterRandom,
                0.0, ProtocolGenerator.CumulativeProbability);
            return ProtocolGenerator.Generate(typeSelector, context);
        }

        public static TSLProtocol GenerateSynProtocol(this TSLGeneratorContext context)
        {
            
        }

        public static TSLProtocol GenerateAsynProtocol(this TSLGeneratorContext context)
        {
            
        }

        public static TSLProtocol GenerateHttpProtocol(this TSLGeneratorContext context)
        {
            
        }
    }
}
