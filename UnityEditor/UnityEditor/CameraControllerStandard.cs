// Decompiled with JetBrains decompiler
// Type: UnityEditor.CameraControllerStandard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UnityEditor
{
  internal class CameraControllerStandard : CameraController
  {
    private static PrefKey kFPSForward = new PrefKey("View/FPS Forward", "w");
    private static PrefKey kFPSBack = new PrefKey("View/FPS Back", "s");
    private static PrefKey kFPSLeft = new PrefKey("View/FPS Strafe Left", "a");
    private static PrefKey kFPSRight = new PrefKey("View/FPS Strafe Right", "d");
    private static PrefKey kFPSUp = new PrefKey("View/FPS Strafe Up", "e");
    private static PrefKey kFPSDown = new PrefKey("View/FPS Strafe Down", "q");
    private static TimeHelper m_FPSTiming = new TimeHelper();
    private ViewTool m_CurrentViewTool = ViewTool.None;
    private float m_StartZoom = 0.0f;
    private float m_ZoomSpeed = 0.0f;
    private float m_TotalMotion = 0.0f;
    private Vector3 m_Motion = new Vector3();
    private float m_FlySpeed = 0.0f;
    private const float kFlyAcceleration = 1.1f;

    public ViewTool currentViewTool
    {
      get
      {
        return this.m_CurrentViewTool;
      }
    }

    private void ResetCameraControl()
    {
      this.m_CurrentViewTool = ViewTool.None;
      this.m_Motion = Vector3.zero;
    }

    private void HandleCameraScrollWheel(CameraState cameraState)
    {
      float y = Event.current.delta.y;
      float num = (float) ((double) Mathf.Abs(cameraState.viewSize.value) * (double) y * 0.0149999996647239);
      if ((double) num > 0.0 && (double) num < 0.300000011920929)
        num = 0.3f;
      else if ((double) num < 0.0 && (double) num > -0.300000011920929)
        num = -0.3f;
      cameraState.viewSize.value += num;
      Event.current.Use();
    }

    private void OrbitCameraBehavior(CameraState cameraState, Camera cam)
    {
      Event current = Event.current;
      cameraState.FixNegativeSize();
      Quaternion target = cameraState.rotation.target;
      Quaternion quaternion1 = Quaternion.AngleAxis((float) ((double) current.delta.y * (3.0 / 1000.0) * 57.2957801818848), target * Vector3.right) * target;
      Quaternion quaternion2 = Quaternion.AngleAxis((float) ((double) current.delta.x * (3.0 / 1000.0) * 57.2957801818848), Vector3.up) * quaternion1;
      if ((double) cameraState.viewSize.value < 0.0)
      {
        cameraState.pivot.value = cam.transform.position;
        cameraState.viewSize.value = 0.0f;
      }
      cameraState.rotation.value = quaternion2;
    }

    private void HandleCameraMouseDrag(CameraState cameraState, Camera cam)
    {
      Event current = Event.current;
      switch (this.m_CurrentViewTool)
      {
        case ViewTool.Orbit:
          this.OrbitCameraBehavior(cameraState, cam);
          break;
        case ViewTool.Pan:
          cameraState.FixNegativeSize();
          Vector3 position = cam.WorldToScreenPoint(cameraState.pivot.value) + new Vector3(-Event.current.delta.x, Event.current.delta.y, 0.0f);
          Vector3 vector3_1 = cam.ScreenToWorldPoint(position) - cameraState.pivot.value;
          if (current.shift)
            vector3_1 *= 4f;
          AnimVector3 pivot = cameraState.pivot;
          pivot.value = pivot.value + vector3_1;
          break;
        case ViewTool.Zoom:
          float num = HandleUtility.niceMouseDeltaZoom * (!current.shift ? 3f : 9f);
          this.m_TotalMotion += num;
          if ((double) this.m_TotalMotion < 0.0)
          {
            cameraState.viewSize.value = this.m_StartZoom * (float) (1.0 + (double) this.m_TotalMotion * (1.0 / 1000.0));
            break;
          }
          cameraState.viewSize.value += (float) ((double) num * (double) this.m_ZoomSpeed * (3.0 / 1000.0));
          break;
        case ViewTool.FPS:
          Vector3 vector3_2 = cameraState.pivot.value - cameraState.rotation.value * Vector3.forward * cameraState.GetCameraDistance();
          Quaternion quaternion1 = cameraState.rotation.value;
          Quaternion quaternion2 = Quaternion.AngleAxis((float) ((double) current.delta.y * (3.0 / 1000.0) * 57.2957801818848), quaternion1 * Vector3.right) * quaternion1;
          Quaternion quaternion3 = Quaternion.AngleAxis((float) ((double) current.delta.x * (3.0 / 1000.0) * 57.2957801818848), Vector3.up) * quaternion2;
          cameraState.rotation.value = quaternion3;
          cameraState.pivot.value = vector3_2 + quaternion3 * Vector3.forward * cameraState.GetCameraDistance();
          break;
      }
      current.Use();
    }

    private void HandleCameraKeyDown()
    {
      if (Event.current.keyCode == KeyCode.Escape)
        this.ResetCameraControl();
      if (this.m_CurrentViewTool != ViewTool.FPS)
        return;
      Event current = Event.current;
      Vector3 motion = this.m_Motion;
      if (current.keyCode == (Event) CameraControllerStandard.kFPSForward.keyCode)
      {
        this.m_Motion.z = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSBack.keyCode)
      {
        this.m_Motion.z = -1f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSLeft.keyCode)
      {
        this.m_Motion.x = -1f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSRight.keyCode)
      {
        this.m_Motion.x = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSUp.keyCode)
      {
        this.m_Motion.y = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSDown.keyCode)
      {
        this.m_Motion.y = -1f;
        current.Use();
      }
      if (current.type != EventType.KeyDown && (double) motion.sqrMagnitude == 0.0)
        CameraControllerStandard.m_FPSTiming.Begin();
    }

    private void HandleCameraKeyUp()
    {
      if (this.m_CurrentViewTool != ViewTool.FPS)
        return;
      Event current = Event.current;
      if (current.keyCode == (Event) CameraControllerStandard.kFPSForward.keyCode || current.keyCode == (Event) CameraControllerStandard.kFPSBack.keyCode)
      {
        this.m_Motion.z = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSLeft.keyCode || current.keyCode == (Event) CameraControllerStandard.kFPSRight.keyCode)
      {
        this.m_Motion.x = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) CameraControllerStandard.kFPSUp.keyCode || current.keyCode == (Event) CameraControllerStandard.kFPSDown.keyCode)
      {
        this.m_Motion.y = 0.0f;
        current.Use();
      }
    }

    private void HandleCameraMouseUp()
    {
      this.ResetCameraControl();
      Event.current.Use();
    }

    private Vector3 GetMovementDirection()
    {
      float p = CameraControllerStandard.m_FPSTiming.Update();
      if ((double) this.m_Motion.sqrMagnitude == 0.0)
      {
        this.m_FlySpeed = 0.0f;
        return Vector3.zero;
      }
      float num = !Event.current.shift ? 1f : 5f;
      if ((double) this.m_FlySpeed == 0.0)
        this.m_FlySpeed = 9f;
      else
        this.m_FlySpeed *= Mathf.Pow(1.1f, p);
      return this.m_Motion.normalized * this.m_FlySpeed * num * p;
    }

    public override void Update(CameraState cameraState, Camera cam)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp)
        this.m_CurrentViewTool = ViewTool.None;
      if (current.type == EventType.MouseDown)
      {
        int button = current.button;
        bool flag = current.control && Application.platform == RuntimePlatform.OSXEditor;
        if (button == 2)
          this.m_CurrentViewTool = ViewTool.Pan;
        else if (button <= 0 && flag || button == 1 && current.alt)
        {
          this.m_CurrentViewTool = ViewTool.Zoom;
          this.m_StartZoom = cameraState.viewSize.value;
          this.m_ZoomSpeed = Mathf.Max(Mathf.Abs(this.m_StartZoom), 0.3f);
          this.m_TotalMotion = 0.0f;
        }
        else if (button <= 0)
          this.m_CurrentViewTool = ViewTool.Orbit;
        else if (button == 1 && !current.alt)
          this.m_CurrentViewTool = ViewTool.FPS;
      }
      switch (current.type)
      {
        case EventType.MouseUp:
          this.HandleCameraMouseUp();
          break;
        case EventType.MouseDrag:
          this.HandleCameraMouseDrag(cameraState, cam);
          break;
        case EventType.KeyDown:
          this.HandleCameraKeyDown();
          break;
        case EventType.KeyUp:
          this.HandleCameraKeyUp();
          break;
        case EventType.ScrollWheel:
          this.HandleCameraScrollWheel(cameraState);
          break;
        case EventType.Layout:
          Vector3 movementDirection = this.GetMovementDirection();
          if ((double) movementDirection.sqrMagnitude == 0.0)
            break;
          cameraState.pivot.value = cameraState.pivot.value + cameraState.rotation.value * movementDirection;
          break;
      }
    }
  }
}
