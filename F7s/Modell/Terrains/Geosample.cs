using F7s.Utility;
using F7s.Utility.Mescherei;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using F7s.Utility.Geometry;

namespace F7s.Modell.Terrains
{
    public class Geosample : TerrainFeature {

        public readonly Vertex Vertex;

        public Plate Plate { get; private set; }
        public float MantleFlux { get; private set; }
        public float TectonicElevation { get; private set; }
        public Farbe FluxColor { get; private set; } // Only applies to flux source, not sink.
        public Vector2 Currents { get; private set; }
        private RheoTecGeo RTG;


        public Geosample (RheoTecGeo rheoTecGeo, Vertex vertex) {
            this.RTG = rheoTecGeo;
            this.Vertex = vertex;
            this.RTG.vertexGeosampleMap.Add(vertex, this);
            this.MantleFlux = Alea.Float(-1000, 1000);
            this.FluxColor = Alea.Color(new Farbe(0.5f, 0.5f, 0.5f, 1.0f), 0.5f) * 0.25f;
        }

        public override string ToString () {
            return base.ToString();
        }

        public override bool Reaches (Vertex vertex) {
            return this.RadiusReaches(this.RTG.GeosampleReach, this.Vertex, vertex);
        }

        protected override void Apply (Vertex vertex) {
            float distanceFactor = this.ProximityFactor(this.Vertex, vertex, this.RTG.GeosampleReach);
            float additionalElevation = this.Plate.RheologicElevation * this.Terrain.RheologyExaggeration + this.TectonicElevation * this.Terrain.TectonicsExaggeration;
            additionalElevation *= this.RTG.ScaleFactor * distanceFactor;
            float minimum = this.RTG.baseRadius * 0.5f;
            float currentElevation = vertex.GetRadius();
            float finalElevation = currentElevation + additionalElevation;
            float effectiveFinalElevation;
            Farbe color;
            if (finalElevation > minimum) {
                effectiveFinalElevation = finalElevation;
                color = this.Plate.AverageColor;
            } else {
                effectiveFinalElevation = minimum;
                color = new Farbe(1, 0, 0);
            }
            color = Farbe.Lerp(vertex.Color, color, distanceFactor);
            vertex.SetRadius(effectiveFinalElevation);
            vertex.SetColor(color);
        }

        public void CalculateCurrents () {
            Vector2 sum = Vector2.Zero;
            foreach (Edge e in this.Vertex.Edges) {
                if (!this.RTG.currentsMap.ContainsKey(e)) {
                    float currentMagnitude = this.RTG.vertexGeosampleMap[e.A].MantleFlux - this.RTG.vertexGeosampleMap[e.B].MantleFlux;
                    PolarCoordinates currentCoordinates = e.B.Coordinates - e.A.Coordinates;
                    Vector2 current = Mathematik.Normalize(currentCoordinates.LongLatToVector2()) * currentMagnitude;
                    this.RTG.currentsMap.Add(e, current);
                }
                sum += this.RTG.currentsMap[e];
            }
            Vector2 average = sum / this.Vertex.EdgeCount();
            this.Currents = average;
        }

        public void AssignToPlate (Plate plate) {
            this.Plate = plate;
            plate.AddGeosample(this);
        }

        private List<Geosample> Neighbors () {
            List<Geosample> neighbors = new List<Geosample>();
            foreach (Vertex neighboringVertex in this.Vertex.Neighbors()) {
                neighbors.Add(this.RTG.vertexGeosampleMap[neighboringVertex]);
            }
            return neighbors;
        }

        private List<(Plate, Geosample)> BorderingPlates () {
            List<Geosample> neighbors = this.Neighbors();
            List<(Plate, Geosample)> borderingPlates = neighbors.Select(neighbor => (neighbor.Plate, neighbor)).Where(p => p.Plate != this.Plate).ToList();
            return borderingPlates;
        }

        public void CalculateTectonicElevation () {
            List<(Plate, Geosample)> borderingPlates = this.BorderingPlates();
            float totalElevation = 0;
            foreach ((Plate, Geosample) borderingPlate in borderingPlates) {
                Vector2 ourMovement = this.Plate.RheologicMovement;
                Vector2 theirMovement = borderingPlate.Item1.RheologicMovement;
                Vector2 relativeVelocity = ourMovement - theirMovement;
                Vector2 relativePosition = (this.Vertex.Coordinates - borderingPlate.Item2.Vertex.Coordinates).LongLatToVector2();
                float sign = Mathematik.ApproachOrSeparate(relativePosition, relativeVelocity);
                float elevation = -sign;
                totalElevation += elevation;
                Debug.Assert(!float.IsNaN(totalElevation));
            }
            this.TectonicElevation = totalElevation;
        }

        public void NormalizeMantleFlux (float averageMantleFlux) {
            this.MantleFlux -= averageMantleFlux;
        }

        public void SetPlate (Plate plate) {
            if (this.Plate != null) {
                throw new Exception();
            }
            this.Plate = plate;
        }
    }
}
