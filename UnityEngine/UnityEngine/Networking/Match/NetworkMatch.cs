// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.NetworkMatch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>A component for communicating with the Unity Multiplayer Matchmaking service.</para>
  /// </summary>
  public class NetworkMatch : MonoBehaviour
  {
    private Uri m_BaseUri = new Uri("https://mm.unet.unity3d.com");

    /// <summary>
    ///   <para>The base URI of the MatchMaker that this NetworkMatch will communicate with.</para>
    /// </summary>
    public Uri baseUri
    {
      get
      {
        return this.m_BaseUri;
      }
      set
      {
        this.m_BaseUri = value;
      }
    }

    /// <summary>
    ///   <para>This method is deprecated. Please instead log in through the editor services panel and setup the project under the Unity Multiplayer section. This will populate the required infomation from the cloud site automatically.</para>
    /// </summary>
    /// <param name="programAppID">Deprecated, see description.</param>
    [Obsolete("This function is not used any longer to interface with the matchmaker. Please set up your project by logging in through the editor connect dialog.", true)]
    public void SetProgramAppID(AppID programAppID)
    {
    }

    public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForMatch, int requestDomain, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
    {
      if (Application.platform == RuntimePlatform.WebGLPlayer)
      {
        UnityEngine.Debug.LogError((object) "Matchmaking is not supported on WebGL player.");
        return (Coroutine) null;
      }
      CreateMatchRequest req = new CreateMatchRequest();
      req.name = matchName;
      req.size = matchSize;
      req.advertise = matchAdvertise;
      req.password = matchPassword;
      req.publicAddress = publicClientAddress;
      req.privateAddress = privateClientAddress;
      req.eloScore = eloScoreForMatch;
      req.domain = requestDomain;
      return this.CreateMatch(req, callback);
    }

    internal Coroutine CreateMatch(CreateMatchRequest req, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting CreateMatch Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/CreateMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Create :" + (object) uri));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", 0);
      formData.AddField("domain", req.domain);
      formData.AddField("name", req.name);
      formData.AddField("size", req.size.ToString());
      formData.AddField("advertise", req.advertise.ToString());
      formData.AddField("password", req.password);
      formData.AddField("publicAddress", req.publicAddress);
      formData.AddField("privateAddress", req.privateAddress);
      formData.AddField("eloScore", req.eloScore.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<CreateMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<CreateMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(this.OnMatchCreate), callback));
    }

    internal virtual void OnMatchCreate(CreateMatchResponse response, NetworkMatch.DataResponseDelegate<MatchInfo> userCallback)
    {
      if (response.success)
        Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
      userCallback(response.success, response.extendedInfo, new MatchInfo(response));
    }

    public Coroutine JoinMatch(NetworkID netId, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForClient, int requestDomain, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
    {
      JoinMatchRequest req = new JoinMatchRequest();
      req.networkId = netId;
      req.password = matchPassword;
      req.publicAddress = publicClientAddress;
      req.privateAddress = privateClientAddress;
      req.eloScore = eloScoreForClient;
      req.domain = requestDomain;
      return this.JoinMatch(req, callback);
    }

    internal Coroutine JoinMatch(JoinMatchRequest req, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting JoinMatch Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/JoinMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Join :" + (object) uri));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", 0);
      formData.AddField("domain", req.domain);
      formData.AddField("networkId", req.networkId.ToString());
      formData.AddField("password", req.password);
      formData.AddField("publicAddress", req.publicAddress);
      formData.AddField("privateAddress", req.privateAddress);
      formData.AddField("eloScore", req.eloScore.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<JoinMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<JoinMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(this.OnMatchJoined), callback));
    }

    internal void OnMatchJoined(JoinMatchResponse response, NetworkMatch.DataResponseDelegate<MatchInfo> userCallback)
    {
      if (response.success)
        Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
      userCallback(response.success, response.extendedInfo, new MatchInfo(response));
    }

    public Coroutine DestroyMatch(NetworkID netId, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
    {
      DestroyMatchRequest req = new DestroyMatchRequest();
      req.networkId = netId;
      req.domain = requestDomain;
      return this.DestroyMatch(req, callback);
    }

    internal Coroutine DestroyMatch(DestroyMatchRequest req, NetworkMatch.BasicResponseDelegate callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting DestroyMatch Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/DestroyMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Destroy :" + uri.ToString()));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
      formData.AddField("domain", req.domain);
      formData.AddField("networkId", req.networkId.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<BasicResponse, NetworkMatch.BasicResponseDelegate>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<BasicResponse, NetworkMatch.BasicResponseDelegate>(this.OnMatchDestroyed), callback));
    }

    internal void OnMatchDestroyed(BasicResponse response, NetworkMatch.BasicResponseDelegate userCallback)
    {
      userCallback(response.success, response.extendedInfo);
    }

    public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
    {
      DropConnectionRequest req = new DropConnectionRequest();
      req.networkId = netId;
      req.nodeId = dropNodeId;
      req.domain = requestDomain;
      return this.DropConnection(req, callback);
    }

    internal Coroutine DropConnection(DropConnectionRequest req, NetworkMatch.BasicResponseDelegate callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting DropConnection Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/DropConnectionRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient DropConnection :" + (object) uri));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
      formData.AddField("domain", req.domain);
      formData.AddField("networkId", req.networkId.ToString());
      formData.AddField("nodeId", req.nodeId.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<DropConnectionResponse, NetworkMatch.BasicResponseDelegate>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<DropConnectionResponse, NetworkMatch.BasicResponseDelegate>(this.OnDropConnection), callback));
    }

    internal void OnDropConnection(DropConnectionResponse response, NetworkMatch.BasicResponseDelegate userCallback)
    {
      userCallback(response.success, response.extendedInfo);
    }

    public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, bool filterOutPrivateMatchesFromResults, int eloScoreTarget, int requestDomain, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback)
    {
      if (Application.platform == RuntimePlatform.WebGLPlayer)
      {
        UnityEngine.Debug.LogError((object) "Matchmaking is not supported on WebGL player.");
        return (Coroutine) null;
      }
      ListMatchRequest req = new ListMatchRequest();
      req.pageNum = startPageNumber;
      req.pageSize = resultPageSize;
      req.nameFilter = matchNameFilter;
      req.filterOutPrivateMatches = filterOutPrivateMatchesFromResults;
      req.eloScore = eloScoreTarget;
      req.domain = requestDomain;
      return this.ListMatches(req, callback);
    }

    internal Coroutine ListMatches(ListMatchRequest req, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting ListMatch Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/ListMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient ListMatches :" + (object) uri));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", 0);
      formData.AddField("domain", req.domain);
      formData.AddField("pageSize", req.pageSize);
      formData.AddField("pageNum", req.pageNum);
      formData.AddField("nameFilter", req.nameFilter);
      formData.AddField("filterOutPrivateMatches", req.filterOutPrivateMatches.ToString());
      formData.AddField("eloScore", req.eloScore.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<ListMatchResponse, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>>>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<ListMatchResponse, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>>>(this.OnMatchList), callback));
    }

    internal void OnMatchList(ListMatchResponse response, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> userCallback)
    {
      List<MatchInfoSnapshot> responseData = new List<MatchInfoSnapshot>();
      foreach (MatchDesc match in response.matches)
        responseData.Add(new MatchInfoSnapshot(match));
      userCallback(response.success, response.extendedInfo, responseData);
    }

    public Coroutine SetMatchAttributes(NetworkID networkId, bool isListed, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
    {
      SetMatchAttributesRequest req = new SetMatchAttributesRequest();
      req.networkId = networkId;
      req.isListed = isListed;
      req.domain = requestDomain;
      return this.SetMatchAttributes(req, callback);
    }

    internal Coroutine SetMatchAttributes(SetMatchAttributesRequest req, NetworkMatch.BasicResponseDelegate callback)
    {
      if (callback == null)
      {
        UnityEngine.Debug.Log((object) "callback supplied is null, aborting SetMatchAttributes Request.");
        return (Coroutine) null;
      }
      Uri uri = new Uri(this.baseUri, "/json/reply/SetMatchAttributesRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient SetMatchAttributes :" + (object) uri));
      WWWForm formData = new WWWForm();
      formData.AddField("version", Request.currentVersion);
      formData.AddField("projectId", Application.cloudProjectId);
      formData.AddField("sourceId", Utility.GetSourceID().ToString());
      formData.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
      formData.AddField("domain", req.domain);
      formData.AddField("networkId", req.networkId.ToString());
      formData.AddField("isListed", req.isListed.ToString());
      formData.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<BasicResponse, NetworkMatch.BasicResponseDelegate>(UnityWebRequest.Post(uri.ToString(), formData), new NetworkMatch.InternalResponseDelegate<BasicResponse, NetworkMatch.BasicResponseDelegate>(this.OnSetMatchAttributes), callback));
    }

    internal void OnSetMatchAttributes(BasicResponse response, NetworkMatch.BasicResponseDelegate userCallback)
    {
      userCallback(response.success, response.extendedInfo);
    }

    [DebuggerHidden]
    private IEnumerator ProcessMatchResponse<JSONRESPONSE, USERRESPONSEDELEGATETYPE>(UnityWebRequest client, NetworkMatch.InternalResponseDelegate<JSONRESPONSE, USERRESPONSEDELEGATETYPE> internalCallback, USERRESPONSEDELEGATETYPE userCallback) where JSONRESPONSE : Response, new()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetworkMatch.\u003CProcessMatchResponse\u003Ec__Iterator0<JSONRESPONSE, USERRESPONSEDELEGATETYPE>() { client = client, internalCallback = internalCallback, userCallback = userCallback };
    }

    /// <summary>
    ///   <para>A delegate that can handle MatchMaker responses that return basic response types (generally only indicating success or failure and extended information if a failure did happen).</para>
    /// </summary>
    /// <param name="success">Indicates if the request succeeded.</param>
    /// <param name="extendedInfo">A text description of the failure if success is false.</param>
    public delegate void BasicResponseDelegate(bool success, string extendedInfo);

    public delegate void DataResponseDelegate<T>(bool success, string extendedInfo, T responseData);

    private delegate void InternalResponseDelegate<T, U>(T response, U userCallback);
  }
}
