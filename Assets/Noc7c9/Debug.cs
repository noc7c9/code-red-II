using UnityEngine;
using System;

namespace Noc7c9 {

    public static class Debug {

        static string[] ConvertToStringArray(object[] objs) {
            return Array.ConvertAll(objs, o => o == null ? "null" : o.ToString());
        }

        public static void Logf(this object _, string format, params object[] objs) {
            Debug.Log(string.Format(format, ConvertToStringArray(objs)));
        }

        public static void Log(this object _, params object[] objs) {
            Debug.Log(string.Join(" ", ConvertToStringArray(objs)));
        }

    }

}
