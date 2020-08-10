using Engine.Shape;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

//------------------------------------------------------------------------------------------------------------------------------------------
//Reference:     
//Adapted from: https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
//------------------------------------------------------------------------------------------------------------------------------------------

namespace Engine.Collision
{
    class QuadTree
    {
        // Max entities in a node before splitting
        private int MaxObjects = 3;
        // maximum number of splits that can happen
        private int MaxLevels = 3;
        //Current level of node
        private int level;
        //List of entities in current node
        private List<IShape> EntityList;
        //Boundary of current node
        private Rectangle bounds;
        //list of subnodes for if the node splits into 4
        private QuadTree[] nodes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pLevel"></param>
        /// <param name="pBounds"></param>
        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            EntityList = new List<IShape>();
            bounds = pBounds;
            nodes = new QuadTree[4];

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pLevel"></param>
        /// <param name="pBounds"></param>
        /// <param name="pMaxObjects"></param>
        /// <param name="pMaxLevels"></param>
        public QuadTree(int pLevel, Rectangle pBounds, int pMaxObjects, int pMaxLevels)
        {
            level = pLevel;
            EntityList = new List<IShape>();
            bounds = pBounds;
            nodes = new QuadTree[4];

            MaxObjects = pMaxObjects;
            MaxLevels = pMaxLevels;

        }


        /// <summary>
        /// clears quad tree ready for reindexing
        /// </summary>
        public void Clear()
        {
            //Clear ent list of current node
            EntityList.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                //Call clear on all child nodes
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    //Once cleared dispose of the node
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Split into 4 nodes
        /// </summary>
        private void Split()
        {
            // Half width of boundary
            int subWidth = (int)(bounds.Width / 2);
            //Half height of boundary
            int subHeight = (int)(bounds.Height / 2);
            int x = (int)bounds.X;
            int y = (int)bounds.Y;

            //Create each of the new child nodes (4 in total) which will be initialised with their level increased by one and a new bound set 
            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// sets the pEnts node , if the entity doesnt fit into one node it is placed in the parent node (index -1)
        /// </summary>
        /// <param name="pEnt">Entity to find index of</param>
        /// <returns></returns>
        private int GetIndex(IShape pEnt)
        {
            Vector2 entPos = pEnt.GetPosition();
            Rectangle hitBox = pEnt.GetBoundingBox();
            // look at storing in each node instead of -1
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            // entity fits into top quad
            bool topQuadrant = (entPos.Y < horizontalMidpoint && entPos.Y + hitBox.Height < horizontalMidpoint);
            // entity fits into bottom quad
            bool bottomQuadrant = (entPos.Y > horizontalMidpoint);

            // entity fits into left quad
            if (entPos.X < verticalMidpoint && entPos.X + hitBox.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // entity fits into right quad
            else if (entPos.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        /// <summary>
        /// Insert object into qad tree, if the capacity is reached for the node it gets split
        /// </summary>
        /// <param name="pEnt">Entity to insert</param>
        public void Insert(IShape pEnt)
        {
            #region method 2

            int index = GetIndex(pEnt);
            // Check to see if entity can fit in a child node first (index not -1)
            if (index >= 0)
            {

                if (nodes[0] != null)
                {
                    // if child nodes exist then insert entity into child node
                    nodes[index].Insert(pEnt);
                }
                // No child nodes have been created 
                else
                {
                    //if the max level hasnt been reached
                    if (level < MaxLevels)
                    {
                        //Split the current node
                        Split();
                        // Insert entity into new child node
                        nodes[index].Insert(pEnt);
                    }
                    else
                        //Max level has been reached so add entity to current node instead
                        EntityList.Add(pEnt);
                }
            }
            //The index is -1 so add it to this node
            else
            {
                EntityList.Add(pEnt);
            }


            #endregion

        }

        /// <summary>
        /// Finds all entities that could collide with a specific entity
        /// </summary>
        /// <param name="returnObjects">List of objects that could collide</param>
        /// <param name="pEnt">Entity to check for potential colliders</param>
        public void FindPossibleCollisions(List<IShape> returnObjects, IShape pEnt)
        {
            //if there has been a division
            if (nodes[0] != null)
            {
                int index = GetIndex(pEnt);
                //If the index of the entity to check doesnt overlap with any nodes only check the one index and its children
                if (index != -1)
                {
                    //will recursively check down through the indicated node and its children and return then as potential collisions
                    nodes[index].FindPossibleCollisions(returnObjects, pEnt);
                }
                else // if the entity overlaps nodes then run through all the child nodes to make sure collisions arent missed, this will show up all 
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodes[i].FindPossibleCollisions(returnObjects, pEnt);
                    }
                }
            }
            //As the method may get called recursively down through the nodes the list has the entities from each node added to returnobjects instead of returned at each call. this means the list gets built up as it goes through each child node
            returnObjects.AddRange(EntityList);
        }
    }

}

