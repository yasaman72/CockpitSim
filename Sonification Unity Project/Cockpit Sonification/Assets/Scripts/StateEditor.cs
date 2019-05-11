using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEditor;
using  UnityEngine.Events;

[CustomEditor(typeof(RealStateEditor))]
public class StateEditor : Editor
{

    public override void OnInspectorGUI()
    {
    RealStateEditor realStateEditor = (RealStateEditor) target;

        base.OnInspectorGUI();

        if (GUILayout.Button("speed up"))
        {
            realStateEditor.speedupEvent.Invoke();
        }
        if (GUILayout.Button("speed down"))
        {
            realStateEditor.speedDownEvent.Invoke();
        }
        GUILayout.Space(20f);



        if (GUILayout.Button("altitude up"))
        {
            realStateEditor.altitudeUpEvent.Invoke();
        }
        if (GUILayout.Button("altitude down"))
        {
            realStateEditor.altitudeDownEvent.Invoke();
        }
        GUILayout.Space(20f);



        if (GUILayout.Button("direction up"))
        {
            realStateEditor.directionUpEvent.Invoke();
        }
        if (GUILayout.Button("direction down"))
        {
            realStateEditor.directionDownEvent.Invoke();
        }
        GUILayout.Space(20f);



        if (GUILayout.Button("rotation up"))
        {
            realStateEditor.rotationUpEvent.Invoke();
        }
        if (GUILayout.Button("rotation down"))
        {
            realStateEditor.rotationDownEvent.Invoke();
        }

    }

	
}
