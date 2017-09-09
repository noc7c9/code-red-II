using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Completely describes a city block.
     */
    [System.Serializable]
    public struct CityBlockSettings {

        public int seed;

        [Range(1, 100)]
        public int width;
        [Range(1, 100)]
        public int height;

        [Range(1, 100)]
        public int gap;

    }

}
