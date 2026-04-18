using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Project.Scripts.Utils;
using Project.Scripts.Utils.Attributes;

namespace Project.Scripts.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        TypeFilterAttribute typeFilter;
        string[] typeNames, typeFullNames;

        void Initialize()
        {
            if (typeFullNames != null) return;
            
            typeFilter = (TypeFilterAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            
            var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeFilter == null ? DefaultFilter(t) : typeFilter.Filter(t))
                .ToArray();
            
            typeNames = filteredTypes.Select(t => 
            {
                // if (t.DeclaringType != null)
                //     return $"{t.DeclaringType.Name}.{t.Name}";
                // return t.FullName ?? t.Name;
                if (t.DeclaringType != null)
                    return $"{t.Name}";
                return t.Name;
            }).ToArray();
            typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }
        
        static bool DefaultFilter(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize();
            var typeIdProperty = property.FindPropertyRelative("assemblyQualifiedName");

            if (string.IsNullOrEmpty(typeIdProperty.stringValue) && typeFullNames.Length > 0)
            {
                typeIdProperty.stringValue = typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            var currentIndex = Array.IndexOf(typeFullNames, typeIdProperty.stringValue);
            var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            
            if (selectedIndex >= 0 && selectedIndex != currentIndex)
            {
                typeIdProperty.stringValue = typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
