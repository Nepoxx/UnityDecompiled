// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualTreeAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  [Serializable]
  public class VisualTreeAsset : ScriptableObject
  {
    [SerializeField]
    private List<VisualTreeAsset.UsingEntry> m_Usings;
    [SerializeField]
    internal StyleSheet inlineSheet;
    [SerializeField]
    private List<VisualElementAsset> m_VisualElementAssets;
    [SerializeField]
    private List<TemplateAsset> m_TemplateAssets;
    [SerializeField]
    private List<VisualTreeAsset.SlotDefinition> m_Slots;
    [SerializeField]
    private int m_ContentContainerId;

    internal List<VisualElementAsset> visualElementAssets
    {
      get
      {
        return this.m_VisualElementAssets;
      }
      set
      {
        this.m_VisualElementAssets = value;
      }
    }

    internal List<TemplateAsset> templateAssets
    {
      get
      {
        return this.m_TemplateAssets;
      }
      set
      {
        this.m_TemplateAssets = value;
      }
    }

    internal List<VisualTreeAsset.SlotDefinition> slots
    {
      get
      {
        return this.m_Slots;
      }
      set
      {
        this.m_Slots = value;
      }
    }

    internal int contentContainerId
    {
      get
      {
        return this.m_ContentContainerId;
      }
      set
      {
        this.m_ContentContainerId = value;
      }
    }

    public VisualElement CloneTree(Dictionary<string, VisualElement> slotInsertionPoints)
    {
      TemplateContainer templateContainer = new TemplateContainer(this.name);
      this.CloneTree((VisualElement) templateContainer, slotInsertionPoints ?? new Dictionary<string, VisualElement>());
      return (VisualElement) templateContainer;
    }

    public void CloneTree(VisualElement target, Dictionary<string, VisualElement> slotInsertionPoints)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target), "Cannot clone a Visual Tree in a null target");
      if ((this.m_VisualElementAssets == null || this.m_VisualElementAssets.Count <= 0) && (this.m_TemplateAssets == null || this.m_TemplateAssets.Count <= 0))
        return;
      Dictionary<int, List<VisualElementAsset>> idToChildren = new Dictionary<int, List<VisualElementAsset>>();
      int num1 = this.m_VisualElementAssets != null ? this.m_VisualElementAssets.Count : 0;
      int num2 = this.m_TemplateAssets != null ? this.m_TemplateAssets.Count : 0;
      for (int index = 0; index < num1 + num2; ++index)
      {
        VisualElementAsset visualElementAsset = index >= num1 ? (VisualElementAsset) this.m_TemplateAssets[index - num1] : this.m_VisualElementAssets[index];
        List<VisualElementAsset> visualElementAssetList;
        if (!idToChildren.TryGetValue(visualElementAsset.parentId, out visualElementAssetList))
        {
          visualElementAssetList = new List<VisualElementAsset>();
          idToChildren.Add(visualElementAsset.parentId, visualElementAssetList);
        }
        visualElementAssetList.Add(visualElementAsset);
      }
      List<VisualElementAsset> visualElementAssetList1;
      if (!idToChildren.TryGetValue(0, out visualElementAssetList1) || visualElementAssetList1 == null)
        return;
      foreach (VisualElementAsset root in visualElementAssetList1)
      {
        Assert.IsNotNull<VisualElementAsset>(root);
        VisualElement child = this.CloneSetupRecursively(root, idToChildren, new CreationContext(slotInsertionPoints, this, target));
        target.shadow.Add(child);
      }
    }

    private VisualElement CloneSetupRecursively(VisualElementAsset root, Dictionary<int, List<VisualElementAsset>> idToChildren, CreationContext context)
    {
      VisualElement content = root.Create(context);
      if (root.id == context.visualTreeAsset.contentContainerId)
      {
        if (context.target is TemplateContainer)
          ((TemplateContainer) context.target).SetContentContainer(content);
        else
          Debug.LogError((object) "Trying to clone a VisualTreeAsset with a custom content container into a element which is not a template container");
      }
      content.name = root.name;
      string slotName1;
      if (context.slotInsertionPoints != null && this.TryGetSlotInsertionPoint(root.id, out slotName1))
        context.slotInsertionPoints.Add(slotName1, content);
      if (root.classes != null)
      {
        for (int index = 0; index < root.classes.Length; ++index)
          content.AddToClassList(root.classes[index]);
      }
      if (root.ruleIndex != -1)
      {
        if ((UnityEngine.Object) this.inlineSheet == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) "VisualElementAsset has a RuleIndex but no inlineStyleSheet");
        }
        else
        {
          StyleRule rule = this.inlineSheet.rules[root.ruleIndex];
          VisualElementStylesData inlineStyle = new VisualElementStylesData(false);
          content.SetInlineStyles(inlineStyle);
          inlineStyle.ApplyRule(this.inlineSheet, int.MaxValue, rule, StyleSheetCache.GetPropertyIDs(this.inlineSheet, root.ruleIndex));
        }
      }
      TemplateAsset templateAsset = root as TemplateAsset;
      List<VisualElementAsset> visualElementAssetList;
      if (!idToChildren.TryGetValue(root.id, out visualElementAssetList))
        return content;
      foreach (VisualElementAsset visualElementAsset in visualElementAssetList)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VisualTreeAsset.\u003CCloneSetupRecursively\u003Ec__AnonStorey0 recursivelyCAnonStorey0 = new VisualTreeAsset.\u003CCloneSetupRecursively\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        recursivelyCAnonStorey0.childVea = visualElementAsset;
        // ISSUE: reference to a compiler-generated field
        VisualElement child = this.CloneSetupRecursively(recursivelyCAnonStorey0.childVea, idToChildren, context);
        if (child != null)
        {
          if (templateAsset == null)
          {
            content.Add(child);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            int index = templateAsset.slotUsages != null ? templateAsset.slotUsages.FindIndex(new Predicate<VisualTreeAsset.SlotUsageEntry>(recursivelyCAnonStorey0.\u003C\u003Em__0)) : -1;
            if (index != -1)
            {
              string slotName2 = templateAsset.slotUsages[index].slotName;
              Assert.IsFalse(string.IsNullOrEmpty(slotName2), "a lost name should not be null or empty, this probably points to an importer or serialization bug");
              VisualElement visualElement;
              if (context.slotInsertionPoints == null || !context.slotInsertionPoints.TryGetValue(slotName2, out visualElement))
              {
                Debug.LogErrorFormat("Slot '{0}' was not found. Existing slots: {1}", new object[2]
                {
                  (object) slotName2,
                  context.slotInsertionPoints != null ? (object) string.Join(", ", context.slotInsertionPoints.Keys.ToArray<string>()) : (object) string.Empty
                });
                content.Add(child);
              }
              else
                visualElement.Add(child);
            }
            else
              content.Add(child);
          }
        }
      }
      if (templateAsset != null && context.slotInsertionPoints != null)
        context.slotInsertionPoints.Clear();
      return content;
    }

    internal bool SlotDefinitionExists(string slotName)
    {
      if (this.m_Slots == null)
        return false;
      return this.m_Slots.Exists((Predicate<VisualTreeAsset.SlotDefinition>) (s => s.name == slotName));
    }

    internal bool AddSlotDefinition(string slotName, int resId)
    {
      if (this.SlotDefinitionExists(slotName))
        return false;
      if (this.m_Slots == null)
        this.m_Slots = new List<VisualTreeAsset.SlotDefinition>(1);
      this.m_Slots.Add(new VisualTreeAsset.SlotDefinition()
      {
        insertionPointId = resId,
        name = slotName
      });
      return true;
    }

    internal bool TryGetSlotInsertionPoint(int insertionPointId, out string slotName)
    {
      if (this.m_Slots == null)
      {
        slotName = (string) null;
        return false;
      }
      for (int index = 0; index < this.m_Slots.Count; ++index)
      {
        VisualTreeAsset.SlotDefinition slot = this.m_Slots[index];
        if (slot.insertionPointId == insertionPointId)
        {
          slotName = slot.name;
          return true;
        }
      }
      slotName = (string) null;
      return false;
    }

    internal VisualTreeAsset ResolveUsing(string templateAlias)
    {
      if (this.m_Usings == null || this.m_Usings.Count == 0)
        return (VisualTreeAsset) null;
      int index = this.m_Usings.BinarySearch(new VisualTreeAsset.UsingEntry(templateAlias, (string) null), VisualTreeAsset.UsingEntry.comparer);
      if (index < 0)
        return (VisualTreeAsset) null;
      string path = this.m_Usings[index].path;
      return Panel.loadResourceFunc != null ? Panel.loadResourceFunc(path, typeof (VisualTreeAsset)) as VisualTreeAsset : (VisualTreeAsset) null;
    }

    internal bool AliasExists(string templateAlias)
    {
      if (this.m_Usings == null || this.m_Usings.Count == 0)
        return false;
      return this.m_Usings.BinarySearch(new VisualTreeAsset.UsingEntry(templateAlias, (string) null), VisualTreeAsset.UsingEntry.comparer) >= 0;
    }

    internal void RegisterUsing(string alias, string path)
    {
      if (this.m_Usings == null)
        this.m_Usings = new List<VisualTreeAsset.UsingEntry>();
      int index = 0;
      while (index < this.m_Usings.Count && alias.CompareTo(this.m_Usings[index].alias) != -1)
        ++index;
      this.m_Usings.Insert(index, new VisualTreeAsset.UsingEntry(alias, path));
    }

    [Serializable]
    internal struct UsingEntry
    {
      internal static readonly IComparer<VisualTreeAsset.UsingEntry> comparer = (IComparer<VisualTreeAsset.UsingEntry>) new VisualTreeAsset.UsingEntryComparer();
      [SerializeField]
      public string alias;
      [SerializeField]
      public string path;

      public UsingEntry(string alias, string path)
      {
        this.alias = alias;
        this.path = path;
      }
    }

    private class UsingEntryComparer : IComparer<VisualTreeAsset.UsingEntry>
    {
      public int Compare(VisualTreeAsset.UsingEntry x, VisualTreeAsset.UsingEntry y)
      {
        return Comparer<string>.Default.Compare(x.alias, y.alias);
      }
    }

    [Serializable]
    internal struct SlotDefinition
    {
      [SerializeField]
      public string name;
      [SerializeField]
      public int insertionPointId;
    }

    [Serializable]
    internal struct SlotUsageEntry
    {
      [SerializeField]
      public string slotName;
      [SerializeField]
      public int assetId;

      public SlotUsageEntry(string slotName, int assetId)
      {
        this.slotName = slotName;
        this.assetId = assetId;
      }
    }
  }
}
