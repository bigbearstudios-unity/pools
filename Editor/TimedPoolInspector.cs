﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using BBUnity.Pools;

namespace BBUnity.Editor {
    [CustomEditor(typeof(TimedPool), true)]
    public class TimedPoolInspector : UnityEditor.Editor {

        private ReorderableList _reorderableList;
        private SerializedProperty _serializedPoolDefinitions = null;

        private TimedPool Target {
            get {
                return target as TimedPool;
            }
        }

        private void OnEnable() {
            _serializedPoolDefinitions = serializedObject.FindProperty("_poolDefinitions");

            _reorderableList = new ReorderableList(serializedObject, _serializedPoolDefinitions, true, true, true, true);
            _reorderableList.drawHeaderCallback += DrawHeader;
            _reorderableList.drawElementCallback += DrawElement;
            _reorderableList.onAddCallback += AddItem;
            _reorderableList.onRemoveCallback += RemoveItem;
            _reorderableList.onReorderCallbackWithDetails += ReorderCallbackDelegateWithDetails;
            _reorderableList.elementHeight = 185.0f;
        }

        private void OnDisable() {
            _reorderableList.drawHeaderCallback -= DrawHeader;
            _reorderableList.drawElementCallback -= DrawElement;

            _reorderableList.onAddCallback -= AddItem;
            _reorderableList.onRemoveCallback -= RemoveItem;
            _reorderableList.onReorderCallbackWithDetails -= ReorderCallbackDelegateWithDetails;
        }

        private void DrawHeader(Rect rect) {
            GUI.Label(rect, "Pool Definitions");
        }

        //TODO Tidy up this row rendering, write a helper to position items correctly
        private void DrawElement(Rect rect, int index, bool active, bool focused) {
            EditorGUI.BeginChangeCheck();

            SerializedProperty element = _serializedPoolDefinitions.GetArrayElementAtIndex(index);

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 10, 100, EditorGUIUtility.singleLineHeight), "Name");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 10, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_name"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 35, 100, EditorGUIUtility.singleLineHeight), "Active");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 35, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_active"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 60, 100, EditorGUIUtility.singleLineHeight), "Spawn Time");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 60, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_spawnTime"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 85, 100, EditorGUIUtility.singleLineHeight), "Default Size");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 85, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_startingSize"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 110, 100, EditorGUIUtility.singleLineHeight), "Maximum Size");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 110, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_maximumSize"),
                GUIContent.none
            ); 

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 135, 100, EditorGUIUtility.singleLineHeight), "Prefab");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 135, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_prefab"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 155, 100, EditorGUIUtility.singleLineHeight), "Use Disabled Instances");
            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y + 155, 125, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("_useDisabledInstances"),
                GUIContent.none
            ); 

            if (EditorGUI.EndChangeCheck()) {
                SaveSerializedObject();
            }
        }

        private void AddItem(ReorderableList list) {
            int newIndex = _serializedPoolDefinitions.arraySize;
            _serializedPoolDefinitions.InsertArrayElementAtIndex(newIndex);
            SerializedProperty element = _serializedPoolDefinitions.GetArrayElementAtIndex(newIndex);

            element.FindPropertyRelative("_active").boolValue = TimedPoolDefinition.DefaultActive;
            element.FindPropertyRelative("_startingSize").intValue = BasePoolDefinition.DefaultStartingSize;
            element.FindPropertyRelative("_maximumSize").intValue = BasePoolDefinition.DefaultMaximumSize;
            element.FindPropertyRelative("_spawnTime").floatValue = TimedPoolDefinition.DefaultSpawnTime;

            SaveSerializedObject();
        }

        private void RemoveItem(ReorderableList list) {
            _serializedPoolDefinitions.DeleteArrayElementAtIndex(list.index);
            SaveSerializedObject();
        }

        private void ReorderCallbackDelegateWithDetails(ReorderableList list, int oldIndex, int newIndex) {
            _serializedPoolDefinitions.MoveArrayElement(oldIndex, newIndex);
            SaveSerializedObject();
        }

        private void SaveSerializedObject() {
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.LabelField("Name");
            Target.Name = EditorGUILayout.TextField(Target.Name);

            EditorGUILayout.Space(15.0f);

            _reorderableList.DoLayoutList();
        }
    }
}
