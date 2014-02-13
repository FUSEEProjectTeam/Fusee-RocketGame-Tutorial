using System.Collections.Generic;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson06
{
    public class GameWorld
    {
        private readonly RenderContext _rc;

        private readonly List<GameEntity> _furniture = new List<GameEntity>();

        private readonly Player _player; 

        private float4x4 _camMatrix;

        public GameWorld(RenderContext rc)
        {
            _rc = rc;

            _player = new Player("Assets/cube.obj.model", _rc);

            _furniture.Add(new GameEntity("Assets/cube.obj.model", _rc));
            _furniture[0].SetColor(1, 0, 0, 1);
            _furniture[0].SetCorrectionMatrix(float4x4.Scale(0.5f));

            _furniture.Add(new GameEntity("Assets/cube.obj.model", _rc, 0, 0, -100));
            _furniture[1].SetColor(0, 1, 0, 1);
        }

        public void RenderAFrame()
        {
            _camMatrix = _player.GetCamMatrix();

            foreach (var gameEntity in _furniture)
            {
                gameEntity.Render(_camMatrix);
            }

            _player.Render(_camMatrix);
            _player.Move();
        }

        public void Resize()
        {}
    }
}
