﻿using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;

namespace F7s.Utility.Geometry {
    public interface Coordinates {

        Vector3 ToVector3 ();
        Vector3d ToVector3d ();
        PolarCoordinatesD Polar ();
        double CartesianDistanceDouble (Coordinates other);
        double PolarDistanceDouble (Coordinates other, double projectionRadius = 1.0);
        double MagnitudeDouble ();

    }

}