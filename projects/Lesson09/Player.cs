using System;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson09
{
    public class Player : GameEntity
    {
        private float3 _rotation;
        private float3 _rotationSpeed = new float3(1, 1, 1);
        private float _speed;
        private float _speedModifier = 1;
        private float SpeedMax = 30f;

        private float3 _oldPos;

        private float3 _nRotXV;
        private float3 _nRotYV;
        private float3 _nRotZV;

        public Player(String meshPath, RenderContext rc, float posX = 0, float posY = 0, float posZ = 0, float angX = 0, float angY = 0, float angZ = 0) : base(meshPath, rc, posX, posY, posZ, angX, angY, angZ)
        {
            UpdateNRVectors();
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public void Move()
        {
            if (Input.Instance.IsKey(KeyCodes.D))
                _rotation.x = _rotationSpeed.x * (float) Time.Instance.DeltaTime;
            else if (Input.Instance.IsKey(KeyCodes.A))
                _rotation.x = -_rotationSpeed.x * (float)Time.Instance.DeltaTime;
            else
                _rotation.x = 0;

            if (Input.Instance.IsKey(KeyCodes.W))
                _rotation.y = _rotationSpeed.y * (float)Time.Instance.DeltaTime;
            else if (Input.Instance.IsKey(KeyCodes.S))
                _rotation.y = -_rotationSpeed.x * (float)Time.Instance.DeltaTime;
            else
                _rotation.y = 0;

            if (Input.Instance.IsKey(KeyCodes.E))
                _rotation.z = _rotationSpeed.z * (float)Time.Instance.DeltaTime;
            else if (Input.Instance.IsKey(KeyCodes.Q))
                _rotation.z = -_rotationSpeed.z * (float)Time.Instance.DeltaTime;
            else
                _rotation.z = 0;

            if (Input.Instance.IsKey(KeyCodes.Up) || Input.Instance.IsKey(KeyCodes.Space))
            {
                _speed += _speedModifier * (float)Time.Instance.DeltaTime;
            }
            else
            {
                _speed -= _speedModifier * (float)Time.Instance.DeltaTime;
            }

            _speed = Clamp(_speed, 0.0f, SpeedMax);

            
            _oldPos.x = Position.Row3.x;
            _oldPos.y = Position.Row3.y;
            _oldPos.z = Position.Row3.z;

            Position *= float4x4.CreateTranslation(-_oldPos) *
                        float4x4.CreateFromAxisAngle(_nRotYV, -_rotation.x) *
                        float4x4.CreateFromAxisAngle(_nRotXV, -_rotation.y) *
                        float4x4.CreateFromAxisAngle(_nRotZV, -_rotation.z) *
                        float4x4.CreateTranslation(_oldPos) *
                        float4x4.CreateTranslation(_nRotZV * -_speed);

            UpdateNRVectors();
        }

        public float4x4 GetCamMatrix()
        {
            return float4x4.LookAt(Position.M41 + (_nRotZV.x * 1000),
                                   Position.M42 + (_nRotZV.y * 1000),
                                   Position.M43 + (_nRotZV.z * 1000),
                                   Position.M41,
                                   Position.M42,
                                   Position.M43,
                                   Position.M21,
                                   Position.M22,
                                   Position.M23)
                                   * float4x4.CreateTranslation(0, -300, 0);
        }

        private static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        private void UpdateNRVectors()
        {
            _nRotXV = float3.Normalize(new float3(Position.Row0));
            _nRotYV = float3.Normalize(new float3(Position.Row1));
            _nRotZV = float3.Normalize(new float3(Position.Row2));
        }

    }
}
