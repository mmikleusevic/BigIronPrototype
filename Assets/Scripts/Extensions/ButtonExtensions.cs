using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class ButtonExtensions
    {
        public static void AddClickAsync(this Button button, Func<Task> asyncAction)
        {
            button.onClick.AddListener(() =>
            {
                _ = button.SafeClickAsync(asyncAction);
            });
        }
        
        private static async Task SafeClickAsync(this Button button, Func<Task> action)
        {
            if (!button || action == null)
                return;

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
                button.interactable = true;
            }
        }
    }
}