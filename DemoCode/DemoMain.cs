using DemoCode.Entities;
using Engine;
using Engine.Entity;
using Game1.Engine.Pathfinding;
using GameCode.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoCode
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Demo
    {
        private IEngineAPI engine;

        // put it like this so u can pass the property value in
        // will suffice for now
        DemoLevelLoader levelLoader;

        public Demo(IEngineAPI pEngine)
        {
            engine = pEngine;
        }

        public void DemoLevel(int playerNum)
        {
            levelLoader = new DemoLevelLoader();
            var level = levelLoader.requestLevel("test-level.tmx");

            IPathFinding path = new PathFinding(levelLoader.grid);
            engine.SetPathFindingGrid(levelLoader.grid, true);

            var star = engine.LoadEntity<Star>("Walls/Star", new Vector2(1500, 100));

            int playerCount = 0;
            foreach (var asset in level.ToList())
            {
                if (asset.info.type.Equals(typeof(Player)))
                {
                    playerCount++;
                    if (playerCount > playerNum)
                    {
                        level.Remove(asset);
                        playerCount--;
                    }
                }
            }

            var ents = engine.LoadLevel(level);

            foreach (var ent in ents)
            {
                ent.LevelFinished += OnLevelFinished;
                ent.EntityRequested += OnEntityRequested;

                if (ent is Player)
                {

                    Player player = (Player)ent;
                    player.injectPathFinding(path, star);
                    ent.EntityRequested += OnEntityRequested;
                }
            }

            if (playerCount > 1)
            {
                string uiSeperator = "Walls/" + playerCount.ToString() + "player";
                engine.LoadUI<UI>(uiSeperator, new Vector2(0, 0));
            }


            engine.LoadUI<HelpMe>("help-me", new Vector2(0, 0));

            List<Vector2> verts = new List<Vector2> { new Vector2(0, 20), new Vector2(25, 0), new Vector2(50, 20), new Vector2(40, 50), new Vector2(10, 50) };
            engine.LoadEntity<Wall>("Walls/pentagon", new Vector2(400, 400), verts);

            List<Vector2> vertsSept = new List<Vector2> { new Vector2(25, 0), new Vector2(50, 14), new Vector2(50, 31), new Vector2(24, 50), new Vector2(13, 50), new Vector2(0, 30), new Vector2(0, 14) };
            engine.LoadEntity<Wall>("Walls/random hex", new Vector2(700, 700), vertsSept);

            List<Vector2> vertsHex = new List<Vector2> { new Vector2(100, 0), new Vector2(200, 48), new Vector2(200, 129), new Vector2(100, 200), new Vector2(0, 128), new Vector2(0, 48) };
            engine.LoadEntity<Wall>("Walls/reg-hex", new Vector2(1050, 450), vertsHex);

        }

        private void OnEntityRequested(object sender, EntityRequestArgs e)
        {
            engine.LoadEntity<BrownPath>(e.Texture, e.Position);
        }

        private void OnLevelFinished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
