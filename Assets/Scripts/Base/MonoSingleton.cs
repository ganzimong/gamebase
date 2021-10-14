using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string PrefabRootPath = "";
    private const string SingletonPostfix = "(Singleton)";
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var prefab = Resources.Load<T>(string.Format("{0}{1}", PrefabRootPath, typeof(T).Name));
                    if (prefab != null)
                    {
                        _instance = Instantiate(prefab);
                    }
                    else
                    {
                        GameObject newObj = new GameObject(typeof(T).Name + SingletonPostfix, typeof(T));
                        _instance = newObj.GetComponent<T>();
                    }

                    DontDestroyOnLoad(_instance);
                }
            }

            return _instance;
        }
    }
}