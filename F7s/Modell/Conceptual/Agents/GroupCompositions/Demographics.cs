namespace F7s.Modell.Conceptual.Agents.GroupDistributions {
    public class Demographics {
        public float averageAge = 30;
        public float maleToFemaleRatio = 1;
        public float averageIntelligence = 100;
        public float averageHealthFactor = 1;

        public static Demographics Default () {
            return new Demographics();
        }
    }
}
