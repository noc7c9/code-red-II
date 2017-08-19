using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The class responsible for the generation of randomized rooms, given a set
     * of parameters.
     */
    public static class RoomGenerator {

        // variables for one generation of a room
        // static so that they are shared with helper methods
        static Room room;
        static RoomSettings settings;
        static System.Random prng;
        static FisherYates.ShuffleList<Coord> openCoords;

        public static Room Generate(RoomSettings settings) {
            RoomGenerator.settings = settings;
            room = new Room(settings);
            prng = new System.Random(settings.seed);

            openCoords = new FisherYates.ShuffleList<Coord>(room.tileCount, prng);

            // fill coord array and initialize room with empty tiles
            for (int x = 0; x < settings.width; x++) {
                for (int y = 0; y < settings.height; y++) {
                    Coord c = new Coord(x, y);
                    InsertEmpty(c);
                    openCoords.Add(c);
                }
            }

            // place Warps
            foreach (RoomSettings.WarpSettings warp in settings.warps) {
                InsertWarp(warp.position, warp.target);
            }

            PlaceRandomObstacles();

            return room;
        }

        static void InsertEmpty(Coord c) {
            room.SetTile(c, new EmptyTile(c));
        }

        static void InsertObstacle(Coord c) {
            float height = Mathf.Lerp(
                    settings.minObstacleHeight, settings.maxObstacleHeight,
                    (float) prng.NextDouble());
            Color color = Color.Lerp(
                    settings.foregroundColor, settings.backgroundColor,
                    c.y / (float)room.size.y);
            ObstacleTile obs = new ObstacleTile(c, height, color);
            room.SetTile(c, obs);
        }

        static void InsertWarp(Coord c, int target) {
            room.SetTile(c, new WarpTile(c, target));
        }

        static void PlaceRandomObstacles() {
            int targetCount = (int)(room.tileCount * settings.obstaclePercent);
            int currentCount = 0;
            bool[,] obstacleMap = new bool[room.size.x, room.size.y];

            for (int i = 0; i < targetCount; i++) {
                Coord randomCoord = openCoords.Next();
                obstacleMap[randomCoord.x, randomCoord.y] = true;
                currentCount++;

                if (randomCoord != room.center && (room.tileCount - currentCount)
                        == GetAccessibleCount(room.center, obstacleMap)) {
                    InsertObstacle(randomCoord);
                    openCoords.Remove(randomCoord);
                } else {
                    obstacleMap[randomCoord.x, randomCoord.y] = false;
                    currentCount--;
                }
            }
        }

        static int GetAccessibleCount(Coord start, bool[,] blockedMap) {
            int roomWidth = room.size.x;
            int roomHeight = room.size.y;
            bool[,] visited = new bool[roomWidth, roomHeight];
            Queue<Coord> queue = new Queue<Coord>(roomWidth * roomHeight);

            // starting point
            queue.Enqueue(start);
            visited[start.x, start.y] = true;
            int accessibleCount = 1;

            while (queue.Count > 0) {
                Coord current = queue.Dequeue();

                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        // ignore diagonals
                        if (x != 0 && y != 0) {
                            continue;
                        }

                        int neighbourX = current.x + x;
                        int neighbourY = current.y + y;

                        // ignore out of bound tiles
                        if (neighbourX < 0 || neighbourX >= roomWidth
                                || neighbourY < 0 || neighbourY >= roomHeight) {
                            continue;
                        }

                        // ignore visited
                        if (visited[neighbourX, neighbourY]) {
                            continue;
                        }

                        // ignore blocked tiles
                        if (blockedMap[neighbourX, neighbourY]) {
                            continue;
                        }

                        // otherwise
                        visited[neighbourX, neighbourY] = true;
                        queue.Enqueue(new Coord(neighbourX, neighbourY));
                        accessibleCount++;
                    }
                }
            }

            return accessibleCount;
        }

    }

}
