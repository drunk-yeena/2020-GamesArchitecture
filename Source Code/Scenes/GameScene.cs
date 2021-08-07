using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public string gameInteractionString;
        public static float dt = 0;
        int keysLeft = 0;
        int playerLives = 3;
        EntityManager entityManager;
        SystemManager systemManager;
        MazeEscapeCollisionManager collisionManager;

        public Camera camera;

        public static GameScene gameInstance;

        bool[] keysPressed = new bool[255];

        //Setting Up Blank Entities
        Entity DroneHazard;
        Entity Key_1;
        Entity Key_2;
        Entity Key_3;
        Entity Portal;
        Entity Map;
        

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            collisionManager = new MazeEscapeCollisionManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(-50, 2, 10), new Vector3(0, 0, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);

            CreateEntities();
            CreateSystems();
            CreateWalls();

        }

        private void CreateEntities()
        {
            DroneHazard = new Entity("Drone");
            DroneHazard.AddComponent(new ComponentPosition(-27.5f, 1.0f, 27.5f));
            DroneHazard.AddComponent(new ComponentGeometry("Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            DroneHazard.AddComponent(new ComponentCollisionSphere(2.5f));
            DroneHazard.AddComponent(new ComponentVelocity(new Vector3(0, 0, 5)));
            entityManager.AddEntity(DroneHazard);

            Key_1 = new Entity("Key 1");
            Key_1.AddComponent(new ComponentPosition(-7.5f, 0.0f, 7.5f));
            Key_1.AddComponent(new ComponentGeometry("Geometry/Keys/moon_key1.obj"));
            Key_1.AddComponent(new ComponentCollisionSphere(2.5f));
            Key_1.AddComponent(new ComponentAudio("Audio/buzz.wav"));
            entityManager.AddEntity(Key_1);
            keysLeft++;

            Key_2 = new Entity("Key 2");
            Key_2.AddComponent(new ComponentPosition(-7.5f, 0.0f, 47.5f));
            Key_2.AddComponent(new ComponentGeometry("Geometry/Keys/moon_key2.obj"));
            Key_2.AddComponent(new ComponentCollisionSphere(2.5f));
            Key_2.AddComponent(new ComponentAudio("Audio/buzz2.wav"));
            entityManager.AddEntity(Key_2);
            keysLeft++;

            Key_3 = new Entity("Key 3");
            Key_3.AddComponent(new ComponentPosition(-47.5f, 0.0f, 47.5f));
            Key_3.AddComponent(new ComponentGeometry("Geometry/Keys/moon_key3.obj"));
            Key_3.AddComponent(new ComponentCollisionSphere(2.5f));
            Key_3.AddComponent(new ComponentAudio("Audio/buzz3.wav"));
            entityManager.AddEntity(Key_3);
            keysLeft++;

            Portal = new Entity("Portal");
            Portal.AddComponent(new ComponentPosition(-47.5f, 0.0f, 7.5f));
            Portal.AddComponent(new ComponentGeometry("Geometry/Portal/moon_portal.obj"));
            Portal.AddComponent(new ComponentCollisionSphere(2.5f));
            entityManager.AddEntity(Portal);
            IComponent collisionComponent = Portal.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE; });
            ComponentCollisionSphere collision = (ComponentCollisionSphere)collisionComponent;
            collision.TurnOffCollision();

            Map = new Entity("Map");
            Map.AddComponent(new ComponentPosition(0.0f, -0.5f, 0.0f));
            Map.AddComponent(new ComponentGeometry("Geometry/Maze/maze.obj"));
            entityManager.AddEntity(Map);
        }

        private void CreateWalls()
        {
            Entity WallGeneric;

            //Room One
            WallGeneric = new Entity("Wall 1-1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 0), new Vector2(-15, 0)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 0), new Vector2(0, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 15), new Vector2(-5, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 15), new Vector2(-10, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 10), new Vector2(-15, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-6");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 0), new Vector2(-15, 5)));
            entityManager.AddEntity(WallGeneric);

            //Room Connector - 1-2
            WallGeneric = new Entity("Wall 1-2 1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-5,15), new Vector2(-5, 40)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-2 2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-10, 15), new Vector2(-10, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-2 3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 0), new Vector2(0, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-2 4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-20, 25), new Vector2(-10, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 1-2 5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-20, 30), new Vector2(-10, 30)));
            entityManager.AddEntity(WallGeneric);

            //Room Two
            WallGeneric = new Entity("Wall 2-1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-5, 40), new Vector2(0, 40)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 2-2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 40), new Vector2(0, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 2-3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 55), new Vector2(0, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 2-4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 50), new Vector2(-15, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 2-5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 40), new Vector2(-15, 45)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 2-6");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-15, 40), new Vector2(-10, 40)));
            entityManager.AddEntity(WallGeneric);

            //Room Connector - 3-2
            WallGeneric = new Entity("Wall 3-2 1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 50), new Vector2(-15, 50)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-2 2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 45), new Vector2(-30, 45)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-2 3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-25, 45), new Vector2(-15, 45)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-2 4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-30, 35), new Vector2(-30, 45)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-2 5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-25, 35), new Vector2(-25, 45)));
            entityManager.AddEntity(WallGeneric);

            //Room Three
            WallGeneric = new Entity("Wall 3-1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 40), new Vector2(-40, 40)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 40), new Vector2(-40, 45)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 50), new Vector2(-40, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-55, 55), new Vector2(-40, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-55, 40), new Vector2(-55, 55)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 3-6");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 0), new Vector2(0, 15)));
            entityManager.AddEntity(WallGeneric);

            //Room Connector - 4-3
            WallGeneric = new Entity("Wall 4-3 1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-50, 15), new Vector2(-50, 40)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-3 2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 15), new Vector2(-45, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-3 3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 25), new Vector2(-45, 30)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-3 4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 25), new Vector2(-35, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-3 5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 30), new Vector2(-35, 30)));
            entityManager.AddEntity(WallGeneric);

            //Room Four
            WallGeneric = new Entity("Wall 4-1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-55, 0), new Vector2(-40, 0)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 0), new Vector2(-40, 5)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 10), new Vector2(-40, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-45, 15), new Vector2(-40, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-55, 15), new Vector2(-50, 15)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-6");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-55, 0), new Vector2(-55, 15)));
            entityManager.AddEntity(WallGeneric);

            //Room Connector - 4-1
            WallGeneric = new Entity("Wall 4-1 1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 5), new Vector2(-15, 5)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-1 2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-40, 10), new Vector2(-30, 10)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-1 3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-25, 10), new Vector2(-15, 10)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-1 4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-30, 10), new Vector2(-30, 20)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 4-1 5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(0, 0), new Vector2(-15, 0)));
            entityManager.AddEntity(WallGeneric);

            //Room Five
            WallGeneric = new Entity("Wall 5-1");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-35, 20), new Vector2(-30, 20)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-2");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-25, 20), new Vector2(-20, 20)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-3");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-35, 20), new Vector2(-35, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-4");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-20, 20), new Vector2(-20, 25)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-5");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-35, 35), new Vector2(-30, 35)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-6");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-25, 35), new Vector2(-20, 35)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-7");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-35, 30), new Vector2(-35, 35)));
            entityManager.AddEntity(WallGeneric);

            WallGeneric = new Entity("Wall 5-8");
            WallGeneric.AddComponent(new ComponentCollisionLine(new Vector2(-20, 30), new Vector2(-20, 35)));
            entityManager.AddEntity(WallGeneric);
        }

        private void OpenPortal()
        {
            IComponent collisionComponent = Portal.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE; });
            ComponentCollisionSphere collision = (ComponentCollisionSphere)collisionComponent;
            collision.TurnOnCollision();
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionCameraSphere(collisionManager,camera);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionCameraLine(collisionManager, camera);
            systemManager.AddSystem(newSystem);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            if (playerLives <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

            gameInteractionString = "";
            dt = (float)e.Time;
            System.Console.WriteLine("fps=" + (int)(1.0/dt));
            System.Console.WriteLine(camera.cameraCurrentPosition.ToString());

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            gameInteractionString = collisionManager.ProcessCollisions();
            Game_Interactions(gameInteractionString);

            IComponent velocityComponentDrone = DroneHazard.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY; });
            ComponentVelocity velocity_Drone = (ComponentVelocity)velocityComponentDrone;
            IComponent positionComponentDrone = DroneHazard.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_POSITION; });
            ComponentPosition position_Drone = (ComponentPosition)positionComponentDrone;
            if(position_Drone.Position.Z >= 50 || position_Drone.Position.Z <= 7)
            {
                velocity_Drone.Velocity = -(velocity_Drone.Velocity);
            }


            //Camera Control
            if (keysPressed[(char)Key.Up] ^ keysPressed[(char)Key.W])
            {
                camera.MoveForward(5.0f * dt);
            }
            if (keysPressed[(char)Key.Down] ^ keysPressed[(char)Key.S])
            {
                camera.MoveForward(-5.0f * dt);
            }
            if (keysPressed[(char)Key.Left] ^ keysPressed[(char)Key.A])
            {
                camera.RotateY(-0.01f);
            }
            if (keysPressed[(char)Key.Right] ^ keysPressed[(char)Key.D])
            {
                camera.RotateY(0.01f);
            }
            if (keysPressed[(char)Key.M])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
            }
            if (keysPressed[(char)Key.O])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), ("Lives: " + playerLives + "\nKeys Left: " + keysLeft), 18, StringAlignment.Near, Color.White);
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;
            ResourceManager.RemoveAllAssets();
        }

        public void Game_Interactions(string gameInteractions)
        {
            switch (gameInteractions)
            {
                case "Lose Life":
                    playerLives -= 1;
                    if (playerLives <= 0)
                    {
                        sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
                    }
                    camera.RespawnCameraPosition(new Vector3(-50, 2, 10));
                    break;

                case "Key Collected 1":
                    keysLeft -= 1;
                    if (keysLeft <= 0)
                    {
                        OpenPortal();
                    }
                    IComponent audioComponent_Key_1 = Key_1.Components.Find(delegate (IComponent component){ return component.ComponentType == ComponentTypes.COMPONENT_AUDIO; });
                    ComponentAudio audio_Key_1 = (ComponentAudio)audioComponent_Key_1;
                    audio_Key_1.PlayAudio();
                    IComponent collisionComponent_Key_1 = Key_1.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE; });
                    ComponentCollisionSphere collision_Key_1 = (ComponentCollisionSphere)collisionComponent_Key_1;
                    collision_Key_1.TurnOffCollision();
                    IComponent geometryComponent_Key_1 = Key_1.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY; });
                    Geometry geometry_Key_1 = ((ComponentGeometry)geometryComponent_Key_1).Geometry();
                    geometry_Key_1.RemoveGeometry();
                    break;

                case "Key Collected 2":
                    keysLeft -= 1;
                    if (keysLeft <= 0)
                    {
                        OpenPortal();
                    }
                    IComponent audioComponent_Key_2 = Key_2.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_AUDIO; });
                    ComponentAudio audio_Key_2 = (ComponentAudio)audioComponent_Key_2;
                    audio_Key_2.PlayAudio();
                    IComponent collisionComponent_Key_2 = Key_2.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE; });
                    ComponentCollisionSphere collision_Key_2 = (ComponentCollisionSphere)collisionComponent_Key_2;
                    collision_Key_2.TurnOffCollision();
                    IComponent geometryComponent_Key_2 = Key_2.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY; });
                    Geometry geometry_Key_2 = ((ComponentGeometry)geometryComponent_Key_2).Geometry();
                    geometry_Key_2.RemoveGeometry();
                    break;

                case "Key Collected 3":
                    keysLeft -= 1;
                    if (keysLeft <= 0)
                    {
                        OpenPortal();
                    }
                    IComponent audioComponent_Key_3 = Key_3.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_AUDIO; });
                    ComponentAudio audio_Key_3 = (ComponentAudio)audioComponent_Key_3;
                    audio_Key_3.PlayAudio();
                    IComponent collisionComponent_Key_3 = Key_3.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE; });
                    ComponentCollisionSphere collision_Key_3 = (ComponentCollisionSphere)collisionComponent_Key_3;
                    collision_Key_3.TurnOffCollision();
                    IComponent geometryComponent_Key_3 = Key_3.Components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY; });
                    Geometry geometry_Key_3 = ((ComponentGeometry)geometryComponent_Key_3).Geometry();
                    geometry_Key_3.RemoveGeometry();
                    break;


                case "Portal":
                    sceneManager.ChangeScene(SceneTypes.SCENE_GAME_WIN);
                    break;

                case "Wall":
                    System.Console.WriteLine("Collision At: " + camera.cameraCurrentPosition.ToString());
                    camera.MoveForward(-20.0f * dt);
                    break;

                case "continue":
                    break;

                default:
                    throw new Exception("Game Interaction String Error - Word Found Not Switch cased");
            }
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }
    }
}
