using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Utils.Editor
{
    [CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
    public class FloatRangeSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int originalIndentLevel = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
        
            SerializedProperty minProperty = property.FindPropertyRelative("_min");
            SerializedProperty maxProperty = property.FindPropertyRelative("_max");
        
            float minValue = minProperty.floatValue;
            float maxValue = maxProperty.floatValue;
        
            // Calculate layout for UI elements
            float fieldWidth = position.width / 4f - 4f;
            float sliderWidth = position.width / 2f;
        
            // Min field
            position.width = fieldWidth;
            minValue = EditorGUI.FloatField(position, minValue);
        
            // Min-Max slider
            position.x += fieldWidth + 4f;
            position.width = sliderWidth;
        
            FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, limit.Min, limit.Max);
        
            // Max field
            position.x += sliderWidth + 4f;
            position.width = fieldWidth;
            maxValue = EditorGUI.FloatField(position, maxValue);
        
            // Validate values
            minValue = Mathf.Clamp(minValue, limit.Min, maxValue);
            maxValue = Mathf.Clamp(maxValue, minValue, limit.Max);
        
            // Apply values back to the properties
            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;
        
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = originalIndentLevel;
        }
    }
}