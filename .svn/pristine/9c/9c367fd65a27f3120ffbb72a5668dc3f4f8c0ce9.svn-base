using System;
using UnityEngine;

public interface ILoadAsset
{
    void ClearLoadAssetTasks();
    void ForceClear();
    void LoadAsset(string prefab, Action<UnityEngine.Object> loaded);
    void LoadAsset(string prefab, Action<UnityEngine.Object> loaded, Action<float> progress);
    void LoadAsset(string prefab, byte priority, Action<UnityEngine.Object> loaded, Action<float> progress);
    void LoadInstance(string prefab, Action<string, int, UnityEngine.Object> loaded);
    void LoadInstance(string prefab, Action<string, int, UnityEngine.Object> loaded, Action<float> progress);
    UnityEngine.Object LoadLocalAsset(string prefab);
    UnityEngine.Object LoadLocalInstance(string prefab);
    void LoadSceneAsset(string prefab, Action<UnityEngine.Object> loaded);
    void LoadSceneInstance(string prefab, Action<string, int, UnityEngine.Object> loaded, Action<float> progress);
    void LoadUIAsset(string prefab, Action<UnityEngine.Object> loaded, Action<float> progress);
    void LoadUIAsset(string prefab, byte priority, Action<UnityEngine.Object> loaded, Action<float> progress);
    bool Release(string prefab);
    void Release(string prefab, bool releaseAsset);
    void ReleaseLocalAsset(string prefab);
    void ReleaseUnusedAssets();
    void SecondLoadAsset(string prefab, Action<UnityEngine.Object> loaded, Action<float> progress);
    void SecondLoadUIAsset(string prefab, Action<UnityEngine.Object> loaded, Action<float> progress);
    UnityEngine.Object SynLoadAsset(string prefab);
    UnityEngine.Object SynLoadInstance(string prefab);
    void UnloadAsset(string prefab);
}

