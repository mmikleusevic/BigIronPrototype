using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Selectable selectable;

        private void OnEnable()
        {
            Select();
        }

        public void Select()
        {
            if (!selectable)
            {
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }
            
            EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            selectable.Select();
        }
    }
}
