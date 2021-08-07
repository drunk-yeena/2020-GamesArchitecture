using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game
{
    class Camera
    {
        public float cameraRadius;
        public Matrix4 view, projection;
        public Vector3 cameraCurrentPosition, cameraOldPosition, cameraDirection, cameraUp;
        private Vector3 targetPosition;
        public Vector2 cameraCurrentPositionVector, cameraOldPositionVector, cameraDirectionalVector, cameraVectorNormal;

        public Camera()
        {
            cameraCurrentPosition = new Vector3(0.0f, 0.0f, 0.0f);
            cameraDirection = new Vector3(0.0f, 0.0f, -1.0f);
            cameraUp = new Vector3(0.0f, 0.1f, 0.0f);
            cameraRadius = 1.5f;
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), 1.0f, 0.1f, 100f);
        }

        public Camera(Vector3 cameraPos, Vector3 targetPos, float ratio, float near, float far)
        {
            cameraUp = new Vector3(0.0f, 0.1f, 0.0f);
            cameraCurrentPosition = cameraOldPosition = cameraPos;
            cameraDirection = targetPos-cameraPos;
            cameraDirection.Normalize();
            UpdateView();
            UpdateVectors();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), ratio, near, far);
        }

        public void MoveForward(float move)
        {
            cameraCurrentPosition.Y = 2;
            cameraOldPosition = cameraCurrentPosition;
            cameraCurrentPosition += move*cameraDirection;
            UpdateView();
        }

        public void Translate(Vector3 move)
        {
            cameraCurrentPosition += move;
            UpdateView();
            UpdateVectors();
        }

        public void RespawnCameraPosition(Vector3 position)
        {
            cameraCurrentPosition = cameraOldPosition = position;
            UpdateView();
            UpdateVectors();
        }

        public void RevertToOldPosition()
        {
            cameraCurrentPosition = cameraOldPosition;
            UpdateView();
            UpdateVectors();
        }


        public void RotateY(float angle)
        {
            cameraDirection = Matrix3.CreateRotationY(angle) * cameraDirection;
            UpdateView();
            UpdateVectors();
        }

        public void UpdateView()
        {
            targetPosition = cameraCurrentPosition + cameraDirection;
            view = Matrix4.LookAt(cameraCurrentPosition, targetPosition, cameraUp);
        }

        public void UpdateVectors()
        {
            cameraCurrentPositionVector = new Vector2(cameraCurrentPosition.X, cameraCurrentPosition.Z);
            cameraOldPositionVector = new Vector2(cameraOldPosition.X, cameraOldPosition.Z);
            cameraDirectionalVector = cameraCurrentPositionVector - cameraOldPositionVector;
            cameraVectorNormal = new Vector2(cameraDirectionalVector.Y, -(cameraDirectionalVector.X));
        }
    }
}
