using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Shapes {
    
    public class OctagonalPipe : PartialOctagonalPipe {

        public OctagonalPipe(float outerDiameter, float length, float internalDiameter)
            : base(
                   outerDiameter,
                   length,
                   internalDiameter,
                   true,
                   true,
                   true,
                   true,
                   true,
                   true,
                   true,
                   true
                  ) { }

    }

}