// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewPicking
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneViewPicking
  {
    private static bool s_RetainHashes = false;
    private static int s_PreviousTopmostHash = 0;
    private static int s_PreviousPrefixHash = 0;

    static SceneViewPicking()
    {
      Action selectionChanged = Selection.selectionChanged;
      // ISSUE: reference to a compiler-generated field
      if (SceneViewPicking.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneViewPicking.\u003C\u003Ef__mg\u0024cache0 = new Action(SceneViewPicking.ResetHashes);
      }
      // ISSUE: reference to a compiler-generated field
      Action fMgCache0 = SceneViewPicking.\u003C\u003Ef__mg\u0024cache0;
      Selection.selectionChanged = selectionChanged + fMgCache0;
    }

    private static void ResetHashes()
    {
      if (!SceneViewPicking.s_RetainHashes)
      {
        SceneViewPicking.s_PreviousTopmostHash = 0;
        SceneViewPicking.s_PreviousPrefixHash = 0;
      }
      SceneViewPicking.s_RetainHashes = false;
    }

    public static GameObject PickGameObject(Vector2 mousePosition)
    {
      SceneViewPicking.s_RetainHashes = true;
      IEnumerator<GameObject> enumerator = SceneViewPicking.GetAllOverlapping(mousePosition).GetEnumerator();
      if (!enumerator.MoveNext())
        return (GameObject) null;
      GameObject current = enumerator.Current;
      GameObject selectionBase = HandleUtility.FindSelectionBase(current);
      GameObject gameObject = !((UnityEngine.Object) selectionBase == (UnityEngine.Object) null) ? selectionBase : current;
      int hashCode = current.GetHashCode();
      int hash = hashCode;
      if ((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) null)
      {
        SceneViewPicking.s_PreviousTopmostHash = hashCode;
        SceneViewPicking.s_PreviousPrefixHash = hash;
        return gameObject;
      }
      if (hashCode != SceneViewPicking.s_PreviousTopmostHash)
      {
        SceneViewPicking.s_PreviousTopmostHash = hashCode;
        SceneViewPicking.s_PreviousPrefixHash = hash;
        return !((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) selectionBase) ? gameObject : current;
      }
      SceneViewPicking.s_PreviousTopmostHash = hashCode;
      if ((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) selectionBase)
      {
        if (hash == SceneViewPicking.s_PreviousPrefixHash)
          return current;
        SceneViewPicking.s_PreviousPrefixHash = hash;
        return selectionBase;
      }
      if ((UnityEngine.Object) HandleUtility.PickGameObject(mousePosition, false, (GameObject[]) null, new GameObject[1]{ Selection.activeGameObject }) == (UnityEngine.Object) Selection.activeGameObject)
      {
        while ((UnityEngine.Object) enumerator.Current != (UnityEngine.Object) Selection.activeGameObject)
        {
          if (!enumerator.MoveNext())
          {
            SceneViewPicking.s_PreviousPrefixHash = hashCode;
            return gameObject;
          }
          SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
        }
      }
      if (hash != SceneViewPicking.s_PreviousPrefixHash)
      {
        SceneViewPicking.s_PreviousPrefixHash = hashCode;
        return gameObject;
      }
      if (!enumerator.MoveNext())
      {
        SceneViewPicking.s_PreviousPrefixHash = hashCode;
        return gameObject;
      }
      SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
      if ((UnityEngine.Object) enumerator.Current == (UnityEngine.Object) selectionBase)
      {
        if (!enumerator.MoveNext())
        {
          SceneViewPicking.s_PreviousPrefixHash = hashCode;
          return gameObject;
        }
        SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
      }
      SceneViewPicking.s_PreviousPrefixHash = hash;
      return enumerator.Current;
    }

    [DebuggerHidden]
    private static IEnumerable<GameObject> GetAllOverlapping(Vector2 position)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SceneViewPicking.\u003CGetAllOverlapping\u003Ec__Iterator0 overlappingCIterator0 = new SceneViewPicking.\u003CGetAllOverlapping\u003Ec__Iterator0() { position = position };
      // ISSUE: reference to a compiler-generated field
      overlappingCIterator0.\u0024PC = -2;
      return (IEnumerable<GameObject>) overlappingCIterator0;
    }

    private static void UpdateHash(ref int hash, object obj)
    {
      hash = hash * 33 + obj.GetHashCode();
    }
  }
}
