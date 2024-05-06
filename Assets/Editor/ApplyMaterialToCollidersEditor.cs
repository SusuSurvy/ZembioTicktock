using System.IO;
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
    public class RenameImagesTool : ScriptableObject
    {
        [MenuItem("Tools/Rename Images")]
        static void RenameImages()
        {
            // 设定目标文件夹路径，需要根据你的情况做适当修改
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder with Images", "", "");

            // 获取所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                // 检查是否是图片文件（根据需要可以扩展其他格式）
                if (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string[] parts = fileName.Split('_');
                    if (parts.Length >= 3)
                    {
                        string newFileName = parts[1];
                        string newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName + Path.GetExtension(file));
                    
                        int counter = 1;
                        // 确保不会覆盖已存在的文件
                        while (File.Exists(newFilePath))
                        {
                            newFileName = $"{parts[1]}_{counter}";
                            newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName + Path.GetExtension(file));
                            counter++;
                        }

                        try
                        {
                            File.Move(file, newFilePath);
                            Debug.Log($"Renamed {file} to {newFilePath}");
                        }
                        catch (IOException e)
                        {
                            Debug.LogError($"Error renaming file {file} to {newFilePath}: {e.Message}");
                        }
                    }
                }
            }

            AssetDatabase.Refresh(); // 刷新Unity编辑器的资产数据库
        }
    }
}