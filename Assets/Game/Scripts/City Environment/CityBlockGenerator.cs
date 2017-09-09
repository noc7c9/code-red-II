using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Responsible for generating the city block layouts.
     */
    public class CityBlockGenerator {

        public static CityBlock Generate(CityBlockSettings settings) {
            var cityBlock = new CityBlock(settings);

            // add horizontal roads
            for (int y = 0; y < cityBlock.size.y; y += settings.gap) {
                for (int x = 0; x < cityBlock.size.x; x++) {
                    cityBlock.SetRoadTile(x, y, RoadPiece.HORIZONTAL);
                }
            }
            // add vertical roads
            for (int x = 0; x < cityBlock.size.x; x += settings.gap) {
                for (int y = 0; y < cityBlock.size.y; y++) {
                    if (cityBlock.GetRoadTile(x, y) != RoadPiece.NONE) {
                        cityBlock.SetRoadTile(x, y, RoadPiece.CROSS_CENTER);
                    } else {
                        cityBlock.SetRoadTile(x, y, RoadPiece.VERTICAL);
                    }
                }
            }
            // add the cross roads borders
            for (int x = 0; x < cityBlock.size.x; x++) {
                for (int y = 0; y < cityBlock.size.y; y++) {
                    if (cityBlock.GetRoadTile(x, y) == RoadPiece.CROSS_CENTER) {
                        cityBlock.SetRoadTile(x - 1, y - 1,
                                RoadPiece.CROSS_CORNER_LEFT_BOTTOM);
                        cityBlock.SetRoadTile(x - 1, y,
                                RoadPiece.CROSS_SIDE_LEFT);
                        cityBlock.SetRoadTile(x - 1, y + 1,
                                RoadPiece.CROSS_CORNER_LEFT_TOP);
                        cityBlock.SetRoadTile(x, y + 1,
                                RoadPiece.CROSS_SIDE_TOP);
                        cityBlock.SetRoadTile(x + 1, y + 1,
                                RoadPiece.CROSS_CORNER_RIGHT_TOP);
                        cityBlock.SetRoadTile(x + 1, y,
                                RoadPiece.CROSS_SIDE_RIGHT);
                        cityBlock.SetRoadTile(x + 1, y - 1,
                                RoadPiece.CROSS_CORNER_RIGHT_BOTTOM);
                        cityBlock.SetRoadTile(x, y - 1,
                                RoadPiece.CROSS_SIDE_BOTTOM);
                    }
                }
            }

            return cityBlock;
        }

    }

}
