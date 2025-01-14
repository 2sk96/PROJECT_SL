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

            // TODO �ΰ��� ��鿡�� �ʿ��� �ʱ�ȭ �۾��� ����

            // TODO : �ΰ��� ȭ�鿡�� �ʿ��� UI Ȱ��ȭ
            UIManager.ShowIngameUIs();
        }

        public override IEnumerator OnEnd()
        {
            yield return null;

            // TODO : �ΰ��� ��鿡�� �ʿ��� ���� �۾��� ����
            UIManager.HideIngameUIs();
        }
    }
}
