// Decompiled with JetBrains decompiler
// Type: UnityEditor.InitialModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class InitialModuleUI : ModuleUI
  {
    public SerializedProperty m_LengthInSec;
    public SerializedProperty m_Looping;
    public SerializedProperty m_Prewarm;
    public SerializedMinMaxCurve m_StartDelay;
    public SerializedProperty m_PlayOnAwake;
    public SerializedProperty m_SimulationSpace;
    public SerializedProperty m_CustomSimulationSpace;
    public SerializedProperty m_SimulationSpeed;
    public SerializedProperty m_UseUnscaledTime;
    public SerializedProperty m_ScalingMode;
    public SerializedMinMaxCurve m_LifeTime;
    public SerializedMinMaxCurve m_Speed;
    public SerializedMinMaxGradient m_Color;
    public SerializedProperty m_Size3D;
    public SerializedMinMaxCurve m_SizeX;
    public SerializedMinMaxCurve m_SizeY;
    public SerializedMinMaxCurve m_SizeZ;
    public SerializedProperty m_Rotation3D;
    public SerializedMinMaxCurve m_RotationX;
    public SerializedMinMaxCurve m_RotationY;
    public SerializedMinMaxCurve m_RotationZ;
    public SerializedProperty m_RandomizeRotationDirection;
    public SerializedMinMaxCurve m_GravityModifier;
    public SerializedProperty m_EmitterVelocity;
    public SerializedProperty m_MaxNumParticles;
    public SerializedProperty m_AutoRandomSeed;
    public SerializedProperty m_RandomSeed;
    public SerializedProperty m_StopAction;
    private static InitialModuleUI.Texts s_Texts;

    public InitialModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "InitialModule", displayName, ModuleUI.VisibilityState.VisibleAndFoldedOut)
    {
      this.Init();
    }

    public override float GetXAxisScalar()
    {
      return this.m_ParticleSystemUI.GetEmitterDuration();
    }

    protected override void Init()
    {
      if (this.m_LengthInSec != null)
        return;
      if (InitialModuleUI.s_Texts == null)
        InitialModuleUI.s_Texts = new InitialModuleUI.Texts();
      this.m_LengthInSec = this.GetProperty0("lengthInSec");
      this.m_Looping = this.GetProperty0("looping");
      this.m_Prewarm = this.GetProperty0("prewarm");
      this.m_StartDelay = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.startDelay, "startDelay", false, true);
      this.m_StartDelay.m_AllowCurves = false;
      this.m_PlayOnAwake = this.GetProperty0("playOnAwake");
      this.m_SimulationSpace = this.GetProperty0("moveWithTransform");
      this.m_CustomSimulationSpace = this.GetProperty0("moveWithCustomTransform");
      this.m_SimulationSpeed = this.GetProperty0("simulationSpeed");
      this.m_UseUnscaledTime = this.GetProperty0("useUnscaledTime");
      this.m_ScalingMode = this.GetProperty0("scalingMode");
      this.m_EmitterVelocity = this.GetProperty0("useRigidbodyForVelocity");
      this.m_AutoRandomSeed = this.GetProperty0("autoRandomSeed");
      this.m_RandomSeed = this.GetProperty0("randomSeed");
      this.m_StopAction = this.GetProperty0("stopAction");
      this.m_LifeTime = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.lifetime, "startLifetime");
      this.m_Speed = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.speed, "startSpeed", ModuleUI.kUseSignedRange);
      this.m_Color = new SerializedMinMaxGradient((SerializedModule) this, "startColor");
      this.m_Color.m_AllowRandomColor = true;
      this.m_Size3D = this.GetProperty("size3D");
      this.m_SizeX = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.x, "startSize");
      this.m_SizeY = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.y, "startSizeY", false, false, this.m_Size3D.boolValue);
      this.m_SizeZ = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.z, "startSizeZ", false, false, this.m_Size3D.boolValue);
      this.m_Rotation3D = this.GetProperty("rotation3D");
      this.m_RotationX = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.x, "startRotationX", ModuleUI.kUseSignedRange, false, this.m_Rotation3D.boolValue);
      this.m_RotationY = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.y, "startRotationY", ModuleUI.kUseSignedRange, false, this.m_Rotation3D.boolValue);
      this.m_RotationZ = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.z, "startRotation", ModuleUI.kUseSignedRange);
      this.m_RotationX.m_RemapValue = 57.29578f;
      this.m_RotationY.m_RemapValue = 57.29578f;
      this.m_RotationZ.m_RemapValue = 57.29578f;
      this.m_RotationX.m_DefaultCurveScalar = 3.141593f;
      this.m_RotationY.m_DefaultCurveScalar = 3.141593f;
      this.m_RotationZ.m_DefaultCurveScalar = 3.141593f;
      this.m_RandomizeRotationDirection = this.GetProperty("randomizeRotationDirection");
      this.m_GravityModifier = new SerializedMinMaxCurve((ModuleUI) this, InitialModuleUI.s_Texts.gravity, "gravityModifier", ModuleUI.kUseSignedRange);
      this.m_MaxNumParticles = this.GetProperty("maxNumParticles");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      double num1 = (double) ModuleUI.GUIFloat(InitialModuleUI.s_Texts.duration, this.m_LengthInSec, "f2", new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      if (EditorGUI.EndChangeCheck() && ModuleUI.GUIToggle(InitialModuleUI.s_Texts.looping, this.m_Looping))
      {
        foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
        {
          if ((double) particleSystem.time >= (double) particleSystem.main.duration)
            particleSystem.time = 0.0f;
        }
      }
      using (new EditorGUI.DisabledScope(!this.m_Looping.boolValue))
        ModuleUI.GUIToggle(InitialModuleUI.s_Texts.prewarm, this.m_Prewarm);
      using (new EditorGUI.DisabledScope(this.m_Prewarm.boolValue && this.m_Looping.boolValue))
        ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.startDelay, this.m_StartDelay);
      ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.lifetime, this.m_LifeTime);
      ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.speed, this.m_Speed);
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor1 = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.size3D, this.m_Size3D);
      if (EditorGUI.EndChangeCheck() && !addToCurveEditor1)
      {
        this.m_SizeY.RemoveCurveFromEditor();
        this.m_SizeZ.RemoveCurveFromEditor();
      }
      if (!this.m_SizeX.stateHasMultipleDifferentValues)
      {
        this.m_SizeZ.SetMinMaxState(this.m_SizeX.state, addToCurveEditor1);
        this.m_SizeY.SetMinMaxState(this.m_SizeX.state, addToCurveEditor1);
      }
      if (addToCurveEditor1)
      {
        this.m_SizeX.m_DisplayName = InitialModuleUI.s_Texts.x;
        this.GUITripleMinMaxCurve(GUIContent.none, InitialModuleUI.s_Texts.x, this.m_SizeX, InitialModuleUI.s_Texts.y, this.m_SizeY, InitialModuleUI.s_Texts.z, this.m_SizeZ, (SerializedProperty) null);
      }
      else
      {
        this.m_SizeX.m_DisplayName = InitialModuleUI.s_Texts.size;
        ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.size, this.m_SizeX);
      }
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor2 = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.rotation3D, this.m_Rotation3D);
      if (EditorGUI.EndChangeCheck() && !addToCurveEditor2)
      {
        this.m_RotationX.RemoveCurveFromEditor();
        this.m_RotationY.RemoveCurveFromEditor();
      }
      if (!this.m_RotationZ.stateHasMultipleDifferentValues)
      {
        this.m_RotationX.SetMinMaxState(this.m_RotationZ.state, addToCurveEditor2);
        this.m_RotationY.SetMinMaxState(this.m_RotationZ.state, addToCurveEditor2);
      }
      if (addToCurveEditor2)
      {
        this.m_RotationZ.m_DisplayName = InitialModuleUI.s_Texts.z;
        this.GUITripleMinMaxCurve(GUIContent.none, InitialModuleUI.s_Texts.x, this.m_RotationX, InitialModuleUI.s_Texts.y, this.m_RotationY, InitialModuleUI.s_Texts.z, this.m_RotationZ, (SerializedProperty) null);
      }
      else
      {
        this.m_RotationZ.m_DisplayName = InitialModuleUI.s_Texts.rotation;
        ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.rotation, this.m_RotationZ);
      }
      double num2 = (double) ModuleUI.GUIFloat(InitialModuleUI.s_Texts.randomizeRotationDirection, this.m_RandomizeRotationDirection);
      this.GUIMinMaxGradient(InitialModuleUI.s_Texts.color, this.m_Color, false);
      ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.gravity, this.m_GravityModifier);
      if (ModuleUI.GUIPopup(InitialModuleUI.s_Texts.simulationSpace, this.m_SimulationSpace, InitialModuleUI.s_Texts.simulationSpaces) == 2 && this.m_CustomSimulationSpace != null)
        ModuleUI.GUIObject(InitialModuleUI.s_Texts.customSimulationSpace, this.m_CustomSimulationSpace);
      double num3 = (double) ModuleUI.GUIFloat(InitialModuleUI.s_Texts.simulationSpeed, this.m_SimulationSpeed);
      ModuleUI.GUIBoolAsPopup(InitialModuleUI.s_Texts.deltaTime, this.m_UseUnscaledTime, new string[2]
      {
        "Scaled",
        "Unscaled"
      });
      if ((UnityEngine.Object) ((IEnumerable<ParticleSystem>) this.m_ParticleSystemUI.m_ParticleSystems).FirstOrDefault<ParticleSystem>((Func<ParticleSystem, bool>) (o => !o.shape.enabled || o.shape.shapeType != ParticleSystemShapeType.SkinnedMeshRenderer && o.shape.shapeType != ParticleSystemShapeType.MeshRenderer)) != (UnityEngine.Object) null)
        ModuleUI.GUIPopup(InitialModuleUI.s_Texts.scalingMode, this.m_ScalingMode, InitialModuleUI.s_Texts.scalingModes);
      bool boolValue = this.m_PlayOnAwake.boolValue;
      bool newPlayOnAwake = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.autoplay, this.m_PlayOnAwake);
      if (boolValue != newPlayOnAwake)
        this.m_ParticleSystemUI.m_ParticleEffectUI.PlayOnAwakeChanged(newPlayOnAwake);
      ModuleUI.GUIBoolAsPopup(InitialModuleUI.s_Texts.emitterVelocity, this.m_EmitterVelocity, new string[2]
      {
        "Transform",
        "Rigidbody"
      });
      ModuleUI.GUIInt(InitialModuleUI.s_Texts.maxParticles, this.m_MaxNumParticles);
      if (!ModuleUI.GUIToggle(InitialModuleUI.s_Texts.autoRandomSeed, this.m_AutoRandomSeed))
      {
        if (this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner is ParticleSystemInspector)
        {
          GUILayout.BeginHorizontal();
          ModuleUI.GUIInt(InitialModuleUI.s_Texts.randomSeed, this.m_RandomSeed);
          if (!this.m_ParticleSystemUI.multiEdit)
          {
            if (GUILayout.Button("Reseed", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.Width(60f) }))
              this.m_RandomSeed.intValue = this.m_ParticleSystemUI.m_ParticleSystems[0].GenerateRandomSeed();
          }
          GUILayout.EndHorizontal();
        }
        else
        {
          ModuleUI.GUIInt(InitialModuleUI.s_Texts.randomSeed, this.m_RandomSeed);
          if (!this.m_ParticleSystemUI.multiEdit && GUILayout.Button("Reseed", EditorStyles.miniButton, new GUILayoutOption[0]))
            this.m_RandomSeed.intValue = this.m_ParticleSystemUI.m_ParticleSystems[0].GenerateRandomSeed();
        }
      }
      ModuleUI.GUIPopup(InitialModuleUI.s_Texts.stopAction, this.m_StopAction, InitialModuleUI.s_Texts.stopActions);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      if (this.m_SimulationSpace.intValue != 0)
        text += "\nLocal space simulation is not being used.";
      if (this.m_GravityModifier.state == MinMaxCurveState.k_Scalar)
        return;
      text += "\nGravity modifier is not constant.";
    }

    private class Texts
    {
      public GUIContent duration = EditorGUIUtility.TextContent("Duration|The length of time the Particle System is emitting particles. If the system is looping, this indicates the length of one cycle.");
      public GUIContent looping = EditorGUIUtility.TextContent("Looping|If true, the emission cycle will repeat after the duration.");
      public GUIContent prewarm = EditorGUIUtility.TextContent("Prewarm|When played a prewarmed system will be in a state as if it had emitted one loop cycle. Can only be used if the system is looping.");
      public GUIContent startDelay = EditorGUIUtility.TextContent("Start Delay|Delay in seconds that this Particle System will wait before emitting particles. Cannot be used together with a prewarmed looping system.");
      public GUIContent maxParticles = EditorGUIUtility.TextContent("Max Particles|The number of particles in the system will be limited by this number. Emission will be temporarily halted if this is reached.");
      public GUIContent lifetime = EditorGUIUtility.TextContent("Start Lifetime|Start lifetime in seconds, particle will die when its lifetime reaches 0.");
      public GUIContent speed = EditorGUIUtility.TextContent("Start Speed|The start speed of particles, applied in the starting direction.");
      public GUIContent color = EditorGUIUtility.TextContent("Start Color|The start color of particles.");
      public GUIContent size3D = EditorGUIUtility.TextContent("3D Start Size|If enabled, you can control the size separately for each axis.");
      public GUIContent size = EditorGUIUtility.TextContent("Start Size|The start size of particles.");
      public GUIContent rotation3D = EditorGUIUtility.TextContent("3D Start Rotation|If enabled, you can control the rotation separately for each axis.");
      public GUIContent rotation = EditorGUIUtility.TextContent("Start Rotation|The start rotation of particles in degrees.");
      public GUIContent randomizeRotationDirection = EditorGUIUtility.TextContent("Randomize Rotation|Cause some particles to spin in the opposite direction. (Set between 0 and 1, where a higher value causes more to flip)");
      public GUIContent autoplay = EditorGUIUtility.TextContent("Play On Awake*|If enabled, the system will start playing automatically. Note that this setting is shared between all Particle Systems in the current particle effect.");
      public GUIContent gravity = EditorGUIUtility.TextContent("Gravity Modifier|Scales the gravity defined in Physics Manager");
      public GUIContent scalingMode = EditorGUIUtility.TextContent("Scaling Mode|Use the combined scale from our entire hierarchy, just this local particle node, or only apply scale to the shape module.");
      public GUIContent simulationSpace = EditorGUIUtility.TextContent("Simulation Space|Makes particle positions simulate in world, local or custom space. In local space they stay relative to their own Transform, and in custom space they are relative to the custom Transform.");
      public GUIContent customSimulationSpace = EditorGUIUtility.TextContent("Custom Simulation Space|Makes particle positions simulate relative to a custom Transform component.");
      public GUIContent simulationSpeed = EditorGUIUtility.TextContent("Simulation Speed|Scale the playback speed of the Particle System.");
      public GUIContent deltaTime = EditorGUIUtility.TextContent("Delta Time|Use either the Delta Time or the Unscaled Delta Time. Useful for playing effects whilst paused.");
      public GUIContent autoRandomSeed = EditorGUIUtility.TextContent("Auto Random Seed|Simulate differently each time the effect is played.");
      public GUIContent randomSeed = EditorGUIUtility.TextContent("Random Seed|Randomize the look of the Particle System. Using the same seed will make the Particle System play identically each time. After changing this value, restart the Particle System to see the changes, or check the Resimulate box.");
      public GUIContent emitterVelocity = EditorGUIUtility.TextContent("Emitter Velocity|When the Particle System is moving, should we use its Transform, or Rigidbody Component, to calculate its velocity?");
      public GUIContent stopAction = EditorGUIUtility.TextContent("Stop Action|When the Particle System is stopped and all particles have died, should the GameObject automatically disable/destroy itself?");
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
      public GUIContent[] simulationSpaces = new GUIContent[3]{ EditorGUIUtility.TextContent("Local"), EditorGUIUtility.TextContent("World"), EditorGUIUtility.TextContent("Custom") };
      public GUIContent[] scalingModes = new GUIContent[3]{ EditorGUIUtility.TextContent("Hierarchy"), EditorGUIUtility.TextContent("Local"), EditorGUIUtility.TextContent("Shape") };
      public GUIContent[] stopActions = new GUIContent[4]{ EditorGUIUtility.TextContent("None"), EditorGUIUtility.TextContent("Disable"), EditorGUIUtility.TextContent("Destroy"), EditorGUIUtility.TextContent("Callback") };
    }
  }
}
