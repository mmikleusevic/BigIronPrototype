using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    public class UIFocusManager : MonoBehaviour
    {
        public static UIFocusManager Instance { get; private set; }

        private readonly Stack<Selectable> focusStack = new Stack<Selectable>();
        private Selectable lastFocusBeforePause;
        private CursorLockMode lastCursoreModeBeforePause;

        private void Awake()
        {
            Instance = this;
        }

        public void PushFocus(Selectable selectable)
        {
            if (!selectable) return;
            focusStack.Push(selectable);
        }

        public void PopFocus()
        {
            if (focusStack.Count > 0) focusStack.Pop();
        }

        public void RestoreFocus()
        {
            if (focusStack.Count == 0) return;

            Selectable top = focusStack.Peek();
            
            if (top && top.interactable) EventSystem.current.SetSelectedGameObject(top.gameObject);
        }

        public void SaveFocus(Selectable selectable, CursorLockMode cursorLockMode)
        {
            lastFocusBeforePause = selectable;
            lastCursoreModeBeforePause = cursorLockMode;
        }

        public void RestoreBeforePause()
        {
            if (lastFocusBeforePause) EventSystem.current.SetSelectedGameObject(lastFocusBeforePause.gameObject);
            Cursor.lockState = lastCursoreModeBeforePause;
        }

        public void ClearFocus()
        {
            focusStack.Clear();
            lastFocusBeforePause = null;
        }
    }
}