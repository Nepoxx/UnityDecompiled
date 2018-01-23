// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreLoginWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetStoreLoginWindow : EditorWindow
  {
    private string m_LoginRemoteMessage = (string) null;
    private string m_Username = "";
    private string m_Password = "";
    private static AssetStoreLoginWindow.Styles styles;
    private static GUIContent s_AssetStoreLogo;
    private const float kBaseHeight = 110f;
    private string m_LoginReason;
    private AssetStoreLoginWindow.LoginCallback m_LoginCallback;

    public static void Login(string loginReason, AssetStoreLoginWindow.LoginCallback callback)
    {
      if (AssetStoreClient.HasActiveSessionID)
        AssetStoreClient.Logout();
      if (!AssetStoreClient.RememberSession || !AssetStoreClient.HasSavedSessionID)
        AssetStoreLoginWindow.ShowAssetStoreLoginWindow(loginReason, callback);
      else
        AssetStoreClient.LoginWithRememberedSession((AssetStoreClient.DoneLoginCallback) (errorMessage =>
        {
          if (string.IsNullOrEmpty(errorMessage))
            callback(errorMessage);
          else
            AssetStoreLoginWindow.ShowAssetStoreLoginWindow(loginReason, callback);
        }));
    }

    public static void Logout()
    {
      AssetStoreClient.Logout();
    }

    public static bool IsLoggedIn
    {
      get
      {
        return AssetStoreClient.HasActiveSessionID;
      }
    }

    public static void ShowAssetStoreLoginWindow(string loginReason, AssetStoreLoginWindow.LoginCallback callback)
    {
      AssetStoreLoginWindow windowWithRect = EditorWindow.GetWindowWithRect<AssetStoreLoginWindow>(new Rect(100f, 100f, 360f, 140f), true, "Login to Asset Store");
      windowWithRect.position = new Rect(100f, 100f, windowWithRect.position.width, windowWithRect.position.height);
      windowWithRect.m_Parent.window.m_DontSaveToLayout = true;
      windowWithRect.m_Password = "";
      windowWithRect.m_LoginCallback = callback;
      windowWithRect.m_LoginReason = loginReason;
      windowWithRect.m_LoginRemoteMessage = (string) null;
      UsabilityAnalytics.Track("/AssetStore/Login");
    }

    private static void LoadLogos()
    {
      if (AssetStoreLoginWindow.s_AssetStoreLogo != null)
        return;
      AssetStoreLoginWindow.s_AssetStoreLogo = new GUIContent("");
    }

    public void OnDisable()
    {
      if (this.m_LoginCallback != null)
        this.m_LoginCallback(this.m_LoginRemoteMessage);
      this.m_LoginCallback = (AssetStoreLoginWindow.LoginCallback) null;
      this.m_Password = (string) null;
    }

    public void OnGUI()
    {
      if (AssetStoreLoginWindow.styles == null)
        AssetStoreLoginWindow.styles = new AssetStoreLoginWindow.Styles();
      AssetStoreLoginWindow.LoadLogos();
      if (AssetStoreClient.LoginInProgress() || AssetStoreClient.LoggedIn())
        GUI.enabled = false;
      GUILayout.BeginVertical();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(5f);
      GUILayout.Label(AssetStoreLoginWindow.s_AssetStoreLogo, GUIStyle.none, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Space(6f);
      GUILayout.Label(this.m_LoginReason, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      Rect lastRect = GUILayoutUtility.GetLastRect();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Space(6f);
      Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      if (this.m_LoginRemoteMessage != null)
      {
        Color color = GUI.color;
        GUI.color = Color.red;
        GUILayout.Label(this.m_LoginRemoteMessage, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
        GUI.color = color;
        rect = GUILayoutUtility.GetLastRect();
      }
      float height = (float) ((double) lastRect.height + (double) rect.height + 110.0);
      if (Event.current.type == EventType.Repaint && (double) height != (double) this.position.height)
      {
        this.position = new Rect(this.position.x, this.position.y, this.position.width, height);
        this.Repaint();
      }
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUI.SetNextControlName("username");
      this.m_Username = EditorGUILayout.TextField("Username", this.m_Username, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      this.m_Password = EditorGUILayout.PasswordField("Password", this.m_Password, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      if (GUILayout.Button(new GUIContent("Forgot?", "Reset your password"), AssetStoreLoginWindow.styles.link, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        Application.OpenURL("https://accounts.unity3d.com/password/new");
      EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
      GUILayout.EndHorizontal();
      bool rememberSession = AssetStoreClient.RememberSession;
      bool flag = EditorGUILayout.Toggle("Remember me", rememberSession, new GUILayoutOption[0]);
      if (flag != rememberSession)
        AssetStoreClient.RememberSession = flag;
      GUILayout.EndVertical();
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
      GUILayout.Space(8f);
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Create account"))
      {
        AssetStore.Open("createuser/");
        this.m_LoginRemoteMessage = "Cancelled - create user";
        this.Close();
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Cancel"))
      {
        this.m_LoginRemoteMessage = "Cancelled";
        this.Close();
      }
      GUILayout.Space(5f);
      if (GUILayout.Button("Login"))
      {
        this.DoLogin();
        this.Repaint();
      }
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.EndVertical();
      if (Event.current.Equals((object) Event.KeyboardEvent("return")))
      {
        this.DoLogin();
        this.Repaint();
      }
      if (!(this.m_Username == ""))
        return;
      EditorGUI.FocusTextInControl("username");
    }

    private void DoLogin()
    {
      this.m_LoginRemoteMessage = (string) null;
      if (AssetStoreClient.HasActiveSessionID)
        AssetStoreClient.Logout();
      AssetStoreClient.LoginWithCredentials(this.m_Username, this.m_Password, AssetStoreClient.RememberSession, (AssetStoreClient.DoneLoginCallback) (errorMessage =>
      {
        this.m_LoginRemoteMessage = errorMessage;
        if (errorMessage == null)
          this.Close();
        else
          this.Repaint();
      }));
    }

    private class Styles
    {
      public GUIStyle link = new GUIStyle(EditorStyles.miniLabel);

      public Styles()
      {
        this.link.normal.textColor = new Color(0.26f, 0.51f, 0.75f, 1f);
      }
    }

    public delegate void LoginCallback(string errorMessage);
  }
}
