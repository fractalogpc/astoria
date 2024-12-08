Incoming:
Start models from scratch here. If continuing off of a blockout, put blockout objects in a 
Blockout collection for reference and start new. If you want to model off of your blockouts
directly, create a copy, move that copy outside the Blockout collection, and work on that.
Name the blend file "ModelName_Blockout"

Goal:
Create a polished, finalized model of your object. Create the inital version first, then make a
copy, and reduce the tricount as much as possible. Ideally, keep the reduced tricount lower than 10,000.
At the end, you should have a finished model contained under one object. 

Tips:
For hard surface modeling (non organic objects), make use of auto smoothing. The bevel modifier with
Harden Normals checked looks good at low segment counts. Make sure your bevels never go above 1-2 segments.
You can combine separate meshes into one objects. It's perfectly fine to clip those meshes into each other.
Subdivision is the only way you can get good results for organic modeling (think curved, flowy surfaces).
It's also the only way compatible with sculpting.

Next Steps:
Once you're ready to make the final model, review the QA Check. 
Make sure your model meets it, then continue. THIS IS IMPORTANT FOR TEXTURING.
Commit to the repo with "Done with modeling for [model name]"
Make a copy of your blend file to the texturing folder.
Commit to the repo with "Starting texturing for [model name]"
Don't delete the modeling file. You might need it later. 
Refer to Incoming in the Texturing README for what to do with the texturing copy.

QA Check:
Is your model made up of multiple objects? If so, combine them into one object/mesh.
Are your objects AND meshes named after the part of the model they represent?
Is the tri count reasonable? Make sure the reduced version is no more than 10,000 triangles, unless
ABOSLUTELY NEEDED. Aim for 1000-5000.
Are most faces quads/tris? N-gons are okay, as long as they're completely flat or in hidden places.
Are there any shading errors that immediately show when using MatCap view?
Is scale applied? Make sure all scale readouts in object mode show 1, 1, 1.
Is rotation applied? Unless it's needed for the model, make sure rotations show 0, 0, 0.
If your model is one object, make sure to move it to the world origin with:
Shift+S -> Cursor To World Origin -> Click on your model -> Shift+S -> Selection To Cursor.
If this doesn't completely move it to the world origin, center it manually and apply position.



