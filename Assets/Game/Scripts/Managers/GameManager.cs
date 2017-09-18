using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The top level manager of the game, should handle management of everything
     * else.
     */
    public class GameManager : Singleton<GameManager> {

        public Gun gun1;
        public Gun gun2;

        public float musicChangeBossHealth;

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

        void Start() {
            StartCoroutine(BossHealthMusicChange());
        }

        IEnumerator BossHealthMusicChange() {
            // start checking after a moment to allow the game to initialize
            yield return new WaitForSeconds(1);

            while (true) {
                if (GetBossController().healthPercentage() < musicChangeBossHealth) {
                    GetMusicManager().PlayTrack(MusicManager.MusicTrack.BOSS_THEME_FAST);
                    break;
                } else {
                    yield return new WaitForSeconds(0.25f);
                }
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

        MusicManager musicManager;
        public MusicManager GetMusicManager() {
            if (musicManager == null) {
                musicManager = FindObjectOfType<MusicManager>();
            }
            return musicManager;
        }

    }

}
