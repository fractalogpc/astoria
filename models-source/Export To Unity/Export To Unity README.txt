Incoming:
Gather all exported textures and FBX files into subdirectory for its model.
After importing textures, follow these import settings:
Diffuse: sRGB Default
Normal: Normal
Metallic: Linear Default
Roughness: Linear Default
AO: sRGB Default
Displacement/Height: Linear Default

Create a material using HDRP/Lit shader. NOTE: If this should have snow receiving, use TVE shader (ask Ansel)

Channel pack Roughness, Metallic, & AO into Mask Map using Mask Packer tool. Make sure to check box for roughness (not smoothness).
Set resultant Mask Map to Linear Default import.

Make sure that if your textures are not 2048x2048, they are set to the correct max resolution in the import settings.

Assign textures to correct slots.
Texture mapping should default to UV0, make sure it is.
If you want to enable POM or vertex displacement (this should usually not be used, and therefore height map would be ignored, ask someone if it is a good idea for use case.)

Adjust sliders based on artist instruction, but probably leave default.

Check the box to enable GPU instancing at the bottom of the material.

Create a new prefab in the folder.

Drag FBX into prefab and assign material, make sure transform scale and rotation are default.

Next Steps:
Go use the prefab!

QA Check: