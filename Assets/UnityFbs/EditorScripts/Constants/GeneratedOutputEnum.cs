using System;
using System.ComponentModel;
using System.Reflection;

namespace UnityFbs.EditorScripts.Constants {
    enum GeneratedOutputEnum : byte {
        [Argument("--csharp")]
        cs,
        [Argument("--python")]
        python,
        [Argument("--cpp")]
        cpp
    };
    class Argument : Attribute {
        public string Text;
        public Argument(string text) {
            Text = text;
        }
    }

    static class EnumExtensions {
        public static string ToArgument(this GeneratedOutputEnum en) {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0) {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(Argument), false);
                if (attrs != null && attrs.Length > 0) return ((Argument)attrs[0]).Text;
            }

            return en.ToString();
        }
    }
}
