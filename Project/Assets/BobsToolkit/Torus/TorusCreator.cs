/**
 * Based on a script by thieberson (http://forum.unity3d.com/threads/torus-in-unity.8487/) (in Torus.zip, originally named Torus.cs),
 * which was based on a script by Steffen ("Primitives.cs" from $primitives_966_104.zip).
 * 
 * Editted by Michael Zoller on December 6, 2015.
 * 
 * Usage Notes:
 * If the color property is preventing you from changing the color in a different Script's start, give this script Execution Order priority.
 * 
 * Paraphrase of original usage notes:
 * This version is improved from Steffen's original to allow the manipulation of the ring outside the script (ex. in the Unity editor while testing).
 * The script can be attached to any GameObject (Main), although an Emtpy one is best.
 * When the script starts, it creates a sibling GameObject to be the ring (meshRing).
 * The user can change the segmentRadius, tubeRadius and numTubes of the meshRing through the
 * transform.scale.x, transform.scale.y and transform.scale.z, respectively, of Main.
 * The position, rotation and color of the meshRing are copied from Main.
 * 
 * Outside the script, the transform.scale of Main can be accessed by: GameObject.Find(name_of_the_Main_Game_Object)
 * 
 * Edit by Bob Jeltes https://github.com/emsylf on November 23, 2020: 
 * TorusCreator.cs is the class that allows the user to create a torus shape like any other basic Unity3D shape, through the GameObject/3D Object menu at the top of the window
 * Torus.cs is now its own class governing everything that has to do with the instance of a torus. 
 * TorusEditor.cs creates a custom inspector that allows you to see the effect of the changing variables in real-time.
 */
using UnityEngine;
using UnityEditor;

public class TorusCreator : MonoBehaviour {

    public static void CreateTorus()
    {

    }
}
