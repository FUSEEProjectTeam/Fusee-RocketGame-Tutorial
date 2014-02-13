using System;
using System.Collections.Generic;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson08
{
    public class GameWorld
    {
        private readonly RenderContext _rc;

        private readonly List<GameEntity> _furniture = new List<GameEntity>();
        private readonly List<Target> _targets = new List<Target>();

        private readonly Player _player; 

        private float4x4 _camMatrix;

        public GameWorld(RenderContext rc)
        {
            _rc = rc;

            _player = new Player("Assets/cube.obj.model", _rc);

            _furniture.Add(new GameEntity("Assets/cube.obj.model", _rc, -100, 0, -500));
            _furniture[0].SetShader(new float4(1, 0, 0, 1));
            _furniture[0].SetCorrectionMatrix(float4x4.Scale(0.5f));

            _furniture.Add(new GameEntity("Assets/cube.obj.model", _rc, 100, 0, -500, (float) Math.PI/4, (float) Math.PI/4));
            _furniture[1].SetShader(new float4(0, 1, 0, 1), "Assets/toon_generic_5_tex.png", new float4(0, 0, 0, 1), new float2(10, 10));

            _targets.Add(new Target("Assets/cube.obj.model", rc, 0, 0, -1000, (float) Math.PI/4, (float) Math.PI/4, (float) Math.PI/4));
            _targets[0].SetCorrectionMatrix(float4x4.Scale(0.5f));
            _targets.Add(new Target("Assets/cube.obj.model", rc, 0, 0, -3000, (float) Math.PI/4, (float) Math.PI/4, (float) Math.PI/4));
            _targets[1].SetCorrectionMatrix(float4x4.Scale(0.5f));
        }

        public void RenderAFrame()
        {
            _camMatrix = _player.GetCamMatrix();

            foreach (var gameEntity in _furniture)
            {
                gameEntity.Render(_camMatrix);
            }

            foreach (var target in _targets)
            {
                var distanceVector = _player.GetPositionVector() - target.GetPositionVector();
                if (distanceVector.Length < 500)
                {
                    target.SetActive();
                }
                else
                {
                    target.SetInactive();
                }
                target.Render(_camMatrix);
            }

            _player.Render(_camMatrix);
            _player.Move();
        }

        public void Resize()
        {}
    }
}
