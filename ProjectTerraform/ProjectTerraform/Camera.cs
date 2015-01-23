using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class Camera
    {
        public float x, y;
        public float dx, dy;
        public int onx, ony;
        public Vector2 mouse;
        public int mouseWheel;
        public int mouseWheelOld;

        public Camera()
        {
            x = 500.0f;
            y = 500.0f;
        }
    }
}
