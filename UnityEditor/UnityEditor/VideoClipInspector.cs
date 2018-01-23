// Decompiled with JetBrains decompiler
// Type: UnityEditor.VideoClipInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Video;

namespace UnityEditor
{
  [CustomEditor(typeof (VideoClip))]
  [CanEditMultipleObjects]
  internal class VideoClipInspector : Editor
  {
    private Vector2 m_Position = Vector2.zero;
    private static readonly GUID kEmptyGUID;
    private VideoClip m_PlayingClip;
    private GUID m_PreviewID;

    public override void OnInspectorGUI()
    {
    }

    private static void Init()
    {
    }

    public void OnDisable()
    {
    }

    public void OnEnable()
    {
    }

    public void OnDestroy()
    {
      this.StopPreview();
    }

    public override bool HasPreviewGUI()
    {
      return this.targets != null;
    }

    private void PlayPreview()
    {
      this.m_PreviewID = VideoUtil.StartPreview(this.m_PlayingClip);
      VideoUtil.PlayPreview(this.m_PreviewID, true);
    }

    private void StopPreview()
    {
      if (!this.m_PreviewID.Empty())
        VideoUtil.StopPreview(this.m_PreviewID);
      this.m_PlayingClip = (VideoClip) null;
      this.m_PreviewID = VideoClipInspector.kEmptyGUID;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      VideoClip target = this.target as VideoClip;
      Event current = Event.current;
      if (current.type != EventType.Repaint && current.type != EventType.Layout && current.type != EventType.Used)
      {
        if (current.type != EventType.MouseDown || !r.Contains(current.mousePosition))
          return;
        if ((UnityEngine.Object) this.m_PlayingClip != (UnityEngine.Object) null)
        {
          if (this.m_PreviewID.Empty() || !VideoUtil.IsPreviewPlaying(this.m_PreviewID))
            this.PlayPreview();
          else
            this.StopPreview();
        }
        current.Use();
      }
      else
      {
        bool flag1 = true;
        bool flag2 = (UnityEngine.Object) target != (UnityEngine.Object) this.m_PlayingClip || !this.m_PreviewID.Empty() && VideoUtil.IsPreviewPlaying(this.m_PreviewID);
        if ((UnityEngine.Object) target != (UnityEngine.Object) this.m_PlayingClip)
        {
          this.StopPreview();
          this.m_PlayingClip = target;
        }
        Texture image = (Texture) null;
        if (!this.m_PreviewID.Empty())
          image = VideoUtil.GetPreviewTexture(this.m_PreviewID);
        if ((UnityEngine.Object) image == (UnityEngine.Object) null || image.width == 0 || image.height == 0)
        {
          image = this.GetAssetPreviewTexture();
          flag1 = false;
        }
        if ((UnityEngine.Object) image == (UnityEngine.Object) null || image.width == 0 || image.height == 0)
          return;
        if (Event.current.type == EventType.Repaint)
          background.Draw(r, false, false, false, false);
        float width = (float) image.width;
        float height = (float) image.height;
        if (this.m_PlayingClip.pixelAspectRatioDenominator > 0U)
          width *= (float) this.m_PlayingClip.pixelAspectRatioNumerator / (float) this.m_PlayingClip.pixelAspectRatioDenominator;
        float num = Mathf.Clamp01((double) r.width / (double) width * (double) height <= (double) r.height ? r.width / width : r.height / height);
        Rect rect = !flag1 ? r : new Rect(r.x, r.y, width * num, (float) image.height * num);
        PreviewGUI.BeginScrollView(r, this.m_Position, rect, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
        if (flag1)
          EditorGUI.DrawTextureTransparent(rect, image, ScaleMode.StretchToFill);
        else
          GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
        this.m_Position = PreviewGUI.EndScrollView();
        if (!flag2)
          return;
        GUIView.current.Repaint();
      }
    }

    private Texture GetAssetPreviewTexture()
    {
      bool flag = AssetPreview.IsLoadingAssetPreview(this.target.GetInstanceID());
      Texture texture = (Texture) AssetPreview.GetAssetPreview(this.target);
      if (!(bool) ((UnityEngine.Object) texture))
      {
        if (flag)
          GUIView.current.Repaint();
        texture = (Texture) AssetPreview.GetMiniThumbnail(this.target);
      }
      return texture;
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      GUI.DrawTexture(iconRect, this.GetAssetPreviewTexture(), ScaleMode.StretchToFill);
    }

    public override string GetInfoString()
    {
      VideoClip target = this.target as VideoClip;
      ulong frameCount = target.frameCount;
      double frameRate = target.frameRate;
      string str = frameRate <= 0.0 ? new TimeSpan(0L).ToString() : TimeSpan.FromSeconds((double) frameCount / frameRate).ToString();
      if (str.IndexOf('.') != -1)
        str = str.Substring(0, str.Length - 4);
      return str + ", " + frameCount.ToString() + " frames" + ", " + frameRate.ToString("F2") + " FPS" + ", " + target.width.ToString() + "x" + target.height.ToString();
    }
  }
}
