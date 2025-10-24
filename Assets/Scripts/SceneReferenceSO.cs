using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scenes/SceneReferenceSO")]
public class SceneReferenceSO : ScriptableObject
{
    public AssetReference SceneReference; 
    private string assetGUID;
    public string AssetGUID => assetGUID;
    
    private void OnValidate()
    {
        if (SceneReference == null || string.IsNullOrEmpty(SceneReference?.AssetGUID)) return;
        
        assetGUID = SceneReference.AssetGUID;
        UnityEditor.EditorUtility.SetDirty(this);
    }
}
