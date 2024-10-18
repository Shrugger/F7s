using F7s.Utility.Shapes.Shapes2D;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Shapes
{


    public class Capsule : Shape3Dim {
        public readonly float diameter;
        public readonly float length;

        public Capsule (float diameter, float length) {
            this.diameter = diameter;
            this.length = length;
        }

        public override bool IsConvex () {
            return true;
        }
        public override string ToString () {
            return this.GetType().Name + "(" + Mathematik.RoundToFirstInterestingDigit(this.diameter) + "x" + Mathematik.RoundToFirstInterestingDigit(this.length) + ")";
        }

        public override Vector3 FullExtents () {
            return new Vector3(this.diameter, this.diameter, this.length);
        }

        public override Shape3Dim GetShapePlusSize (float addition) {
            return new Capsule(this.diameter + addition, this.length + addition);
        }

        public override float ProfileArea (Vector3 angle) {
            throw new NotImplementedException();
        }

        public override bool UnityColliderIsPitchedForward () {
            return true;
        }

        public override ShapeType ShapeType () {
            return F7s.Utility.Shapes.ShapeType.Capsule;
        }

        public override float SurfaceArea () {
            float hemispheres = Geom.SphereSurfaceArea(this.diameter);
            float cylinder = Geom.CylinderSideSurfaceArea(this.diameter * 0.5f, this.length);
            return hemispheres + cylinder;
        }

        public override List<Vector3> RelativeVertices () {
            Vector3 start = new Vector3(0, 0, -this.length / 2.0f);
            float rimHalfLength = this.length / 4.0f;
            float rimRadius = this.diameter / 2.0f;
            Vector3 startUp = new Vector3(0, rimRadius, -rimHalfLength);
            Vector3 startDown = new Vector3(0, -rimRadius, -rimHalfLength);
            Vector3 startLeft = new Vector3(-rimRadius, 0, -rimHalfLength);
            Vector3 startRight = new Vector3(rimRadius, 0, -rimHalfLength);
            Vector3 endUp = new Vector3(0, rimRadius, rimHalfLength);
            Vector3 endDown = new Vector3(0, -rimRadius, rimHalfLength);
            Vector3 endLeft = new Vector3(-rimRadius, 0, rimHalfLength);
            Vector3 endRight = new Vector3(rimRadius, 0, rimHalfLength);
            Vector3 end = new Vector3(0, 0, this.length / 2.0f);
            return new List<Vector3>() { start, startUp, startDown, startLeft, startRight, endUp, endLeft, endRight, endDown, end };
        }

        public override float SubstantialVolume () {
            float hemispheresLength = this.diameter;
            float cylinderLength = this.length - hemispheresLength;

            float cylinderVolume = Geom.CylinderVolume(this.diameter / 2.0f, cylinderLength);
            float hemispheresVolume = Geom.SphereVolumeFromDiameter(this.diameter);

            return cylinderVolume + hemispheresVolume;
        }

        public Vector3 DownwardPitchAddition () {
            return new Vector3(90, 0, 0);
        }

        public override bool ShapeIsPitchedNinetyDegrees () {
            return true;
        }


        public override Circle GetCircularCrossSection () {
            return new Circle(this.diameter / 2.0f);
        }

        public override Vector3 RandomPointWithin () {
            return this.GetBoundingBox().RandomPointWithin();
        }
    }
}