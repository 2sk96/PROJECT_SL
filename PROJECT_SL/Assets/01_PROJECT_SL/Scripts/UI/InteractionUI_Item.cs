using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectSL
{
    public class InteractionUI_Item : MonoBehaviour
    {
        public bool isSelected { set => interactionContent.SetActive(value); }
        public string ContentsText { set => text.text = value; }
        public IInteractable InteractableData { get; set; }

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject interactionContent;
    }
}
