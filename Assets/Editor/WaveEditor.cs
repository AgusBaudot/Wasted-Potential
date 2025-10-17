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
        for (int wi = 0; wi < manager.AllWaves.Count; wi++)
        {
            var w = manager.AllWaves[wi];
            int sum = 0;
            if (w.spawnDistribution != null)
                foreach (var p in w.spawnDistribution) sum += p;

            if (sum != 100)
            {
                EditorGUILayout.HelpBox($"Wave {wi}: percentages sum to {sum} (should be 100).", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Normalize Percentages"))
                {
                    Undo.RecordObject(manager, "Normalize Percentages");
                    w.NormalizePercentages();
                    EditorUtility.SetDirty(manager);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}