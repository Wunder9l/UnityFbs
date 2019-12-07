using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityFbs.EditorScripts.Settings {
    // More about: https://docs.unity3d.com/ScriptReference/SettingsProvider.html
    // and here https://forum.unity.com/threads/using-settingsprovider-values-in-project-settings-window-are-saved-to-asset.693910/
    class UnityFbsSettings : ScriptableObject {
        public const string SettingsPath = "Assets/UnityFbs/Settings/UnityFbsSettings.asset";

        [SerializeField]
        private string flatcPath;
        public string FlatcPath { get => flatcPath; }

        [SerializeField]
        private List<string> csAdditionalArguments;
        public List<string> CsAdditionalArguments { get => csAdditionalArguments; }
        [SerializeField]
        private List<string> pythonAdditionalArguments;
        public List<string> PythonAdditionalArguments { get => pythonAdditionalArguments; }
        [SerializeField]
        private List<string> cppAdditionalArguments;
        public List<string> CppAdditionalArguments { get => cppAdditionalArguments; }

        private static string GetDefaultFlatcPath() {
            switch (SystemInfo.operatingSystemFamily) {
                case OperatingSystemFamily.Windows:
                    return "Assets/UnityFbs/Plugins/FlatBuffers/Windows/flatc.exe";
                case OperatingSystemFamily.MacOSX:
                    return "Assets/UnityFbs/Plugins/FlatBuffers/MacOSX/flatc";
                case OperatingSystemFamily.Linux:
                    return "Assets/UnityFbs/Plugins/FlatBuffers/MacOSX/flatc";
            }
            return "";
        }

        internal static bool Exists() {
            return File.Exists(SettingsPath);
        }

        internal static UnityFbsSettings GetOrCreateSettings() {
            var settings = AssetDatabase.LoadAssetAtPath<UnityFbsSettings>(SettingsPath);
            if (settings == null) {
                settings = CreateInstance<UnityFbsSettings>();
                settings.flatcPath = GetDefaultFlatcPath();
                settings.csAdditionalArguments = new List<string> { GetDefaultIncludeArgument() };
                settings.pythonAdditionalArguments = new List<string> { GetDefaultIncludeArgument() };
                settings.cppAdditionalArguments = new List<string> { GetDefaultIncludeArgument() };
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        private static string GetDefaultIncludeArgument() {
            return "-I Assets";
        }

        public static void SetFlatcCompiler(string flatc) {
            var settings = GetSerializedSettings();
            var property = settings.FindProperty("flatcPath");
            property.stringValue = flatc;
            settings.ApplyModifiedProperties();
        }

        public static void SetIncludeDirectory(string dir) {
            var settings = GetSerializedSettings();
            SetIncludeDirectory(dir, settings.FindProperty("csAdditionalArguments"));
            SetIncludeDirectory(dir, settings.FindProperty("pythonAdditionalArguments"));
            SetIncludeDirectory(dir, settings.FindProperty("cppAdditionalArguments"));

            settings.ApplyModifiedProperties();
        }
        private static bool SetIncludeDirectory(string dir, SerializedProperty property) {
            if (property.propertyType == SerializedPropertyType.String) {
                if (property.stringValue?.StartsWith("-I ") == true) {
                    property.stringValue = $"-I {dir}";
                    return true;
                }
            } else if (property.isArray && property.arrayElementType == "string") {
                for (int i = 0; i < property.arraySize; ++i) {
                    if (SetIncludeDirectory(dir, property.GetArrayElementAtIndex(i))) {
                        return true;
                    }
                }
                // if not found - create new one
                var newIndex = property.arraySize;
                property.InsertArrayElementAtIndex(newIndex);
                property.GetArrayElementAtIndex(newIndex).stringValue = $"-I {dir}";
                return true;
            }
            return false;
        }

        internal static SerializedObject GetSerializedSettings() {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
