using UnityEngine;
using UnityEditor;
using TMPro;

// MonoBehaviour yerine EditorWindow kullanmali
public class FontChanger : EditorWindow
{
    public TMP_FontAsset targetFont;

    [MenuItem("Tools/Font Replacer")]
    public static void ShowWindow()
    {
        GetWindow<FontChanger>("Font Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Global Font Settings", EditorStyles.boldLabel);

        targetFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", targetFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Update All Fonts in Scene"))
        {
            if (targetFont == null)
            {
                Debug.LogError("Assign a Font Asset first!");
                return;
            }

            TMP_Text[] allTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            int count = 0;

            foreach (var txt in allTexts)
            {
                if (txt.gameObject.scene.name != null)
                {
                    Undo.RecordObject(txt, "Bulk Font Change");
                    txt.font = targetFont;
                    EditorUtility.SetDirty(txt);
                    count++;
                }
            }
            Debug.Log("Success: " + count + " objects updated.");
        }
    }
}