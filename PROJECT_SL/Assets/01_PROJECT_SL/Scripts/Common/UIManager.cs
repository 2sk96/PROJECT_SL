using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class UIManager : SingletonBase<UIManager>
    {
        public static void ShowIngameUIs()
        {
            // INGAME 씬에서 사용하는 UI들 활성화
            // 초기에 켜져야 하는 UI들 여기서 Show 해주면 된다
            //Show<IngameMinimapUI>(UIList.IngameMinimapUI);
            //Show<MainHUDUI>(UIList.MainHUDUI);
        }

        public static void HideIngameUIs()
        {
            // Ingame 씬을 떠날 때 모든 UI들 비활성화
            // Ingame 씬에서 사용하는 모든 UI를 비활성화 하면 된다
            //Hide<IngameMinimapUI>(UIList.IngameMinimapUI);
            //Hide<MainHUDUI>(UIList.MainHUDUI);
        }
        
        public static T Show<T>(UIList uiName) where T : UIBase
        {
            var targetUI = Singleton.GetUI<T>(uiName);
            targetUI.Show();

            return targetUI;
        }

        public static T Hide<T>(UIList uiName) where T : UIBase
        {
            var targetUI = Singleton.GetUI<T>(uiName);
            targetUI.Hide();

            return targetUI;
        }

        private Dictionary<UIList, UIBase> panelContainer = new Dictionary<UIList, UIBase>();
        private Dictionary<UIList, UIBase> popupContainer = new Dictionary<UIList, UIBase>();

        private Transform panelRoot;
        private Transform popupRoot;

        private const string UI_PATH = "UI/Prefabs/";

        public void Initialize()
        {
            if (panelRoot == null)
            {
                GameObject panelGo = new GameObject("Panel Root");
                panelGo.transform.SetParent(transform);
                panelRoot = panelGo.transform;
            }

            if (popupRoot == null)
            {
                GameObject popupGo = new GameObject("Popup Root");
                popupGo.transform.SetParent(transform);
                popupRoot = popupGo.transform;
            }

            for (int i = (int)UIList.PANEL_START + 1; i < (int)UIList.PANEL_END; i++)
            {
                panelContainer.Add((UIList)i, null);
            }

            for (int i = (int)UIList.POPUP_START + 1; i < (int)UIList.POPUP_END; i++)
            {
                popupContainer.Add((UIList)i, null);
            }
        }

        public T GetUI<T>(UIList uiName, bool isReload = false) where T : UIBase
        {
            Dictionary<UIList, UIBase> targetContainer = null;
            targetContainer = uiName is > UIList.PANEL_START and < UIList.PANEL_END ? panelContainer : popupContainer;

            if (!targetContainer.ContainsKey(uiName))
            {
                return null;
            }

            if (isReload && targetContainer[uiName])
            {
                Destroy(targetContainer[uiName].gameObject);
                targetContainer[uiName] = null;
            }

            if (!targetContainer[uiName])
            {
                string path = UI_PATH + $"UI.{uiName}";
                T assetUI = Resources.Load<UIBase>(path) as T;

                if (!assetUI)
                {
                    return null;
                }

                Transform targetTransform = uiName is > UIList.PANEL_START and < UIList.PANEL_END ? panelRoot : popupRoot;
                T newUI = Instantiate(assetUI, targetTransform);
                newUI.gameObject.SetActive(false);

                targetContainer[uiName] = newUI;
            }

            return targetContainer[uiName] as T;
        }

        public void HideAllUIs()
        {
            foreach (var ui in panelContainer)
            {
                if (ui.Value)
                {
                    ui.Value.Hide();
                }
            }

            foreach (var ui in popupContainer)
            {
                if (ui.Value)
                {
                    ui.Value.Hide();
                }
            }
        }

        public void HideAllPanelUIs()
        {
            foreach (var ui in panelContainer)
            {
                if (ui.Value)
                {
                    ui.Value.Hide();
                }
            }
        }

        public void HideAllPopupUIs()
        {
            foreach (var ui in popupContainer)
            {
                if (ui.Value)
                {
                    ui.Value.Hide();
                }
            }
        }
    }
}
