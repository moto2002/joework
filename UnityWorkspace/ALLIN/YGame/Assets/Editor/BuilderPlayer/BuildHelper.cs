using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class BuildHelper : Editor
{
    static private string m_outPutName = "myVR";
    static private List<string> levels = new List<string>();

    #region Common
    private static string GetSavePath(string path)
    {
        int index = path.LastIndexOf('/');
        return path.Substring(0, index);
    }


    static private void RestBuildSettingScenes()
    {
        levels.Clear();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;
            levels.Add(scene.path);
        }
    }
    #endregion

    #region API
    [MenuItem("Tools/Switch Only/Switch2IOSTarget")]
    public static void Switch2IOSTarget()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
    }

    [MenuItem("Tools/Switch Only/Switch2AndroidTarget")]
    public static void Switch2AndroidTarget()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
    }

    [MenuItem("Tools/Switch Only/Switch2PCTarget")]
    public static void Switch2PCTarget()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows64);
    }


    [MenuItem("Tools/BuildPlayer/BuildAndroid")]
    public static void BuildAndroid()
    {
        RestBuildSettingScenes();
        string apk_outPut = string.Format("/{0}.apk", m_outPutName);
        string error = BuildPipeline.BuildPlayer(levels.ToArray(), GetSavePath(Application.dataPath) + apk_outPut, BuildTarget.Android, BuildOptions.None);
        if (error.Length > 0)
            Debug.LogError("BuildPlayer failure: " + error);
    }

    [MenuItem("Tools/BuildPlayer/BuildIOS")]
    public static void BuildIOS()
    {
        RestBuildSettingScenes();
        string xcode_outPut = string.Format("/{0}IOSProj", m_outPutName);
        string error = BuildPipeline.BuildPlayer(levels.ToArray(), GetSavePath(Application.dataPath) + xcode_outPut, BuildTarget.iOS, BuildOptions.None);
        if (error.Length > 0)
            Debug.LogError("BuildPlayer failure: " + error);
    }

    [MenuItem("Tools/BuildPlayer/BuildPC")]
    public static void BuildPC()
    {
        RestBuildSettingScenes();
        string pc_outPut = string.Format("/{0}.exe", m_outPutName);
        string error = BuildPipeline.BuildPlayer(levels.ToArray(), GetSavePath(Application.dataPath) + pc_outPut, BuildTarget.StandaloneWindows64, BuildOptions.None);
        if (error.Length > 0)
            Debug.LogError("BuildPlayer failure: " + error);
    }
    #endregion

}
