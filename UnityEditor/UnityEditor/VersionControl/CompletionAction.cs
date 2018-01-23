// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.CompletionAction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Different actions a version control task can do upon completion.</para>
  /// </summary>
  public enum CompletionAction
  {
    UpdatePendingWindow = 1,
    OnChangeContentsPendingWindow = 2,
    OnIncomingPendingWindow = 3,
    OnChangeSetsPendingWindow = 4,
    OnGotLatestPendingWindow = 5,
    OnSubmittedChangeWindow = 6,
    OnAddedChangeWindow = 7,
    OnCheckoutCompleted = 8,
  }
}
