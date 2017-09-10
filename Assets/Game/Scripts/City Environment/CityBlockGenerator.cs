using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Responsible for generating the city block layouts.
     */
    public class CityBlockGenerator {

        public static CityBlock Generate(CityBlockSettings settings) {
            var cityBlock = new CityBlock(settings);
            int x, y;


            // create roads

            // add horizontal roads
            for (y = 1; y < cityBlock.size.y - 1; y += settings.gap) {
                for (x = 1; x < cityBlock.size.x - 1; x++) {
                    cityBlock.SetRoadTile(x, y, RoadPiece.HORIZONTAL);
                }
            }
            // add vertical roads
            for (x = 1; x < cityBlock.size.x - 1; x += settings.gap) {
                for (y = 1; y < cityBlock.size.y - 1; y++) {
                    if (cityBlock.GetRoadTile(x, y) != RoadPiece.NONE) {
                        cityBlock.SetRoadTile(x, y, RoadPiece.CROSS_CENTER);
                    } else {
                        cityBlock.SetRoadTile(x, y, RoadPiece.VERTICAL);
                    }
                }
            }
            // add the cross roads borders
            for (x = 0; x < cityBlock.size.x; x++) {
                for (y = 0; y < cityBlock.size.y; y++) {
                    if (cityBlock.GetRoadTile(x, y) == RoadPiece.CROSS_CENTER) {
                        cityBlock.SetRoadTile(x - 1, y - 1,
                                RoadPiece.CROSS_SW);
                        cityBlock.SetRoadTile(x - 1, y,
                                RoadPiece.CROSS_W);
                        cityBlock.SetRoadTile(x - 1, y + 1,
                                RoadPiece.CROSS_NW);
                        cityBlock.SetRoadTile(x, y + 1,
                                RoadPiece.CROSS_N);
                        cityBlock.SetRoadTile(x + 1, y + 1,
                                RoadPiece.CROSS_NE);
                        cityBlock.SetRoadTile(x + 1, y,
                                RoadPiece.CROSS_E);
                        cityBlock.SetRoadTile(x + 1, y - 1,
                                RoadPiece.CROSS_SE);
                        cityBlock.SetRoadTile(x, y - 1,
                                RoadPiece.CROSS_S);
                    }
                }
            }


            // create pavements

            // wrap road with pavement
            for (x = 0; x < cityBlock.size.x; x++) {
                for (y = 0; y < cityBlock.size.y; y++) {
                    var road = cityBlock.GetRoadTile(x, y);
                    if (road == RoadPiece.CROSS_NW) {
                        cityBlock.SetPavementTile(x, y, PavementPiece.CORNER_NW);
                    } else if (road == RoadPiece.CROSS_SW) {
                        cityBlock.SetPavementTile(x, y, PavementPiece.CORNER_SW);
                    } else if (road == RoadPiece.CROSS_SE) {
                        cityBlock.SetPavementTile(x, y, PavementPiece.CORNER_SE);
                    } else if (road == RoadPiece.CROSS_NE) {
                        cityBlock.SetPavementTile(x, y, PavementPiece.CORNER_NE);
                    } else if (road == RoadPiece.HORIZONTAL) {
                        cityBlock.SetPavementTile(x, y - 1, PavementPiece.SIDE_S);
                        cityBlock.SetPavementTile(x, y + 1, PavementPiece.SIDE_N);
                    } else if (road == RoadPiece.VERTICAL) {
                        cityBlock.SetPavementTile(x - 1, y, PavementPiece.SIDE_W);
                        cityBlock.SetPavementTile(x + 1, y, PavementPiece.SIDE_E);
                    }
                }
            }

            // city block border pavement
            for (x = 1; x < cityBlock.size.x-1; x++) {
                cityBlock.SetPavementTile(x, 0, PavementPiece.SIDE_S);
                cityBlock.SetPavementTile(x, cityBlock.size.y-1, PavementPiece.SIDE_N);
            }
            for (y = 1; y < cityBlock.size.y-1; y++) {
                cityBlock.SetPavementTile(0, y, PavementPiece.SIDE_W);
                cityBlock.SetPavementTile(cityBlock.size.x-1, y, PavementPiece.SIDE_E);
            }

            // set city block border corners
            cityBlock.SetPavementTile(0, 0, PavementPiece.CORNER_NE);
            cityBlock.SetPavementTile(cityBlock.size.x-1, 0,
                    PavementPiece.CORNER_NW);
            cityBlock.SetPavementTile(0, cityBlock.size.y-1,
                    PavementPiece.CORNER_SE);
            cityBlock.SetPavementTile(cityBlock.size.x-1, cityBlock.size.y-1,
                    PavementPiece.CORNER_SW);


            // create buildings

            // set corner buildings
            for (x = 0; x < cityBlock.size.x; x++) {
                for (y = 0; y < cityBlock.size.y; y++) {
                    var pavement = cityBlock.GetPavementTile(x, y);
                    if (pavement == PavementPiece.CORNER_NW) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.CORNER_NW);
                    } else if (pavement == PavementPiece.CORNER_SW) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.CORNER_SW);
                    } else if (pavement == PavementPiece.CORNER_SE) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.CORNER_SE);
                    } else if (pavement == PavementPiece.CORNER_NE) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.CORNER_NE);
                    } else if (pavement == PavementPiece.SIDE_S) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.SIDE_S);
                    } else if (pavement == PavementPiece.SIDE_N) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.SIDE_N);
                    } else if (pavement == PavementPiece.SIDE_W) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.SIDE_W);
                    } else if (pavement == PavementPiece.SIDE_E) {
                        cityBlock.SetBuildingTile(x, y, BuildingPiece.SIDE_E);
                    }
                }
            }

            // set city block border corner buildings
            cityBlock.SetBuildingTile(0, 0,
                    BuildingPiece.CORNER_INNER_SW);
            cityBlock.SetBuildingTile(cityBlock.size.x-1, 0,
                    BuildingPiece.CORNER_INNER_SE);
            cityBlock.SetBuildingTile(0, cityBlock.size.y-1,
                    BuildingPiece.CORNER_INNER_NW);
            cityBlock.SetBuildingTile(cityBlock.size.x-1, cityBlock.size.y-1,
                    BuildingPiece.CORNER_INNER_NE);


            // set street lamps

            // set corner lamps
            for (x = 0; x < cityBlock.size.x; x++) {
                for (y = 0; y < cityBlock.size.y; y++) {
                    var pavement = cityBlock.GetPavementTile(x, y);
                    if (pavement == PavementPiece.CORNER_NW) {
                        cityBlock.SetMiscTile(x, y, MiscPiece.STREET_LAMP_CORNER_NW);
                    } else if (pavement == PavementPiece.CORNER_SW) {
                        cityBlock.SetMiscTile(x, y, MiscPiece.STREET_LAMP_CORNER_SW);
                    } else if (pavement == PavementPiece.CORNER_SE) {
                        cityBlock.SetMiscTile(x, y, MiscPiece.STREET_LAMP_CORNER_SE);
                    } else if (pavement == PavementPiece.CORNER_NE) {
                        cityBlock.SetMiscTile(x, y, MiscPiece.STREET_LAMP_CORNER_NE);
                    }
                }
            }

            // set roads with crossings
            for (x = 0; x < cityBlock.size.x; x++) {
                for (y = 0; y < cityBlock.size.y; y++) {
                    var road = cityBlock.GetRoadTile(x, y);
                    if (road == RoadPiece.CROSS_N) {
                        cityBlock.SetRoadTile(x, y+1, RoadPiece.VERTICAL_WITH_CROSSING);
                    } else if (road == RoadPiece.CROSS_S) {
                        cityBlock.SetRoadTile(x, y-1, RoadPiece.VERTICAL_WITH_CROSSING);
                    } else if (road == RoadPiece.CROSS_E) {
                        cityBlock.SetRoadTile(x+1, y, RoadPiece.HORIZONTAL_WITH_CROSSING);
                    } else if (road == RoadPiece.CROSS_W) {
                        cityBlock.SetRoadTile(x-1, y, RoadPiece.HORIZONTAL_WITH_CROSSING);
                    }
                }
            }


            return cityBlock;
        }

    }

}
