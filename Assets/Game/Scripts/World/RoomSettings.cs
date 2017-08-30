using System.Text;
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

        public int numberOfEnemies;

        public WarpSettings[] warps;

        [System.Serializable]
        public struct WarpSettings {

            public Coord position;
            // TODO: currently points to a level index
            public int target;

        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder("Room Settings {\n");
            sb.AppendFormat("    seed             = {0}\n", seed);
            sb.AppendFormat("    width x height   = {0} x {1}\n", width, height);
            sb.AppendFormat("    obstacle percent = {0}\n", obstaclePercent);
            sb.AppendFormat("    obstacle height  = [{0}, {1}]\n",
                    minObstacleHeight, maxObstacleHeight);
            sb.AppendFormat("    foreground color = {0}\n", foregroundColor);
            sb.AppendFormat("    background color = {0}\n", backgroundColor);
            sb.AppendFormat("    warps            = [\n");
            for (int i = 0; i < warps.Length; i++) {
                sb.AppendFormat("        {0}\n", warps[i].position);
            }
            sb.AppendFormat("    ]\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
