#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scenes/SceneReferenceSO")]
public class SceneReferenceSO : ScriptableObject
{
    public AssetReference SceneReference; 
    
    [SerializeField] private string assetGUID;
    [SerializeField] private string assetName;
    public string AssetGUID => assetGUID;
    public string AssetName => assetName;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (SceneReference == null || string.IsNullOrEmpty(SceneReference?.AssetGUID))
        {
            assetGUID = "";
            assetName = "";
            return;
        }
        
        assetGUID = SceneReference.AssetGUID;
        assetName = SceneReference.editorAsset ? SceneReference.editorAsset.name : "";
        
        EditorUtility.SetDirty(this);
#endif
    }
}
