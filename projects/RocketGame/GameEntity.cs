﻿using System;
using System.IO;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame
{
    public class GameEntity
    {
        protected float4x4 Position;
        protected float3 Rotation;
        protected float3 NRotXV;
        protected float3 NRotYV;
        protected float3 NRotZV;
        protected float ScaleX = 1;
        protected float ScaleY = 1;
        protected float ScaleZ = 1;
        private float _speed = 0;
        protected float4x4 CorrectionMatrix;

        private readonly Mesh _mesh;

        private ShaderEffect _shaderEffect;
        private ITexture _iTexture1;
        private ITexture _iTexture2;
        private float4 _color = new float4(0.5f, 0.5f, 0.5f, 1);

        private readonly float4 _defaultLineColor = new float4(0, 0, 0, 1);
        private readonly float2 _defaultLineWidth = new float2(5, 5);

        private readonly RenderContext _rc;

        private MySerializer _ser;

        public GameEntity(String meshPath, RenderContext rc, float posX = 0, float posY = 0, float posZ = 0, float angX = 0, float angY = 0, float angZ = 0)
        {
            if (meshPath.Contains("protobuf"))
            {
                _ser = new MySerializer();
                using (var file = File.OpenRead(meshPath))
                {
                    _mesh = _ser.Deserialize(file, null, typeof(Mesh)) as Mesh;
                }
            }
            else
            {
                _mesh = MeshReader.LoadMesh(meshPath);
            }
            _rc = rc;

            Position = float4x4.Identity;
            Position = float4x4.CreateRotationX(angX) *
                        float4x4.CreateRotationY(angY) *
                        float4x4.CreateRotationZ(angZ) *
                        float4x4.CreateTranslation(posX, posY, posZ);

            CorrectionMatrix = float4x4.Identity;

            UpdateNVectors();

            SetDiffuseShader();
        }

        public GameEntity(String meshPath, RenderContext rc, float3 posxyz, float3 angxyz)
        {
            _mesh = MeshReader.LoadMesh(meshPath);
            _rc = rc;

            Position = float4x4.Identity;
            Position = float4x4.CreateRotationX(angxyz.x) *
                        float4x4.CreateRotationY(angxyz.y) *
                        float4x4.CreateRotationZ(angxyz.z) *
                        float4x4.CreateTranslation(posxyz.x, posxyz.y, posxyz.z);

            CorrectionMatrix = float4x4.Identity;

            UpdateNVectors();
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float4x4 GetPosition()
        {
            return Position;
        }

        public void SetPosition(float4x4 position)
        {
            Position = position;
        }

        public void SetPosition(float3 position)
        {
            Position.Row3 = new float4(position, 1);
        }

        public void SetScale(float scale)
        {
            ScaleX = scale;
            ScaleY = scale;
            ScaleZ = scale;
        }

        public void SetScale(float scaleX, float scaleY, float scaleZ)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
        }

        public void SetCorrectionMatrix(float4x4 corrMatrix)
        {
            CorrectionMatrix = corrMatrix;
        }

        public float3 GetPositionVector()
        {
            return new float3(Position.M41, Position.M42, Position.M43);
        }

        public void SetShader(float4 color)
        {
            _color = color;
            SetDiffuseShader();
        }

        public void SetShader(float r,float g, float b, float a=1)
        {
            _color = new float4(r,g,b,a);
            SetDiffuseShader();
        }

        public void SetShader(String texturePath)
        {
            SetTextureShader(texturePath);
        }
        public void SetShader(float4 baseColor, String colorMapTexturePath, float4 lineColor, float2 lineWidth)
        {
            SetTextureShader(baseColor, colorMapTexturePath, lineColor, lineWidth);
        }
        public void SetShader(String baseTexturePath, String colorMapTexturePath, float4 lineColor, float2 lineWidth)
        {
            SetTextureShader(baseTexturePath, colorMapTexturePath, lineColor, lineWidth);
        }

        public void Render(float4x4 camMatrix)
        {
            _rc.ModelView = CorrectionMatrix * float4x4.Scale(ScaleX, ScaleY, ScaleZ) * Position * camMatrix;

            _shaderEffect.RenderMesh(_mesh);
        }

        protected void UpdateNVectors()
        {
            NRotXV = float3.Normalize(new float3(Position.Row0));
            NRotYV = float3.Normalize(new float3(Position.Row1));
            NRotZV = float3.Normalize(new float3(Position.Row2));
        }

        protected void SetDiffuseShader()
        {
            _shaderEffect = Shader.GetShaderEffect(_rc, _color);
        }

        protected void SetTextureShader(String texturePath)
        {
            var imgData = _rc.LoadImage(texturePath);
            _iTexture1 = _rc.CreateTexture(imgData);

            _shaderEffect = Shader.GetShaderEffect(_rc, _iTexture1);
        }
        protected void SetTextureShader(float4 baseColor, String texturePath, float4 lineColor, float2 lineWidth)
        {
            var imgData = _rc.LoadImage(texturePath);
            _iTexture1 = _rc.CreateTexture(imgData);

            _shaderEffect = Shader.GetShaderEffect(_rc, baseColor, _iTexture1, lineColor, lineWidth);
        }
        protected void SetTextureShader(String texturePath1, String texturePath2, float4 lineColor, float2 lineWidth)
        {
            var imgData = _rc.LoadImage(texturePath1);
            _iTexture1 = _rc.CreateTexture(imgData);
            imgData = _rc.LoadImage(texturePath2);
            _iTexture2 = _rc.CreateTexture(imgData);


            _shaderEffect = Shader.GetShaderEffect(_rc, _iTexture1, _iTexture2, lineColor, lineWidth);
        }
    }
}
