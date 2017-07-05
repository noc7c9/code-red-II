using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Noc7c9 {

    /* Fisher-Yates shuffle functions.
     *
     * Original Source: https://github.com/SebLague/Create-a-Game-Source
     */
    public static class FisherYates {

        static Random sharedPrng = new Random();

        static Random Seed2Prng(int? seed) {
            return seed.HasValue ? new Random(seed.Value) : sharedPrng;
        }

        public static T[] ShuffleToArray<T>(IEnumerable<T> enumerable, int? seed=null) {
            return ShuffleToArray<T>(enumerable, Seed2Prng(seed));
        }

        public static T[] ShuffleToArray<T>(IEnumerable<T> enumerable, Random prng) {
            T[] array = enumerable.ToArray();
            ShuffleInPlace<T>(array, prng);
            return array;
        }

        public static void ShuffleInPlace<T>(IList<T> array, int? seed=null) {
            ShuffleInPlace<T>(array, Seed2Prng(seed));
        }

        public static void ShuffleInPlace<T>(IList<T> array, Random prng) {
            for (int i = 0; i < array.Count - 1; i++) {
                int randomIndex = prng.Next(i, array.Count);

                T tmp = array[randomIndex];
                array[randomIndex] = array[i];
                array[i] = tmp;
            }
        }


        /* A generic shuffled list with support for indexing, iteration as well as
         * the addition and removal of items.
         *
         * Reshuffling:
         * - Complete enumeration will reshuffle when the last element is
         *   accessed.
         * - Manual indexing (get or set) will not result in reshuffling.
         *
         * - Adding an item will not reshuffle, but insert the item in a random
         *   position. Time Complexity: O(1)
         * - Removing an item will not reshuffle. Time Complexity: O(n)
         *
         * - Manually reshuffling is also possible via the Reshuffle() method.
         *
         * PRNG:
         * - Can be given a seed which will be used to create a System.Random
         *   instance.
         * - Can also be given a System.Random instance directly.
         * - If neither is given a static System.Random instance will be used
         *   instead.
         */
        public class ShuffleList<T> : IList<T> {

            Random prng;
            List<T> list;
            int startIndex;

            // Constructors

            // empty
            public ShuffleList(int? seed=null)
                : this(Seed2Prng(seed)) {}
            public ShuffleList(Random prng) {
                this.prng = prng;
                list = new List<T>();
            }

            // with capacity
            public ShuffleList(int capacity, int? seed=null)
                : this(capacity, Seed2Prng(seed)) {}
            public ShuffleList(int capacity, Random prng) {
                this.prng = prng;
                list = new List<T>(capacity);
            }

            // with IEnumerable
            public ShuffleList(IEnumerable<T> collection, int? seed=null)
                : this(collection, Seed2Prng(seed)) {}
            public ShuffleList(IEnumerable<T> collection, Random prng) {
                this.prng = prng;
                list = new List<T>(collection);
                Reshuffle();
            }

            // Public API

            public T Next() {
                if (list.Count == 0) {
                    throw new InvalidOperationException();
                }

                if (startIndex == list.Count) {
                    Reshuffle();
                    startIndex = 0;
                }

                return list[startIndex++];
            }

            public void Reshuffle() {
                ShuffleInPlace(list, prng);
            }

            // IEnumerable<T> implementations

            IEnumerator<T> IEnumerable<T>.GetEnumerator() {
                for (int i = 0; i < list.Count; i++) {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return list.GetEnumerator();
            }

            // ICollection<T> implementations

            public int Count {
                get {
                    return list.Count;
                }
            }

            public bool IsReadOnly {
                get {
                    return false;
                }
            }

            public void Add(T item) {
                // add the item to the end of the list
                list.Add(item);

                // swap the newly added item with a random item in the list
                // (including itself)
                int lastIndex = list.Count - 1;
                int randomIndex = prng.Next(0, list.Count);
                if (lastIndex != randomIndex) {
                    T tmp = list[randomIndex];
                    list[randomIndex] = list[lastIndex];
                    list[lastIndex] = tmp;
                }
            }

            public void Clear() {
                list.Clear();
                startIndex = 0;
            }

            public bool Contains(T item) {
                return list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex) {
                if (array == null) {
                    throw new ArgumentNullException();
                }
                if (arrayIndex < 0) {
                    throw new ArgumentOutOfRangeException();
                }
                if (list.Count > array.Length + arrayIndex) {
                    throw new ArgumentException();
                }

                for (int i = 0; i < list.Count; i++) {
                    array[arrayIndex + i] = this[i];
                }
            }

            public bool Remove(T item) {
                bool success = list.Remove(item);
                FixStartIndex();
                return success;
            }

            // IList<T> implementations

            public int IndexOf(T item) {
                return GetWrappedIndex(list.IndexOf(item));
            }

            public void Insert(int wrappedIndex, T item) {
                list.Insert(GetRealIndex(wrappedIndex), item);
            }

            public void RemoveAt(int wrappedIndex) {
                list.RemoveAt(GetRealIndex(wrappedIndex));
                FixStartIndex();
            }

            public T this[int wrappedIndex] {
                get {
                    return list[GetRealIndex(wrappedIndex)];
                }
                set {
                    list[GetRealIndex(wrappedIndex)] = value;
                }
            }

            int GetRealIndex(int wrappedIndex) {
                if (wrappedIndex < 0 || wrappedIndex >= list.Count) {
                    throw new ArgumentOutOfRangeException();
                }

                int realIndex = wrappedIndex + startIndex;
                if (realIndex >= list.Count) {
                    realIndex -= list.Count;
                }
                return realIndex;
            }

            int GetWrappedIndex(int realIndex) {
                if (realIndex < 0 || realIndex >= list.Count) {
                    throw new ArgumentOutOfRangeException();
                }

                int wrappedIndex = realIndex - startIndex;
                if (wrappedIndex < 0) {
                    wrappedIndex += list.Count;
                }
                return wrappedIndex;
            }

            void FixStartIndex() {
                if (startIndex < 0) {
                    startIndex = 0;
                } else if (startIndex > list.Count) {
                    startIndex = list.Count;
                }
            }

        }

    }

}
