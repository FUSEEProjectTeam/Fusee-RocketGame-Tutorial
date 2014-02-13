using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson04
{
    public class RocketGame : RenderCanvas
    {
        private GameEntity _gameEntity;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(1, 1, 1, 1);

            _gameEntity = new GameEntity("Assets/cube.obj.model", RC);
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            var mtxCam = float4x4.LookAt(0, 200, 500, 0, 0, 0, 0, 1, 0);

            _gameEntity.Render(mtxCam);

            Present();
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new RocketGame();
            app.Run();
        }

    }
}
