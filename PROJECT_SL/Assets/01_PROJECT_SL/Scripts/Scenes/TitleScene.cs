using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectSL
{
    public class TitleScene : SceneBase
    {
        public override IEnumerator OnStart()
        {
            AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
            while (!sceneLoadAsync.isDone)
            {
                yield return null;
            }

            // TODO 타이틀 장면에서 필요한 초기화 작업을 수행

            // 타이틀 화면에서 필요한 UI 활성화
            //UIManager.Show<TitleUI>(UIList.TitleUI);
        }

        public override IEnumerator OnEnd()
        {
            yield return null;

            // 타이틀 장면에서 필요한 정리 작업을 수행
            //UIManager.Hide<TitleUI>(UIList.TitleUI);
        }
    }
}
