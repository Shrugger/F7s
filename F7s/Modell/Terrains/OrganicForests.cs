using F7s.Utility;
using F7s.Utility.Lazies;
using F7s.Utility.Mescherei;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Terrains {

    public class OrganicForests : TerrainFeature {

        public OrganicForests () {
            OrganicForests feature = this;
            this.lazyHospitability = new LazyMonoMemory<Vertex, float>((v) => feature.CalculateHospitability(v));
        }

        public override bool Reaches (Vertex vertex) {
            return this.Hospitability(vertex) > 0;
        }

        private LazyMonoMemory<Vertex, float> lazyHospitability;
        protected float Hospitability (Vertex vertex) {
            return this.lazyHospitability.GetValue(vertex);
        }
        protected float CalculateHospitability (Vertex v) {
            float temperature = this.Terrain.CalculateSurfaceTemperature(v);
            float optimumTemperature = 10;
            float maximumDeviation = 20;
            float habitability = (maximumDeviation - MathF.Abs(optimumTemperature - temperature)) / maximumDeviation;
            return habitability;
        }

        protected override void Apply (Vertex vertex) {
            float hospitability = this.Hospitability(vertex);
            if (hospitability <= 0) {
                throw new System.Exception();
            }

            float height = hospitability * 100;
            float latitude = this.Terrain.AbsoluteSurfaceLatitude(vertex);

            Farbe color;
            bool differentiateByLatitude = true;
            if (differentiateByLatitude) {
                Farbe conifer = new Farbe(0, 0.2f, 0.1f);
                Farbe deciduo = new Farbe(0.1f, 0.2f, 0f);
                float deciduousBias = 15;
                float coniferousBias = 15;
                color = deciduo.Lerp(conifer, (latitude - deciduousBias) / (90f - coniferousBias));
            } else {
                color = Terrain.Colors.Forest;
            }
            vertex.SetColor(color);
            vertex.ShiftRadius(height);
        }
    }
}
