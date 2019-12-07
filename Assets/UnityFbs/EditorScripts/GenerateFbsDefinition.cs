using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityFbs.EditorScripts.Constants;
using UnityFbs.EditorScripts.Settings;

namespace UnityFbs.EditorScripts {
    public class GenerateFbsDefinition : Editor {
        private const string FAIL_DIALOG_TITLE = "Convert FlatBuffers message failed";

        [MenuItem("Assets/UnityFbs/Generate C# definition")]
        private static void UnityFbsGenerateCSDefinition() {
            GenerateDefinition(GeneratedOutputEnum.cs);
        }

        [MenuItem("Assets/UnityFbs/Generate C# definition", true)]
        private static bool UnityFbsGenerateCSDefinitionValidation() {
            return CheckFbsMessageSelected();
        }


        [MenuItem("Assets/UnityFbs/Generate Python definition")]
        private static void UnityFbsGeneratePythonDefinition() {
            GenerateDefinition(GeneratedOutputEnum.python);
        }

        [MenuItem("Assets/UnityFbs/Generate Python definition", true)]
        private static bool UnityFbsGeneratePythonDefinitionValidation() {
            return CheckFbsMessageSelected();
        }

        [MenuItem("Assets/UnityFbs/Generate C++ definition")]
        private static void UnityFbsGenerateCppDefinition() {
            GenerateDefinition(GeneratedOutputEnum.cpp);
        }

        [MenuItem("Assets/UnityFbs/Generate C++ definition", true)]
        private static bool UnityFbsGenerateCppDefinitionValidation() {
            return CheckFbsMessageSelected();
        }

        private static void GenerateDefinition(GeneratedOutputEnum target) {
            var files = GetSelectedFbsFiles();
            string fileOrFiles = (files.Count == 1) ? "file" : "files";
            Debug.Log($"Generating {target} {fileOrFiles} for {string.Join(", ", files)}");

            var exe = new ExeRunner(UnityFbsSettings.GetOrCreateSettings().FlatcPath);
            var errors = exe.Run(files, target);
            if (errors.Length > 0) {
                EditorUtility.DisplayDialog(FAIL_DIALOG_TITLE, $"Generating {target} definition for [{string.Join(", ", files)}] failed:\n{errors}", "Ok");
            }
            AssetDatabase.Refresh();
        }

        private static bool CheckFbsMessageSelected() {
            var files = GetSelectedFbsFiles();
            return files.Count > 0;
        }

        private static List<string> GetSelectedFbsFiles() {
            List<string> files = new List<string>();
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path) && path.EndsWith(".fbs")) {
                    files.Add(path);
                }
            }
            return files;
        }
    }
}