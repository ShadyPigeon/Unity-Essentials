using UnityEngine;

namespace ShadyPigeon.UnityEssentials
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance = null;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    var type = typeof(T);
                    m_Instance = (T)FindObjectOfType(type);
                    if (m_Instance == null)
                    {
                        m_Instance = (new GameObject(type.Name)).AddComponent<T>();
                    }
                    DontDestroyOnLoad(m_Instance);
                }
                return m_Instance;
            }
        }
    }
}