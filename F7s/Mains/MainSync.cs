using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Localities;
using F7s.Modell.Populators;
using F7s.Modell.Terrains;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Mescherei;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.Core.Mathematics;
using Stride.Core.Serialization.Contents;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;
using Stride.Rendering;
using Stride.Rendering.Lights;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System.Linq;

namespace F7s.Mains {

    public class MainSync : SyncScript {

        private readonly bool simulationStarted = false;

        private Populator populator;
        private TextBlock textBlock;


        private static MainSync instance;

        public static ContentManager ContentManager { get; private set; }
        public static new GraphicsDevice GraphicsDevice { get; private set; }
        public static InputManager InputManager { get; private set; }
        public static new Game Game { get; private set; }
        public static Scene Scene;

        public override void Start () {

            Scene = Entity.Scene;

            {
                // Singleton
                if (instance != null) {
                    throw new System.Exception();
                } else {
                    instance = this;
                }
            }

            {
                // Make Stride systems available as static fields.
                Game = (Game) base.Game;
                Assert.IsNotNull(Game);

                GraphicsDevice = base.GraphicsDevice;
                ContentManager = base.Content;
                InputManager = base.Input;
            }

            Entity.Add(new MainAsync());
            Kamera.BuildStrideHierarchy(Scene, SceneSystem.GraphicsCompositor);

            {
                // Player
                Locality playerLocation = new Fixed(null, RootLocality.Instance, MatrixD.Transformation(new Double3(0, 0, 0), QuaternionD.Identity));
                playerLocation.Name = "Player Location";
                PhysicalEntity playerEntity = new Body("Player", new Double3(1.0f, 2.0f, 0.5f), new Farbe(0.0f, 0.5f, 0.25f));
                Locality playerEntityLocation = new Fixed(playerEntity, playerLocation, MatrixD.Identity);
                playerEntityLocation.Name = "Player Entity Location";
                playerEntity.SetQuantity(new Quantity(100));
                Player.SetPhysicalEntity(playerEntity);
            }

            {
                // Floating Origin and Control Scheme
                Origin.UseKameraAsFloatingOrigin();
                Player.ActivateFreeCameraControls();
                Kamera.DetachFromPlayer();
            }

            {
                // Directional Light
                Entity lightEntity = new Entity("Directional Light Entity");
                lightEntity.Scene = Scene;
                LightComponent lightComponent = new LightComponent();
                lightEntity.Add(lightComponent);
                lightComponent.Type = new LightDirectional();
                lightComponent.Intensity = 20;
                lightComponent.SetColor(new Color3(1, 1, 1));
            }
            {
                // Ambient Light
                Entity lightEntity = new Entity("Ambient Light Entity");
                lightEntity.Scene = Scene;
                LightComponent lightComponent = new LightComponent();
                lightEntity.Add(lightComponent);
                lightComponent.Type = new LightAmbient();
                lightComponent.Intensity = 1;
                lightComponent.SetColor(new Color3(1, 1, 1));
            }

            {
                // Nonphysical entites
                int interval = 50;
                for (int x = -100; x <= 100; x += interval) {

                    for (int y = -100; y <= 100; y += interval) {

                        for (int z = -100; z <= 100; z += interval) {
                            Vector3 location = new Vector3(x, y, z);
                            if (location.Length() < 10) {
                                continue;
                            }

                            Entity marker = new Entity("Marker " + x + " " + y + " " + z, location);
                            marker.Scene = Scene;

                            ModelComponent modelComponent = new ModelComponent();
                            marker.Add(modelComponent);

                            Graph graph = Icosahedra.IcosphereGraph();
                            graph.ApplyToAllVertices((Vertex v) => v.SetColor(new Farbe(v.Position.X, v.Position.Y, v.Position.Z)));
                            modelComponent.Model = Mesch.FromGraph(graph, GraphicsResourceUsage.Immutable).model;

                        }
                    }
                }
            }
            {
                // Physical entities
                int interval = 50;
                for (int x = -100; x <= 100; x += interval) {

                    for (int y = -100; y <= 100; y += interval) {

                        for (int z = -100; z <= 100; z += interval) {
                            Vector3 rawLocation = new Vector3(x, y, z) + Vector3.One;
                            if (rawLocation.Length() < 10) {
                                continue;
                            }

                            Locality locality = new Fixed(null, anchor: RootLocality.Instance, transform: MatrixD.Translation(rawLocation));
                            Vector3 location = (Vector3) locality.GetAbsoluteTransform().TranslationVector;
                            Assert.AreEqual(rawLocation, location);

                            Entity marker = new Entity("Marker " + x + " " + y + " " + z, location);
                            marker.Scene = Scene;

                            ModelComponent modelComponent = new ModelComponent();
                            marker.Add(modelComponent);

                            Graph graph = Icosahedra.IcosphereGraph();
                            graph.ApplyToAllVertices((Vertex v) => v.SetColor(new Farbe(1, 0, 0)));
                            modelComponent.Model = Mesch.FromGraph(graph, GraphicsResourceUsage.Immutable).model;

                        }
                    }
                }
            }

            {
                // Terrain - Does not work so far
                Terrain terrain = new Terrain("Tiny Planet", 1, 2, Entity, new PlanetologyData(1, 1, 1, true, true, 5));
                Mesch terrainMesch = terrain.Render(Stride.Graphics.GraphicsResourceUsage.Default);
                Assert.IsNotNull(terrainMesch);
                Entity terrainEntity = new Entity("Terrain");
                ModelComponent modelComponent = new ModelComponent();
                modelComponent.Model = terrainMesch.model;
                terrainEntity.Add(modelComponent);
                terrainEntity.Scene = Scene;
            }

            {
                // Populator, obviously.
                populator = new PerduePopulator();
            }

            {
                // UI Test
                Assert.IsNotNull(base.Game);
                Assert.IsNotNull(base.Game.Content);
                SpriteFont? font = null;
                font = base.Game.Content.Load<SpriteFont>("StrideDefaultFont");
                var canvas = new Canvas {
                    Width = 500,
                    Height = 200,
                    BackgroundColor = new Color(0, 0, 0, 255),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                };

                textBlock = new TextBlock {
                    Text = "Main Sync to Fleet Command.",
                    TextColor = Color.White,
                    Font = font,
                    TextSize = 16,
                    Margin = new Thickness(3, 3, 3, 0),
                    WrapText = true,
                };
                canvas.Children.Add(textBlock);

                var uiEntity = new Entity
                {
                    new UIComponent
                    {
                        Page = new UIPage { RootElement = canvas },
                        RenderGroup = RenderGroup.Group31
                    }
                };

                uiEntity.Scene = Scene;
            }

            {
                // Frograms
                new InputHandler();
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

            string timestamp = "F+" + Game.UpdateTime.FrameCount + ": ";
            string inputReport = InputHandler.pressedButUnreleasedKeys.ToList().Aggregate("", (l, k) => l + "\n" + k); //Input.Events.Aggregate(seed: "", func: (string accumulation, InputEvent e) => accumulation += "\n" + e.ToString());
            textBlock.Text = timestamp + inputReport;

            DebugText.Print("IsPressed: " + Input.IsKeyDown(Keys.G), new Stride.Core.Mathematics.Int2(50, 50));
        }
    }
}
