using UnityEngine;
using UnityEditor;

public class ApplyMaterialToCollidersEditor : ScriptableObject
{
    [MenuItem("Tools/Apply Material To All Colliders")]
    static void ApplyMaterialToAllColliders()
    {
        Object selectedObject = Selection.activeObject;
        PhysicMaterial selectedMaterial = (PhysicMaterial) selectedObject;
        // Open a material selection dialog
     //   PhysicMaterial selectedMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("Select Material", null, typeof(PhysicMaterial), false);

        if (selectedMaterial == null)
        {
            Debug.LogWarning("No material selected. Operation canceled.");
            return;
        }

        // Confirm the action
        if (!EditorUtility.DisplayDialog("Apply Material",
            "This will apply the selected material to all objects with a collider. Proceed?",
            "Yes", "No"))
        {
            return;
        }

        // Find all game objects in the scene
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.material = selectedMaterial;
           
        }

        Debug.Log("Material applied to all colliders.");
    }
}