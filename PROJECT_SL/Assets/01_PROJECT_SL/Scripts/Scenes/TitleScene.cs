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

            // TODO Ÿ��Ʋ ��鿡�� �ʿ��� �ʱ�ȭ �۾��� ����

            // Ÿ��Ʋ ȭ�鿡�� �ʿ��� UI Ȱ��ȭ
            //UIManager.Show<TitleUI>(UIList.TitleUI);
        }

        public override IEnumerator OnEnd()
        {
            yield return null;

            // Ÿ��Ʋ ��鿡�� �ʿ��� ���� �۾��� ����
            //UIManager.Hide<TitleUI>(UIList.TitleUI);
        }
    }
}
