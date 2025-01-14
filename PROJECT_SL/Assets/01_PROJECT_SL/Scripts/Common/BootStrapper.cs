#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace ProjectSL
{
    public class BootStrapper
    {
        private const string BootStrapperMenuPath = "ProjectSL/BootStrapper/Activate Auto SystemLoad";

        private static bool IsActivateBootStrapper
        {
            get => UnityEditor.EditorPrefs.GetBool(BootStrapperMenuPath, false);
            set
            {
                UnityEditor.EditorPrefs.SetBool(BootStrapperMenuPath, value);
                UnityEditor.Menu.SetChecked(BootStrapperMenuPath, value);
            }
        }

        [UnityEditor.MenuItem(BootStrapperMenuPath, false)]

        private static void ActiveBootStrapper()
        {
            IsActivateBootStrapper = !IsActivateBootStrapper;
            UnityEditor.Menu.SetChecked(BootStrapperMenuPath, IsActivateBootStrapper);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SystemBoot()
        {
            Scene activeScene = EditorSceneManager.GetActiveScene();
            if (IsActivateBootStrapper && false == activeScene.name.Equals("Main"))
            {
                InternalBoot();
            }
        }

        public static void InternalBoot()
        {
            Main.Singleton.Initialize();

            // BootStrapper에서 추가적인 커스텀 작업을 추가하고 싶을 경우 여기에 추가해 주면 된다.
        }
    }
}
#endif