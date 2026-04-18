using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class WeightedRandom
    {
        public static T Get<T>((T item, int weight)[] array)
        {
            var totalWeight = 0f;
            List<float> inverted = new List<float>();

            foreach (var element in array)
            {
                if (element.weight == 0)
                    throw new System.DivideByZeroException();

                var inv = 1f / element.weight;
                inverted.Add(inv);
                totalWeight += inv;
            }

            var r = Random.value * totalWeight;

            var cumulative = 0f;
            for (int i = 0; i < array.Length; i++)
            {
                cumulative += inverted[i];
                if (r <= cumulative)
                    return array[i].item;
            }

            return array[array.Length - 1].item;
        }
    }
}