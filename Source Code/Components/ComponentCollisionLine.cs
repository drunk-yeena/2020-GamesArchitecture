using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentCollisionLine : IComponent
    {
        Vector2 lineStartPoint;
        Vector2 lineEndPoint;
        Vector2 lineDirectionVector;
        Vector2 lineNormal;

        public ComponentCollisionLine(Vector2 lineStartPoint,Vector2 lineEndPoint)
        {
            this.lineStartPoint = lineStartPoint;
            this.lineEndPoint = lineEndPoint;

            lineDirectionVector = lineEndPoint - lineStartPoint;
            lineNormal = new Vector2(lineDirectionVector.Y, -(lineDirectionVector.X));
        }

        public Vector2 LineStartPoint
        {
            get { return lineStartPoint; }
            set { lineStartPoint = value; }
        }

        public Vector2 LineNormal
        {
            get { return lineNormal; }
            set { lineNormal = value; }
        }

        public Vector2 LineDirectionVector
        {
            get { return lineDirectionVector; }
            set { lineDirectionVector = value; }
        }

        public Vector2 LineEndPoint
        {
            get { return lineEndPoint; }
            set { lineEndPoint = value; }
        }


        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_LINE; }
        }
    }
}
