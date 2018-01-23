// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ISerializableJsonDictionary
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal interface ISerializableJsonDictionary
  {
    void Set<T>(string key, T value) where T : class;

    T Get<T>(string key) where T : class;

    T GetScriptable<T>(string key) where T : ScriptableObject;

    void Overwrite(object obj, string key);

    bool ContainsKey(string key);

    void OnBeforeSerialize();

    void OnAfterDeserialize();
  }
}
