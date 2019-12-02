using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using System.IO;
using UnityFbs.EditorScripts.Constants;
using UnityFbs.EditorScripts.Settings;

namespace UnityFbs.EditorScripts {
    class ExeRunner {
        private string flatcPath;

        public ExeRunner(string flatcPath) {
            this.flatcPath = flatcPath;
        }

        public string Run(List<string> inputFiles, GeneratedOutputEnum output) {
            if (inputFiles.Count == 0) return "";
            if (!CheckFile(flatcPath)) {
                return $"flatc-executable file not found, please check UnityFbsSettings for correctness. Current file path is {flatcPath}";
            }

            string arguments = MakeArguments(inputFiles, output);
            UnityEngine.Debug.Log($"Run fltac with args: {arguments}");
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = Path.GetFullPath(flatcPath),
                    Arguments = MakeArguments(inputFiles, output),
                    UseShellExecute = false,
                    //RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            var started = process.Start();
            if (!started) {
                return "Process flatc was not started";
            }

            StringBuilder errors = new StringBuilder();
            while (!process.StandardOutput.EndOfStream) {
                var line = process.StandardOutput.ReadLine();
                UnityEngine.Debug.LogError(line);
                errors.Append(line);
            }

            process.WaitForExit();
            return errors.ToString();
        }

        private bool CheckFile(string path) {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        private string MakeArguments(List<string> inputFiles, GeneratedOutputEnum output) {
            var outputDir = Path.GetDirectoryName(inputFiles.First());
            var builder = new StringBuilder(output.ToArgument());
            builder.Append(" --gen-onefile");
            builder.Append($" -o {outputDir}");
            GetAdditionalArgs(output).ForEach(arg => builder.Append($" {arg}"));
            inputFiles.ForEach(f => builder.Append($" {f}"));
            return builder.ToString();
        }

        private List<string> GetAdditionalArgs(GeneratedOutputEnum output) {
            switch (output) {
                case GeneratedOutputEnum.cs:
                    return UnityFbsSettings.GetOrCreateSettings().CsAdditionalArguments;
                case GeneratedOutputEnum.python:
                    return UnityFbsSettings.GetOrCreateSettings().PythonAdditionalArguments;
                case GeneratedOutputEnum.cpp:
                    return UnityFbsSettings.GetOrCreateSettings().CppAdditionalArguments;
            }
            return new List<string>();
        }
    }
}
