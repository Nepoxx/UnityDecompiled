// Decompiled with JetBrains decompiler
// Type: UnityEditor.IMGUI.Controls.TreeViewItemExpansionAnimator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.IMGUI.Controls
{
  internal class TreeViewItemExpansionAnimator
  {
    private static bool s_Debug = false;
    private TreeViewAnimationInput m_Setup;
    private bool m_InsideGUIClip;
    private Rect m_CurrentClipRect;

    public void BeginAnimating(TreeViewAnimationInput setup)
    {
      if (this.m_Setup != null)
      {
        if (this.m_Setup.item.id == setup.item.id && this.m_Setup.expanding != setup.expanding)
        {
          if (this.m_Setup.elapsedTime >= 0.0)
            setup.elapsedTime = this.m_Setup.animationDuration - this.m_Setup.elapsedTime;
          else
            Debug.LogError((object) ("Invalid duration " + (object) this.m_Setup.elapsedTime));
          this.m_Setup = setup;
        }
        else
        {
          this.SkipAnimating();
          this.m_Setup = setup;
        }
        this.m_Setup.expanding = setup.expanding;
      }
      this.m_Setup = setup;
      if (this.m_Setup == null)
        Debug.LogError((object) "Setup is null");
      if (this.printDebug)
        Console.WriteLine("Begin animating: " + (object) this.m_Setup);
      this.m_CurrentClipRect = this.GetCurrentClippingRect();
    }

    public void SkipAnimating()
    {
      if (this.m_Setup == null)
        return;
      this.m_Setup.FireAnimationEndedEvent();
      this.m_Setup = (TreeViewAnimationInput) null;
    }

    public bool CullRow(int row, ITreeViewGUI gui)
    {
      if (!this.isAnimating)
        return false;
      if (this.printDebug && row == 0)
        Console.WriteLine("--------");
      if (row <= this.m_Setup.startRow || row > this.m_Setup.endRow || (double) gui.GetRowRect(row, 1f).y - (double) this.m_Setup.startRowRect.y <= (double) this.m_CurrentClipRect.height)
        return false;
      if (this.m_InsideGUIClip)
        this.EndClip();
      return true;
    }

    public void OnRowGUI(int row)
    {
      if (!this.printDebug)
        return;
      Console.WriteLine(row.ToString() + " Do item " + this.DebugItemName(row));
    }

    public Rect OnBeginRowGUI(int row, Rect rowRect)
    {
      if (!this.isAnimating)
        return rowRect;
      if (row == this.m_Setup.startRow)
        this.BeginClip();
      if (row >= this.m_Setup.startRow && row <= this.m_Setup.endRow)
        rowRect.y -= this.m_Setup.startRowRect.y;
      else if (row > this.m_Setup.endRow)
        rowRect.y -= this.m_Setup.rowsRect.height - this.m_CurrentClipRect.height;
      return rowRect;
    }

    public void OnEndRowGUI(int row)
    {
      if (!this.isAnimating || !this.m_InsideGUIClip || row != this.m_Setup.endRow)
        return;
      this.EndClip();
    }

    private void BeginClip()
    {
      GUI.BeginClip(this.m_CurrentClipRect);
      this.m_InsideGUIClip = true;
      if (!this.printDebug)
        return;
      Console.WriteLine("BeginClip startRow: " + (object) this.m_Setup.startRow);
    }

    private void EndClip()
    {
      GUI.EndClip();
      this.m_InsideGUIClip = false;
      if (!this.printDebug)
        return;
      Console.WriteLine("EndClip endRow: " + (object) this.m_Setup.endRow);
    }

    public void OnBeforeAllRowsGUI()
    {
      if (!this.isAnimating)
        return;
      this.m_CurrentClipRect = this.GetCurrentClippingRect();
      if (this.m_Setup.elapsedTime <= this.m_Setup.animationDuration)
        return;
      this.m_Setup.FireAnimationEndedEvent();
      this.m_Setup = (TreeViewAnimationInput) null;
      if (this.printDebug)
        Debug.Log((object) "Animation ended");
    }

    public void OnAfterAllRowsGUI()
    {
      if (this.m_InsideGUIClip)
        this.EndClip();
      if (this.isAnimating)
        HandleUtility.Repaint();
      if (!this.isAnimating || Event.current.type != EventType.Repaint)
        return;
      this.m_Setup.CaptureTime();
    }

    public bool IsAnimating(int itemID)
    {
      if (!this.isAnimating)
        return false;
      return this.m_Setup.item.id == itemID;
    }

    public float expandedValueNormalized
    {
      get
      {
        float elapsedTimeNormalized = this.m_Setup.elapsedTimeNormalized;
        return !this.m_Setup.expanding ? 1f - elapsedTimeNormalized : elapsedTimeNormalized;
      }
    }

    public int startRow
    {
      get
      {
        return this.m_Setup.startRow;
      }
    }

    public int endRow
    {
      get
      {
        return this.m_Setup.endRow;
      }
    }

    public float deltaHeight
    {
      get
      {
        return Mathf.Floor(this.m_Setup.rowsRect.height - this.m_Setup.rowsRect.height * this.expandedValueNormalized);
      }
    }

    public bool isAnimating
    {
      get
      {
        return this.m_Setup != null;
      }
    }

    public bool isExpanding
    {
      get
      {
        return this.m_Setup.expanding;
      }
    }

    private Rect GetCurrentClippingRect()
    {
      Rect rowsRect = this.m_Setup.rowsRect;
      rowsRect.height *= this.expandedValueNormalized;
      return rowsRect;
    }

    private bool printDebug
    {
      get
      {
        return TreeViewItemExpansionAnimator.s_Debug && this.m_Setup != null && this.m_Setup.treeView != null && Event.current.type == EventType.Repaint;
      }
    }

    private string DebugItemName(int row)
    {
      return this.m_Setup.treeView.data.GetRows()[row].displayName;
    }
  }
}
