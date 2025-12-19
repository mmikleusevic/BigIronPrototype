using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class ButtonExtensions
    {
        public static void AddClickAsync(this Button button, Func<UniTask> asyncAction)
        {
            button.onClick.AddListener(() =>
            {
                button.SafeClickAsync(asyncAction).Forget();
            });
        }
        
        private static async UniTaskVoid SafeClickAsync(this Button button, Func<UniTask> action)
        {
            if (!button || action == null) return;

            try
            {
                button.interactable = false;
                await action();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Button action failed: {ex}");
            }
            finally
            {
                if (button) button.interactable = true;
            }
        }
    }
}