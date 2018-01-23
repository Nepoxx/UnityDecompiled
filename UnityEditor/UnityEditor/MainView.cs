// Decompiled with JetBrains decompiler
// Type: UnityEditor.MainView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MainView : View, ICleanuppable
  {
    private static readonly Vector2 kMinSize = new Vector2(950f, 300f);
    private static readonly Vector2 kMaxSize = new Vector2(10000f, 10000f);
    private const float kStatusbarHeight = 20f;

    private void OnEnable()
    {
      this.SetMinMaxSizes(MainView.kMinSize, MainView.kMaxSize);
    }

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      if (this.children.Length == 0)
        return;
      Toolbar child = (Toolbar) this.children[0];
      this.children[0].position = new Rect(0.0f, 0.0f, newPos.width, child.CalcHeight());
      if (this.children.Length <= 2)
        return;
      this.children[1].position = new Rect(0.0f, child.CalcHeight(), newPos.width, newPos.height - child.CalcHeight() - this.children[2].position.height);
      this.children[2].position = new Rect(0.0f, newPos.height - this.children[2].position.height, newPos.width, this.children[2].position.height);
    }

    protected override void ChildrenMinMaxChanged()
    {
      if (this.children.Length == 3)
      {
        Toolbar child = (Toolbar) this.children[0];
        this.SetMinMaxSizes(new Vector2(MainView.kMinSize.x, Mathf.Max(MainView.kMinSize.y, child.CalcHeight() + 20f + this.children[1].minSize.y)), MainView.kMaxSize);
      }
      base.ChildrenMinMaxChanged();
    }

    public static void MakeMain()
    {
      ContainerWindow instance1 = ScriptableObject.CreateInstance<ContainerWindow>();
      MainView instance2 = ScriptableObject.CreateInstance<MainView>();
      instance2.SetMinMaxSizes(MainView.kMinSize, MainView.kMaxSize);
      instance1.rootView = (View) instance2;
      Resolution currentResolution = Screen.currentResolution;
      int num1 = Mathf.Clamp(currentResolution.width * 3 / 4, 800, 1400);
      int num2 = Mathf.Clamp(currentResolution.height * 3 / 4, 600, 950);
      instance1.position = new Rect(60f, 20f, (float) num1, (float) num2);
      instance1.Show(ShowMode.MainWindow, true, true);
      instance1.DisplayAllViews();
    }

    public void Cleanup()
    {
      if (this.children[1].children.Length != 0)
        return;
      this.window.position.height = ((Toolbar) this.children[0]).CalcHeight() + 20f;
    }
  }
}
