// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.Client
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.PackageManager.Requests;

namespace UnityEditor.PackageManager
{
  /// <summary>
  ///   <para>Unity Package Manager client class.</para>
  /// </summary>
  public static class Client
  {
    /// <summary>
    ///   <para>Lists the packages the project depends on.</para>
    /// </summary>
    /// <returns>
    ///   <para>A ListRequest instance, which you can use to monitor the asynchronous operation, and when complete, get the result.</para>
    /// </returns>
    public static ListRequest List()
    {
      long operationId;
      NativeClient.StatusCode initialStatus = NativeClient.List(out operationId);
      return new ListRequest(operationId, initialStatus);
    }

    /// <summary>
    ///   <para>Adds a package dependency to the project.</para>
    /// </summary>
    /// <param name="packageIdOrName">The name or ID of the package to add.  If only the name is specified, the latest version of the package is installed.</param>
    /// <returns>
    ///   <para>An AddRequest instance, which you can use to monitor the asynchronous operation, and when complete, get the result.</para>
    /// </returns>
    public static AddRequest Add(string packageIdOrName)
    {
      long operationId;
      NativeClient.StatusCode initialStatus = NativeClient.Add(out operationId, packageIdOrName);
      return new AddRequest(operationId, initialStatus);
    }

    /// <summary>
    ///   <para>Removes a previously added package from the project.</para>
    /// </summary>
    /// <param name="packageIdOrName">The name or the ID of the package to remove from the project.  If only the name is specified, the package is removed regardless of the installed version.</param>
    /// <returns>
    ///   <para>A RemoveRequest instance, which you can use to monitor the status of the asynchronous operation.</para>
    /// </returns>
    public static RemoveRequest Remove(string packageIdOrName)
    {
      long operationId;
      NativeClient.StatusCode initialStatus = NativeClient.Remove(out operationId, packageIdOrName);
      return new RemoveRequest(operationId, initialStatus, packageIdOrName);
    }

    /// <summary>
    ///   <para>Searches the Unity package registry for the given package.</para>
    /// </summary>
    /// <param name="packageIdOrName">The name or ID of the package.</param>
    /// <returns>
    ///   <para>A SearchRequest instance, which you can use to monitor the asynchronous operation, and when complete, get the result.</para>
    /// </returns>
    public static SearchRequest Search(string packageIdOrName)
    {
      long operationId;
      NativeClient.StatusCode initialStatus = NativeClient.Search(out operationId, packageIdOrName);
      return new SearchRequest(operationId, initialStatus, packageIdOrName);
    }

    /// <summary>
    ///   <para>Resets the list of packages installed for this project to the editor's default configuration. This operation will clear all packages added to the project and keep only the packages set for the current editor default configuration.</para>
    /// </summary>
    /// <returns>
    ///   <para>A ResetToEditorDefaultsRequest instance, which you can use to monitor the status of the asynchronous operation.</para>
    /// </returns>
    public static ResetToEditorDefaultsRequest ResetToEditorDefaults()
    {
      long operationId;
      NativeClient.StatusCode editorDefaults = NativeClient.ResetToEditorDefaults(out operationId);
      return new ResetToEditorDefaultsRequest(operationId, editorDefaults);
    }
  }
}
