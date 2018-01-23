// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.Speech.GrammarRecognizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Windows.Speech
{
  public sealed class GrammarRecognizer : PhraseRecognizer
  {
    public GrammarRecognizer(string grammarFilePath)
      : this(grammarFilePath, ConfidenceLevel.Medium)
    {
    }

    public GrammarRecognizer(string grammarFilePath, ConfidenceLevel minimumConfidence)
    {
      if (grammarFilePath == null)
        throw new ArgumentNullException(nameof (grammarFilePath));
      if (grammarFilePath.Length == 0)
        throw new ArgumentException("Grammar file path cannot be empty.");
      this.GrammarFilePath = grammarFilePath;
      this.m_Recognizer = this.CreateFromGrammarFile(grammarFilePath, minimumConfidence);
    }

    public string GrammarFilePath { get; private set; }
  }
}
