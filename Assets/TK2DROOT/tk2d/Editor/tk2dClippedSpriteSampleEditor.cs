using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(tk2dClippedSpriteSample))]
class tk2dClippedSpriteSampleEditor : tk2dSpriteEditor
{
	public override void OnInspectorGUI()
    {
        tk2dClippedSpriteSample sprite = (tk2dClippedSpriteSample)target;
		base.OnInspectorGUI();
		
		GUILayout.Label("Clip region");
		EditorGUI.indentLevel++;
		sprite.clipBottomLeft = EditorGUILayout.Vector2Field("Bottom Left", sprite.clipBottomLeft);
		sprite.clipTopRight = EditorGUILayout.Vector2Field("Top Right", sprite.clipTopRight);
		EditorGUI.indentLevel--;
    }

    [MenuItem("GameObject/Create Other/tk2d/Clipped Sprite (Sample)", false, 12901)]
    static void DoCreateClippedSpriteObject()
    {
		tk2dSpriteCollectionData sprColl = null;
		if (sprColl == null)
		{
			// try to inherit from other Sprites in scene
			tk2dSprite spr = GameObject.FindObjectOfType(typeof(tk2dSprite)) as tk2dSprite;
			if (spr)
			{
				sprColl = spr.Collection;
			}
		}

		if (sprColl == null)
		{
			tk2dSpriteCollectionIndex[] spriteCollections = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
			foreach (var v in spriteCollections)
			{
				GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(v.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
				var sc = scgo.GetComponent<tk2dSpriteCollectionData>();
				if (sc != null && sc.spriteDefinitions != null && sc.spriteDefinitions.Length > 0)
				{
					sprColl = sc;
					break;
				}
			}

			if (sprColl == null)
			{
				EditorUtility.DisplayDialog("Create Clipped Sprite", "Unable to create clipped sprite as no SpriteCollections have been found.", "Ok");
				return;
			}
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Clipped Sprite");
		tk2dClippedSpriteSample sprite = go.AddComponent<tk2dClippedSpriteSample>();
		sprite.Collection = sprColl;
		sprite.Build();

		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create Clipped Sprite");
    }
}

