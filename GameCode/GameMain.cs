using Engine;
using Engine.Entity;
using Engine.UI;
using GameCode.Entities;
using GameCode.Misc;
using GameCode.Special_Abilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameCode
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain
    {
        #region Data Members

        public static int ScreenWidth = EngineMain.ScreenWidth, ScreenHeight = EngineMain.ScreenHeight;

        private IEngineAPI engine;

        // events for main menu
        event LoadUIButtons Loadbutton;
        event UnloadUIButtons UnloadButtons;
        event StartLevel startLevel;

        // class members
        LevelLoader levelLoader;
        MainMenu mainMenu;
        ISpecialAbilityManager abilityManager;
        LoadingScreen loadingScreen;

        List<String> fileLevels = new List<string>();

        string levelFilePath = @"Level\LevelFiles";

        int levelCounter = 0, players = 0;

        #endregion

        public GameMain(IEngineAPI pEngine)
        {
            engine = pEngine;
        }

        /// <summary>
        /// Start screen
        /// </summary>
        public void Start()
        {
            fileLevels.Clear();

            // get all '.tmx' files from level path
            fileLevels.AddRange(Directory.GetFiles(levelFilePath, "*.tmx").Select(Path.GetFileName));

            Loadbutton += LoadUIButtons;
            UnloadButtons += RemoveUIButtons;
            startLevel += LoadLevel;

            // load main menu
            mainMenu = new MainMenu(Loadbutton, UnloadButtons, startLevel);
            levelLoader = new LevelLoader();
            levelCounter = 0;
        }

        /// <summary>
        /// Load loading screen ui
        /// </summary>
        void LoadingScreen()
        {
            loadingScreen = engine.LoadUI<LoadingScreen>("Loading/DoctorDashLoading", Vector2.Zero);
            loadingScreen.EntityRequested += OnEntityRequested;
            loadingScreen.EntityDestroy += OnRequestDestroy;
            loadingScreen.LevelFinished += OnLevelFinished;
            loadingScreen.load = true;
        }

        /// <summary>
        /// Load level
        /// </summary>
        /// <param name="playerNum"></param>
        public void LoadLevel(int playerNum)
        {
            // unsub
            startLevel -= LoadLevel;

            // remove ui
            mainMenu.RemoveUI();

            if (playerNum == 0)
            {
                playerNum = players;
            }
            else
            {
                players = playerNum;
            }

            LoadingScreen();
        }

        /// <summary>
        /// Load the game level
        /// </summary>
        void LoadLevel()
        {
            #region Manual loading of ui artefact and patient
            var uiArtefact = engine.LoadUI<ArtefactUI>("ArtefactsUI/UIArtefact", Vector2.Zero);
            uiArtefact.LevelFinished += OnLevelFinished;
            uiArtefact.EntityRequested += OnEntityRequested;
            uiArtefact.EntityDestroy += OnRequestDestroy;

            var person = engine.LoadEntity<Patient>(@"Patient\Patient", levelLoader.GetObjectPosition(fileLevels[levelCounter], "PatientLocation"),
                ReadFile.GetVerticiesFromTMX(fileLevels[levelCounter]));
            person.LevelFinished += OnLevelFinished;
            person.EntityRequested += OnEntityRequested;
            person.EntityDestroy += OnRequestDestroy;
            #endregion

            // get level info
            var level = levelLoader.requestLevel(fileLevels[levelCounter]);

            int playerCount = 0;
            level.Where(assest => assest.info.type.Equals(typeof(Player))).ToList().
                ForEach(x =>
                {
                    if (x.info.type.Equals(typeof(Player)))
                        playerCount++;
                    if (playerCount > players)
                    {
                        // remove players that arent in game
                        level.Remove(x);
                        playerCount--;
                    }
                });

            int pCount = 0;
            // load level
            var ents = engine.LoadLevel(level);
            // for each where it doesnt equal floor or wall
            foreach (var ent in ents.Select(x => x).Where(x => x.GetType() != typeof(Floor) && x.GetType() != typeof(Wall)))
            {
                // sub
                ent.LevelFinished += OnLevelFinished;
                ent.EntityRequested += OnEntityRequested;
                ent.EntityDestroy += OnRequestDestroy;

                if (ent.GetType().Equals(typeof(Player)))
                {
                    // if player inject whats needed
                    abilityManager = new SpecialAbilityManager();
                    ((Player)ent).InjectGrid(levelLoader.grid);
                    ((Player)ent).InjectAbility(abilityManager);
                    ((Player)ent).SetPlayerCount(pCount);
                    pCount++;
                    if (abilityManager.PlayerAbility != null)
                    {
                        abilityManager.PlayerAbility.LevelFinished += OnLevelFinished;
                        abilityManager.PlayerAbility.EntityRequested += OnEntityRequested;
                        abilityManager.PlayerAbility.EntityDestroy += OnRequestDestroy;
                    }
                    // inject each player to ui artefact
                    uiArtefact.InjectPlayers(ent as Player);
                }

                if (ent.GetType().Equals(typeof(Artefact)))
                {
                    // if artefact inject ui
                    ((Artefact)ent).InjectUI(uiArtefact);
                    // and inject artefact to paitent
                    person.InjectArtefact((Artefact)ent);
                }
            }

            if (playerCount > 1)
            {
                // ui seperator
                string uiSeperator = "UISeperator/" + playerCount.ToString() + "player";
                engine.LoadUI<UI>(uiSeperator, new Vector2(0, 0));
            }

            levelCounter += 1;
        }

        private void OnEntityRequested(object sender, EntityRequestArgs e)
        {
            // load requested entity
            if (e.type.Name == typeof(SpecialAbilityWall).Name)
            {
                e.gameObject = engine.LoadEntity<SpecialAbilityWall>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(SpecialAbilityIce).Name)
            {
                e.gameObject = engine.LoadEntity<SpecialAbilityIce>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(SpecialAbilitySlug).Name)
            {
                e.gameObject = engine.LoadEntity<SpecialAbilitySlug>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(SpecialAbilityAdrenalineShot).Name)
            {
                e.gameObject = engine.LoadEntity<SpecialAbilityAdrenalineShot>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(PlayerPath).Name)
            {
                e.gameObject = engine.LoadEntity<PlayerPath>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(LoadingBar).Name)
            {
                e.gameObject = engine.LoadUI<LoadingBar>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(OpenDoor).Name)
            {
                e.gameObject = engine.LoadEntity<OpenDoor>(e.Texture, e.Position);
            }
            else if (e.type.Name == typeof(ClosedDoor).Name)
            {
                e.gameObject = engine.LoadEntity<ClosedDoor>(e.Texture, e.Position);
            }

            if (e.gameObject == null)
            {
                //e.gameObject = null;
                return;
            }
            e.gameObject.LevelFinished += OnLevelFinished;
            e.gameObject.EntityRequested += OnEntityRequested;
            e.gameObject.EntityDestroy += OnRequestDestroy;
        }

        private void OnLevelFinished(object sender, EventArgs e)
        {
            // clear level
            engine.UnloadWholeLevel();

            if (abilityManager != null)
            {
                // remove static players in ability manager
                abilityManager.ClearStaticPlayers();
            }

            // if player
            if (sender is Player player)
            {
                // show winning screen
                WinningScreen winning;

                // if theres more levels ahead then show this
                if (fileLevels.Count > levelCounter)
                {
                    if (player.playerCount == 0)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player1Wins", Vector2.Zero);
                    }
                    else if (player.playerCount == 1)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player2Wins", Vector2.Zero);
                    }
                    else if (player.playerCount == 2)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player3Wins", Vector2.Zero);
                    }
                    else
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player4Wins", Vector2.Zero);
                    }
                }
                // else final level
                else
                {
                    if (player.playerCount == 0)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player1WinsFinalLevel", Vector2.Zero);
                    }
                    else if (player.playerCount == 1)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player2WinsFinalLevel", Vector2.Zero);
                    }
                    else if (player.playerCount == 2)
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player3WinsFinalLevel", Vector2.Zero);
                    }
                    else
                    {
                        winning = engine.LoadUI<WinningScreen>("Winning Screen/Player4WinsFinalLevel", Vector2.Zero);
                    }
                }
                winning.LevelFinished += OnLevelFinished;
                winning.EntityRequested += OnEntityRequested;

                return;
            }

            // if more level files
            if (fileLevels.Count > levelCounter)
            {
                if (levelCounter > 0)
                {
                    if (!loadingScreen.load)
                    {
                        // if not loading load level
                        LoadLevel();
                    }
                    else
                    {
                        LoadingScreen();
                        loadingScreen.load = false;
                    }
                }
                else
                {
                    LoadLevel();
                }
            }
            else
            {
                // all levels have been players show main menu
                Start();
            }
        }

        private void OnRequestDestroy(object sender, EventArgs e)
        {
            // destoy requested entity
            iEntity entity = sender as iEntity;

            entity.LevelFinished -= OnLevelFinished;
            entity.EntityRequested -= OnEntityRequested;
            entity.EntityDestroy -= OnRequestDestroy;

            engine.UnLoad(entity);
        }

        #region UI
        IInteractiveUI LoadUIButtons(IInteractiveUI pButton, string pTex, Vector2 pPos)
        {
            // load ui buttons
            return pButton = engine.LoadUI<Button>(pTex, pPos);
            //return pButton = engine.LoadUI<Button>(pTex, pPos);
        }

        void RemoveUIButtons(IInteractiveUI pButton)
        {
            // remove ui buttons
            engine.UnLoad(pButton as iEntity);
        }
        #endregion
    }
}