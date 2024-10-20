using F7s.Engine;
using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;
using F7s.Utility.Time;
using Stride.Engine;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Modell.Terrains {

    public class Terrain {

        private RheoTecGeo RTG;

        public static class Colors {
            public static readonly Farbe Base = new Farbe(0.7f, 0.3f, 0.1f);
            public static readonly Farbe Mountain = new Farbe(0.25f, 0.25f, 0.3f);
            public static readonly Farbe Mesa = new Farbe(0.75f, 0.1f, 0.0f);
            public static readonly Farbe Ocean = new Farbe(0.0f, 0.25f, 1.0f);
            public static readonly Farbe Forest = new Farbe(0.0f, 0.2f, 0.0f);
        }

        private Graph Graph;

        public float RheologyExaggeration { get; private set; }
        public float TectonicsExaggeration { get; private set; }

        private readonly PlanetologyData Planetology;
        private List<TerrainFeature> TerrainFeatures = new List<TerrainFeature>();

        private const bool FORCE_REGENERATE_TERRAIN = false;

        public readonly float BaseRadius;
        public readonly string name;

        public Terrain (string name, float radius, int resolution, Entity parent, PlanetologyData planetology) {
            this.name = name;
            BaseRadius = radius;
            Planetology = planetology;
            RheologyExaggeration = planetology.Rheology;
            TectonicsExaggeration = planetology.Tectonics;

            string resourceKey = name + "-" + resolution;


            Graph = Icosahedra.IcosphereGraph(resolution: resolution, radius: radius);

            Graph.Vertices.ForEach(v => Debug.Assert(Mathematik.IsEqualApprox(radius, v.GetRadius(), radius / 1000f)));

            RTG = new RheoTecGeo(name, resolution, radius);

            AddFeature(new BaseTerrain(null), 0);

            Stoppuhr stoppuhr = new Stoppuhr();
            RTG.Geosamples().ForEach(gs => {
                AddFeature(gs, 1);
            }
            );

            for (int impact = 0; impact < 2; impact++) {
                float force = Alea.Float(100000, 500000);
                AddFeature(new ImpactCrater(RandomCoordinates(), force, force / 10), Alea.Int(500, 1000));
            }

            if (planetology.Atmosphere) {
                // this.AddFeature(new AeolianErosion(), 5);
            } else {
                for (int impact = 0; impact < 3; impact++) {
                    float force = Alea.Float(10000, 100000);
                    AddFeature(new ImpactCrater(RandomCoordinates(), force, force / 10), Alea.Int(900, 1000));
                }
            }

            if (planetology.Biosphere) {
                AddFeature(new OrganicForests(), 950);
            }
            if (planetology.SeaLevel.HasValue) {
                AddFeature(new Snow(), 1000);
                AddFeature(new Oceans(planetology.SeaLevel.Value), 975);
            }

            ApplyAllFeaturesToAllVertices();

            stoppuhr.Print("Generating terrain for " + name + " took ", 0.1f);

            DownscaleVertexRadii(); // Visible scaling is then done by MeschInstande3D parentage - TODO: a roundabout system that needs to be deprecated.
        }

        public Mesch Render (GraphicsDevice graphicsDevice, GraphicsResourceUsage graphicsResourceUsage) {
            return Mesch.FromGraph(Graph, graphicsDevice, graphicsResourceUsage);
        }

        public override string ToString () {
            return name;
        }

        private void DownscaleVertexRadii () {
            Graph.ApplyToAllVertices(v => v.SetRadius(v.GetRadius() / BaseRadius));
        }

        public void Clear () {
            Graph?.Delete();
            Graph = null;
            RTG?.Delete();
            RTG = null;
            TerrainFeatures?.Clear();
            TerrainFeatures = null;
        }

        private void AddPlaceholderFeatures () {
            float size = BaseRadius * 0.1f;
            for (int mesa = 0; mesa < 2; mesa++) {
                AddFeature(new Mesas(RandomCoordinates(), size * Alea.Float(0.5f, 4.0f), Alea.Float(size * 0.25f, size * 0.5f)), 500);
            }
            for (int ocean = 0; ocean < 4; ocean++) {
                AddFeature(new Ocean(RandomCoordinates(), size * Alea.Float(2f, 8.0f)), 600);
            }
            for (int mountain = 0; mountain < 2; mountain++) {
                AddFeature(new Mountains(RandomCoordinates(), size * Alea.Float(0.5f, 4.0f), Alea.Float(size, size * 2)), 400);
            }
            for (int forest = 0; forest < 5; forest++) {
                AddFeature(new Forest(RandomCoordinates(), size * Alea.Float(0.5f, 4.0f), Colors.Forest, Alea.Float(size * 0.05f, size * 0.2f)), 1500);
            }
        }

        private PolarCoordinates RandomCoordinates () {
            return Alea.Coordinates(BaseRadius);
        }

        public float CalculateSurfaceTemperature (Vertex vertex) {
            float latitude = AbsoluteSurfaceLatitude(vertex);
            float elevation = CalculateSurfaceElevation(vertex);

            float equatorialSeaLevelTemperature = 35 + Planetology.baseTemperature;
            float polarSeaLevelTemperature = -50 + Planetology.baseTemperature;
            float solarInflux = MathF.Cos(latitude / 90f * MathF.PI / 2f);
            float latitudeFactor = Mathematik.Lerp(polarSeaLevelTemperature, equatorialSeaLevelTemperature, solarInflux);
            float elevationFactor = -elevation * 0.0001f;

            float temperature = latitudeFactor + elevationFactor;
            return temperature;
        }

        public float CalculateSurfaceElevation (Vertex vertex) {
            return CalculateSurfaceElevation((float) vertex.GetRadius());
        }
        public float CalculateSurfaceElevation (float rawElevation) {
            return rawElevation - BaseRadius;
        }

        public float SurfaceLatitude (Vertex vertex) {
            return (float) vertex.Coordinates.latitude;
        }

        public float AbsoluteSurfaceLatitude (Vertex vertex) {
            return MathF.Abs((float) vertex.Coordinates.latitude);
        }

        public void Update () {

        }

        private Vertex GetVertex (int index) {
            return Graph.Vertices[index];
        }


        public void AddFeature (TerrainFeature feature, double date) {
            feature.SetTerrain(this);
            feature.SetDate(date);
            TerrainFeatures.Add(feature);
        }

        private void ApplyAllFeaturesToAllVertices () {
            TerrainFeatures = TerrainFeatures.OrderBy(f => f.Date).ToList();
            foreach (TerrainFeature feature in TerrainFeatures) {
                foreach (Vertex vertex in Graph.Vertices) {
                    feature.ApplyIfApplicable(vertex);
                }
            }
        }
    }

}
