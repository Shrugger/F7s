namespace F7s.Geometry {
    public static class Intersections {
        public static bool SphereSphere(double radius1, double radius2, double distance) {
            return distance <= radius1 + radius2;
        }
    }
}
