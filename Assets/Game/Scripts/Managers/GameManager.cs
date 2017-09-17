using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The top level manager of the game, should handle management of everything
     * else.
     */
    public class GameManager : Singleton<GameManager> {

        // gameplay settings
        public Gun gun1;
        public Gun gun2;

        public bool generateLevelOnAwake;
        public CityBlockSettings cityBlockSettings;

        PlayerInput playerInput;

        CityBlock loadedCityBlock;

        public void ReloadCityBlock() {
            loadedCityBlock = CityBlockGenerator.Generate(cityBlockSettings);
            GetCityBlockLoader().Load(loadedCityBlock);
        }

        void Awake() {
            playerInput = FindObjectOfType<PlayerInput>();

            if (generateLevelOnAwake) {
                ReloadCityBlock();
                FindObjectOfType<EnemySpawner>().PopulateStage(
                        loadedCityBlock, GetCityBlockLoader().pieceWidth);
            }
        }

        // TODO: this needs to be in an input manager class
        public Vector3 GetCursorPosition() {
            if (playerInput == null) {
                return Vector3.zero;
            }
            return playerInput.GetLookAtPoint();
        }

        // singleton manager getter methods

        CityBlockLoader cityBlockLoader;
        public CityBlockLoader GetCityBlockLoader() {
            if (cityBlockLoader == null) {
                cityBlockLoader = FindObjectOfType<CityBlockLoader>();
            }
            return cityBlockLoader;
        }

        PlayerController playerController;
        public PlayerController GetPlayerController() {
            if (playerController == null) {
                playerController = FindObjectOfType<PlayerController>();
            }
            return playerController;
        }

        BossController bossController;
        public BossController GetBossController() {
            if (bossController == null) {
                bossController = FindObjectOfType<BossController>();
            }
            return bossController;
        }

    }

}
