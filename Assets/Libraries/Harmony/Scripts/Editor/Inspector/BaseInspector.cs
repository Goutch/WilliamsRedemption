using System;
using System.Collections.Generic;
using Rotorz.ReorderableList;
using Rotorz.ReorderableList.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Harmony
{
    /// <summary>
    /// Base pour créer facilement un inspecteur personalisé sous Unity, avec quelques fonctionalitées supplémentaires.
    /// </summary>
    /// <inheritdoc />
    public abstract class BaseInspector : UnityEditor.Editor
    {
        private bool isRefreshing;

        private void Awake()
        {
            Initialize();
        }

        protected void DrawDefault()
        {
            var property = serializedObject.GetIterator();
            var hasFirst = property.NextVisible(true);
            if (hasFirst)
            {
                do
                {
                    DrawSerializedProperty(property);
                } while (property.NextVisible(false));
            }
        }

        private static void DrawSerializedProperty(SerializedProperty property)
        {
            var isDefaultScriptProperty = property.name.Equals("m_Script") &&
                                          property.type.Equals("PPtr<MonoScript>") &&
                                          property.propertyType == SerializedPropertyType.ObjectReference &&
                                          property.propertyPath.Equals("m_Script");
            if (isDefaultScriptProperty)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(property, property.isExpanded);
                GUI.enabled = true;
            }
            else if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                EditorGUILayout.PropertyField(property, false);
                if (property.isExpanded)
                {
                    ReorderableListGUI.ListField(property,
                        () => EditorGUILayout.LabelField(property.displayName),
                        ReorderableListFlags.DisableContextMenu);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                EditorGUILayout.PropertyField(property, property.isExpanded);
            }
        }

        protected BasicProperty GetBasicProperty(string name)
        {
            return new BasicProperty(serializedObject.FindProperty(name));
        }

        protected ListProperty GetListProperty(string name, bool isFixedSize = false, int startIndex = 0)
        {
            return new ListProperty(serializedObject.FindProperty(name), isFixedSize, startIndex);
        }

        protected GridProperty GetGridProperty(string name, int nbCols = 3)
        {
            return new GridProperty(serializedObject.FindProperty(name), nbCols);
        }

        protected EnumProperty GetEnumProperty(string name, Type enumType)
        {
            return new EnumProperty(serializedObject.FindProperty(name), enumType);
        }

        protected void DrawProperty(ICustomProperty property)
        {
            if (isRefreshing) return;

            //"property" might be null. Thus, we can't convert to method group.
            //ReSharper disable once ConvertClosureToMethodGroup
            RefreshIfNeededOrDraw(property, () => property.Draw());
        }

        protected void DrawPropertyWithLabel(ICustomProperty property)
        {
            if (isRefreshing) return;

            //"property" might be null. Thus, we can't convert to method group.
            //ReSharper disable once ConvertClosureToMethodGroup
            RefreshIfNeededOrDraw(property, () => property.DrawWithLabel());
        }


        protected void DrawPropertyWithTitleLabel(ICustomProperty property)
        {
            if (isRefreshing) return;

            //"property" might be null. Thus, we can't convert to method group.
            //ReSharper disable once ConvertClosureToMethodGroup
            RefreshIfNeededOrDraw(property, () => property.DrawWithTitleLabel());
        }

        protected void BeginHorizontal()
        {
            if (isRefreshing) return;

            EditorGUILayout.BeginHorizontal();
        }

        protected void EndHorizontal()
        {
            if (isRefreshing) return;

            EditorGUILayout.EndHorizontal();
        }

        protected void BeginVertical()
        {
            if (isRefreshing) return;

            EditorGUILayout.BeginVertical();
        }

        protected void EndVertical()
        {
            if (isRefreshing) return;

            EditorGUILayout.EndVertical();
        }

        protected void BeginTable()
        {
            if (isRefreshing) return;

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        }

        protected void EndTable()
        {
            if (isRefreshing) return;

            EditorGUILayout.EndHorizontal();
        }

        protected void BeginTableRow()
        {
            if (isRefreshing) return;

            EditorGUILayout.BeginHorizontal();
        }

        protected void EndTableRow()
        {
            if (isRefreshing) return;

            EditorGUILayout.EndHorizontal();
        }

        protected void BeginTableCol()
        {
            if (isRefreshing) return;

            EditorGUILayout.BeginVertical();
        }

        protected void EndTableCol()
        {
            if (isRefreshing) return;

            EditorGUILayout.EndVertical();
        }

        protected void DrawLabel(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.LabelField(text);
        }

        protected void DrawTitleLabel(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        protected void DrawSection(string text)
        {
            if (isRefreshing) return;

            var style = EditorStyles.largeLabel;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 15;
            style.fixedHeight = 25;
            EditorGUILayout.LabelField(text, style);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected void DrawImage(Texture2D image)
        {
            if (isRefreshing) return;

            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(image, centeredStyle);
        }

        protected void DrawTableCell(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.TextArea(text, EditorStyles.label);
        }

        protected void DrawTableCell(string text, Color color)
        {
            if (isRefreshing) return;

            var guiStyle = new GUIStyle(EditorStyles.label) {normal = {textColor = color}};
            EditorGUILayout.TextArea(text, guiStyle);
        }

        protected void DrawTableHeader(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.TextArea(text, EditorStyles.boldLabel);
        }

        protected void DrawInfoBox(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.HelpBox(text, MessageType.Info);
        }

        protected void DrawWarningBox(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.HelpBox(text, MessageType.Warning);
        }

        protected void DrawErrorBox(string text)
        {
            if (isRefreshing) return;

            EditorGUILayout.HelpBox(text, MessageType.Error);
        }

        protected void DrawButton(string text, UnityAction actionOnClick)
        {
            if (isRefreshing) return;

            if (GUILayout.Button(text))
            {
                actionOnClick();
            }
        }

        protected void DrawDisabledButton(string text)
        {
            if (isRefreshing) return;

            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Button(text);
            EditorGUI.EndDisabledGroup();
        }

        protected Color DrawColorField(string text, Color color)
        {
            return EditorGUILayout.ColorField(text, color);
        }

        private void RefreshIfNeededOrDraw(ICustomProperty property, UnityAction drawAction)
        {
            if (property == null || property.NeedRefresh())
            {
                Initialize();
                isRefreshing = true;
                Repaint();
            }
            else
            {
                drawAction();
            }
        }

        public override void OnInspectorGUI()
        {
            if (isRefreshing && Event.current.type == EventType.Repaint)
            {
                return;
            }

            if (isRefreshing && Event.current.type == EventType.Layout)
            {
                isRefreshing = false;
            }

            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void Initialize();

        protected abstract void Draw();

        protected interface ICustomProperty
        {
            void Draw();
            void DrawWithLabel();
            void DrawWithTitleLabel();

            bool NeedRefresh();
        }

        protected sealed class BasicProperty : ICustomProperty
        {
            private readonly SerializedProperty property;

            public BasicProperty(SerializedProperty property)
            {
                this.property = property;
            }

            public void Draw()
            {
                EditorGUILayout.PropertyField(property, GUIContent.none);
                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithLabel()
            {
                EditorGUILayout.PropertyField(property);
                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithTitleLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(property, GUIContent.none);
                property.serializedObject.ApplyModifiedProperties();
            }

            public bool IsNull()
            {
                return property.objectReferenceValue == null;
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }
        }

        protected sealed class ListProperty : ICustomProperty
        {
            private readonly SerializedProperty property;
            private readonly bool isFixedSize;
            private readonly int startIndex;

            public ListProperty(SerializedProperty property,
                                bool isFixedSize,
                                int startIndex)
            {
                this.property = property;
                this.isFixedSize = isFixedSize;
                this.startIndex = startIndex;
            }

            public void Draw()
            {
                ReorderableListGUI.ListField(new ListPropertyAdapter(property, startIndex),
                                             () => EditorGUILayout.LabelField(property.displayName),
                                             isFixedSize
                                                 ? ReorderableListFlags.DisableReordering |
                                                   ReorderableListFlags.HideAddButton |
                                                   ReorderableListFlags.HideRemoveButtons |
                                                   ReorderableListFlags.DisableContextMenu
                                                 : ReorderableListFlags.DisableContextMenu);
                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);
                ReorderableListGUI.ListField(new ListPropertyAdapter(property, startIndex),
                                             () => EditorGUILayout.LabelField(property.displayName),
                                             isFixedSize
                                                 ? ReorderableListFlags.DisableReordering |
                                                   ReorderableListFlags.HideAddButton |
                                                   ReorderableListFlags.HideRemoveButtons |
                                                   ReorderableListFlags.DisableContextMenu
                                                 : ReorderableListFlags.DisableContextMenu);
                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithTitleLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);
                ReorderableListGUI.ListField(new ListPropertyAdapter(property, startIndex),
                                             () => EditorGUILayout.LabelField(property.displayName),
                                             isFixedSize
                                                 ? ReorderableListFlags.DisableReordering |
                                                   ReorderableListFlags.HideAddButton |
                                                   ReorderableListFlags.HideRemoveButtons |
                                                   ReorderableListFlags.DisableContextMenu
                                                 : ReorderableListFlags.DisableContextMenu);
                property.serializedObject.ApplyModifiedProperties();
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }
        }

        protected sealed class GridProperty : ICustomProperty
        {
            private readonly SerializedProperty property;
            private readonly int nbCols;

            public GridProperty(SerializedProperty property,
                                int nbCols)
            {
                this.property = property;
                this.nbCols = nbCols;
            }

            public void Draw()
            {
                EditorGUILayout.BeginVertical();

                var numCol = 0;
                for (var i = 0; i < property.arraySize; i++)
                {
                    if (numCol == 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }

                    var element = property.GetArrayElementAtIndex(i);

                    //If it's Texture2D or Sprite, draw the right picker
                    if (property.arrayElementType == "PPtr<$Sprite>")
                    {
                        element.objectReferenceValue = EditorGUILayout.ObjectField("",
                                                                                   element.objectReferenceValue,
                                                                                   typeof(Sprite),
                                                                                   false,
                                                                                   GUILayout.Width(64));
                    }
                    else if (property.arrayElementType == "PPtr<$Texture2D>")
                    {
                        element.objectReferenceValue = EditorGUILayout.ObjectField("",
                                                                                   element.objectReferenceValue,
                                                                                   typeof(Texture2D),
                                                                                   false,
                                                                                   GUILayout.Width(64));
                    }
                    else
                    {
                        EditorGUILayout.ObjectField(element);
                    }

                    numCol++;
                    if (numCol == nbCols)
                    {
                        numCol = 0;
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.EndVertical();

                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);
                Draw();
            }

            public void DrawWithTitleLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);
                Draw();
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }
        }

        protected sealed class EnumProperty : ICustomProperty
        {
            private readonly SerializedProperty property;

            private List<EnumPropertyValue> Values { get; }
            private string[] ValuesAsString { get; }

            private int ValueIndex
            {
                get
                {
                    for (var i = 0; i < Values.Count; i++)
                    {
                        if (property.intValue == Values[i].AsInt)
                        {
                            return i;
                        }
                    }

                    return -1;
                }
                set
                {
                    if (value >= 0 && value < Values.Count)
                    {
                        property.intValue = Values[value].AsInt;
                    }
                }
            }

            public EnumProperty(SerializedProperty property, Type enumType)
            {
                this.property = property;

                //Enum values
                var enumValues = Enum.GetValues(enumType);
                //Enum value names
                var enumNames = Enum.GetNames(enumType);

                //Create a list of enum values to display and sort it by name
                //If there is a "None" value, make it first.
                Values = new List<EnumPropertyValue>();
                var noneValueIndex = -1;
                for (var i = 0; i < enumValues.Length; i++)
                {
                    if (enumNames[i] == "None")
                    {
                        noneValueIndex = i;
                    }
                    else
                    {
                        Values.Add(new EnumPropertyValue((int) enumValues.GetValue(i), enumNames[i]));
                    }
                }

                Values.Sort((displayable1, displayable2) => string.Compare(displayable1.AsString, displayable2.AsString, StringComparison.Ordinal));
                if (noneValueIndex != -1)
                {
                    Values.Insert(0, new EnumPropertyValue((int) enumValues.GetValue(noneValueIndex), enumNames[noneValueIndex]));
                }

                //Create array of enum value names
                ValuesAsString = new string[Values.Count];
                for (var i = 0; i < enumNames.Length; i++)
                {
                    ValuesAsString[i] = Values[i].AsString;
                }
            }

            public void Draw()
            {
                ValueIndex = EditorGUILayout.Popup(ValueIndex,
                                                   ValuesAsString);
                if (ValueIndex == -1)
                {
                    EditorGUILayout.HelpBox("Current value doesn't exist anymore. Please assign a new one.", MessageType.Warning);
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);

                if (ValueIndex == -1)
                {
                    EditorGUILayout.HelpBox("Current value doesn't exist anymore. Please assign a new one.", MessageType.Warning);
                }

                ValueIndex = EditorGUILayout.Popup(ValueIndex,
                                                   ValuesAsString);
                property.serializedObject.ApplyModifiedProperties();
            }

            public void DrawWithTitleLabel()
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);

                if (ValueIndex == -1)
                {
                    EditorGUILayout.HelpBox("Current value doesn't exist anymore. Please assign a new one.", MessageType.Warning);
                }

                ValueIndex = GUILayout.SelectionGrid(ValueIndex,
                                                     ValuesAsString,
                                                     2,
                                                     EditorStyles.radioButton);
                EditorGUILayout.Space();
                property.serializedObject.ApplyModifiedProperties();
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }
        }

        private sealed class EnumPropertyValue
        {
            public int AsInt { get; }
            public string AsString { get; }

            public EnumPropertyValue(int asInt, string asString)
            {
                AsInt = asInt;
                AsString = asString;
            }
        }

        private sealed class ListPropertyAdapter : IReorderableListAdaptor
        {
            private readonly SerializedProperty property;
            private readonly int startIndex;

            public ListPropertyAdapter(SerializedProperty property, int startIndex)
            {
                this.property = property;
                this.startIndex = startIndex;
            }

            public int Count
            {
                get
                {
                    var count = property.arraySize - startIndex;
                    return count < 0 ? 0 : count;
                }
            }

            public bool CanDrag(int index)
            {
                return true;
            }

            public bool CanRemove(int index)
            {
                return true;
            }

            public void Add()
            {
                var arraySize = property.arraySize;
                ++property.arraySize;
                SerializedPropertyUtility.ResetValue(property.GetArrayElementAtIndex(arraySize));
            }

            public void Insert(int index)
            {
                property.InsertArrayElementAtIndex(index + startIndex);
                SerializedPropertyUtility.ResetValue(property.GetArrayElementAtIndex(index));
            }

            public void Duplicate(int index)
            {
                property.InsertArrayElementAtIndex(index + startIndex);
            }

            public void Remove(int index)
            {
                var arrayElementAtIndex = property.GetArrayElementAtIndex(index + startIndex);
                if (arrayElementAtIndex.propertyType == SerializedPropertyType.ObjectReference)
                {
                    arrayElementAtIndex.objectReferenceValue = null;
                }

                property.DeleteArrayElementAtIndex(index + startIndex);
            }

            public void Move(int sourceIndex, int destIndex)
            {
                if (destIndex > sourceIndex)
                {
                    --destIndex;
                }

                property.MoveArrayElement(sourceIndex + startIndex, destIndex + startIndex);
            }

            public void Clear()
            {
                property.ClearArray();
            }

            public void BeginGUI()
            {
                //Not needed, thus empty.
            }

            public void EndGUI()
            {
                //Not needed, thus empty.
            }

            public void DrawItemBackground(Rect position, int index)
            {
                //Not needed, thus empty.
            }

            public void DrawItem(Rect position, int index)
            {
                EditorGUI.PropertyField(position, property.GetArrayElementAtIndex(index + startIndex), GUIContent.none, false);
            }

            public float GetItemHeight(int index)
            {
                return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index + startIndex), GUIContent.none, false);
            }
        }
    }
}