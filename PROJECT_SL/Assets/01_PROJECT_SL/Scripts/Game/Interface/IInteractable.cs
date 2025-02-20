using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public interface IInteractable
    {
        public string Message { get; }

        public void Interact();
    }
}
