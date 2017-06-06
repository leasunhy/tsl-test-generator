using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace TSLTestGenerator
{
    public static class EnumerableExtensions
    {
        public static T Choice<T>(this IList<T> list, Random random)
        {
            return list[DiscreteUniform.Sample(random, 0, list.Count - 1)];
        }

        public static T Choice<T>(this IEnumerable<T> enumerable, Random random)
        {
            var list = enumerable.ToList();
            return list[DiscreteUniform.Sample(random, 0, list.Count - 1)];
        }

        public static TValue? GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : struct
        {
            return dict.TryGetValue(key, out TValue value) ? value : default(TValue?);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            TKey key, TValue defaultValue = default(TValue))
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            TKey key, Func<TValue> defaultValueFunc)
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValueFunc();
        }

        public static void WithProbabilityThreshold<T>(this IEnumerable<KeyValuePair<T, double>> enumerable, double threshold, Action<T> action)
        {
            foreach (var pair in enumerable)
            {
                if (pair.Value < threshold)
                    action(pair.Key);
            }
        }

        public static IEnumerable<TResult> WithProbabilityThreshold<TSource, TResult>(this IEnumerable<KeyValuePair<TSource, double>> enumerable,
            double threshold, Func<TSource, TResult> func)
        {
            foreach (var pair in enumerable)
            {
                if (pair.Value < threshold)
                    yield return func(pair.Key);
                else
                    yield return default(TResult);
            }
        }

        public static IEnumerable<TResult> WithProbabilityThreshold<TSource, TResult>(this IEnumerable<TSource> enumerable,
            double threshold, Func<TSource, TResult> func, Func<TSource, double> probabilityProvider)
        {
            foreach (var item in enumerable)
            {
                if (probabilityProvider(item) < threshold)
                    yield return func(item);
                else
                    yield return default(TResult);
            }
        }
    }
}