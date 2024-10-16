using F7s.Modell.Physical.Bodies.Weapons.Abstract;
using System.Collections.Generic;

namespace F7s.Modell.Physical.Abstract;
public class Combatant : AbstractGameEntity {

    // Typology
    public enum BaseTypes { Undefined, Human, Lifeform, Machine, MannedVehicle }
    public BaseTypes BaseType { get; private set; } = BaseTypes.Undefined;

    // Mobility - Space
    public float DeltaVelocityInVacuum { get; private set; } = 0;
    public float MaximumAccelerationInVacuum { get; private set; } = 0;

    // Mobility - Air
    public float MaximumAirSpeed { get; private set; } = 0;

    // Mobility - Ground
    public float MaximumGroundSpeed { get; private set; } = 0;

    // Protection
    public enum ArmorSchemes { Undefined, Protected, AllOrNothing, Uniform }
    public ArmorSchemes ArmorScheme { get; private set; } = ArmorSchemes.Undefined;

    public bool ArmorAblative;
    public bool ArmorReactive;
    public float ArmorThickness;
    public float ArmorCoverage;

    // Armaments
    private readonly List<Weapon> weapons = new List<Weapon>();

    // Sensorics
    public float VisualResolution { get; private set; }

    // Communications
    public float AudioStrength { get; private set; }
    public float RadioStrength { get; private set; }


}
