using F7s.Utility;
using F7s.Utility.Mescherei;
using F7s.Utility.Time;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Modell.Terrains {
    /*
     Rheologic/Tectonic/Geologic Model

        Based on a layer cake of Mesches.

        Mantle flux, adjusted to be zero-sum
        Mantle currents
        Plate division
        Plate elevation and depression (and oceanic extra depression) based on mantle flux sums
        Plate movement based on mantle current
        Crust ridges and rifts based on relative plate edge collision velocities
     
     */

    public class RheoTecGeo {

        private Graph geograph;
        private List<Geosample> geosamples = new List<Geosample>();
        private List<Plate> plates = new List<Plate>();
        public readonly float baseRadius;
        public readonly float GeosampleReach;
        public Dictionary<Vertex, Geosample> vertexGeosampleMap = new Dictionary<Vertex, Geosample>();
        public Dictionary<Edge, Vector2> currentsMap = new Dictionary<Edge, Vector2>();

        public float ScaleFactor { get; private set; }

        public RheoTecGeo (string name, int resolution, float radius) {
            Stoppuhr stoppuhr = new Stoppuhr();
            this.baseRadius = radius;
            float earthRadius = 6000000;
            this.ScaleFactor = this.baseRadius / earthRadius;
            this.geograph = Icosahedra.IcosphereGraph(resolution, radius);

            this.GeosampleReach = this.baseRadius / 10; // Alternatively: this.geograph.Edges.Take(5).Average(e => e.Length(true, radius)) * 2f;

            foreach (Vertex vertex in this.geograph.Vertices) {
                this.geosamples.Add(new Geosample(this, vertex));
            }

            float averageMantleFlux = this.geosamples.Average(x => x.MantleFlux);
            foreach (Geosample geosample in this.geosamples) {
                geosample.NormalizeMantleFlux(averageMantleFlux);
            }

            foreach (Geosample geosample in this.geosamples) {
                geosample.CalculateCurrents();
            }

            this.GeneratePlates();

            foreach (Geosample geosample in this.geosamples) {
                geosample.CalculateTectonicElevation();
            }

            stoppuhr.Print("Setting up Rheology, Tectonics and Geology for " + name + " took", 0.1f);
        }

        public List<Geosample> Geosamples () {
            return this.geosamples;
        }

        private void GeneratePlates () {

            HashSet<Geosample> available = new HashSet<Geosample>();
            this.geosamples.ForEach(gs => available.Add(gs));

            void Assign (Plate plate, Geosample sample) {
                if (!available.Contains(sample)) {
                    throw new Exception();
                }
                plate.AddGeosample(sample);
                available.Remove(sample);
            }

            for (int p = 1; p <= 10; p++) {
                Plate plate = new Plate(this);
                this.plates.Add(plate);
                Geosample core = Alea.Item(available);
                Assign(plate, core);
            }

            while (available.Count > 0) {
                foreach (Plate plate in this.plates) {
                    plate.Grow(available);
                }
            }

            foreach (Plate plate in this.plates) {
                plate.CalculateAttributes();
            }

            Debug.Assert(this.geosamples.Count() == this.plates.Sum(p => p.GeosampleCount()));
        }

        public void Delete () {
            this.geosamples.Clear();
            this.geosamples = null;
            this.plates.Clear();
            this.plates = null;
            this.geograph.Delete();
            this.geograph = null;
            this.vertexGeosampleMap.Clear();
            this.vertexGeosampleMap = null;
            this.currentsMap.Clear();
            this.currentsMap = null;
        }
    }
}
