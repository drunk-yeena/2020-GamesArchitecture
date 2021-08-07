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

namespace OpenGL_Game.Systems
{
    class SystemCollisionCameraLine : ISystem
    {
        CollisionManager collisionManager;
        Camera camera;

        //The line being made is from the two wall points
        Vector2 LineStart_PlayerStart;
        Vector2 LineStart_PlayerEnd;
        double Dot_WallLine_Pos_1;
        double Dot_WallLine_Pos_2;
        double WallLine_Product;
        //The line being made is from the two player points
        Vector2 PlayerStart_LineStart;
        Vector2 PlayerStart_LineEnd;
        double Dot_PlayerLine_Pos_1;
        double Dot_PlayerLine_Pos_2;
        double PlayerLine_Product;

        const ComponentTypes MASK_LINE_COLL = ComponentTypes.COMPONENT_COLLISION_LINE;

        public SystemCollisionCameraLine(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public void Collision(Entity entity, ComponentCollisionLine collisionLine)
        {
            //WallLine Calculations
            LineStart_PlayerStart = camera.cameraOldPositionVector - collisionLine.LineStartPoint;
            LineStart_PlayerEnd = camera.cameraCurrentPositionVector - collisionLine.LineStartPoint;

            Dot_WallLine_Pos_1 = Vector2.Dot(collisionLine.LineNormal, LineStart_PlayerStart);
            Dot_WallLine_Pos_2 = Vector2.Dot(collisionLine.LineNormal, LineStart_PlayerEnd);
            WallLine_Product = Dot_WallLine_Pos_1 * Dot_WallLine_Pos_2;

            //PlayerLine Calculations
            PlayerStart_LineStart = collisionLine.LineStartPoint - camera.cameraCurrentPositionVector;
            PlayerStart_LineEnd = collisionLine.LineEndPoint - camera.cameraCurrentPositionVector;

            Dot_PlayerLine_Pos_1 = Vector2.Dot(camera.cameraVectorNormal, PlayerStart_LineStart);
            Dot_PlayerLine_Pos_2 = Vector2.Dot(camera.cameraVectorNormal, PlayerStart_LineEnd);
            PlayerLine_Product = Dot_PlayerLine_Pos_1 * Dot_PlayerLine_Pos_2;

            if (WallLine_Product < 0 && PlayerLine_Product < 0)
            {
                collisionManager.CollisionBetweenCamera(entity, COLLISIONTYPE.SPHERE_LINE);
            }

        }

        public void OnAction(Entity entity)
        {
            if((entity.Mask & MASK_LINE_COLL) == MASK_LINE_COLL)
            {
                List<IComponent> components = entity.Components;

                IComponent collisionComponent = components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_LINE;});
                ComponentCollisionLine collision = (ComponentCollisionLine)collisionComponent;

                Collision(entity, collision);
            }
        }



        public string Name
        {
            get { return "SystemCollisionCameraLine"; }
        }
    }
}
