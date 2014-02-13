using System;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson06
{
    public class GameEntity
    {
        private readonly Mesh _mesh;

        private readonly RenderContext _rc;

        protected float4x4 Position;
        protected float4x4 CorrectionMatrix = float4x4.Identity;

        private readonly ShaderProgram _shaderProgram;
        private readonly IShaderParam _shaderParam;
        private float4 _color = new float4(0.5f, 0.5f, 0.5f, 1);

        public GameEntity(String meshPath, RenderContext rc, float posX = 0, float posY = 0, float posZ = 0, float angX = 0, float angY = 0, float angZ = 0)
        {
            _mesh = MeshReader.LoadMesh(meshPath);
            _rc = rc;

            Position = float4x4.CreateRotationX(angX) *
                        float4x4.CreateRotationY(angY) *
                        float4x4.CreateRotationZ(angZ) *
                        float4x4.CreateTranslation(posX, posY, posZ);

            _shaderProgram = MoreShaders.GetDiffuseColorShader(rc);
            _shaderParam = _shaderProgram.GetShaderParam("color");
        }

        public void Render(float4x4 camMatrix)
        {
            _rc.ModelView = CorrectionMatrix * Position * camMatrix;
            _rc.SetShader(_shaderProgram);
            _rc.SetShaderParam(_shaderParam, _color);
            _rc.Render(_mesh);
        }

        public float4x4 GetPosition()
        {
            return Position;
        }
        public float3 GetPositionVector()
        {
            return new float3(Position.M41, Position.M42, Position.M43);
        }

        public void SetPosition(float4x4 position)
        {
            Position = position;
        }

        public void SetCorrectionMatrix(float4x4 corrMatrix)
        {
            CorrectionMatrix = corrMatrix;
        }

        public float4 GetColor()
        {
            return _color;
        }

        public void SetColor(float4 color)
        {
            _color = color;
        }
        public void SetColor(float r, float g, float b, float a)
        {
            _color.x = r;
            _color.y = g;
            _color.z = b;
            _color.w = a;
        }
    }
}
