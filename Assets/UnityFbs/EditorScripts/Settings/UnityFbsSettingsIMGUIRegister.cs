using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityFbs.EditorScripts.Settings {
    class UnityFbsSettingsIMGUIRegister : SettingsProvider {

        private SerializedObject settings;
        class Styles {
            public static GUIContent FlatcPath = new GUIContent("Path to flatc");
            public static GUIContent CsAdditionalArguments = new GUIContent("Additional arguments for .cs generating");
            public static GUIContent PythonAdditionalArguments = new GUIContent("Additional arguments for .py generating");
            public static GUIContent CppAdditionalArguments = new GUIContent("Additional arguments for .h (c++) generating");
        }

        public UnityFbsSettingsIMGUIRegister(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public static bool IsSettingsAvailable() {
            return true;
            //return UnityFbsSettings.Exists();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement) {
            // This function is called when the user clicks on the SmartNSSettings element in the Settings window.
            settings = UnityFbsSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext) {
            settings.Update();

            // Preferences GUI
            EditorGUILayout.HelpBox("UnityFbs introduces flatbuffers to Unity. You can create .fbs files and generate definition for different languages (for C# for example) just by right mouse click. UnityFbs uses flatc to generate definitions, please provide a link to actual file", MessageType.None);

            EditorGUILayout.PropertyField(settings.FindProperty("flatcPath"), Styles.FlatcPath);
            EditorGUILayout.PropertyField(settings.FindProperty("csAdditionalArguments"), Styles.CsAdditionalArguments, true);
            EditorGUILayout.PropertyField(settings.FindProperty("pythonAdditionalArguments"), Styles.PythonAdditionalArguments, true);
            EditorGUILayout.PropertyField(settings.FindProperty("cppAdditionalArguments"), Styles.CppAdditionalArguments, true);

            settings.ApplyModifiedProperties();
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateSmartNSSettingsProvider() {
            if (IsSettingsAvailable()) {
                var provider = new UnityFbsSettingsIMGUIRegister("Project/UnityFbs", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
                provider.label = "UnityFbs";
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }
    }
}
