using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CommadManager))]
public class CommadManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CommadManager commadManager =(CommadManager) target;
        GUIStyle style = new GUIStyle(GUI.skin.customStyles[2]);
        style.fixedWidth = 500;
        if (commadManager.commands.Count > 0)
        {
            EditorGUILayout.BeginVertical(style);
            for (int i = 0; i < commadManager.commands.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("第{0}个执行命令：{1}", i + 1, commadManager.commands.ToArray()[i].GetType()));
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();


        if (commadManager.unDoCommands.Count > 0)
        {
            EditorGUILayout.BeginVertical(style);

            for (int i = 0; i < commadManager.unDoCommands.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("第{0}个撤销命令：{1}", commadManager.unDoCommands.Count - i, commadManager.unDoCommands.ToArray()[i].GetType()));
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();


        if (commadManager.nextCommands.Count > 0)
        {

            EditorGUILayout.BeginVertical(style);

            for (int i = 0; i < commadManager.nextCommands.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("第{0}个临时执行命令：{1}", i + 1, commadManager.nextCommands.ToArray()[i].GetType()));
            }

            EditorGUILayout.EndVertical();
        }

    }

}
