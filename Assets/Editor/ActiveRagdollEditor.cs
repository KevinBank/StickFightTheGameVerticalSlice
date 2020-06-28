using UnityEngine;
using UnityEditor;

public class ActiveRagdollEditor : EditorWindow
{
    [MenuItem("Custom/ActiveRagdoll")]
    public static void ShowWindow()
    {
        GetWindow<ActiveRagdollEditor>("ActiveRagdoll");
    }

    private void OnGUI()
    {

        GUILayout.Label("Hey", EditorStyles.boldLabel);
    }
}
