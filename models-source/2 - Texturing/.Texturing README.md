Incoming:
Name your blend file "ModelName_Texturing"
Ensure QA Check of last step was completed. QA here is vital for the texturing workflow. 
Create a folder named your model's name in the Resources folder. Here, put any images, outside textures,
and references you might need to use. Make sure your blend file references the files in that folder only.
Never import an image from anywhere else. Only in the corrosponding resources folder.

Goal:
UV unwrap your models. If you don't know how to do this, look it up on YouTube for some great guides.
Either import materials from outside sources or create your own procedural ones. MAKE SURE TO PUT IMPORTS
IN YOUR MODEL'S RESOURCES FOLDER. Texture Paint those materials on your model and add decals, wear, etc.
Use Eevee to preview your model. Eevee's rendering style is closer to what it'll look like in Unity.
When you're done texturing, complete Next Steps.

Next Steps:
1. Create a folder in your model's resources file called "PreExportTextures".
2. Ensure that your model meets the Modeled QA Check.
3. Commit to the repo with: "Finished texturing for [Model Name]"
4. Bake the textures. If you don't know how to bake in blender, look it up online first. 
   Make sure to bake at resolutions that make sense. If you're modeling a small prop in a room,
   you want 1024x1024 or 2048x2048 resolution textures.
   But if you're modeling up close weapons or viewmodels, use 4096x4096 or even 8192x8192 resolution.
   Then, bake out these texture maps. 
   Make sure to COMMIT AND PUSH to the repo for EACH texture map, with "Baked [MapType] for [Model Name]".
   Save them to PreExportTextures, and name them "ModelName_MapType". Ex: TallCup_Normal.
   -Diffuse
   -Normal
   -Metallic
   -Roughness
   -Ambient Occlusion
   -Displacement/Height
5. Ensure your model meets the Baked QA Check.
6. If your model doesn't need to be animated, proceed to step 7. Otherwise, skip past to step 8A.
7. Make a copy of your blend file to the Export to Unity folder. Refer to the Incoming section
   in Export To Unity README for what to do next. 
8A.

Modeled QA Check:

Baked QA Check: