// Decompiled with JetBrains decompiler
// Type: UnityEngine.Analytics.Analytics
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
  /// <summary>
  ///   <para>Unity Analytics provides insight into your game users e.g. DAU, MAU.</para>
  /// </summary>
  public static class Analytics
  {
    private static UnityAnalyticsHandler s_UnityAnalyticsHandler;

    internal static UnityAnalyticsHandler GetUnityAnalyticsHandler()
    {
      if (UnityEngine.Analytics.Analytics.s_UnityAnalyticsHandler == null)
        UnityEngine.Analytics.Analytics.s_UnityAnalyticsHandler = new UnityAnalyticsHandler();
      return UnityEngine.Analytics.Analytics.s_UnityAnalyticsHandler;
    }

    /// <summary>
    ///   <para>Controls whether to limit user tracking at runtime.</para>
    /// </summary>
    public static bool limitUserTracking
    {
      get
      {
        return UnityAnalyticsHandler.limitUserTracking;
      }
      set
      {
        UnityAnalyticsHandler.limitUserTracking = value;
      }
    }

    /// <summary>
    ///   <para>Controls whether the sending of device stats at runtime is enabled.</para>
    /// </summary>
    public static bool deviceStatsEnabled
    {
      get
      {
        return UnityAnalyticsHandler.deviceStatsEnabled;
      }
      set
      {
        UnityAnalyticsHandler.deviceStatsEnabled = value;
      }
    }

    /// <summary>
    ///   <para>Controls whether the Analytics service is enabled at runtime.</para>
    /// </summary>
    public static bool enabled
    {
      get
      {
        UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
        if (analyticsHandler == null)
          return false;
        return analyticsHandler.enabled;
      }
      set
      {
        UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
        if (analyticsHandler == null)
          return;
        analyticsHandler.enabled = value;
      }
    }

    /// <summary>
    ///   <para>Attempts to flush immediately all queued analytics events to the network and filesystem cache if possible (optional).</para>
    /// </summary>
    public static AnalyticsResult FlushEvents()
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.FlushEvents();
    }

    /// <summary>
    ///   <para>User Demographics (optional).</para>
    /// </summary>
    /// <param name="userId">User id.</param>
    public static AnalyticsResult SetUserId(string userId)
    {
      if (string.IsNullOrEmpty(userId))
        throw new ArgumentException("Cannot set userId to an empty or null string");
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.SetUserId(userId);
    }

    /// <summary>
    ///   <para>User Demographics (optional).</para>
    /// </summary>
    /// <param name="gender">Gender of user can be "Female", "Male", or "Unknown".</param>
    public static AnalyticsResult SetUserGender(Gender gender)
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.SetUserGender(gender);
    }

    /// <summary>
    ///   <para>User Demographics (optional).</para>
    /// </summary>
    /// <param name="birthYear">Birth year of user. Must be 4-digit year format, only.</param>
    public static AnalyticsResult SetUserBirthYear(int birthYear)
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (UnityEngine.Analytics.Analytics.s_UnityAnalyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.SetUserBirthYear(birthYear);
    }

    /// <summary>
    ///   <para>Tracking Monetization (optional).</para>
    /// </summary>
    /// <param name="productId">The id of the purchased item.</param>
    /// <param name="amount">The price of the item.</param>
    /// <param name="currency">Abbreviation of the currency used for the transaction. For example “USD” (United States Dollars). See http:en.wikipedia.orgwikiISO_4217 for a standardized list of currency abbreviations.</param>
    /// <param name="receiptPurchaseData">Receipt data (iOS)  receipt ID (android)  for in-app purchases to verify purchases with Apple iTunes / Google Play. Use null in the absence of receipts.</param>
    /// <param name="signature">Android receipt signature. If using native Android use the INAPP_DATA_SIGNATURE string containing the signature of the purchase data that was signed with the private key of the developer. The data signature uses the RSASSA-PKCS1-v1_5 scheme. Pass in null in absence of a signature.</param>
    /// <param name="usingIAPService">Set to true when using UnityIAP.</param>
    public static AnalyticsResult Transaction(string productId, Decimal amount, string currency)
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, (string) null, (string) null);
    }

    /// <summary>
    ///   <para>Tracking Monetization (optional).</para>
    /// </summary>
    /// <param name="productId">The id of the purchased item.</param>
    /// <param name="amount">The price of the item.</param>
    /// <param name="currency">Abbreviation of the currency used for the transaction. For example “USD” (United States Dollars). See http:en.wikipedia.orgwikiISO_4217 for a standardized list of currency abbreviations.</param>
    /// <param name="receiptPurchaseData">Receipt data (iOS)  receipt ID (android)  for in-app purchases to verify purchases with Apple iTunes / Google Play. Use null in the absence of receipts.</param>
    /// <param name="signature">Android receipt signature. If using native Android use the INAPP_DATA_SIGNATURE string containing the signature of the purchase data that was signed with the private key of the developer. The data signature uses the RSASSA-PKCS1-v1_5 scheme. Pass in null in absence of a signature.</param>
    /// <param name="usingIAPService">Set to true when using UnityIAP.</param>
    public static AnalyticsResult Transaction(string productId, Decimal amount, string currency, string receiptPurchaseData, string signature)
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature);
    }

    /// <summary>
    ///   <para>Tracking Monetization (optional).</para>
    /// </summary>
    /// <param name="productId">The id of the purchased item.</param>
    /// <param name="amount">The price of the item.</param>
    /// <param name="currency">Abbreviation of the currency used for the transaction. For example “USD” (United States Dollars). See http:en.wikipedia.orgwikiISO_4217 for a standardized list of currency abbreviations.</param>
    /// <param name="receiptPurchaseData">Receipt data (iOS)  receipt ID (android)  for in-app purchases to verify purchases with Apple iTunes / Google Play. Use null in the absence of receipts.</param>
    /// <param name="signature">Android receipt signature. If using native Android use the INAPP_DATA_SIGNATURE string containing the signature of the purchase data that was signed with the private key of the developer. The data signature uses the RSASSA-PKCS1-v1_5 scheme. Pass in null in absence of a signature.</param>
    /// <param name="usingIAPService">Set to true when using UnityIAP.</param>
    public static AnalyticsResult Transaction(string productId, Decimal amount, string currency, string receiptPurchaseData, string signature, bool usingIAPService)
    {
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature, usingIAPService);
    }

    /// <summary>
    ///   <para>Custom Events (optional).</para>
    /// </summary>
    /// <param name="customEventName"></param>
    public static AnalyticsResult CustomEvent(string customEventName)
    {
      if (string.IsNullOrEmpty(customEventName))
        throw new ArgumentException("Cannot set custom event name to an empty or null string");
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      return analyticsHandler.CustomEvent(customEventName);
    }

    /// <summary>
    ///   <para>Custom Events (optional).</para>
    /// </summary>
    /// <param name="customEventName"></param>
    /// <param name="position"></param>
    public static AnalyticsResult CustomEvent(string customEventName, Vector3 position)
    {
      if (string.IsNullOrEmpty(customEventName))
        throw new ArgumentException("Cannot set custom event name to an empty or null string");
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      CustomEventData eventData = new CustomEventData(customEventName);
      eventData.Add("x", (double) Convert.ToDecimal(position.x));
      eventData.Add("y", (double) Convert.ToDecimal(position.y));
      eventData.Add("z", (double) Convert.ToDecimal(position.z));
      return analyticsHandler.CustomEvent(eventData);
    }

    public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
    {
      if (string.IsNullOrEmpty(customEventName))
        throw new ArgumentException("Cannot set custom event name to an empty or null string");
      UnityAnalyticsHandler analyticsHandler = UnityEngine.Analytics.Analytics.GetUnityAnalyticsHandler();
      if (analyticsHandler == null)
        return AnalyticsResult.NotInitialized;
      if (eventData == null)
        return analyticsHandler.CustomEvent(customEventName);
      CustomEventData eventData1 = new CustomEventData(customEventName);
      eventData1.Add(eventData);
      return analyticsHandler.CustomEvent(eventData1);
    }
  }
}
