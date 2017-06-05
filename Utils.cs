using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace TSLTestGenerator
{
    public static class EnumerableExtensions
    {
        public static T Choice<T>(this IList<T> list, Random random)
        {
            return list[DiscreteUniform.Sample(random, 0, list.Count - 1)];
        }
    }
}