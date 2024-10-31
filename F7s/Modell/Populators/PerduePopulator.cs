using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Conceptual.Agents.GroupDistributions;
using F7s.Modell.Conceptual.Cultures;
using F7s.Modell.Economics.Industries.Agriculture;
using F7s.Modell.Economics.Scavenging;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;

namespace F7s.Modell.Populators {
    public class PerduePopulator : Populator {
        public PerduePopulator () {
            GeneratePlayer();
            GenerateGroups();

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


            Group mankind = new Group("Mankind", new GroupComposition(1000000000000, RootLocality.Instance));
            mankind.SetCulture(humanCulture);

            Group locals = mankind.EstablishSubgroup("Locals", new GroupComposition(1000000, RootLocality.Instance));

            for (int s = 1; s <= 1; s++) {
                Locality scavengerCommunityLocality = new Fixed(null,
                    RootLocality.Instance
,
                    MatrixD.Transformation(-MM.RightD * 200, QuaternionD.Identity));
                Group scavengerCommunity = locals.EstablishSubgroup("Scavengers #" + s, new GroupComposition(100, scavengerCommunityLocality));
                scavengerCommunity.InstituteInstitution(new SubsistenceScavengingInstitution());
                scavengerCommunity.ManifestMember("Jim", scavengerCommunityLocality);
            }
            for (int f = 1; f <= 1; f++) {
                Locality farmingCommunityLocality = new Fixed(null,
                    RootLocality.Instance
,
                    MatrixD.Transformation(MM.RightD * 10, QuaternionD.Identity));
                Group farmingCommunity = locals.EstablishSubgroup("Farmers #" + f, new GroupComposition(100, farmingCommunityLocality));
                farmingCommunity.InstituteInstitution(new SubsistenceFarming());
                Human bob = (Human) farmingCommunity.ManifestMember("Bob", farmingCommunityLocality);
            }
        }

        private void GeneratePlayer () {

            Locality playerLocation = new Fixed(null, RootLocality.Instance, MatrixD.Transformation(new Double3(0, 6, 0), QuaternionD.Identity));
            playerLocation.Name = "Player Location";
            PhysicalEntity playerEntity = new Body("Player", new Double3(1.0, 2.0, 0.5), new Farbe(0.0f, 0.5f, 0.25f));
            Locality playerEntityLocation = new Fixed(playerEntity, playerLocation, MatrixD.Identity);
            playerEntityLocation.Name = "Player Entity Location";
            playerEntity.SetQuantity(new Quantity(100));
            Player.SetPhysicalEntity(playerEntity);
        }
    }
}