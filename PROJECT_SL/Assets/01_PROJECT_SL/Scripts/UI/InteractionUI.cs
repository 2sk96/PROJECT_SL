using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InteractionUI : UIBase
    {
        public List<InteractionUI_Item> createdContents = new List<InteractionUI_Item>();
        [SerializeField] private int selectedIndex = -1;

        [SerializeField] private InteractionUI_Item itemPrefab;
        [SerializeField] private Transform contentsGroup;

        [SerializeField] private Vector3 playerPosition;
        
        private void Awake()
        {
            itemPrefab.gameObject.SetActive(false);
        }

        public override void Hide()
        {
            base.Hide();
            RemoveAllInteractionContent();
        }

        private void Update()
        {
            if (createdContents.Count > 0)
            {
                SelectClosestInteractableData();
            }
        }

        // 가장 가까운 interaction content 만 활성화 시켜주는 함수
        private void SelectClosestInteractableData()
        {
            float closestDistance = 0;
            int closestIndex = 0;
            for (int i = 0; i < createdContents.Count; i++)
            {
                float distance = CalculateDistance(createdContents[i].InteractableData.Position);
                if (closestDistance == 0)
                {
                    closestDistance = distance;
                    closestIndex = i;
                    createdContents[i].isSelected = true;
                }
                else
                {
                    if (distance < closestDistance)
                    {
                        createdContents[closestIndex].isSelected = false;
                        closestDistance = distance;
                        closestIndex = i;
                        createdContents[i].isSelected = true;
                    }
                    else
                    {
                        createdContents[i].isSelected = false;
                    }
                }
            }
            selectedIndex = closestIndex;
        }

        private float CalculateDistance(Vector3 itemPosition)
        {
            float distance = Vector3.Distance(itemPosition, playerPosition);
            return distance;
        }

        public void GetPlayerPosition(Vector3 position)
        {
            playerPosition = position;
        }

        public void AddInteractionContent(IInteractable interactable)
        {
            if (createdContents.Exists(x => x.InteractableData == interactable)) return;

            InteractionUI_Item newItem = Instantiate(itemPrefab, contentsGroup);
            newItem.gameObject.SetActive(true);
            newItem.ContentsText = interactable.Message;
            // createdContents가 새로 생길 경우에는 isSelected 설정
            if (createdContents.Count == 0)
            {
                newItem.isSelected = true;
            }
            // 기존 createdContents가 있을 경우 isSelected 설정 X
            else
            {
                newItem.isSelected = false;
            }
            newItem.InteractableData = interactable;

            createdContents.Add(newItem);
        }

        public void RemoveInteractionContent(IInteractable interactable)
        {
            if (!createdContents.Exists(x => x.InteractableData == interactable)) return;

            int targetIndex = createdContents.FindIndex(x => x.InteractableData == interactable);
            Destroy(createdContents[targetIndex].gameObject);
            createdContents.RemoveAt(targetIndex);
        }


        public void RemoveAllInteractionContent()
        {
            for (int i = 0; i < createdContents.Count; i++)
            {
                Destroy(createdContents[i].gameObject);
            }
            createdContents.Clear();
        }

        public void ExecuteInteract()
        {
            if (selectedIndex >= 0 && selectedIndex < createdContents.Count)
            {
                createdContents[selectedIndex].InteractableData.Interact();
            }
        }
    }
}
