Incoming:
This is for cleaning up exports from Unity ProBuilder mockups/greyboxes.
If your mockup is a scene/set of models, split up the models into individual files. 
(Ex. Room -> Chair, Table, Desk)
Name the file ModelName_Blockout.

Goal:
Prep ProBuilder exports to be ready for remodeling. Get the general shape and size down of your model. 
When you're done with the blockout, go to Next Steps.

Next Steps:
Once you're ready to make the final model, review the QA Check. 
Make sure your model meets it, then continue.
Commit to the repo with "Done with blockout for [model name]"
Make a copy of your blend file to the modeling folder.
Commit to the repo with "Starting modeling for [model name]"
Don't delete the blockout file. You might need it later. 
Refer to Incoming in the Modeling README for what to do with the modeling copy.

QA Check:
Is scale applied? Make sure all scale readouts in object mode show 1, 1, 1.
Is rotation applied? Unless it's needed for the model, make sure rotations show 0, 0, 0.
If your model is one object, make sure to move it to the world origin with:
Shift+S -> Cursor To World Origin -> Click on your model -> Shift+S -> Selection To Cursor.
If this doesn't completely move it to the world origin, center it manually and apply position.
Are there any lights, cameras, or other non-models in the blender file? If so, delete them.

