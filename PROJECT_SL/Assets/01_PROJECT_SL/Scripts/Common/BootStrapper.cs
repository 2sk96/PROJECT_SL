using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class BootStrapper
    {
        private static List<string> AutoBootStrapperScenes = new List<string>()
        {
            // Boot Strapper가 자동으로 실행되어야 하는 씬의 이름들을 추가
            "Ingame",
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SystemBoot()
        {

#if UNITY_EDITOR
            UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            for(int i = 0; i < AutoBootStrapperScenes.Count; i++)
            {
                if (activeScene.name.Equals(AutoBootStrapperScenes[i]))
                {
                    InternalBoot();
                    return;
                }
            }

#endif
        }

        public static void InternalBoot()
        {
            // 시스템 초기화 수행
            // Main.cs의 Initialize 에서 수행할 Singleton.Initialize를 여기서도 진행
            UIManager.Singleton.Initialize();
            InputSystem.Singleton.Initialize();

            // 인게임 씬 UI 띄워주기
            UIManager.ShowIngameUIs();

            GameObject eventSystemPrefab = Resources.Load<GameObject>("EventSystem/EventSystem");
            GameObject esInstance = UnityEngine.Object.Instantiate(eventSystemPrefab);
        }
    }
}
