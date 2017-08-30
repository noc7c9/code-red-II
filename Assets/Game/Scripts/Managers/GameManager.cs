using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The top level manager of the game, should handle management of everything
     * else.
     */
    public class GameManager : Singleton<GameManager> {

        // singleton manager getter methods

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

            ReloadRoom();
        }

        PlayerInput playerInput;

        // TODO: this needs to be in an input manager class
        public Vector3 GetCursorPosition() {
            if (playerInput == null) {
                return Vector3.zero;
            }
            return playerInput.GetLookAtPoint();
        }

        // gameplay settings
        public Gun gun;
        public RoomSettings roomSettings;

        public void ReloadRoom() {
            GetRoomLoader().GenerateAndLoad(roomSettings);
        }

    }

}
