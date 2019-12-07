using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityFbs.EditorScripts.Settings;

namespace UnityFbs.EditorScripts {
    public class CreateFbs : ScriptableWizard {
        [MenuItem("Assets/Create/FlatBuffers message")]
        [MenuItem("Assets/UnityFbs/Create flatbuffers message here")]
        static void CreateFbsMessage() {
            CreateFbsFile();
        }
        [MenuItem("Assets/UnityFbs/Create flatbuffers message here", true)]
        static bool CreateFbsMessageValidation() {
            return GetSelectedDirectory() != null;
        }

        [MenuItem("Assets/UnityFbs/Set as include root")]
        static void SetAsIncludeRoot() {
            string dir = GetSelectedDirectory();
            if (dir != null) {
                UnityFbsSettings.SetIncludeDirectory(dir);
            }
        }

        [MenuItem("Assets/UnityFbs/Set as include root", true)]
        private static bool SetAsIncludeRootValidation() {
            return GetSelectedDirectory() != null;
        }

        [MenuItem("Assets/UnityFbs/Set as flatc-compiler file")]
        static void SetFlatcCompiler() {
            var files = GetSelectedFiles();
            if (files.Count == 1) {
                UnityFbsSettings.SetFlatcCompiler(files[0]);
            }
        }

        [MenuItem("Assets/UnityFbs/Set as flatc-compiler file", true)]
        private static bool SetFlatcCompilerValidation() {
            return GetSelectedFiles().Count == 1;
        }

        public static void CreateFbsFile() {
            var path = GetSelectedDirectory();
            if (path == null) {
                EditorUtility.DisplayDialog("Please select folder first", "You have not chosen a folder to create a flatbuffer message", "Ok");
                return;
            }

            var fullPath = CreateFullFilename(path, "NewFbsMessage");
            CreatFbsFile(fullPath);
        }

        private static void CreatFbsFile(string fullPath) {
            Debug.Log("Creating file: " + fullPath);
            ProjectWindowUtil.CreateAssetWithContent(fullPath, FBS_EXAMPLE_CONTENT);
        }

        private static string CreateFullFilename(string path, string baseName) {
            if (!File.Exists(Path.Combine(path, baseName + ".fbs"))) {
                return Path.Combine(path, baseName + ".fbs");
            }
            int i;
            for (i = 1; File.Exists(Path.Combine(path, $"{baseName}_{i}.fbs")); ++i) { }
            return Path.Combine(path, $"{baseName}_{i}.fbs");
        }

        public static string GetSelectedDirectory() {
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) {
                    return path;
                }
            }
            return null;
        }
        public static List<string> GetSelectedFiles() {
            List<string> files = new List<string>();
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
                    files.Add(path);
                }
            }
            return files;
        }

        private const string FBS_EXAMPLE_CONTENT =
@"// Taken from here: https://google.github.io/flatbuffers/flatbuffers_guide_tutorial.html
// Example IDL file for our monster's schema. 
namespace MyGame.Sample;
enum Color:byte { Red = 0, Green, Blue = 2 }
union Equipment { Weapon } // Optionally add more tables.
struct Vec3 {
  x:float;
  y:float;
  z:float;
}
table Monster {
  pos:Vec3; // Struct.
  mana:short = 150;
  hp:short = 100;
  name:string;
  friendly:bool = false (deprecated);
  inventory:[ubyte];  // Vector of scalars.
  color:Color = Blue; // Enum.
  weapons:[Weapon];   // Vector of tables.
  equipped:Equipment; // Union.
  path:[Vec3];        // Vector of structs.
}
table Weapon {
  name:string;
  damage:short;
}
root_type Monster;";

    }
}