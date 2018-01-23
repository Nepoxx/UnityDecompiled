// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.Speech.PhraseRecognizedEventArgs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Windows.Speech
{
  public struct PhraseRecognizedEventArgs
  {
    public readonly ConfidenceLevel confidence;
    public readonly SemanticMeaning[] semanticMeanings;
    public readonly string text;
    public readonly DateTime phraseStartTime;
    public readonly TimeSpan phraseDuration;

    internal PhraseRecognizedEventArgs(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, DateTime phraseStartTime, TimeSpan phraseDuration)
    {
      this.text = text;
      this.confidence = confidence;
      this.semanticMeanings = semanticMeanings;
      this.phraseStartTime = phraseStartTime;
      this.phraseDuration = phraseDuration;
    }
  }
}
