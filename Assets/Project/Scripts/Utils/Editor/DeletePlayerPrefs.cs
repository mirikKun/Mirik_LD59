using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Utils.Editor
{
    public static class DeletePlayerPrefs
    {
        [MenuItem("Tools/Delete All PlayerPrefs")]
        public static void DeleteAll()
        {
            if (EditorUtility.DisplayDialog("Delete All PlayerPrefs",
                "This will delete ALL PlayerPrefs data. Are you sure?",
                "Yes", "Cancel"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("All PlayerPrefs have been deleted.");
            }
        }
    }
}

