using System;
using UnityEngine;
using UnityEngine.UI;

namespace PokerDiceRoom
{
    public class DieVisual : MonoBehaviour
    {
        [SerializeField] private Camera dieCamera;
        [SerializeField] private RectTransform dieUI;
        [SerializeField] private RawImage dieImage;
        [SerializeField] private Outline dieOutline;
        [SerializeField] private GameObject highlight;
        [SerializeField] private ParticleSystem rolledParticleEffect;
        [SerializeField] private int textureSize;
        
        private bool isHighlighted;
        
        public void Initialize(RectTransform uiContainer)
        {
            RenderTexture renderTexture = new RenderTexture(textureSize, textureSize, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            
            dieUI.sizeDelta = new Vector2(textureSize, textureSize);
            dieUI.transform.SetParent(uiContainer, false);
            
            dieImage.texture = renderTexture;
            
            dieCamera.targetTexture = renderTexture;

            ResetVisual();
        }

        public void ResetDieUITransform()
        {
            dieUI.transform.SetParent(transform.parent, false);
        }

        private void ResetVisual()
        {
            dieOutline.enabled = true;
            highlight.gameObject.SetActive(false);
            rolledParticleEffect.Stop();
            dieCamera.Render();
            dieCamera.gameObject.SetActive(false);
        }

        public void SetVisual(bool isHeld)
        {
            dieOutline.enabled = !isHeld;
        }

        public void ToggleHighlight()
        {
            isHighlighted = !isHighlighted;

            highlight.gameObject.SetActive(isHighlighted);
            
            dieCamera.Render();
        }

        public void SetCamera(bool value)
        {
            dieCamera.gameObject.SetActive(value);
        }

        public void PlayRolledParticleEffect()
        {
            rolledParticleEffect.Stop();
            rolledParticleEffect.Play();
        }
    }
}
