// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyReferenceChecker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor
{
  internal class AssemblyReferenceChecker
  {
    private readonly HashSet<string> _referencedMethods = new HashSet<string>();
    private HashSet<string> _referencedTypes = new HashSet<string>();
    private readonly HashSet<string> _userReferencedMethods = new HashSet<string>();
    private readonly HashSet<string> _definedMethods = new HashSet<string>();
    private HashSet<AssemblyDefinition> _assemblyDefinitions = new HashSet<AssemblyDefinition>();
    private readonly HashSet<string> _assemblyFileNames = new HashSet<string>();
    private DateTime _startTime = DateTime.MinValue;
    private float _progressValue = 0.0f;
    private Action _updateProgressAction;

    public AssemblyReferenceChecker()
    {
      this.HasMouseEvent = false;
      this._updateProgressAction = new Action(this.DisplayProgress);
    }

    public bool HasMouseEvent { get; private set; }

    public static AssemblyReferenceChecker AssemblyReferenceCheckerWithUpdateProgressAction(Action action)
    {
      return new AssemblyReferenceChecker() { _updateProgressAction = action };
    }

    private void CollectReferencesFromRootsRecursive(string dir, IEnumerable<string> roots, bool ignoreSystemDlls)
    {
      DefaultAssemblyResolver assemblyResolver = AssemblyReferenceChecker.AssemblyResolverFor(dir);
      foreach (string root in roots)
      {
        string fileName = Path.Combine(dir, root);
        if (!this._assemblyFileNames.Contains(root))
        {
          AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(fileName, new ReaderParameters() { AssemblyResolver = (IAssemblyResolver) assemblyResolver });
          if (!ignoreSystemDlls || !AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition.Name.Name))
          {
            this._assemblyFileNames.Add(root);
            this._assemblyDefinitions.Add(assemblyDefinition);
            foreach (AssemblyNameReference assemblyReference in assemblyDefinition.MainModule.AssemblyReferences)
            {
              string str = assemblyReference.Name + ".dll";
              if (!this._assemblyFileNames.Contains(str))
                this.CollectReferencesFromRootsRecursive(dir, (IEnumerable<string>) new string[1]
                {
                  str
                }, (ignoreSystemDlls ? 1 : 0) != 0);
            }
          }
        }
      }
    }

    public void CollectReferencesFromRoots(string dir, IEnumerable<string> roots, bool collectMethods, float progressValue, bool ignoreSystemDlls)
    {
      this._progressValue = progressValue;
      this.CollectReferencesFromRootsRecursive(dir, roots, ignoreSystemDlls);
      AssemblyDefinition[] array = this._assemblyDefinitions.ToArray<AssemblyDefinition>();
      this._referencedTypes = MonoAOTRegistration.BuildReferencedTypeList(array);
      if (!collectMethods)
        return;
      this.CollectReferencedAndDefinedMethods((IEnumerable<AssemblyDefinition>) array);
    }

    public void CollectReferences(string path, bool collectMethods, float progressValue, bool ignoreSystemDlls)
    {
      this._progressValue = progressValue;
      this._assemblyDefinitions = new HashSet<AssemblyDefinition>();
      string[] strArray = !Directory.Exists(path) ? new string[0] : Directory.GetFiles(path);
      DefaultAssemblyResolver assemblyResolver = AssemblyReferenceChecker.AssemblyResolverFor(path);
      foreach (string str in strArray)
      {
        if (!(Path.GetExtension(str) != ".dll"))
        {
          AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(str, new ReaderParameters() { AssemblyResolver = (IAssemblyResolver) assemblyResolver });
          if (!ignoreSystemDlls || !AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition.Name.Name))
          {
            this._assemblyFileNames.Add(Path.GetFileName(str));
            this._assemblyDefinitions.Add(assemblyDefinition);
          }
        }
      }
      AssemblyDefinition[] array = this._assemblyDefinitions.ToArray<AssemblyDefinition>();
      this._referencedTypes = MonoAOTRegistration.BuildReferencedTypeList(array);
      if (!collectMethods)
        return;
      this.CollectReferencedAndDefinedMethods((IEnumerable<AssemblyDefinition>) array);
    }

    private void CollectReferencedAndDefinedMethods(IEnumerable<AssemblyDefinition> assemblyDefinitions)
    {
      foreach (AssemblyDefinition assemblyDefinition in assemblyDefinitions)
      {
        bool isSystem = AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition.Name.Name);
        foreach (TypeDefinition type in assemblyDefinition.MainModule.Types)
          this.CollectReferencedAndDefinedMethods(type, isSystem);
      }
    }

    internal void CollectReferencedAndDefinedMethods(TypeDefinition type)
    {
      this.CollectReferencedAndDefinedMethods(type, false);
    }

    internal void CollectReferencedAndDefinedMethods(TypeDefinition type, bool isSystem)
    {
      if (this._updateProgressAction != null)
        this._updateProgressAction();
      foreach (TypeDefinition nestedType in type.NestedTypes)
        this.CollectReferencedAndDefinedMethods(nestedType, isSystem);
      foreach (MethodDefinition method in type.Methods)
      {
        if (method.HasBody)
        {
          foreach (Instruction instruction in method.Body.Instructions)
          {
            if (OpCodes.Call == instruction.OpCode)
            {
              string str = instruction.Operand.ToString();
              if (!isSystem)
                this._userReferencedMethods.Add(str);
              this._referencedMethods.Add(str);
            }
          }
          this._definedMethods.Add(method.ToString());
          this.HasMouseEvent |= this.MethodIsMouseEvent(method);
        }
      }
    }

    private bool MethodIsMouseEvent(MethodDefinition method)
    {
      return (method.Name == "OnMouseDown" || method.Name == "OnMouseDrag" || (method.Name == "OnMouseEnter" || method.Name == "OnMouseExit") || (method.Name == "OnMouseOver" || method.Name == "OnMouseUp") || method.Name == "OnMouseUpAsButton") && method.Parameters.Count == 0 && this.InheritsFromMonoBehaviour((TypeReference) method.DeclaringType);
    }

    private bool InheritsFromMonoBehaviour(TypeReference type)
    {
      if (type.Namespace == "UnityEngine")
      {
        if (type.Name == "MonoBehaviour")
          return true;
      }
      try
      {
        TypeDefinition typeDefinition = type.Resolve();
        if (typeDefinition.BaseType != null)
          return this.InheritsFromMonoBehaviour(typeDefinition.BaseType);
      }
      catch (AssemblyResolutionException ex)
      {
      }
      return false;
    }

    private void DisplayProgress()
    {
      TimeSpan timeSpan = DateTime.Now - this._startTime;
      string[] strArray = new string[2]{ "Fetching assembly references", "Building list of referenced assemblies..." };
      if (timeSpan.TotalMilliseconds < 100.0)
        return;
      if (EditorUtility.DisplayCancelableProgressBar(strArray[0], strArray[1], this._progressValue))
        throw new OperationCanceledException();
      this._startTime = DateTime.Now;
    }

    public bool HasReferenceToMethod(string methodName)
    {
      return this.HasReferenceToMethod(methodName, false);
    }

    public bool HasReferenceToMethod(string methodName, bool ignoreSystemDlls)
    {
      return ignoreSystemDlls ? this._userReferencedMethods.Any<string>((Func<string, bool>) (item => item.Contains(methodName))) : this._referencedMethods.Any<string>((Func<string, bool>) (item => item.Contains(methodName)));
    }

    public bool HasDefinedMethod(string methodName)
    {
      return this._definedMethods.Any<string>((Func<string, bool>) (item => item.Contains(methodName)));
    }

    public bool HasReferenceToType(string typeName)
    {
      return this._referencedTypes.Any<string>((Func<string, bool>) (item => item.StartsWith(typeName)));
    }

    public AssemblyDefinition[] GetAssemblyDefinitions()
    {
      return this._assemblyDefinitions.ToArray<AssemblyDefinition>();
    }

    public string[] GetAssemblyFileNames()
    {
      return this._assemblyFileNames.ToArray<string>();
    }

    public string WhoReferencesClass(string klass, bool ignoreSystemDlls)
    {
      foreach (AssemblyDefinition assemblyDefinition in this._assemblyDefinitions)
      {
        if (!ignoreSystemDlls || !AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition.Name.Name))
        {
          if (MonoAOTRegistration.BuildReferencedTypeList(new AssemblyDefinition[1]{ assemblyDefinition }).Any<string>((Func<string, bool>) (item => item.StartsWith(klass))))
            return assemblyDefinition.Name.Name;
        }
      }
      return (string) null;
    }

    public static bool IsIgnoredSystemDll(string name)
    {
      return name.StartsWith("System") || name.Equals("UnityEngine") || name.StartsWith("UnityEngine.") && name.EndsWith("Module") || (name.Equals("UnityEngine.Networking") || name.Equals("Mono.Posix")) || name.Equals("Moq");
    }

    public static bool GetScriptsHaveMouseEvents(string path)
    {
      AssemblyReferenceChecker referenceChecker = new AssemblyReferenceChecker();
      referenceChecker.CollectReferences(path, true, 0.0f, true);
      return referenceChecker.HasMouseEvent;
    }

    private static DefaultAssemblyResolver AssemblyResolverFor(string path)
    {
      DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
      if (File.Exists(path) || Directory.Exists(path))
      {
        if ((File.GetAttributes(path) & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory)
          path = Path.GetDirectoryName(path);
        assemblyResolver.AddSearchDirectory(Path.GetFullPath(path));
      }
      return assemblyResolver;
    }
  }
}
