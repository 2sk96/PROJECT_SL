using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public abstract class UIBase : MonoBehaviour
    {
        public virtual bool IsCursorVisible { get; } = false;


        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
