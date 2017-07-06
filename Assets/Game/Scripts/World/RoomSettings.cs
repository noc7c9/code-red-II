using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* A struct that describes all the settings to define a Room.
     */
    [System.Serializable]
    public struct RoomSettings {

        public int seed;

        public int width;
        public int height;

        [Range(0, 1)]
        public float obstaclePercent;

        public float minObstacleHeight;
        public float maxObstacleHeight;

        [ColorUsageAttribute(false)]
        public Color foregroundColor;

        [ColorUsageAttribute(false)]
        public Color backgroundColor;

    }

}
