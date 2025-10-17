using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveManager))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //Draw the default fields (including waves)

        WaveManager manager = (WaveManager)target;

        //Start with waves percentages calculation.
        int sum = 0;
        foreach (var w in manager.AllWaves)
        {
            foreach (var p in w.spawnDistribution)
            {
                sum += p;
            }

            if (sum != 100)
            {
                EditorGUILayout.HelpBox($"Percentages sum to {sum} - should be 100.", MessageType.Warning);
                if (GUILayout.Button("Normalize Percentages"))
                {
                    Undo.RecordObject(manager, "Normalize Percentages");
                    //Call normalizePercentages of wave.
                    EditorUtility.SetDirty(manager);
                }
            }
        }
    }
}