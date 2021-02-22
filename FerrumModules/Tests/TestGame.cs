﻿using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FerrumModules.Engine;

namespace FerrumModules.Tests
{
    public class TestGame : FerrumContext
    {
        public TestGame() : base(640, 480) { }

        public enum RenderLayers { TileLayer, EnemyLayer, PlayerLayer }

        public override void InitGame()
        {
            base.InitGame();
            var marioFrames = new List<int>() { 0, 1, 2, 3, 4 };
            var marioAnim = new Animation(marioFrames, 6);
            
            var marioTexture = Assets.Textures["mario"];

            var marioPhys = CurrentScene.AddChild(new RigidBody());

            var mario2Phys = CurrentScene.AddChild(new RigidBody());

            var mario = marioPhys.AddChild(new AnimatedSprite(marioTexture, 16, 16, marioAnim));
            mario.SetRenderLayer(RenderLayers.PlayerLayer);
            var mario2 = mario.AddChild(new StaticSprite(marioTexture, 16, 16, 8));
            var mario3 = mario2.AddChild(new StaticSprite(marioTexture, 16, 16, 5));
            mario2.SetRenderLayer(RenderLayers.EnemyLayer);

            mario.Name = "Mario";
            marioPhys.Name = "MarioPhys";
            mario2.Name = "Koopa";
            mario2Phys.Name = "KoopaPhys";

            var testTileSet = CurrentScene.AddChild(new TileMap("big"));
            testTileSet.SetRenderLayer(RenderLayers.TileLayer);
            testTileSet.Name = "TileMap";

            var testCamera = marioPhys.AddChild(new Camera());
            //testCamera.Centered = false;
            CurrentScene.Camera = testCamera;
            testCamera.Zoom = 2;
            marioPhys.PositionOffset = new Vector2(16, 0);
            testCamera.AngleOffset = Rotation.PI / 8;

            marioPhys.Velocity = new Vector2(50, 25);
            marioPhys.ScaleOffset = new Vector2(1, 1);

            mario2.PositionOffset = new Vector2(16, 0);
            mario3.PositionOffset = new Vector2(0, 16);
            //mario2.ScaleOffset = new Vector2(0.5f, 0.5f);

            marioPhys.GlobalPosition = new Vector2(32, -64);
            mario2Phys.GlobalPosition = new Vector2(0, -32);

            Input.AddAction("move_left", Keys.Left, Buttons.LeftThumbstickLeft);
            Input.AddAction("move_right", Keys.Right, Buttons.LeftThumbstickRight);
            Input.AddAction("move_up", Keys.Up, Buttons.LeftThumbstickUp);
            Input.AddAction("move_down", Keys.Down, Buttons.LeftThumbstickDown);
        }

        public override void UpdateGame(float delta)
        {
            base.UpdateGame(delta);
            var player = CurrentScene.GetByName<RigidBody>("MarioPhys");
            var tileSet = CurrentScene.GetByName<Entity>("TileMap");
            //CurrentScene.Camera.Zoom += 0.01f;
            //CurrentScene.Camera.PositionOffset = new Vector2(-16, -16);
            //CurrentScene.Camera.AngleOffset += (float)Math.PI / 600;

            if (Input.IsActionPressed("move_right"))
                player.Velocity = new Vector2(player.Velocity.X + 1.0f, player.Velocity.Y);
            else if (Input.IsActionPressed("move_left"))
                player.Velocity = new Vector2(player.Velocity.X - 1.0f, player.Velocity.Y);

            if (Input.IsActionJustPressed("move_up"))
                player.Velocity = new Vector2(player.Velocity.X, -100);
        }
    }
}
