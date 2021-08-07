using System.Collections.Generic;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        LINE_LINE,
        SPHERE_LINE
    }

    struct Collision
    {
        public Entity entity;
        public COLLISIONTYPE collisionType;
    }

    abstract class CollisionManager
    {
        protected List<Collision> collisionList = new List<Collision>();

        public CollisionManager()
        {

        }

        public void ClearList()
        {
            collisionList.Clear();
        }

        public void CollisionBetweenCamera(Entity entity, COLLISIONTYPE collisionType)
        {
            foreach (Collision collide in collisionList)
            {
                if (collide.entity == entity)
                    return;
            }

            Collision collision;
            collision.entity = entity;
            collision.collisionType = collisionType;
            collisionList.Add(collision);
        }

        public abstract string ProcessCollisions();
    }
}
