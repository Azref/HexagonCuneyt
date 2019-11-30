using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.Editor.Code.ContextList
{
  [CustomPropertyDrawer(typeof(AppContextAttribute))]
  class AppContextAttributeEditor : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      List<string> paths = CodeUtilities.GetContextNames();

      property.intValue = EditorGUI.Popup(position, label.text, property.intValue, paths.ToArray());
    }
  }
}