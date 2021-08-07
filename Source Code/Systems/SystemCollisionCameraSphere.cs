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
    class SystemCollisionCameraSphere : ISystem
    {
        CollisionManager collisionManager;
        Camera camera;

        const ComponentTypes MASK_POS_COLLSPHERE = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);

        public SystemCollisionCameraSphere(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public void Collision(Entity entity, ComponentPosition position, ComponentCollisionSphere collisionSphere)
        {
            if(collisionSphere.IsCollidable == true)
            {
                if ((position.Position - camera.cameraCurrentPosition).Length < (collisionSphere.CollisionRadius + camera.cameraRadius))
                {
                    collisionManager.CollisionBetweenCamera(entity, COLLISIONTYPE.SPHERE_SPHERE);
                }
            }
        }

        public void OnAction(Entity entity)
        {
            if((entity.Mask & MASK_POS_COLLSPHERE) == MASK_POS_COLLSPHERE)
            {
                List<IComponent> components = entity.Components;

                IComponent collisionComponent = components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;});
                ComponentCollisionSphere collision = (ComponentCollisionSphere)collisionComponent;

                IComponent positionComponent = components.Find(delegate (IComponent component) { return component.ComponentType == ComponentTypes.COMPONENT_POSITION; });
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(entity, position, collision);
            }
        }

        public string Name
        {
            get { return "SystemCollisionCameraSphere"; }
        }
    }
}
