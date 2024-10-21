﻿using F7s.Modell.Physical.Bodies;
using F7s.Utility;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical {
    public class Vehicle : Body {
        public Vehicle (string name, Vector3 scale, Farbe color) : base(name, scale, color) {
        }
    }
}
