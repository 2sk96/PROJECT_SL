using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class TabArrowButtonUI : MonoBehaviour
    {
        [SerializeField] private TabController tabController;

        private void Start()
        {
            if (tabController == null)
            {
                tabController = transform.parent.GetComponent<TabController>();
            }
        }

        public void SwitchTabUsingLeftArrow()
        {
            int newTabIndex = (tabController.tabIndex - 1 + tabController.tabButtons.Length) % (tabController.tabButtons.Length);
            tabController.SwitchTabUsingIndex(newTabIndex);
        }

        public void SwithTabUsingRightArrow()
        {
            int newTabIndex = (tabController.tabIndex + 1) % (tabController.tabButtons.Length);
            tabController.SwitchTabUsingIndex(newTabIndex);
        }
    }
}
