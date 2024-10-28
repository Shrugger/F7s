using F7s.Engine;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Populators;
using F7s.Modell.Terrains;
using F7s.Utility;
using F7s.Utility.Mescherei;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.Core.Mathematics;
using Stride.Core.Serialization.Contents;
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

        public CameraComponent camera;
        public static CameraComponent Camera { get; private set; }
        public static Entity CameraEntity { get; private set; }
        public static Entity CameraParentEntity { get; private set; }

        public static ContentManager ContentManager { get; private set; }

        public static new GraphicsDevice GraphicsDevice { get; private set; }

        public override void Start () {

            Stride.Core.Collections.TrackingCollection<Entity> SceneSystemEntities = SceneSystem.SceneInstance.RootScene.Entities;

            if (instance != null) {
                throw new System.Exception();
            } else {
                instance = this;
            }

            {
                game = (Game) Game;
                Assert.IsNotNull(game);
            }

            GraphicsDevice = base.GraphicsDevice;
            ContentManager = base.Content;


            Entity.Add(new MainAsync());

            // InitializeCamera(); // TODO: Reactivate
            void InitializeCamera () {
                CameraParentEntity = new Entity("Camera Yawer");
                SceneSystemEntities.Add(CameraParentEntity);

                CameraEntity = new Entity("Camera Pitcher");
                CameraParentEntity.AddChild(CameraEntity);

                Camera = new CameraComponent();
                CameraEntity.Add(Camera);
                Camera.Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();
            }

            {
                Entity lightEntity = new Entity("Light Entity");
                SceneSystemEntities.Add(lightEntity);
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

                            Entity marker = new Entity("Marker " + x + " " + y + " " + z, new Vector3(x, y, z));
                            // SceneSystemEntities.Add(marker);
                            marker.Scene = Entity.Scene;
                            ModelComponent modelComponent = new ModelComponent();
                            marker.Add(modelComponent);

                            Graph graph = Icosahedra.IcosphereGraph();
                            modelComponent.Model = Mesch.FromGraph(graph, GraphicsResourceUsage.Immutable).model;

                        }
                    }
                }
            }

            {
                Terrain terrain = new Terrain("Tiny Planet", 1, 2, Entity, new PlanetologyData(1, 1, 1, true, true, 5)); // TODO: Reactivate after child's play.
                Mesch terrainMesch = terrain.Render(Stride.Graphics.GraphicsResourceUsage.Default);
                Assert.IsNotNull(terrainMesch);
                Entity terrainEntity = new Entity("Terrain");
                ModelComponent modelComponent = new ModelComponent();
                modelComponent.Model = terrainMesch.model;
                terrainEntity.Add(modelComponent);
                SceneSystemEntities.Add(terrainEntity);
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
                    Text = "Main Sync to Fleet Command.",
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

                SceneSystemEntities.Add(uiEntity);
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

            if (camera != null) {
                // TODO: Remove these.
                Camera = camera;
                CameraEntity = camera.Entity;
                CameraParentEntity = CameraEntity;
            }

            InitializeSimulationUpdateListeners();

            Frogram.UpdateAll();

            double deltaTime = Zeit.DeltaTimeSeconds();
            Origin.Update(deltaTime);

            Player.Update(deltaTime);
            // Kamera.Update(deltaTime); // TODO: Reactivate

            if (!Zeit.Paused) {
                GameEntity.OnEngineUpdate(1.0, false);
                populator?.Update(deltaTime);
            }
        }
    }
}
