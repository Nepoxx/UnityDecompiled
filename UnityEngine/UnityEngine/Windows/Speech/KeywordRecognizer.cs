// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.Speech.KeywordRecognizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Windows.Speech
{
  public sealed class KeywordRecognizer : PhraseRecognizer
  {
    public KeywordRecognizer(string[] keywords)
      : this(keywords, ConfidenceLevel.Medium)
    {
    }

    public KeywordRecognizer(string[] keywords, ConfidenceLevel minimumConfidence)
    {
      if (keywords == null)
        throw new ArgumentNullException(nameof (keywords));
      if (keywords.Length == 0)
        throw new ArgumentException("At least one keyword must be specified.", nameof (keywords));
      int length = keywords.Length;
      for (int index = 0; index < length; ++index)
      {
        if (keywords[index] == null)
          throw new ArgumentNullException(string.Format("Keyword at index {0} is null.", (object) index));
      }
      this.Keywords = (IEnumerable<string>) keywords;
      this.m_Recognizer = this.CreateFromKeywords(keywords, minimumConfidence);
    }

    public IEnumerable<string> Keywords { get; private set; }
  }
}
