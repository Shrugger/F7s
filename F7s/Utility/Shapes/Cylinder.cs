using F7s.Utility.Shapes.Shapes2D;
using Stride.Core.Mathematics;
using System;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Shapes
{


    public class Cylinder : Capsule {
        public Cylinder(float diameter, float length) : base(diameter, length) {
        }

        public override ShapeType ShapeType() {
            return F7s.Utility.Shapes.ShapeType.Cylinder;
        }

        public override float SurfaceArea() {
            return Mathematik.CylinderSurfaceArea(this.diameter / 2.0f, this.BoundingHeight);
        }

        public override List<Vector3> RelativeVertices() {
            float halfLength = this.length / 2.0f;
            float halfDiameter = this.diameter / 2.0f;
            Vector3 startUp = new Vector3(0, halfDiameter, -halfLength);
            Vector3 startDown = new Vector3(0, -halfDiameter, -halfLength);
            Vector3 startLeft = new Vector3(-halfDiameter, 0, -halfLength);
            Vector3 startRight = new Vector3(halfDiameter, 0, -halfLength);
            Vector3 endUp = new Vector3(0, halfDiameter, halfLength);
            Vector3 endDown = new Vector3(0, -halfDiameter, halfLength);
            Vector3 endLeft = new Vector3(-halfDiameter, 0, halfLength);
            Vector3 endRight = new Vector3(halfDiameter, 0, halfLength);
            return new List<Vector3>() { startUp, startDown, startLeft, startRight, endUp, endLeft, endRight, endDown };
        }
        public override float SubstantialVolume() {
            return this.BaseCircle().SurfaceArea() * this.length;
        }

        public override float ProfileArea(Vector3 angle) {
            throw new NotImplementedException();
        }

        public Circle BaseCircle() {
            return this.GetCircularCrossSection();
        }
    }

}