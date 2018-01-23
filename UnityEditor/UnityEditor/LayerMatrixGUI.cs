// Decompiled with JetBrains decompiler
// Type: UnityEditor.LayerMatrixGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LayerMatrixGUI
  {
    public static void DoGUI(string title, ref bool show, ref Vector2 scrollPos, LayerMatrixGUI.GetValueFunc getValue, LayerMatrixGUI.SetValueFunc setValue)
    {
      int num1 = 100;
      int num2 = 0;
      for (int layer = 0; layer < 32; ++layer)
      {
        if (LayerMask.LayerToName(layer) != "")
          ++num2;
      }
      for (int layer = 0; layer < 32; ++layer)
      {
        Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(LayerMask.LayerToName(layer)));
        if ((double) num1 < (double) vector2.x)
          num1 = (int) vector2.x;
      }
      GUILayout.BeginHorizontal();
      GUILayout.Space(0.0f);
      show = EditorGUILayout.Foldout(show, title, true);
      GUILayout.EndHorizontal();
      if (!show)
        return;
      scrollPos = GUILayout.BeginScrollView(scrollPos, new GUILayoutOption[2]
      {
        GUILayout.MinHeight((float) (num1 + 20)),
        GUILayout.MaxHeight((float) (num1 + (num2 + 1) * 16))
      });
      Rect rect1 = GUILayoutUtility.GetRect((float) (16 * num2 + num1), (float) num1);
      Rect topmostRect = GUIClip.topmostRect;
      Vector2 vector2_1 = GUIClip.Unclip(new Vector2(rect1.x, rect1.y));
      int num3 = 0;
      for (int layer = 0; layer < 32; ++layer)
      {
        if (LayerMask.LayerToName(layer) != "")
        {
          float num4 = (float) (num1 + 30 + (num2 - num3) * 16) - (topmostRect.width + scrollPos.x);
          if ((double) num4 < 0.0)
            num4 = 0.0f;
          GUI.matrix = Matrix4x4.TRS(new Vector3((float) (num1 + 30 + 16 * (num2 - num3)) + vector2_1.y + vector2_1.x + scrollPos.y - num4, vector2_1.y + scrollPos.y, 0.0f), Quaternion.identity, Vector3.one) * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 90f), Vector3.one);
          GUI.Label(new Rect(2f - vector2_1.x - scrollPos.y, scrollPos.y - num4, (float) num1, 16f), LayerMask.LayerToName(layer), (GUIStyle) "RightLabel");
          ++num3;
        }
      }
      GUI.matrix = Matrix4x4.identity;
      int num5 = 0;
      for (int index1 = 0; index1 < 32; ++index1)
      {
        if (LayerMask.LayerToName(index1) != "")
        {
          int num4 = 0;
          Rect rect2 = GUILayoutUtility.GetRect((float) (30 + 16 * num2 + num1), 16f);
          GUI.Label(new Rect(rect2.x + 30f, rect2.y, (float) num1, 16f), LayerMask.LayerToName(index1), (GUIStyle) "RightLabel");
          for (int index2 = 31; index2 >= 0; --index2)
          {
            if (LayerMask.LayerToName(index2) != "")
            {
              if (num4 < num2 - num5)
              {
                GUIContent content = new GUIContent("", LayerMask.LayerToName(index1) + "/" + LayerMask.LayerToName(index2));
                bool flag = getValue(index1, index2);
                bool val = GUI.Toggle(new Rect((float) (num1 + 30) + rect2.x + (float) (num4 * 16), rect2.y, 16f, 16f), flag, content);
                if (val != flag)
                  setValue(index1, index2, val);
              }
              ++num4;
            }
          }
          ++num5;
        }
      }
      GUILayout.EndScrollView();
    }

    public delegate bool GetValueFunc(int layerA, int layerB);

    public delegate void SetValueFunc(int layerA, int layerB, bool val);
  }
}
