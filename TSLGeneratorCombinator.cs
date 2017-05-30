using MathNet.Numerics.Distributions;

namespace TSLTestGenerator
{
    public class TSLGeneratorCombinator<R>
    {
        public delegate R GenerateDelegate(TSLGeneratorContext context);
        public double Probability { get; }
        public double CumulativeProbability { get; }
        public GenerateDelegate GenerateFunc { get; }
        public TSLGeneratorCombinator<R> NextCombinator { get; }

        public TSLGeneratorCombinator(double probability, GenerateDelegate generateFunc, TSLGeneratorCombinator<R> next = null)
        {
            Probability = probability;
            CumulativeProbability = probability + (next?.CumulativeProbability ?? 0.0);
            GenerateFunc = generateFunc;
            NextCombinator = next;
        }

        public R Generate(double typeSelector, TSLGeneratorContext context)
        {
            if (typeSelector - Probability > 0)
                return NextCombinator != null ? NextCombinator.Generate(typeSelector - Probability, context) : default(R);
            else
                return GenerateFunc(context);
        }

        public R DefaultGenerate(TSLGeneratorContext context)
        {
            var typeSelector = ContinuousUniform.Sample(context.MasterRandom, 0.0, CumulativeProbability);
            return Generate(typeSelector, context);
        }

        public TSLGeneratorCombinator<R> WithNext(TSLGeneratorCombinator<R> next) => new TSLGeneratorCombinator<R>(Probability, GenerateFunc, next);

        //public TSLGeneratorCombinator WithLast(TSLGeneratorCombinator last) => WithNext(NextCombinator?.WithLast(last) ?? last);

        //// if only `|` is right-associative!
        //public static TSLGeneratorCombinator operator |(TSLGeneratorCombinator lhs, TSLGeneratorCombinator rhs) => lhs.WithLast(rhs);

        public static TSLGeneratorCombinator<R> operator |(TSLGeneratorCombinator<R> lhs, TSLGeneratorCombinator<R> rhs) => rhs.WithNext(lhs);
    }
}