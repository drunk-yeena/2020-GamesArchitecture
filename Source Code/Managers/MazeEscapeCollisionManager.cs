using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Managers
{
    class MazeEscapeCollisionManager : CollisionManager
    {
        string ProcessEntityName;
        string gameSceneInteraction;
        COLLISIONTYPE ProcessCollisionType;

        public override string ProcessCollisions()
        {
            gameSceneInteraction = "continue";

            foreach (Collision collision in collisionList)
            {
                if (collision.entity.Name.Contains("Wall"))
                {
                    ProcessEntityName = "Wall";
                }
                else
                {
                    ProcessEntityName = collision.entity.Name;
                }
                ProcessCollisionType = collision.collisionType;

                switch (ProcessEntityName)
                {
                    case "Drone":
                        gameSceneInteraction = "Lose Life";
                        collisionList.Clear();
                        return gameSceneInteraction;

                    case "Key 1":
                        gameSceneInteraction = "Key Collected 1";
                        collisionList.Clear();
                        return gameSceneInteraction;

                    case "Key 2":
                        gameSceneInteraction = "Key Collected 2";
                        collisionList.Clear();
                        return gameSceneInteraction;

                    case "Key 3":
                        gameSceneInteraction = "Key Collected 3";
                        collisionList.Clear();
                        return gameSceneInteraction;


                    case "Portal":
                        gameSceneInteraction = "Portal";
                        collisionList.Clear();
                        return gameSceneInteraction;

                    case "Wall":
                        gameSceneInteraction = "Wall";
                        collisionList.Clear();
                        return gameSceneInteraction;
                    
                    default:
                        throw new System.Exception("Collision occured with no process method");
                }
            }
            collisionList.Clear();
            gameSceneInteraction = "continue";
            return gameSceneInteraction;
        }

    }
}
