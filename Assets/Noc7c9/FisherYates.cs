using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Noc7c9 {

    /* Fisher-Yates shuffle functions.
     *
     * Original Source: https://github.com/SebLague/Create-a-Game-Source
     */
    public static class FisherYates {

        public static T[] ShuffleToArray<T>(IEnumerable<T> enumerable, int seed) {
            return ShuffleArrayInPlace<T>(enumerable.ToArray(), seed);
        }

        public static T[] ShuffleArrayInPlace<T>(T[] array, int seed) {
            System.Random prng = new System.Random(seed);

            for (int i = 0; i < array.Length - 1; i++) {
                int randomIndex = prng.Next(i, array.Length);

                T tmp = array[randomIndex];
                array[randomIndex] = array[i];
                array[i] = tmp;
            }

            return array;
        }

    }

}
