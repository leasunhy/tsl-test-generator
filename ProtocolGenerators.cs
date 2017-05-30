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
            => ProtocolGenerator.DefaultGenerate(context);

        public static TSLGeneratorCombinator<ITSLType> GetProtocolReqRspGenerator(ReqRspProbabilitySpecifier probabilities)
        {
            return new TSLGeneratorCombinator<ITSLType>(probabilities.Stream, _ => StreamType.Instance) |
                   new TSLGeneratorCombinator<ITSLType>(probabilities.Void, _ => VoidType.Instance) |
                   new TSLGeneratorCombinator<ITSLType>(probabilities.Struct, TypeGenerators.GenerateStructType);
        }

        private static readonly Func<TSLGeneratorContext, ITSLType> SynRequestTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Syn.Request()).DefaultGenerate;
        private static readonly Func<TSLGeneratorContext, ITSLType> SynResponseTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Syn.Response()).DefaultGenerate;

        private static readonly Func<TSLGeneratorContext, ITSLType> AsynRequestTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Asyn.Request()).DefaultGenerate;
        private static readonly Func<TSLGeneratorContext, ITSLType> AsynResponseTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Asyn.Response()).DefaultGenerate;

        private static readonly Func<TSLGeneratorContext, ITSLType> HttpRequestTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Http.Request()).DefaultGenerate;
        private static readonly Func<TSLGeneratorContext, ITSLType> HttpResponseTypeGenerator =
            GetProtocolReqRspGenerator(new ProtocolProbabilities.Http.Response()).DefaultGenerate;

        public static TSLProtocol GenerateSynProtocol(this TSLGeneratorContext context) =>
            new TSLProtocol(name: $"SynProtocol{context.TopLevelElementCount + 1}",
                            type: TSLProtocolType.Syn,
                            requestType: SynRequestTypeGenerator(context),
                            responseType: SynResponseTypeGenerator(context));

        public static TSLProtocol GenerateAsynProtocol(this TSLGeneratorContext context) =>
            new TSLProtocol(name: $"AsynProtocol{context.TopLevelElementCount + 1}",
                            type: TSLProtocolType.Asyn,
                            requestType: AsynRequestTypeGenerator(context),
                            responseType: AsynResponseTypeGenerator(context));

        public static TSLProtocol GenerateHttpProtocol(this TSLGeneratorContext context) =>
            new TSLProtocol(name: $"HttpProtocol{context.TopLevelElementCount + 1}",
                            type: TSLProtocolType.Http,
                            requestType: HttpRequestTypeGenerator(context),
                            responseType: HttpResponseTypeGenerator(context));
    }
}
