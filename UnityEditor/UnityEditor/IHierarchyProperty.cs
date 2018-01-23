// Decompiled with JetBrains decompiler
// Type: UnityEditor.IHierarchyProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface IHierarchyProperty
  {
    void Reset();

    int instanceID { get; }

    Object pptrValue { get; }

    string name { get; }

    bool hasChildren { get; }

    int depth { get; }

    int row { get; }

    int colorCode { get; }

    string guid { get; }

    Texture2D icon { get; }

    bool isValid { get; }

    bool isMainRepresentation { get; }

    bool hasFullPreviewImage { get; }

    IconDrawStyle iconDrawStyle { get; }

    bool isFolder { get; }

    bool IsExpanded(int[] expanded);

    bool Next(int[] expanded);

    bool NextWithDepthCheck(int[] expanded, int minDepth);

    bool Previous(int[] expanded);

    bool Parent();

    int[] ancestors { get; }

    bool Find(int instanceID, int[] expanded);

    int[] FindAllAncestors(int[] instanceIDs);

    bool Skip(int count, int[] expanded);

    int CountRemaining(int[] expanded);
  }
}
