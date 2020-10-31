using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Cloud))]
public class CloudEditor : Editor
{
 public override void OnInspectorGUI()
 {
        Cloud cloudBeingInspected = target as Cloud;
        base.OnInspectorGUI();
        if (GUILayout.Button("New Cloud Field"))
        {
           cloudBeingInspected.New();
        }
         if (GUILayout.Button("Delete Cloud Field"))
        {
           cloudBeingInspected.DeleteClouds();
        }

 }


}

//
//public override void OnInspectorGUI()
//{
//  Scale scaleBeingInspected = target as Scale;
//
//  DrawDefaultInspector();
//  if (GUILayout.Button("My editor button"))
//  {
//     scaleBeingInspected.SomeFunction();
//  }
//}
//DeleteClouds