using System;
using UnityEngine;

public interface ILoadAsset
{
    void ClearLoadAssetTasks();
    void ForceClear();
    void LoadAsset(string prefab, Action<Object> loaded);
    void LoadAsset(string prefab, Action<Object> loaded, Action<float> progress);
    void LoadInstance(string prefab, Action<string, int, Object> loaded);
    void LoadInstance(string prefab, Action<string, int, Object> loaded, Action<float> progress);
    Object LoadLocalAsset(string prefab);
    Object LoadLocalInstance(string prefab);
    void LoadSceneAsset(string prefab, Action<Object> loaded);
    void LoadSceneInstance(string prefab, Action<string, int, Object> loaded, Action<float> progress);
    void LoadUIAsset(string prefab, Action<Object> loaded, Action<float> progress);
    void Release(string prefab);
    void Release(string prefab, bool releaseAsset);
    void ReleaseLocalAsset(string prefab);
    void ReleaseUnusedAssets();
    void SecondLoadAsset(string prefab, Action<Object> loaded, Action<float> progress);
    void SecondLoadUIAsset(string prefab, Action<Object> loaded, Action<float> progress);
    void SetPathMap();
    Object SynLoadAsset(string prefab);
    Object SynLoadInstance(string prefab);
    void UnloadAsset(string prefab);
}

