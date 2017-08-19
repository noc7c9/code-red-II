using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The top level manager of the game, should handle management of everything
     * else.
     */
    public class GameManager : Singleton<GameManager> {

        // singleton manager getter methods

        Spawner spawner;
        public Spawner GetSpawner() {
            if (spawner == null) {
                spawner = FindObjectOfType<Spawner>();
            }
            return spawner;
        }

        RoomLoader roomLoader;
        public RoomLoader GetRoomLoader() {
            if (roomLoader == null) {
                roomLoader = FindObjectOfType<RoomLoader>();
            }
            return roomLoader;
        }

        PlayerController playerController;
        public PlayerController GetPlayerController() {
            if (playerController == null) {
                playerController = FindObjectOfType<PlayerController>();
            }
            return playerController;
        }

        void Awake() {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        PlayerInput playerInput;

        // TODO: this needs to be in an input manager class
        public Vector3 GetCursorPosition() {
            if (playerInput == null) {
                return Vector3.zero;
            }
            return playerInput.GetLookAtPoint();
        }

        // level information

        public int visibleLevel;
        public Level[] levels;

        public Gun GetGun(int levelIndex) {
            return levels[levelIndex].gun;
        }

        public Spawner.Wave GetWave(int levelIndex) {
            return levels[levelIndex].wave;
        }

        public RoomSettings GetRoomSettings(int levelIndex) {
            return levels[levelIndex].roomSettings;
        }

        public int GetLevelsCount() {
            return levels.Length;
        }

        public void LoadRoom(int levelIndex) {
            GetRoomLoader().GenerateAndLoad(levels[levelIndex].roomSettings);
        }

        [System.Serializable]
        public struct Level {
            public Gun gun;
            public Spawner.Wave wave;
            public RoomSettings roomSettings;
        }

    }

}
