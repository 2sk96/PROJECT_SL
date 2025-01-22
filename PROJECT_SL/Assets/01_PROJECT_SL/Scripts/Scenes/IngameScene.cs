using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectSL
{
    public class IngameScene : SceneBase
    {
        public override IEnumerator OnStart()
        {
            AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("Ingame", LoadSceneMode.Single);
            while (!sceneLoadAsync.isDone)
            {
                yield return null;
            }

            // TODO 인게임 장면에서 필요한 초기화 작업을 수행

            // TODO : 인게임 화면에서 필요한 UI 활성화
            UIManager.ShowIngameUIs();
        }

        public override IEnumerator OnEnd()
        {
            yield return null;

            // TODO : 인게임 장면에서 필요한 정리 작업을 수행
            UIManager.HideIngameUIs();
        }
    }
}
