using UnityEngine;

namespace Utils
{
    public class Singleton<T>: MonoBehaviour
    {
        #region Singleton
        private static T singleton;
        public static T GetSingleton() => singleton;
        private static GameObject singletonObject;

        private void ControlSingleton()
        {
            if (singleton == null)
            {
                //Set the singleton to be this object
                singleton = GetComponent<T>();
                singletonObject = gameObject;
                return;
            }
            //Destroy the new instance
            Destroy(singletonObject);
        }
        #endregion

        protected virtual void Awake()
        {
            ControlSingleton();
        }
    }
}