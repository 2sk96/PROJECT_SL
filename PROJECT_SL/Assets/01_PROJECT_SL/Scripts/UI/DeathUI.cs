using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class DeathUI : UIBase
    {
        public override bool IsCursorVisible => true;

        public override void Show()
        {
            base.Show();
            // popup UI 다 끄기
            UIManager.Singleton.HideAllPopupUIs();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void OnClickRestartButton()
        {
            UIManager.Hide<DeathUI>(UIList.DeathUI);
            Main.Singleton.ChangeScene(SceneType.Ingame, isForceLoad: true);
        }

        public void OnClickBackToTitleButton()
        {
            Main.Singleton.ChangeScene(SceneType.Title);
        }
    }
}
