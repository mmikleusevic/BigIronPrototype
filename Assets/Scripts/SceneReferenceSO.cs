using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scenes/SceneReferenceSO")]
public class SceneReferenceSO : ScriptableObject
{
    public AssetReference SceneReference; 
    private string assetGUID;
    public string AssetGUID => assetGUID;
    private string assetName;
    public string AssetName => assetName;
    
    private void OnValidate()
    {
        if (SceneReference == null || string.IsNullOrEmpty(SceneReference?.AssetGUID)) return;
        
        assetGUID = SceneReference.AssetGUID;
        
#if UNITY_EDITOR
        assetName = SceneReference.editorAsset ? SceneReference.editorAsset.name : "";
        EditorUtility.SetDirty(this);
#endif
    }
}
