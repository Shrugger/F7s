namespace F7s.Modell.Terrains {
    public class PlanetologyData {
        public readonly float Rheology;
        public readonly float Tectonics;
        public readonly float? SeaLevel;
        public readonly bool Biosphere;
        public readonly bool Atmosphere;
        public readonly float baseTemperature;

        public PlanetologyData(float rheology, float tectonics, float? seaLevel, bool biosphere, bool atmosphere, float baseTemperature = 0) {
            this.Rheology = rheology;
            this.Tectonics = tectonics;
            this.SeaLevel = seaLevel;
            this.Biosphere = biosphere;
            this.Atmosphere = atmosphere;
            this.baseTemperature = baseTemperature;
        }
    }

}
