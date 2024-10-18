using F7s.Utility;
using F7s.Utility.Mescherei;
using Stride.Core.Mathematics;
using System.Collections.Generic;
using System.Linq;
using F7s.Utility.Geometry;

namespace F7s.Modell.Terrains {
    public class Plate {
        public Vector2 RheologicMovement { get; private set; }
        public float RheologicElevation { get; private set; }
        public double BorneMass { get; private set; }
        public Color AverageColor { get; private set; }
        private readonly List<Geosample> geosamples = new List<Geosample>();
        private RheoTecGeo RheoTecGeo;

        private List<Geosample> growthTargets = new List<Geosample>();

        public Plate (RheoTecGeo rheoTecGeo) {
            this.RheoTecGeo = rheoTecGeo;
        }
        public int GeosampleCount () {
            return this.geosamples.Count();
        }
        public void Grow (HashSet<Geosample> availableGeosamples) {

            Geosample next = DrawNextTarget();
            if (next != null) {
                this.AddGeosample(next);
                availableGeosamples.Remove(next);
            }

            Geosample FromVertex (Vertex vertex) {
                return this.RheoTecGeo.vertexGeosampleMap[vertex];
            }

            Geosample DrawNextTarget () {
                RefreshGrowthTargets();
                if (this.growthTargets.Count > 0) {
                    Geosample attempt = this.growthTargets.Last();
                    this.growthTargets.RemoveAt(this.growthTargets.Count - 1);
                    if (availableGeosamples.Contains(attempt)) {
                        return attempt;
                    }
                }
                return null;
            }

            void RefreshGrowthTargets () {
                if (this.growthTargets.Count == 0) {
                    this.growthTargets.AddRange(this.geosamples.SelectMany(gs => gs.Vertex.Neighbors().Select(n => FromVertex(n))).Where(gs => availableGeosamples.Contains(gs)));
                    if (this.growthTargets.Count == 0) {
                        return;
                    }
                }
            }
        }

        public void CalculateAttributes () {
            // TODO: Account for plate mass and borne mass.
            this.RheologicElevation = this.geosamples.Average(gs => gs.MantleFlux);

            this.AverageColor = Farbe.Average(this.geosamples.Where(gs => gs.MantleFlux > 0).Select(gs => gs.FluxColor));

            Vector2 currents = Vector2.Zero;
            foreach (Geosample gs in this.geosamples) {
                currents += gs.Currents;
            }
            currents /= this.geosamples.Count;
            this.RheologicMovement = currents;

            // Cap for sanity
            this.RheologicMovement = Geom.Normalize(this.RheologicMovement);
        }


        public void BearMass (double mass) {
            this.BorneMass += mass;
        }

        public void AddGeosample (Geosample geosample) {
            if (!this.geosamples.Contains(geosample)) {
                this.geosamples.Add(geosample);
                geosample.SetPlate(this);
            }
        }
    }
}
