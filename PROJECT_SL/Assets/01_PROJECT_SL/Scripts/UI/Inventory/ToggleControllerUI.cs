using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public enum TabContentType
    {
        Consumable,
        Equipment,
        Crafting,
    };

    public class ToggleControllerUI : MonoBehaviour
    {
        

        [SerializeField] private Toggle consumableTab;
        [SerializeField] private Toggle equipmentTab;
        [SerializeField] private Toggle craftingTab;

        [SerializeField] private GameObject consumableContent;
        [SerializeField] private GameObject equipmentContent;
        [SerializeField] private GameObject craftingContent;

        public TabContentType currentActiveTabContent = TabContentType.Consumable;

        private void Awake()
        {
            consumableTab.onValueChanged.AddListener((isOn) => {
                consumableContent.SetActive(isOn);
                if (isOn) currentActiveTabContent = TabContentType.Consumable;
                });
            equipmentTab.onValueChanged.AddListener((isOn) => { 
                equipmentContent.SetActive(isOn);
                if (isOn) currentActiveTabContent = TabContentType.Equipment;
            });
            craftingTab.onValueChanged.AddListener((isOn) => { 
                craftingContent.SetActive(isOn);
                if (isOn) currentActiveTabContent = TabContentType.Crafting;
            });
        }

        private void OnEnable()
        {
            currentActiveTabContent = TabContentType.Consumable;
            
            consumableTab.isOn = true;
            equipmentTab.isOn = false;
            craftingTab.isOn = false;

            consumableContent.SetActive(true);
            equipmentContent.SetActive(false);
            craftingContent.SetActive(false);
        }

        // 오른쪽 화살표 클릭
        public void OnClickNextTab()
        {
            switch (currentActiveTabContent)
            {
                case TabContentType.Consumable: equipmentTab.isOn = true; break;
                case TabContentType.Equipment: craftingTab.isOn = true; break;
                case TabContentType.Crafting: consumableTab.isOn = true; break;
            }
        }

        // 왼쪽 화살표 클릭
        public void OnClickPrevTab()
        {
            switch (currentActiveTabContent)
            {
                case TabContentType.Consumable: craftingTab.isOn = true; break;
                case TabContentType.Equipment: consumableTab.isOn = true; break;
                case TabContentType.Crafting: equipmentTab.isOn = true; break;
            }
        }
    }
}
