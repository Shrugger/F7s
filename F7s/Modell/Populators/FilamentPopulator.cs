using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Modell.Conceptual;
using F7s.Modell.Conceptual.Agents.GroupDistributions;
using F7s.Modell.Conceptual.Cultures;
using F7s.Modell.Economics.Agriculture;
using F7s.Modell.Economics.Scavenging;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Celestial;
using F7s.Modell.Physical.Localities;
using F7s.Modell.Terrains;
using F7s.Utility;
using F7s.Utility.Measurements;
using Stride.Core.Mathematics;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Modell.Populators {


    public class FilamentPopulator : Populator {
        private const float scale = 1f;

        private Star coer;
        private SolidPlanet lun;

        private readonly Terrain terrain;
        private CelestialBody lastViewedCelestialBody;

        public static FilamentPopulator Instance { get; private set; }

        public FilamentPopulator () {
            Instance = this;

            Orbiting.OrbitSpeedMultiplier = 10000;
            Zeit.SetSimulationDateGetter(() => Zeit.GetEngineTimeMilliseconds() * 1);

            PopulateLocalStarSystem();
            GenerateGroups();
            ConfigureControls();

            Origin.UseKameraAsFloatingOrigin();
            Player.ActivateFreeCameraControls();
            Kamera.DetachFromPlayer();
        }

        private void GenerateGroups () {
            Culture humanCulture = new Culture();
            Culture interstellarSpacerCulture = new Culture();
            Culture localSpacerCulture = new Culture();
            Culture nativePlonotaryCulture = new Culture();
            Culture landedPlonotaryCulture = new Culture();

            Group mankind = new Group("Mankind", new GroupComposition(1000000000000, coer.Locality));
            mankind.SetCulture(humanCulture);

            Group locals = mankind.EstablishSubgroup("Locals", new GroupComposition(1000000, coer.Locality));

            for (int s = 1; s <= 1; s++) {
                Locality locality = new Fixed(null,
                    MatrixD.Transformation(-Vector3.UnitX * 200, Matrix3x3d.Identity),
                    Player.GetLocalityParent().HierarchySuperior()
                    );
                Group scavengerCommunity = locals.EstablishSubgroup("Scavengers #" + s, new GroupComposition(100, locality));
                scavengerCommunity.InstituteInstitution(new SubsistenceScavengingInstitution());
                scavengerCommunity.ManifestMember("Jim", locality);
            }
            for (int f = 1; f <= 1; f++) {
                Locality locality = new Fixed(null,
                    MatrixD.Transformation(Vector3.UnitX * 10, Matrix3x3d.Identity),
                    Player.GetLocalityParent().HierarchySuperior()
                    );
                Group farmingCommunity = locals.EstablishSubgroup("Farmers #" + f, new GroupComposition(100, locality));
                farmingCommunity.InstituteInstitution(new SubsistenceFarming());
                Human bob = (Human) farmingCommunity.ManifestMember("Bob", locality);
            }
        }

        private void PopulateLocalStarSystem () {


            float massScale = scale * scale;
            int planetResolution = 4;

            coer = new Star("Cör Alpha", Constants.RadiusOfTheSun * 25f * scale, new Farbe(1, 0.8f, 0));
            new Fixed(coer, MatrixD.Identity, RootLocality.Instance);

            SolidPlanet tophet = new SolidPlanet("Tophet", 15000000 * scale, new Farbe(0.1f, 0.0f, 0.2f), planetResolution, new PlanetologyData(1000, 0, null, false, false, 2000));
            new Orbiting(tophet, coer.radius * 5 * scale, coer);

            CelestialBody orage = new CelestialBody("Orage", 30000000, new Farbe(1.0f, 0.5f, 0.0f));
            new Orbiting(orage, Constants.AstronomicUnit * 0.5f * scale, coer);

            SolidPlanet caldo = new SolidPlanet("Caldo", 5000000 * scale, new Farbe(0.9f, 0.7f, 0.1f), planetResolution, new PlanetologyData(0, 100, -100, false, true, 75));
            new Orbiting(caldo, Constants.AstronomicUnit * 0.75f * scale, coer);

            CelestialBody via = new CelestialBody("Via", 70000000 * scale, new Farbe(0.8f, 0.1f, 0f));
            new Orbiting(via, Constants.AstronomicUnit * scale, coer);

            SolidPlanet peccavi = new SolidPlanet("Peccavi", 4500000 * scale, new Farbe(0.2f, 0.0f, 1.0f), planetResolution, new PlanetologyData(100, 100, null, false, true, -20));
            new Orbiting(peccavi, via.radius * 1.5f, via);

            SolidPlanet plonat = new SolidPlanet("Plonat", 7000000 * scale, new Farbe(1, 0.5f, 0f), planetResolution + 1, new PlanetologyData(100, 50, 0, true, true, 0));
            new Orbiting(plonat, 200000000 * scale, via);

            SolidPlanet tehom = new SolidPlanet("Tehom", 15000000, Farbe.blueWaters, planetResolution, new PlanetologyData(0, 0, 5000, false, false, -20));
            new Orbiting(tehom, 300000000 * scale, via);

            SolidPlanet bamot = new SolidPlanet("Bamot", 4500000, new Farbe(0.75f, 0.75f, 0.75f), planetResolution, new PlanetologyData(0, 200, null, false, true, 0));
            new Orbiting(bamot, 450000000 * scale, via);

            SolidPlanet moon = new SolidPlanet("Lün", 2000000 * scale, new Farbe(0.75f, 0.75f, 0.75f), planetResolution, new PlanetologyData(1000, 0, null, false, false, -20));
            new Orbiting(moon, 30000000 * scale, plonat);
            lun = moon;

            PhysicalEntity pml1 = new PhysicalEntity("P-L L1");
            new Lagrange1(pml1, plonat, moon);
            pml1.SetQuantity(new Quantity(Constants.MassOfEarth * 0.5 * massScale));

            Body megastructure = new Body("Megastructure", new Vector3(2000, 500, 4000) * scale, new Farbe(0.25f, 0.25f, 0.25f, 1));
            new Fixed(megastructure, MatrixD.Identity, pml1);
            megastructure.SetQuantity(new Quantity(1000000 * massScale));

            Body terminusStation = new Body("Terminus Station", new Vector3(1000, 2000, 1000) * scale, new Farbe(0.25f, 0.25f, 0.25f, 1));
            new Attached(terminusStation, plonat, MatrixD.Transformation(Vector3.UnitX * (float) plonat.radius * 1.5f, Matrix3x3d.Identity));
            terminusStation.SetQuantity(new Quantity(1000000000 * massScale));

            Body terminusThrone = new Body("Terminus Throne", new Vector3(10, 5, 10), new Farbe(0.25f, 0.25f, 0.25f, 1));
            new Attached(terminusThrone, terminusStation, MatrixD.Transformation(new Vector3(0, (1000 * scale) + 2.5f, 0), Matrix3x3d.Identity));

            CornerTerminus(new Vector3(1, 1, 1));
            CornerTerminus(new Vector3(-1, 1, 1));
            CornerTerminus(new Vector3(1, 1, -1));
            CornerTerminus(new Vector3(-1, 1, -1));
            void CornerTerminus (Vector3 axis) {
                Body terminusCorner = new Body("Terminus Corner", new Vector3(50, 500, 50), new Farbe(0.25f, 0.25f, 0.25f, 1));
                new Attached(terminusCorner, terminusStation, MatrixD.Transformation(new Vector3(axis.X * 500 * scale, axis.Y * 1000 * scale, axis.Z * 500 * scale)
,
                    Matrix3x3d.Identity));
            }

            Locality playerLocation = new Fixed(null, MatrixD.Transformation(new Vector3(0, 6, 0) * scale, Matrix3x3d.Identity), terminusThrone);
            playerLocation.Name = "Player Location";
            PhysicalEntity playerEntity = new Body("Player", new Vector3(1.0f, 2.0f, 0.5f), new Farbe(0.0f, 0.5f, 0.25f));
            Locality playerEntityLocation = new Fixed(playerEntity, MatrixD.Identity, playerLocation);
            playerEntityLocation.Name = "Player Entity Location";
            playerEntity.SetQuantity(new Quantity(100 * massScale));
            Player.SetPhysicalEntity(playerEntity);

            lastViewedCelestialBody = plonat;

            SolidPlanet ultimaThule = new SolidPlanet("Ultima Thule", 5000000 * scale, new Farbe(0.4f, 0.5f, 1.0f), planetResolution, new PlanetologyData(0, 0, -5000, false, false, -30));
            new Orbiting(ultimaThule, Constants.AstronomicUnit * 1.25f * scale, coer);

            for (int a = 1; a <= 10; a++) {
                Body rockyAsteroid = new Body(
                    "Rocky Asteroid #" + a,
                    new Vector3(Alea.Float(0.5f, 2.0f), Alea.Float(0.5f, 2.0f), Alea.Float(0.5f, 2.0f)) * Alea.Float(1000, 1000000) * scale,
                    Farbe.Randomise(new Farbe(0.6f, 0.5f, 0.4f), 0.2f)
                    );
                new Orbiting(rockyAsteroid, Constants.AstronomicUnit * scale * 2f * Alea.Float(0.95f, 1.1f), coer);
                rockyAsteroid.SetQuantity(new Quantity(Mathematik.CubeVolumeFromDimensions(rockyAsteroid.scale), 6.0));
            }
            for (int a = 1; a <= 5; a++) {
                Body iceAsteroid = new Body(
                    "Ice Asteroid #" + a,
                    new Vector3(Alea.Float(0.5f, 2.0f), Alea.Float(0.5f, 2.0f), Alea.Float(0.5f, 2.0f)) * Alea.Float(1000, 1000000) * scale,
                    Farbe.Randomise(new Farbe(0.4f, 0.5f, 0.6f), 0.2f)
                    );
                new Orbiting(iceAsteroid, Constants.AstronomicUnit * scale * 2f * Alea.Float(0.95f, 1.1f), coer);
                iceAsteroid.SetQuantity(new Quantity(Mathematik.CubeVolumeFromDimensions(iceAsteroid.scale), 3.0));
            }

            CelestialBody spiritus = new CelestialBody("Spiritus", 500000000 * scale, new Farbe(0.5f, 0.6f, 7f));
            new Orbiting(spiritus, Constants.AstronomicUnit * scale * 2.5f, coer);

            SolidPlanet glack = new SolidPlanet("Glack", 8000000, new Farbe(1, 1, 1), planetResolution, new PlanetologyData(0, 0, -5000, false, false, -150));
            new Orbiting(glack, spiritus.radius * 4, spiritus);

            for (int m = 1; m <= 4; m++) {
                Body moonlet = new Body(
                    "Moonlet #" + m,
                    new Vector3(Alea.Float(0.5f, 1.5f), Alea.Float(0.5f, 1.5f), Alea.Float(0.5f, 1.5f)) * Alea.Float(10000, 10000000) * scale,
                    Farbe.Randomise(new Farbe(0.5f, 0.5f, 0.5f), 0.25f)
                    );
                new Orbiting(moonlet, spiritus.radius * Alea.Float(1.5f, 3.5f), spiritus);
                moonlet.SetQuantity(new Quantity(Mathematik.CubeVolumeFromDimensions(moonlet.scale), 6.0));
            }

            for (int p = 1; p <= 3; p++) {
                Body planetoid = new Body(
                    "Planetoid #" + p,
                    new Vector3(Alea.Float(0.5f, 1.5f), Alea.Float(0.5f, 1.5f), Alea.Float(0.5f, 1.5f)) * Alea.Float(10000, 10000000) * scale,
                    Farbe.Randomise(new Farbe(0.5f, 0.5f, 0.5f), 0.25f)
                    );
                new Orbiting(planetoid, Constants.AstronomicUnit * scale * Alea.Float(3f, 4f), coer);
                planetoid.SetQuantity(new Quantity(Mathematik.CubeVolumeFromDimensions(planetoid.scale), 6.0));
            }

            // TODO: Star Cör B and its randomized planets
            // TODO: Some randomized neighboring stars

            //this.brownPopulator = new BrownPopulator(new Fixed(null, MatrixD.Transformation(Matrix3x3d.Identity, new Vector3(0, 16, 0) * scale), terminusThrone));

        }

        public override void Update (double deltaTime) {
            terrain?.Update();
            UpdateGlobalScaleFactor();
        }

        private void ConfigureControls () {
            Player.UiControls.Add(new InputVectorAction(() => ViewCelestialBody(-1), Keys.OemComma));
            Player.UiControls.Add(new InputVectorAction(() => ViewCelestialBody(+1), Keys.OemPeriod));
        }

        private void UpdateGlobalScaleFactor () {

            return; // TODO: debugging

            double shortestDistance = CelestialBody.AllCelestialBodies.Min(cb => cb.DistanceToCamera());
            double scaleFactor;
            if (shortestDistance > 0) {
                const double minimumDistance = 0.001f;
                scaleFactor = shortestDistance / minimumDistance;
            } else {
                scaleFactor = 1.0;
            }
            RepresentationSettings.SetGlobalScaleFactor((float) scaleFactor);
        }

        private void ViewCelestialBody (int direction) {
            List<CelestialBody> celestialBodies = CelestialBody.AllCelestialBodies;
            int currentIndex = celestialBodies.IndexOf(lastViewedCelestialBody);
            Debug.Assert(-1 != currentIndex);
            int desiredIndex = (currentIndex + direction) % celestialBodies.Count;
            if (desiredIndex < 0) {
                desiredIndex = celestialBodies.Count - 1;
            }
            CelestialBody celestialBody = celestialBodies[desiredIndex];
            ViewPhysicalEntity(celestialBody);
            lastViewedCelestialBody = celestialBody;
        }

        public static void ViewPhysicalEntity (PhysicalEntity physicalEntity) {
            float radius = physicalEntity.BoundingRadius();
            float distance = radius;
            Debug.Assert(!float.IsNaN(distance));
            Console.WriteLine(
                "Viewing " + physicalEntity +
                " with Radius " + Measurement.MeasureLength(radius) +
                " from Distance " + Measurement.MeasureLength(distance) +
                " (" + Mathematik.RoundToFirstInterestingDigit(distance / radius, 2) + ")"
                );
            Vector3 direction;
            if (physicalEntity != Instance.coer) {
                direction = Instance.coer.RelativePosition(physicalEntity).ToVector3();
            } else {
                direction = new Vector3(Alea.Float(), 0, Alea.Float());
            }
            float finalDistance = radius + distance;
            Vector3 position = Mathematik.Normalize(direction) * finalDistance;
            Kamera.View(physicalEntity, position);
            Player.SetPanSpeed(distance / 1f);
        }

        public static void ViewWholePhysicalSystemTopDown (PhysicalEntity entity) {
            List<PhysicalEntity> subentities = entity.SubentitiesRecursive();
            float furthestOutDistance = (float) subentities.Max(se => se.CenterDistance(entity));

            if (furthestOutDistance <= 0) {
                PhysicalEntity parent = entity.Locality.HierarchySuperior()?.physicalEntity;
                if (parent != null) {
                    ViewWholePhysicalSystemTopDown(parent);
                } else {
                    ViewPhysicalEntity(entity);
                }
            } else {
                Kamera.View(entity, Vector3.UnitY * furthestOutDistance, Vector3.UnitZ);
                Player.SetPanSpeed(furthestOutDistance);
            }
        }
    }

}