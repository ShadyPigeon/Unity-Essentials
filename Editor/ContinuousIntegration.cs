using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Build.Reporting;
using UnityEditor;

public class ContinuousIntegration
{
    static string[] scenes = FindEnabledEditorScenes();

    static string GetAppName()
    {
        return Application.productName;
    }

    static string GetTargetName(BuildTargetGroup targetGroup)
    {
        return targetGroup.ToString();
    }

    public static void PerformBuild()
    {
        PerformAndroidBuild();

    }

    [MenuItem("Custom/CI/Android")]
    static void PerformAndroidBuild()
    {
        string targetDir = GetAppName() + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget target = BuildTarget.Android;
        BuildOptions options = BuildOptions.None;
        GenericBuild(scenes, GetTargetName(targetGroup) + "/" + targetDir, targetGroup, target, options);
    }

    [MenuItem("Custom/CI/Build Mac OS X")]
    static void PerformMacOSXBuild()
    {
        string targetDir = GetAppName() + ".app";
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildTarget target = BuildTarget.StandaloneOSX;
        BuildOptions options = BuildOptions.None;
        GenericBuild(scenes, GetTargetName(targetGroup) + "/" + targetDir, targetGroup, target, options);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        BuildReport buildReport = BuildPipeline.BuildPlayer(scenes, "Builds" + "/" + targetDir, buildTarget, buildOptions);
        BuildSummary summary = buildReport.summary;
        switch (summary.result)
        {
            case BuildResult.Unknown:
                Debug.Log("Succeeded!");
                break;
            case BuildResult.Succeeded:
                Debug.Log("Succeeded!");
                break;
            case BuildResult.Failed:
                Debug.Log("Failed!");
                break;
            case BuildResult.Cancelled:
                Debug.Log("Succeeded!");
                break;
            default:
                break;
        }
    }
}