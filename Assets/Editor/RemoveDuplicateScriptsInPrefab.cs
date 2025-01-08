using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemoveDuplicateScriptsInPrefab : MonoBehaviour
{
    [MenuItem("Tools/Remove Duplicate Scripts")]
    public static void RemoveDuplicateScripts()
    {
        GameObject[] prefabs = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            MonoBehaviour[] scripts = prefab.GetComponents<MonoBehaviour>();

            var scriptNames = new System.Collections.Generic.HashSet<string>();

            foreach (MonoBehaviour script in scripts)
            {
                if (scriptNames.Contains(script.GetType().Name))
                {
                    DestroyImmediate(script);
                }
                else
                {
                    scriptNames.Add(script.GetType().Name);
                }
            }
            PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.UserAction);
        }
    }
}
