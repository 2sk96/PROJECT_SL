using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class TabController : MonoBehaviour
    {
        public int tabIndex;
        public TabButton[] tabButtons;
        
        [SerializeField] private Sprite defaultButtonSprite;
        [SerializeField] private Color defaultButtonColor;
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Color selectedButtonColor;


        private void Start()
        {
            tabButtons = GetComponentsInChildren<TabButton>();
            SwitchTab(tabButtons[0]);
        }

        public void SwitchTab(TabButton target)
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                bool isActiveTab = target == tabButtons[i];

                tabButtons[i].GetTabContent.SetActive(isActiveTab);

                if (isActiveTab)
                {
                    tabButtons[i].SetSelectedButton(selectedButtonSprite, selectedButtonColor);
                    tabIndex = i;
                }
                else
                {
                    tabButtons[i].SetSelectedButton(defaultButtonSprite, defaultButtonColor);
                }
            }
        }

        public void SwitchTabUsingIndex(int index)
        {
            for (int i = 0; i < tabButtons.Length;i++)
            {
                bool isActiveTab = i == index;
                tabButtons[i].GetTabContent.SetActive(isActiveTab);
                if (isActiveTab)
                {
                    tabButtons[i].SetSelectedButton(selectedButtonSprite, selectedButtonColor);
                    tabIndex = i;
                }
                else
                {
                    tabButtons[i].SetSelectedButton(defaultButtonSprite, defaultButtonColor);
                }

            }
        }

    }
}
