// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Toast
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.WSA
{
  /// <summary>
  ///         <para>Represents a toast notification in Windows Store Apps.
  /// </para>
  ///       </summary>
  public sealed class Toast
  {
    private int m_ToastId;

    private Toast(int id)
    {
      this.m_ToastId = id;
    }

    /// <summary>
    ///         <para>Get template XML for toast notification.
    /// </para>
    ///       </summary>
    /// <param name="templ">A template identifier.</param>
    /// <returns>
    ///   <para>string, which is an empty XML document to be filled and used for toast notification.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetTemplate(ToastTemplate templ);

    /// <summary>
    ///   <para>Create toast notification.</para>
    /// </summary>
    /// <param name="xml">XML document with tile data.</param>
    /// <param name="image">Uri to image to show on a toast, can be empty, in that case text-only notification will be shown.</param>
    /// <param name="text">A text to display on a toast notification.</param>
    /// <returns>
    ///   <para>A toast object for further work with created notification or null, if creation of toast failed.</para>
    /// </returns>
    public static Toast Create(string xml)
    {
      int toastXml = Toast.CreateToastXml(xml);
      if (toastXml < 0)
        return (Toast) null;
      return new Toast(toastXml);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int CreateToastXml(string xml);

    /// <summary>
    ///   <para>Create toast notification.</para>
    /// </summary>
    /// <param name="xml">XML document with tile data.</param>
    /// <param name="image">Uri to image to show on a toast, can be empty, in that case text-only notification will be shown.</param>
    /// <param name="text">A text to display on a toast notification.</param>
    /// <returns>
    ///   <para>A toast object for further work with created notification or null, if creation of toast failed.</para>
    /// </returns>
    public static Toast Create(string image, string text)
    {
      int toastImageAndText = Toast.CreateToastImageAndText(image, text);
      if (toastImageAndText < 0)
        return (Toast) null;
      return new Toast(toastImageAndText);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int CreateToastImageAndText(string image, string text);

    /// <summary>
    ///   <para>Arguments to be passed for application when toast notification is activated.</para>
    /// </summary>
    public string arguments
    {
      get
      {
        return Toast.GetArguments(this.m_ToastId);
      }
      set
      {
        Toast.SetArguments(this.m_ToastId, value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetArguments(int id);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetArguments(int id, string args);

    /// <summary>
    ///   <para>Show toast notification.</para>
    /// </summary>
    public void Show()
    {
      Toast.Show(this.m_ToastId);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Show(int id);

    /// <summary>
    ///   <para>Hide displayed toast notification.</para>
    /// </summary>
    public void Hide()
    {
      Toast.Hide(this.m_ToastId);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Hide(int id);

    /// <summary>
    ///   <para>true if toast was activated by user.</para>
    /// </summary>
    public bool activated
    {
      get
      {
        return Toast.GetActivated(this.m_ToastId);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetActivated(int id);

    /// <summary>
    ///   <para>true if toast notification was dismissed (for any reason).</para>
    /// </summary>
    public bool dismissed
    {
      get
      {
        return Toast.GetDismissed(this.m_ToastId, false);
      }
    }

    /// <summary>
    ///   <para>true if toast notification was explicitly dismissed by user.</para>
    /// </summary>
    public bool dismissedByUser
    {
      get
      {
        return Toast.GetDismissed(this.m_ToastId, true);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetDismissed(int id, bool byUser);
  }
}
