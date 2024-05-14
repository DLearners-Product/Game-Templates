using UnityEngine;

namespace DLearners
{

    public class MonoGenericSingleton<T> : MonoBehaviour where T : MonoGenericSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake()
        {
            Debug.Log(this.gameObject.name, this.gameObject);

            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = (T)this;
            }
        }

    }

}