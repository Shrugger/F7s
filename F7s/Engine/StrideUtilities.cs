using F7s.Mains;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;

namespace F7s.Engine;
internal static partial class StrideUtilities {
    // TODO: Better naming for this class.

    public static MaterialInstance PlaceholderMaterial (bool useCommunityToolkit = true) {

        if (useCommunityToolkit) {
            return new MaterialInstance {
                Material = Material.New(MainSync.GraphicsDevice, new MaterialDescriptor {
                    Attributes = new MaterialAttributes {
                        DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                        Diffuse = new MaterialDiffuseMapFeature {
                            DiffuseMap = new ComputeVertexStreamColor()
                        },
                    }
                })
            };
        } else {

            MaterialDescriptor descriptor = new MaterialDescriptor();

            MaterialAttributes attributes = new MaterialAttributes();

            attributes.DiffuseModel = new MaterialDiffuseLambertModelFeature();
            ComputeColor diffuseMapColor = new ComputeColor();
            diffuseMapColor.Key = MaterialKeys.DiffuseValue;
            MaterialDiffuseMapFeature materialDiffuseMapFeature = new MaterialDiffuseMapFeature(diffuseMapColor);
            attributes.Diffuse = materialDiffuseMapFeature;
            attributes.DiffuseModel = new MaterialDiffuseLambertModelFeature();

            descriptor.Attributes = attributes;

            Material material = Material.New(MainSync.GraphicsDevice, descriptor);
            material.Descriptor = descriptor;
            return material;
        }
    }
}
