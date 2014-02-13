using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson02
{
    public class RocketGame : RenderCanvas
    {
        private Mesh _cubeMesh;
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;

        private float _cubeXPos, _cubeYPos, _cubeXRot, _cubeYRot;

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

            _cubeYRot += Input.Instance.GetAxis(InputAxis.MouseX);
            _cubeXRot -= Input.Instance.GetAxis(InputAxis.MouseY);

            if (Input.Instance.IsKey(KeyCodes.Right))
                _cubeXPos += 1;
            if (Input.Instance.IsKey(KeyCodes.Left))
                _cubeXPos -= 1;
            if (Input.Instance.IsKey(KeyCodes.Up))
                _cubeYPos += 1;
            if (Input.Instance.IsKey(KeyCodes.Down))
                _cubeYPos -= 1;

            var mtxRot = float4x4.CreateRotationY(_cubeYRot) * float4x4.CreateRotationX(_cubeXRot);
            var mtxPos = float4x4.CreateTranslation(_cubeXPos, _cubeYPos, 0);
            var mtxCam = float4x4.LookAt(0, 200, 500, 0, 0, 0, 0, 1, 0);

            RC.ModelView = mtxPos * mtxRot * mtxCam;
            RC.SetShader(_spColor);
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.Render(_cubeMesh);

            RC.ModelView = mtxCam;
            RC.SetShader(_spColor);
            RC.SetShaderParam(_colorParam, new float4(0.8f, 0.5f, 0, 1));
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
