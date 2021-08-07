using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        float collisionRadius;
        bool iscollidable;

        public ComponentCollisionSphere(float collisionRadius)
        {
            this.collisionRadius = collisionRadius;
            iscollidable = true;
        }

        public float CollisionRadius
        {
            get { return collisionRadius; }
            set { collisionRadius = value; }
        }

        public bool IsCollidable
        {
            get { return iscollidable; }
            set { iscollidable = value; }
        }

        public void TurnOffCollision()
        {
            iscollidable = false;
        }

        public void TurnOnCollision()
        {
            iscollidable = true;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SPHERE; }
        }
    }
}
