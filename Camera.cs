using Microsoft.Xna.Framework;

namespace Final_Project
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Rectangle target)
        {
            var position = Matrix.CreateTranslation(
              -target.X - (target.Width / 2),
              -target.Y - (target.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                800 / 2,
                600 / 2,
                0);

            Transform = position * offset;
        }

    }
}
