using F7s.Engine;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Populators;
using F7s.Modell.Terrains;
using F7s.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.CommunityToolkit.Engine;
using Stride.CommunityToolkit.Rendering.Compositing;
using Stride.CommunityToolkit.Rendering.ProceduralModels;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Physics;
using Stride.Rendering;
using Stride.Rendering.Lights;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace F7s.Mains {

    public class MainSync : SyncScript {

        private readonly bool simulationStarted = false;

        private Populator populator;

        public static Game game { get; private set; }

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

            System.Diagnostics.Debug.WriteLine("Starting " + this + " with game " + game + " in scene " + Entity.Scene + ".");


            {
                game = (Game) Game;
                Assert.IsNotNull(game);
            }

            {
                game.AddGraphicsCompositor().AddCleanUIStage();
                //game.Add3DCamera().Add3DCameraController();
                game.AddDirectionalLight();
                game.Add3DGround();
                game.AddProfiler();
                game.AddGroundGizmo(position: new Vector3(-5, 0.1f, -5), showAxisName: true);
            }

            Entity.Add(new MainAsync());

            {
                CameraParentEntity = new Entity("Camera Yawer");
                CameraParentEntity.Scene = Entity.Scene;

                CameraEntity = new Entity("Camera Pitcher");
                CameraParentEntity.AddChild(CameraEntity);

                Camera = new CameraComponent();
                CameraEntity.Add(Camera);
                Camera.Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();

                CameraEntity.Add3DCameraController();
            }

            {
                Entity lightEntity = new Entity("Light Entity");
                lightEntity.Scene = Entity.Scene;
                LightComponent lightComponent = new LightComponent();
                lightEntity.Add(lightComponent);
                lightComponent.Type = new LightDirectional();
                lightComponent.Intensity = 1;
                lightComponent.SetColor(new Color3(1, 1, 1));
            }

            {
                for (int x = -100; x <= 100; x += 25) {

                    for (int y = -100; y <= 100; y += 25) {

                        for (int z = -100; z <= 100; z += 25) {
                            Vector3 location = new Vector3(x, y, z);
                            if (location.Length() < 10) {
                                continue;
                            }

                            Entity marker = game.Create3DPrimitive(PrimitiveModelType.Cube, new() {
                                Material = game.CreateMaterial(Color.Gold),
                                IncludeCollider = false // No collider for simple movement
                            });
                            marker.Name = "Marker " + x + " " + y + " " + z;
                            marker.Scene = Entity.Scene;
                            ;
                        }
                    }
                }
            }

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
