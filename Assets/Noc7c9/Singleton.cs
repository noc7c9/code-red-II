using UnityEngine;

namespace Noc7c9 {

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        static T instance_;
        static object getLock = new object();

        public static T Instance {
            get {
                lock (getLock) {
                    if (instance_ == null) {
                        instance_ = (T) FindObjectOfType(typeof(T));
                        DontDestroyOnLoad(instance_);
                    }
                    return instance_;
                }
            }
        }

    }

}
