using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Editor-værktøj: fjerner gamle (legacy) Animation-komponenter fra den åbne scene.
// Animator-komponenter røres ikke. Kan fortrydes med Ctrl+Z.
public static class RemoveLegacyAnimation
{
    [MenuItem("Tools/Fjern legacy Animation-komponenter")]
    static void Run()
    {
        var all = Object.FindObjectsByType<Animation>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int removed = 0;
        foreach (var anim in all)
        {
            if (EditorUtility.IsPersistent(anim)) continue; // spring assets over
            string name = anim.gameObject.name;
            Undo.DestroyObjectImmediate(anim);
            Debug.Log("Fjernede Animation-komponent fra: " + name);
            removed++;
        }
        EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Færdig. Fjernede " + removed + " Animation-komponent(er). Husk at gemme scenen.");
    }
}
