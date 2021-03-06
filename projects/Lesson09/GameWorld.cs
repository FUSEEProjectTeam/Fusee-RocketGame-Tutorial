﻿using System;
using System.Collections.Generic;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.RocketGame.Lesson09
{
    public class GameWorld
    {
        private readonly RenderContext _rc;

        private readonly List<GameEntity> _furniture = new List<GameEntity>();
        private readonly List<Target> _targets = new List<Target>();

        private readonly Player _player; 

        private float4x4 _camMatrix;

        private int _currentGameState;
        private int _lastGameState;

        private int _curScore;
        private int _maxScore;

        public GameWorld(RenderContext rc)
        {
            _rc = rc;

            _currentGameState = (int) GameState.Running;

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

            _maxScore = _targets.Count;
        }

        public void RenderAFrame()
        {
            if (_currentGameState != _lastGameState)
            {
                switch (_currentGameState)
                {
                    case (int)GameState.StartScreen:
                        _camMatrix = float4x4.CreateTranslation(0, 0, 0);
                        break;

                    case (int)GameState.Running:
                        _player.SetPosition(float4x4.Identity);
                        break;

                    case (int)GameState.GameOver:
                        foreach (var target in _targets)
                        {
                            target.SetInactive();
                        }
                        _camMatrix = float4x4.CreateTranslation(0, 0, 0);
                        _player.Speed = 0;
                        break;
                }
                _lastGameState = _currentGameState;
            }
            
            if (_currentGameState == (int) GameState.Running)
            {
                _player.Move();
                _camMatrix = _player.GetCamMatrix();
                _player.Render(_camMatrix);

                int activeGoalCount = 0;
                foreach (var target in _targets)
                {
                    var distanceVector = _player.GetPositionVector() - target.GetPositionVector();
                    if (distanceVector.Length < 500)
                    {
                        target.SetActive();
                    }
                    if (target.GetStatus())
                    {
                        activeGoalCount++;
                    }
                    target.Render(_camMatrix);
                }
                _curScore = activeGoalCount;

                if (_curScore == _maxScore)
                {
                    _currentGameState = (int) GameState.GameOver;
                }

            }
            else
            {
                _camMatrix *= float4x4.CreateRotationY(0.02f);
            }
            
            foreach (var gameEntity in _furniture)
            {
                gameEntity.Render(_camMatrix);
            }
        }

        public void Resize()
        {}
    }

    public enum GameState
    {
        StartScreen,
        Running,
        GameOver
    }
}
