using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The top level manager of the game, should handle management of everything
     * else.
     */
    public class GameManager : Singleton<GameManager> {

        public PlayerController playerController;

        PlayerInput playerInput;

        void Awake() {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        public GameObject GetPlayer() {
            return playerController.gameObject;
        }

        public Vector3 GetCursorPosition() {
            return playerInput.GetLookAtPoint();
        }

    }

}
