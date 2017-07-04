using UnityEngine;
using System;

namespace Noc7c9 {

    public static class Debug {

        static string[] ConvertToStringArray(object[] objs) {
            return Array.ConvertAll(objs, o => o == null ? "null" : o.ToString());
        }

        public static void Logf(string format, params object[] objs) {
            UnityEngine.Debug.Log(string.Format(format, ConvertToStringArray(objs)));
        }

        public static void Log(params object[] objs) {
            UnityEngine.Debug.Log(string.Join(" ", ConvertToStringArray(objs)));
        }

        // MonoBehaviour extension methods
        public static void logf(this MonoBehaviour mb,
                string format, params object[] objs) {
            Debug.Logf(format, objs);
        }

        public static void log(this MonoBehaviour mb,
                params object[] objs) {
            Debug.Log(objs);
        }

    }

}
