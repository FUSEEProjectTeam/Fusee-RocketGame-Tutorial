using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson01
{
    public class RocketGame : RenderCanvas
    {
        private Mesh _cubeMesh;
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(1, 1, 1, 1);

            _cubeMesh = MeshReader.LoadMesh(@"Assets/cube.obj.model");
            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            var mtxCam = float4x4.LookAt(0, 200, 500, 0, 0, 0, 0, 1, 0);

            RC.ModelView = mtxCam;
            RC.SetShader(_spColor);
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.Render(_cubeMesh);

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
