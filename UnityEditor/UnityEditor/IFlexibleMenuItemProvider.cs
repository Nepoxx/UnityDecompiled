// Decompiled with JetBrains decompiler
// Type: UnityEditor.IFlexibleMenuItemProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal interface IFlexibleMenuItemProvider
  {
    int Count();

    object GetItem(int index);

    int Add(object obj);

    void Replace(int index, object newPresetObject);

    void Remove(int index);

    object Create();

    void Move(int index, int destIndex, bool insertAfterDestIndex);

    string GetName(int index);

    bool IsModificationAllowed(int index);

    int[] GetSeperatorIndices();
  }
}
