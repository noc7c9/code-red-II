using System.Linq;
using System.Collections;
using System.Collections.Generic;

/* Helper functions.
 */
public static class Utility {

    // Fisher-Yates shuffle
    public static T[] ShuffleArray<T>(IEnumerable<T> enumerable, int seed) {
        return ShuffleArray<T>(enumerable.ToArray(), seed);
    }
    public static T[] ShuffleArray<T>(T[] array, int seed) {
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
