using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Selectable firstSelectable;

        private void OnEnable()
        {
            SelectFirst();
        }

        public void SelectFirst()
        {
            if (!firstSelectable) return;
            
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            firstSelectable.Select();
        }
    }
}
