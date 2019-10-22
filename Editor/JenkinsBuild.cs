// -------------------------------------------------------------------------------------------------
// Assets/Editor/JenkinsBuild.cs
// -------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using System.IO;
using System;

// ------------------------------------------------------------------------
// https://docs.unity3d.com/Manual/CommandLineArguments.html
// ------------------------------------------------------------------------
public class JenkinsBuild
{
    static string[] enabledScenes = FindEnabledEditorScenes();
    static string appName = GetAppName();

    public static string GetAppName()
    {
        return Application.productName;
    }
    public static string GetTargetDirectory(BuildTarget buildTarget)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-targetDirectory")
            {
                return args[i + 1] + Path.DirectorySeparatorChar + buildTarget.ToString();
            }
        }
        return "Builds" + Path.DirectorySeparatorChar + buildTarget.ToString();
    }

    // ------------------------------------------------------------------------
    // called from Jenkins
    // ------------------------------------------------------------------------
    [MenuItem("Build/Android")]
    public static void BuildAndroid()
    {
        string fileType = ".apk";
        BuildTarget targetPlatform = BuildTarget.Android;
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildOptions buildOptions = BuildOptions.None;

        string targetDir = GetTargetDirectory(targetPlatform);
        string fullPathToFile = targetDir + Path.DirectorySeparatorChar + appName;
        BuildProject(enabledScenes, fullPathToFile + fileType, targetGroup, targetPlatform, buildOptions);
    }

    [MenuItem("Build/iOS")]
    public static void BuildiOS()
    {
        string fileType = ".ipa";
        BuildTarget targetPlatform = BuildTarget.iOS;
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildOptions buildOptions = BuildOptions.None;

        string targetDir = GetTargetDirectory(targetPlatform);
        string fullPathToFile = targetDir + Path.DirectorySeparatorChar + appName;
        BuildProject(enabledScenes, fullPathToFile + fileType, targetGroup, targetPlatform, buildOptions);
    }

    [MenuItem("Build/Windows (x86)")]
    public static void BuildWindowsx86()
    {
        string fileType = ".exe";
        BuildTarget targetPlatform = BuildTarget.StandaloneWindows;
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildOptions buildOptions = BuildOptions.None;

        string targetDir = GetTargetDirectory(targetPlatform);
        string fullPathToFile = targetDir + Path.DirectorySeparatorChar + appName;
        BuildProject(enabledScenes, fullPathToFile + fileType, targetGroup, targetPlatform, buildOptions);
    }

    [MenuItem("Build/Windows (x64)")]
    public static void BuildWindowsx64()
    {
        string fileType = ".exe";
        BuildTarget targetPlatform = BuildTarget.StandaloneWindows64;
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildOptions buildOptions = BuildOptions.None;

        string targetDir = GetTargetDirectory(targetPlatform);
        string fullPathToFile = targetDir + Path.DirectorySeparatorChar + appName;
        BuildProject(enabledScenes, fullPathToFile + fileType, targetGroup, targetPlatform, buildOptions);
    }

    [MenuItem("Build/Mac OSX")]
    public static void BuildMacOSX()
    {
        string fileType = ".app";
        BuildTarget targetPlatform = BuildTarget.StandaloneOSX;
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        BuildOptions buildOptions = BuildOptions.None;

        string targetDir = GetTargetDirectory(targetPlatform);
        string fullPathToFile = targetDir + Path.DirectorySeparatorChar + appName;
        BuildProject(enabledScenes, fullPathToFile + fileType, targetGroup, targetPlatform, buildOptions);
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    private static string[] FindEnabledEditorScenes()
    {

        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                EditorScenes.Add(scene.path);
            }
        }
        return EditorScenes.ToArray();
    }

    // ------------------------------------------------------------------------
    // e.g. BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX
    // ------------------------------------------------------------------------
    private static void BuildProject(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        Console.WriteLine("[JenkinsBuild] Building:" + targetDir + " buildTargetGroup:" + buildTargetGroup.ToString() + " buildTarget:" + buildTarget.ToString());

        // https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.SwitchActiveBuildTarget.html
        bool switchResult = EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        if (switchResult)
        {
            Console.WriteLine("[JenkinsBuild] Successfully changed Build Target to: " + buildTarget.ToString());
        }
        else
        {
            Console.WriteLine("[JenkinsBuild] Unable to change Build Target to: " + buildTarget.ToString() + " Exiting...");
            return;
        }

        // https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html
        BuildReport buildReport = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);
        BuildSummary buildSummary = buildReport.summary;
        if (buildSummary.result == BuildResult.Succeeded)
        {
            Console.WriteLine("[JenkinsBuild] Build Success: Time:" + buildSummary.totalTime + " Size:" + buildSummary.totalSize + " bytes");
        }
        else
        {
            Console.WriteLine("[JenkinsBuild] Build Failed: Time:" + buildSummary.totalTime + " Total Errors:" + buildSummary.totalErrors);
        }
    }
}
