using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class LightingWindowLightmapPreviewTab
	{
		private class Styles
		{
			public GUIStyle selectedLightmapHighlight = "LightmapEditorSelectedHighlight";

			public GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout);

			public GUIContent LightProbes = EditorGUIUtility.TrTextContent("Light Probes", "A different LightProbes.asset can be assigned here. These assets are generated by baking a scene containing light probes.", null);

			public GUIContent LightingDataAsset = EditorGUIUtility.TrTextContent("Lighting Data Asset", "A different LightingData.asset can be assigned here. These assets are generated by baking a scene in the OnDemand mode.", null);

			public GUIContent MapsArraySize = EditorGUIUtility.TrTextContent("Array Size", "The length of the array of lightmaps.", null);

			public Styles()
			{
				this.boldFoldout.fontStyle = FontStyle.Bold;
			}
		}

		private enum GlobalMapsViewType
		{
			Performance,
			Memory
		}

		private enum Precision
		{
			Tenths,
			Hundredths
		}

		private const string kEditorPrefsGBuffersLightmapsAlbedoEmissive = "LightingWindowGlobalMapsGLAE";

		private const string kEditorPrefsTransmissionTextures = "LightingWindowGlobalMapsTT";

		private const string kEditorPrefsGeometryData = "LightingWindowGlobalMapsGD";

		private const string kEditorPrefsInFlight = "LightingWindowGlobalMapsIF";

		private Vector2 m_ScrollPositionLightmaps = Vector2.zero;

		private Vector2 m_ScrollPositionMaps = Vector2.zero;

		private int m_SelectedLightmap = -1;

		private static LightingWindowLightmapPreviewTab.Styles s_Styles;

		private static void DrawHeader(Rect rect, bool showdrawDirectionalityHeader, bool showShadowMaskHeader, float maxLightmaps)
		{
			rect.width /= maxLightmaps;
			EditorGUI.DropShadowLabel(rect, "Intensity");
			rect.x += rect.width;
			if (showdrawDirectionalityHeader)
			{
				EditorGUI.DropShadowLabel(rect, "Directionality");
				rect.x += rect.width;
			}
			if (showShadowMaskHeader)
			{
				EditorGUI.DropShadowLabel(rect, "Shadowmask");
			}
		}

		private void MenuSelectLightmapUsers(Rect rect, int lightmapIndex)
		{
			if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
			{
				string[] texts = new string[]
				{
					"Select Lightmap Users"
				};
				Rect position = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f);
				EditorUtility.DisplayCustomMenu(position, EditorGUIUtility.TempContent(texts), -1, new EditorUtility.SelectMenuItemFunction(this.SelectLightmapUsers), lightmapIndex);
				Event.current.Use();
			}
		}

		private void SelectLightmapUsers(object userData, string[] options, int selected)
		{
			int num = (int)userData;
			ArrayList arrayList = new ArrayList();
			MeshRenderer[] array = UnityEngine.Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
			MeshRenderer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				MeshRenderer meshRenderer = array2[i];
				if (meshRenderer != null && meshRenderer.lightmapIndex == num)
				{
					arrayList.Add(meshRenderer.gameObject);
				}
			}
			Terrain[] array3 = UnityEngine.Object.FindObjectsOfType(typeof(Terrain)) as Terrain[];
			Terrain[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				Terrain terrain = array4[j];
				if (terrain != null && terrain.lightmapIndex == num)
				{
					arrayList.Add(terrain.gameObject);
				}
			}
			Selection.objects = (arrayList.ToArray(typeof(UnityEngine.Object)) as UnityEngine.Object[]);
		}

		public void LightmapPreview(Rect r)
		{
			if (LightingWindowLightmapPreviewTab.s_Styles == null)
			{
				LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
			}
			GUI.Box(r, "", "PreBackground");
			this.m_ScrollPositionLightmaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLightmaps, new GUILayoutOption[]
			{
				GUILayout.Height(r.height)
			});
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			for (int i = 0; i < lightmaps.Length; i++)
			{
				LightmapData lightmapData = lightmaps[i];
				if (lightmapData.lightmapDir != null)
				{
					flag = true;
				}
				if (lightmapData.shadowMask != null)
				{
					flag2 = true;
				}
			}
			float num2 = 1f;
			if (flag)
			{
				num2 += 1f;
			}
			if (flag2)
			{
				num2 += 1f;
			}
			Rect rect = GUILayoutUtility.GetRect(r.width, r.width, 20f, 20f);
			LightingWindowLightmapPreviewTab.DrawHeader(rect, flag, flag2, num2);
			LightmapData[] lightmaps2 = LightmapSettings.lightmaps;
			for (int j = 0; j < lightmaps2.Length; j++)
			{
				LightmapData lightmapData2 = lightmaps2[j];
				if (lightmapData2.lightmapColor == null && lightmapData2.lightmapDir == null && lightmapData2.shadowMask == null)
				{
					num++;
				}
				else
				{
					int num3 = (!lightmapData2.lightmapColor) ? -1 : Math.Max(lightmapData2.lightmapColor.width, lightmapData2.lightmapColor.height);
					int num4 = (!lightmapData2.lightmapDir) ? -1 : Math.Max(lightmapData2.lightmapDir.width, lightmapData2.lightmapDir.height);
					int num5 = (!lightmapData2.shadowMask) ? -1 : Math.Max(lightmapData2.shadowMask.width, lightmapData2.shadowMask.height);
					Texture2D texture2D;
					if (num3 > num4)
					{
						texture2D = ((num3 <= num5) ? lightmapData2.shadowMask : lightmapData2.lightmapColor);
					}
					else
					{
						texture2D = ((num4 <= num5) ? lightmapData2.shadowMask : lightmapData2.lightmapDir);
					}
					GUILayoutOption[] options = new GUILayoutOption[]
					{
						GUILayout.MaxWidth(r.width),
						GUILayout.MaxHeight((float)texture2D.height)
					};
					Rect aspectRect = GUILayoutUtility.GetAspectRect(num2, options);
					float num6 = 5f;
					aspectRect.width /= num2;
					aspectRect.width -= num6;
					aspectRect.x += num6 / 2f;
					EditorGUI.DrawPreviewTexture(aspectRect, lightmapData2.lightmapColor);
					this.MenuSelectLightmapUsers(aspectRect, num);
					if (lightmapData2.lightmapDir)
					{
						aspectRect.x += aspectRect.width + num6;
						EditorGUI.DrawPreviewTexture(aspectRect, lightmapData2.lightmapDir);
						this.MenuSelectLightmapUsers(aspectRect, num);
					}
					if (lightmapData2.shadowMask)
					{
						aspectRect.x += aspectRect.width + num6;
						EditorGUI.DrawPreviewTexture(aspectRect, lightmapData2.shadowMask);
						this.MenuSelectLightmapUsers(aspectRect, num);
					}
					GUILayout.Space(10f);
					num++;
				}
			}
			EditorGUILayout.EndScrollView();
		}

		public void UpdateLightmapSelection()
		{
			Terrain terrain = null;
			MeshRenderer component;
			if (Selection.activeGameObject == null || ((component = Selection.activeGameObject.GetComponent<MeshRenderer>()) == null && (terrain = Selection.activeGameObject.GetComponent<Terrain>()) == null))
			{
				this.m_SelectedLightmap = -1;
			}
			else
			{
				this.m_SelectedLightmap = ((!(component != null)) ? terrain.lightmapIndex : component.lightmapIndex);
			}
		}

		private string SizeString(float size)
		{
			return size.ToString("0.0") + " MB";
		}

		private float SumSizes(float[] sizes)
		{
			float num = 0f;
			for (int i = 0; i < sizes.Length; i++)
			{
				float num2 = sizes[i];
				num += num2;
			}
			return num;
		}

		private ulong SumCounts(ulong[] counts)
		{
			ulong num = 0uL;
			for (int i = 0; i < counts.Length; i++)
			{
				ulong num2 = counts[i];
				num += num2;
			}
			return num;
		}

		private void ShowObjectNamesSizesAndCounts(string foldoutName, string editorPrefsName, string[] objectNames, float[] sizes, ulong[] counts, LightingWindowLightmapPreviewTab.Precision precision)
		{
			if (objectNames.Length != 0)
			{
				string text = (counts.Length <= 0) ? "" : (this.SumCounts(counts).ToString() + " tris, ");
				string text2 = this.SizeString(this.SumSizes(sizes));
				string content = string.Concat(new string[]
				{
					foldoutName,
					" (",
					text,
					text2,
					")"
				});
				bool @bool = EditorPrefs.GetBool(editorPrefsName, true);
				bool flag = EditorGUILayout.Foldout(@bool, content, true, LightingWindowLightmapPreviewTab.s_Styles.boldFoldout);
				if (flag != @bool)
				{
					EditorPrefs.SetBool(editorPrefsName, flag);
				}
				if (flag)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					string[] separator = new string[]
					{
						" | "
					};
					GUILayout.Space(20f);
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					for (int i = 0; i < objectNames.Length; i++)
					{
						string text3 = objectNames[i];
						string[] array = text3.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						string text4 = array[0];
						string tooltip = "";
						if (array.Length > 1)
						{
							tooltip = array[1];
						}
						GUILayout.Label(new GUIContent(text4, tooltip), EditorStyles.miniLabel, new GUILayoutOption[0]);
					}
					GUILayout.EndVertical();
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					for (int j = 0; j < counts.Length; j++)
					{
						GUILayout.Label(counts[j].ToString(), EditorStyles.miniLabel, new GUILayoutOption[0]);
					}
					GUILayout.EndVertical();
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					string format = (precision != LightingWindowLightmapPreviewTab.Precision.Tenths) ? "0.00" : "0.0";
					for (int k = 0; k < sizes.Length; k++)
					{
						GUILayout.Label(sizes[k].ToString(format) + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.Space(10f);
				}
			}
		}

		public void Maps()
		{
			if (LightingWindowLightmapPreviewTab.s_Styles == null)
			{
				LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
			}
			GUI.changed = false;
			if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
			{
				SerializedObject serializedObject = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
				SerializedProperty property = serializedObject.FindProperty("m_LightingDataAsset");
				EditorGUILayout.PropertyField(property, LightingWindowLightmapPreviewTab.s_Styles.LightingDataAsset, new GUILayoutOption[0]);
				serializedObject.ApplyModifiedProperties();
			}
			GUILayout.Space(10f);
			LightingWindowLightmapPreviewTab.GlobalMapsViewType globalMapsViewType = LightingWindowLightmapPreviewTab.GlobalMapsViewType.Performance;
			if (EditorPrefs.GetBool("DeveloperMode", false))
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				globalMapsViewType = (LightingWindowLightmapPreviewTab.GlobalMapsViewType)EditorPrefs.GetInt("LightingWindowGlobalMapsViewType", (int)globalMapsViewType);
				EditorGUI.BeginChangeCheck();
				globalMapsViewType = (LightingWindowLightmapPreviewTab.GlobalMapsViewType)GUILayout.Toolbar((int)globalMapsViewType, new string[]
				{
					"Performance",
					"Memory"
				}, EditorStyles.miniButton, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (EditorGUI.EndChangeCheck())
				{
					EditorPrefs.SetInt("LightingWindowGlobalMapsViewType", (int)globalMapsViewType);
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			this.m_ScrollPositionMaps = GUILayout.BeginScrollView(this.m_ScrollPositionMaps, new GUILayoutOption[0]);
			bool showDirLightmap = false;
			bool showShadowMask = false;
			LightmapData[] array = lightmaps;
			for (int i = 0; i < array.Length; i++)
			{
				LightmapData lightmapData = array[i];
				if (lightmapData.lightmapDir != null)
				{
					showDirLightmap = true;
				}
				if (lightmapData.shadowMask != null)
				{
					showShadowMask = true;
				}
			}
			if (globalMapsViewType == LightingWindowLightmapPreviewTab.GlobalMapsViewType.Performance)
			{
				this.PerformanceCentricView(lightmaps, showDirLightmap, showShadowMask, globalMapsViewType);
			}
			else
			{
				this.MemoryCentricView(lightmaps, showDirLightmap, showShadowMask, globalMapsViewType);
			}
			GUILayout.EndScrollView();
		}

		private void MemoryCentricView(LightmapData[] lightmaps, bool showDirLightmap, bool showShadowMask, LightingWindowLightmapPreviewTab.GlobalMapsViewType viewType)
		{
			Lightmapping.ResetExplicitlyShownMemLabels();
			Dictionary<Hash128, SortedList<int, int>> dictionary = new Dictionary<Hash128, SortedList<int, int>>();
			for (int i = 0; i < lightmaps.Length; i++)
			{
				Hash128 key;
				if (Lightmapping.GetGBufferHash(i, out key))
				{
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new SortedList<int, int>());
					}
					dictionary[key].Add(i, i);
				}
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			foreach (KeyValuePair<Hash128, SortedList<int, int>> current in dictionary)
			{
				Hash128 key2 = current.Key;
				float gBufferMemory = Lightmapping.GetGBufferMemory(ref key2);
				num += gBufferMemory;
				SortedList<int, int> value = current.Value;
				foreach (KeyValuePair<int, int> current2 in value)
				{
					LightmapMemory lightmapMemory = Lightmapping.GetLightmapMemory(current2.Value);
					num2 += lightmapMemory.lightmapDataSize;
					num2 += lightmapMemory.lightmapTexturesSize;
					num3 += lightmapMemory.albedoDataSize;
					num3 += lightmapMemory.albedoTextureSize;
					num3 += lightmapMemory.emissiveDataSize;
					num3 += lightmapMemory.emissiveTextureSize;
				}
			}
			if (dictionary.Count > 0)
			{
				string content = string.Format("G-buffers ({0}) | Lightmaps ({1}) | Albedo/Emissive ({2})", this.SizeString(num), this.SizeString(num2), this.SizeString(num3));
				bool @bool = EditorPrefs.GetBool("LightingWindowGlobalMapsGLAE", true);
				bool flag = EditorGUILayout.Foldout(@bool, content, true, LightingWindowLightmapPreviewTab.s_Styles.boldFoldout);
				if (flag != @bool)
				{
					EditorPrefs.SetBool("LightingWindowGlobalMapsGLAE", flag);
				}
				if (flag)
				{
					foreach (KeyValuePair<Hash128, SortedList<int, int>> current3 in dictionary)
					{
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						GUILayout.Space(15f);
						GUILayout.BeginVertical(new GUILayoutOption[0]);
						Hash128 key3 = current3.Key;
						GUILayout.Label(EditorGUIUtility.TrTextContent("G-buffer: " + Lightmapping.GetGBufferMemory(ref key3).ToString("0.0") + " MB", key3.ToString(), null), EditorStyles.miniLabel, new GUILayoutOption[]
						{
							GUILayout.ExpandWidth(false)
						});
						SortedList<int, int> value2 = current3.Value;
						foreach (KeyValuePair<int, int> current4 in value2)
						{
							this.LightmapRow(current4.Value, lightmaps, showDirLightmap, showShadowMask, viewType);
						}
						GUILayout.EndVertical();
						GUILayout.EndHorizontal();
						GUILayout.Space(10f);
					}
				}
			}
			ulong[] counts = new ulong[0];
			string[] objectNames;
			float[] sizes;
			Lightmapping.GetTransmissionTexturesMemLabels(out objectNames, out sizes);
			this.ShowObjectNamesSizesAndCounts("Transmission textures", "LightingWindowGlobalMapsTT", objectNames, sizes, counts, LightingWindowLightmapPreviewTab.Precision.Tenths);
			string[] objectNames2;
			float[] sizes2;
			ulong[] counts2;
			Lightmapping.GetGeometryMemory(out objectNames2, out sizes2, out counts2);
			this.ShowObjectNamesSizesAndCounts("Geometry data", "LightingWindowGlobalMapsGD", objectNames2, sizes2, counts2, LightingWindowLightmapPreviewTab.Precision.Hundredths);
			string[] objectNames3;
			float[] sizes3;
			Lightmapping.GetNotShownMemLabels(out objectNames3, out sizes3);
			string foldoutName = (!Lightmapping.isProgressiveLightmapperDone) ? "In-flight" : "Leaks";
			this.ShowObjectNamesSizesAndCounts(foldoutName, "LightingWindowGlobalMapsIF", objectNames3, sizes3, counts, LightingWindowLightmapPreviewTab.Precision.Tenths);
		}

		private void PerformanceCentricView(LightmapData[] lightmaps, bool showDirLightmap, bool showShadowMask, LightingWindowLightmapPreviewTab.GlobalMapsViewType viewType)
		{
			for (int i = 0; i < lightmaps.Length; i++)
			{
				this.LightmapRow(i, lightmaps, showDirLightmap, showShadowMask, viewType);
			}
		}

		private void LightmapRow(int index, LightmapData[] lightmaps, bool showDirLightmap, bool showShadowMask, LightingWindowLightmapPreviewTab.GlobalMapsViewType viewType)
		{
			int num = (viewType != LightingWindowLightmapPreviewTab.GlobalMapsViewType.Performance) ? 7 : 5;
			float size = (float)(2 * EditorStyles.miniLabel.margin.top + (num - 1) * Mathf.Max(EditorStyles.miniLabel.margin.top, EditorStyles.miniLabel.margin.bottom) + 2 * EditorStyles.miniLabel.padding.top + (num - 1) * EditorStyles.miniLabel.padding.vertical) + (float)(num - 1) * EditorStyles.miniLabel.lineHeight + (float)EditorStyles.miniLabel.font.fontSize;
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(20f);
			lightmaps[index].lightmapColor = this.LightmapField(lightmaps[index].lightmapColor, index, size);
			if (showDirLightmap)
			{
				GUILayout.Space(5f);
				lightmaps[index].lightmapDir = this.LightmapField(lightmaps[index].lightmapDir, index, size);
			}
			if (showShadowMask)
			{
				GUILayout.Space(5f);
				lightmaps[index].shadowMask = this.LightmapField(lightmaps[index].shadowMask, index, size);
			}
			GUILayout.Space(5f);
			if (viewType == LightingWindowLightmapPreviewTab.GlobalMapsViewType.Performance)
			{
				this.LightmapPerformanceStats(index);
			}
			else
			{
				this.LightmapMemoryStats(index);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private Texture2D LightmapField(Texture2D lightmap, int index, float size)
		{
			Rect rect = GUILayoutUtility.GetRect(size, size, EditorStyles.objectField);
			this.MenuSelectLightmapUsers(rect, index);
			Texture2D result = null;
			using (new EditorGUI.DisabledScope(true))
			{
				result = (EditorGUI.ObjectField(rect, lightmap, typeof(Texture2D), false) as Texture2D);
			}
			if (index == this.m_SelectedLightmap && Event.current.type == EventType.Repaint)
			{
				LightingWindowLightmapPreviewTab.s_Styles.selectedLightmapHighlight.Draw(rect, false, false, false, false);
			}
			return result;
		}

		private void LightmapPerformanceStats(int index)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Index: " + index, EditorStyles.miniLabel, new GUILayoutOption[0]);
			LightmapConvergence lightmapConvergence = Lightmapping.GetLightmapConvergence(index);
			if (lightmapConvergence.IsValid())
			{
				GUILayout.Label("Occupied: " + InternalEditorUtility.CountToString((ulong)((long)lightmapConvergence.occupiedTexelCount)), EditorStyles.miniLabel, new GUILayoutOption[0]);
				GUIContent content = EditorGUIUtility.TrTextContent(string.Concat(new object[]
				{
					"Direct: ",
					lightmapConvergence.minDirectSamples,
					" / ",
					lightmapConvergence.maxDirectSamples,
					" / ",
					lightmapConvergence.avgDirectSamples,
					""
				}), "min / max / avg samples per texel", null);
				GUILayout.Label(content, EditorStyles.miniLabel, new GUILayoutOption[0]);
				GUIContent content2 = EditorGUIUtility.TrTextContent(string.Concat(new object[]
				{
					"GI: ",
					lightmapConvergence.minGISamples,
					" / ",
					lightmapConvergence.maxGISamples,
					" / ",
					lightmapConvergence.avgGISamples,
					""
				}), "min / max / avg samples per texel", null);
				GUILayout.Label(content2, EditorStyles.miniLabel, new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Occupied: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
				GUILayout.Label("Direct: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
				GUILayout.Label("GI: N/A", EditorStyles.miniLabel, new GUILayoutOption[0]);
			}
			float lightmapBakePerformance = Lightmapping.GetLightmapBakePerformance(index);
			if ((double)lightmapBakePerformance >= 0.0)
			{
				GUILayout.Label(lightmapBakePerformance.ToString("0.00") + " mrays/sec", EditorStyles.miniLabel, new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("N/A mrays/sec", EditorStyles.miniLabel, new GUILayoutOption[0]);
			}
			GUILayout.EndVertical();
		}

		private void LightmapMemoryStats(int index)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Index: " + index, EditorStyles.miniLabel, new GUILayoutOption[0]);
			LightmapMemory lightmapMemory = Lightmapping.GetLightmapMemory(index);
			GUILayout.Label("Lightmap data: " + lightmapMemory.lightmapDataSize.ToString("0.0") + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUIContent content;
			if (lightmapMemory.lightmapTexturesSize > 0f)
			{
				content = EditorGUIUtility.TrTextContent("Lightmap textures: " + this.SizeString(lightmapMemory.lightmapTexturesSize), null, null);
			}
			else
			{
				content = EditorGUIUtility.TrTextContent("Lightmap textures: N/A", "This lightmap has converged and is not owned by the Progressive Lightmapper anymore.", null);
			}
			GUILayout.Label(content, EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.Label("Albedo data: " + lightmapMemory.albedoDataSize.ToString("0.0") + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.Label("Albedo texture: " + lightmapMemory.albedoTextureSize.ToString("0.0") + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.Label("Emissive data: " + lightmapMemory.emissiveDataSize.ToString("0.0") + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.Label("Emissive texture: " + lightmapMemory.emissiveTextureSize.ToString("0.0") + " MB", EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}
	}
}