using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  
using UnityEditorInternal;
[CustomEditor(typeof(WaveData))]
public class WaveDataEditor : Editor {

 private ReorderableList list;

    private void OnEnable() {
        list = new ReorderableList(serializedObject, 
                serializedObject.FindProperty("listEnemy"), 
                true, true, true, true);
			list.drawElementCallback =  
    (Rect rect, int index, bool isActive, bool isFocused) => {
    var element = list.serializedProperty.GetArrayElementAtIndex(index);
    rect.y += 2;
    EditorGUI.PropertyField(
        new Rect(rect.x, rect.y, 120, EditorGUIUtility.singleLineHeight),
        element.FindPropertyRelative("enemyPrefab"), GUIContent.none);
    EditorGUI.PropertyField(
        new Rect(rect.x + 150, rect.y, rect.width - 150 - 50, EditorGUIUtility.singleLineHeight),
        element.FindPropertyRelative("Count"), GUIContent.none);
    EditorGUI.PropertyField(
        new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
        element.FindPropertyRelative("wait"), GUIContent.none);
};
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
