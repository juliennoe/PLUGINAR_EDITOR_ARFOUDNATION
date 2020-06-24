#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(pluginAR))]
public class pluginAREditor : Editor
{
    pluginAR _pluginAR;
    bool ARplane = false;
    bool ARpoint = false;

    public override void OnInspectorGUI()
    {



        base.OnInspectorGUI();
        pluginAR pAR = target as pluginAR;

        var styleGreen = new GUIStyle(GUI.skin.button);
        styleGreen.normal.textColor = Color.black;
        Color _onButtonGreen = GUI.color;
        GUI.color = Color.magenta;


        if (GUILayout.Button("CREATE TREE FOLDERS",styleGreen))
        {
   
            pAR.CreateFolder();
        }

        var styleYellow = new GUIStyle(GUI.skin.button);
        styleYellow.normal.textColor = Color.black;
        Color _onButtonYellow = GUI.color;
        GUI.color = Color.yellow;

        if (GUILayout.Button("SCENE MENU", styleYellow))
        {

            pAR.SceneMenu();
        }

        GUI.color = Color.green;
        if (GUILayout.Button("BACKGROUND MENU"))
        {
          
            pAR.CreateBackground();

        }
        if (GUILayout.Button("OPEN AR SCENE"))
        {
            pAR.CreateOpenButton();
        }
        if (GUILayout.Button("QUIT APPLICATION"))
        {
            pAR.ExitButton();
        }

        GUI.color = Color.white;
        GUILayout.BeginHorizontal("box");
        ARplane = EditorGUILayout.Toggle("AR PLANE ANCHORS", ARplane);
        if (ARplane)
        {
            pluginAR.arPlaneDetection = true;
        }
        else
        {
            pluginAR.arPlaneDetection = false;
        }

        ARpoint = EditorGUILayout. Toggle("AR PLANE ANCHORS", ARpoint);
        if (ARpoint)
        {
            pluginAR.arCloudDetection = true;
        }
        else
        {
            pluginAR.arCloudDetection = false;
        }

        GUILayout.EndHorizontal();





        GUI.color = Color.yellow;
        if (GUILayout.Button("AR SCENE", styleYellow))
        {
     
            pAR.ARScene();
        }

        GUI.color = Color.green;
        if (GUILayout.Button("SHOW/HIDE AR ANCHORS", styleYellow))
        {

            pAR.AnchorButton();
        }

    
    }
}
#endif