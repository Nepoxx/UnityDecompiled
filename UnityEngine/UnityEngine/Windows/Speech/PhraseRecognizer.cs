// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.Speech.PhraseRecognizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
  /// <summary>
  ///   <para>A common base class for both keyword recognizer and grammar recognizer.</para>
  /// </summary>
  public abstract class PhraseRecognizer : IDisposable
  {
    protected IntPtr m_Recognizer;

    internal PhraseRecognizer()
    {
    }

    protected IntPtr CreateFromKeywords(string[] keywords, ConfidenceLevel minimumConfidence)
    {
      IntPtr num;
      PhraseRecognizer.INTERNAL_CALL_CreateFromKeywords(this, keywords, minimumConfidence, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CreateFromKeywords(PhraseRecognizer self, string[] keywords, ConfidenceLevel minimumConfidence, out IntPtr value);

    protected IntPtr CreateFromGrammarFile(string grammarFilePath, ConfidenceLevel minimumConfidence)
    {
      IntPtr num;
      PhraseRecognizer.INTERNAL_CALL_CreateFromGrammarFile(this, grammarFilePath, minimumConfidence, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CreateFromGrammarFile(PhraseRecognizer self, string grammarFilePath, ConfidenceLevel minimumConfidence, out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Start_Internal(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Stop_Internal(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsRunning_Internal(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Destroy(IntPtr recognizer);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DestroyThreaded(IntPtr recognizer);

    public event PhraseRecognizer.PhraseRecognizedDelegate OnPhraseRecognized;

    ~PhraseRecognizer()
    {
      if (!(this.m_Recognizer != IntPtr.Zero))
        return;
      PhraseRecognizer.DestroyThreaded(this.m_Recognizer);
      this.m_Recognizer = IntPtr.Zero;
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///   <para>Makes the phrase recognizer start listening to phrases.</para>
    /// </summary>
    public void Start()
    {
      if (this.m_Recognizer == IntPtr.Zero)
        return;
      PhraseRecognizer.Start_Internal(this.m_Recognizer);
    }

    /// <summary>
    ///   <para>Stops the phrase recognizer from listening to phrases.</para>
    /// </summary>
    public void Stop()
    {
      if (this.m_Recognizer == IntPtr.Zero)
        return;
      PhraseRecognizer.Stop_Internal(this.m_Recognizer);
    }

    /// <summary>
    ///   <para>Disposes the resources used by phrase recognizer.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_Recognizer != IntPtr.Zero)
      {
        PhraseRecognizer.Destroy(this.m_Recognizer);
        this.m_Recognizer = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///   <para>Tells whether the phrase recognizer is listening for phrases.</para>
    /// </summary>
    public bool IsRunning
    {
      get
      {
        return this.m_Recognizer != IntPtr.Zero && PhraseRecognizer.IsRunning_Internal(this.m_Recognizer);
      }
    }

    [RequiredByNativeCode]
    private void InvokePhraseRecognizedEvent(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, long phraseStartFileTime, long phraseDurationTicks)
    {
      // ISSUE: reference to a compiler-generated field
      PhraseRecognizer.PhraseRecognizedDelegate phraseRecognized = this.OnPhraseRecognized;
      if (phraseRecognized == null)
        return;
      phraseRecognized(new PhraseRecognizedEventArgs(text, confidence, semanticMeanings, DateTime.FromFileTime(phraseStartFileTime), TimeSpan.FromTicks(phraseDurationTicks)));
    }

    [RequiredByNativeCode]
    private static unsafe SemanticMeaning[] MarshalSemanticMeaning(IntPtr keys, IntPtr values, IntPtr valueSizes, int valueCount)
    {
      SemanticMeaning[] semanticMeaningArray = new SemanticMeaning[valueCount];
      int num1 = 0;
      for (int index1 = 0; index1 < valueCount; ++index1)
      {
        uint num2 = *(uint*) ((IntPtr) (void*) valueSizes + (IntPtr) index1 * 4);
        SemanticMeaning semanticMeaning = new SemanticMeaning() { key = new string(((char**) (void*) keys)[index1]), values = new string[(IntPtr) num2] };
        for (int index2 = 0; (long) index2 < (long) num2; ++index2)
          semanticMeaning.values[index2] = new string(((char**) (void*) values)[num1 + index2]);
        semanticMeaningArray[index1] = semanticMeaning;
        num1 += (int) num2;
      }
      return semanticMeaningArray;
    }

    /// <summary>
    ///   <para>Delegate for OnPhraseRecognized event.</para>
    /// </summary>
    /// <param name="args">Information about a phrase recognized event.</param>
    public delegate void PhraseRecognizedDelegate(PhraseRecognizedEventArgs args);
  }
}
