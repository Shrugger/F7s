using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Terrains
{

    public abstract class TerrainFeature {
        public Terrain Terrain { get; private set; }
        bool calculateDistancesAccurately = false; // Expensive!

        public double Date { get; protected set; } // Denotes the priority with which this feature is applied. Earlier dates come first, later ones later, i.e., keep features ordered by ascending date.

        protected TerrainFeature () { }
        protected TerrainFeature (Terrain terrain) {
            this.Terrain = terrain;
        }

        protected float VertexElevation (Vertex v) {
            return this.Terrain.CalculateSurfaceElevation(v);
        }

        protected float VertexLatitude (Vertex v) {
            return this.Terrain.SurfaceLatitude(v);
        }

        protected float VertexTemperature (Vertex vertex) {
            return this.Terrain.CalculateSurfaceTemperature(vertex);
        }

        public override string ToString () {
            return this.GetType().Name + "@" + this.Date;
        }

        public abstract bool Reaches (Vertex vertex);

        public void ApplyIfApplicable (Vertex vertex) {
            if (this.Reaches(vertex)) {
                this.Apply(vertex);
            }
        }
        public void SetTerrain (Terrain terrain) {
            if (this.Terrain != null) {
                throw new System.Exception();
            }
            this.Terrain = terrain;
        }

        public void SetDate (double date) {
            this.Date = date;
        }

        protected abstract void Apply (Vertex vertex);

        protected bool RadiusReaches (double featureRadius, PolarCoordinatesD featureCoordinates, PolarCoordinatesD positionCoordinates) {
            double distance = this.Distance(featureCoordinates, positionCoordinates);
            return distance <= featureRadius;
        }

        protected double Distance (PolarCoordinatesD featureCoordinates, PolarCoordinatesD positionCoordinates) {
            return featureCoordinates.PolarDistanceDouble(positionCoordinates, this.Terrain.BaseRadius);
        }
        protected bool RadiusReaches (double featureRadius, Vector3 featurePosition, Vector3 position) {
            double distance = this.Distance(featurePosition, position);
            return distance <= featureRadius;
        }

        protected double Distance (Vector3 featurePosition, Vector3 position) {
            // TODO: Ignores great-circle.
            return Vector3.Distance(MM.Normalize(featurePosition), MM.Normalize(position)) * this.Terrain.BaseRadius;
        }
        protected bool RadiusReaches (double featureRadius, Vertex featureVertex, Vertex vertex) {
            double distance = this.Distance(featureVertex, vertex);
            return distance <= featureRadius;
        }
        protected double Distance (Vertex featureVertex, Vertex vertex) {
            return this.Distance(featureVertex.Position, vertex.Position);
        }

        protected float ProximityFactor (Vector3 from, Vector3 to, float reach) {
            return this.ProximityFactor((float) this.Distance(from, to), reach);
        }

        protected float ProximityFactor (PolarCoordinatesD from, PolarCoordinatesD to, float reach) {
            return this.ProximityFactor((float) this.Distance(from, to), reach);
        }

        protected float ProximityFactor (Vertex from, Vertex to, float reach) {
            float distance = (float) this.Distance(from, to);
            return this.ProximityFactor(distance, reach);
        }

        protected float ProximityFactor (float distance, float reach) {
            return 1 - distance / reach;
        }
    }
}
