using F7s.Engine;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Populators;
using F7s.Modell.Terrains;
using F7s.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Physics;
using Stride.Rendering;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System;

namespace F7s.Mains {

    public class MainSync : SyncScript {

        private readonly bool simulationStarted = false;

        private Populator populator;

        public static IGame game { get; private set; }

        private static MainSync instance;

        public static CameraComponent Camera { get; private set; }
        public static Entity CameraEntity { get; private set; }
        public static Entity CameraParentEntity { get; private set; }

        public override void Start () {

            if (instance != null) {
                throw new System.Exception();
            } else {
                instance = this;
            }

            Console.WriteLine("Starting.");

            Entity.Add(new MainAsync());

            {
                CameraParentEntity = new Entity("Camera Yawer");
                CameraParentEntity.Scene = Entity.Scene;

                CameraEntity = new Entity("Camera Pitcher");
                CameraParentEntity.AddChild(CameraEntity);

                Camera = new CameraComponent();
                CameraEntity.Add(Camera);


            }

            game = Game;
            Assert.IsNotNull(game);

            {
                Terrain terrain = new Terrain("Tiny Planet", 1, 2, Entity, new PlanetologyData(1, 1, 1, true, true, 5)); // TODO: Reactivate after child's play.
                Mesch terrainMesch = terrain.Render(GraphicsDevice, Stride.Graphics.GraphicsResourceUsage.Default);
                Assert.IsNotNull(terrainMesch);
            }
            populator = new PerduePopulator();

            {
                // UI Test
                Assert.IsNotNull(Game);
                Assert.IsNotNull(Game.Content);
                SpriteFont? font = null;
                font = Game.Content.Load<SpriteFont>("StrideDefaultFont");
                var canvas = new Canvas {
                    Width = 300,
                    Height = 100,
                    BackgroundColor = new Color(248, 177, 149, 100),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                };

                canvas.Children.Add(new TextBlock {
                    Text = "Let's see about this.",
                    TextColor = Color.White,
                    Font = font,
                    TextSize = 24,
                    Margin = new Thickness(3, 3, 3, 0),
                });

                var uiEntity = new Entity
                {
                    new UIComponent
                    {
                        Page = new UIPage { RootElement = canvas },
                        RenderGroup = RenderGroup.Group31
                    }
                };

                uiEntity.Scene = Entity.Scene;
            }
        }

        private void PrePhysicsUpdate (Simulation sender, float tick) {
            Frogram.PrePhysicsUpdateAll(sender, tick);
        }

        private void PostPhysicsUpdate (Simulation sender, float tick) {
            Frogram.PostPhysicsUpdateAll(sender, tick);
        }

        private void InitializeSimulationUpdateListeners () {
            if (simulationStarted) {
                return;
            }
            Simulation simulation = this.GetSimulation();
            if (simulation != null) {
                simulation.PreTick += PrePhysicsUpdate;
                simulation.PostTick += PostPhysicsUpdate;
            }
        }

        public override void Update () {
            InitializeSimulationUpdateListeners();

            Frogram.UpdateAll();

            double deltaTime = Zeit.DeltaTimeSeconds();
            Origin.Update(deltaTime);

            Player.Update(deltaTime);
            Kamera.Update(deltaTime);

            if (!Zeit.Paused) {
                GameEntity.OnEngineUpdate(1.0, false);
                populator?.Update(deltaTime);
            }
        }
    }
}
