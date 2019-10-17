using UnityEngine;

namespace ShadyPigeon.UnityEssentials
{
    public class Singleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
    {
        private static TSelfType m_Instance = null;
        public static TSelfType Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    var type = typeof(TSelfType);
                    m_Instance = (TSelfType)FindObjectOfType(type);
                    if (m_Instance == null)
                    {
                        m_Instance = (new GameObject(type.Name)).AddComponent<TSelfType>();
                    }
                    DontDestroyOnLoad(m_Instance);
                }
                return m_Instance;
            }
        }
    }
}