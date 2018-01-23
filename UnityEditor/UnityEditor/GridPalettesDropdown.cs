// Decompiled with JetBrains decompiler
// Type: UnityEditor.GridPalettesDropdown
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal class GridPalettesDropdown : FlexibleMenu
  {
    public GridPalettesDropdown(IFlexibleMenuItemProvider itemProvider, int selectionIndex, FlexibleMenuModifyItemUI modifyItemUi, Action<int, object> itemClickedCallback, float minWidth)
      : base(itemProvider, selectionIndex, modifyItemUi, itemClickedCallback)
    {
      this.minTextWidth = minWidth;
    }

    internal class MenuItemProvider : IFlexibleMenuItemProvider
    {
      public int Count()
      {
        return GridPalettes.palettes.Count + 1;
      }

      public object GetItem(int index)
      {
        if (index < GridPalettes.palettes.Count)
          return (object) GridPalettes.palettes[index];
        return (object) null;
      }

      public int Add(object obj)
      {
        throw new NotImplementedException();
      }

      public void Replace(int index, object newPresetObject)
      {
        throw new NotImplementedException();
      }

      public void Remove(int index)
      {
        throw new NotImplementedException();
      }

      public object Create()
      {
        throw new NotImplementedException();
      }

      public void Move(int index, int destIndex, bool insertAfterDestIndex)
      {
        throw new NotImplementedException();
      }

      public string GetName(int index)
      {
        if (index < GridPalettes.palettes.Count)
          return GridPalettes.palettes[index].name;
        return index == GridPalettes.palettes.Count ? "Create New Palette" : "";
      }

      public bool IsModificationAllowed(int index)
      {
        return false;
      }

      public int[] GetSeperatorIndices()
      {
        return new int[1]{ GridPalettes.palettes.Count - 1 };
      }
    }
  }
}
