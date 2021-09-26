using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string SingletonPostfix = "(Singleton)";
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<T>();
                if (obj != null)
                {
                    _instance = obj;

                    DontDestroyOnLoad(obj);
                }
                else
                {
                    GameObject newObj = new GameObject(typeof(T).Name + SingletonPostfix, typeof(T));
                    _instance = newObj.GetComponent<T>();

                    DontDestroyOnLoad(newObj);
                }
            }

            return _instance;
        }
    }
}