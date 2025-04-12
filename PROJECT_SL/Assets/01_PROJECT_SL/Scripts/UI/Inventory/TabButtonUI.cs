using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public class TabButton : MonoBehaviour
    {
        public GameObject GetTabContent => tabContent;
        [SerializeField] private GameObject tabContent;
        [SerializeField] private Image image;
        [SerializeField] private TabController tabController;

        private void Start()
        {
            if (tabController == null)
            {
                tabController = transform.parent.GetComponent<TabController>();
            }
        }

        public void SwitchTab()
        {
            tabController.SwitchTab(this);
        }

        public void SetSelectedButton(Sprite sprite, Color color)
        {
            if (image == null) return;
            image.sprite = sprite;
            image.color = color;
        }
    }
}
