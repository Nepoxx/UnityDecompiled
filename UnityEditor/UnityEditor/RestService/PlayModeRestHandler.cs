// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.PlayModeRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class PlayModeRestHandler : JSONHandler
  {
    protected override JSONValue HandlePost(Request request, JSONValue payload)
    {
      string str1 = payload.Get("action").AsString();
      string str2 = this.CurrentState();
      if (str1 != null)
      {
        if (!(str1 == "play"))
        {
          if (!(str1 == "pause"))
          {
            if (str1 == "stop")
              EditorApplication.isPlaying = false;
            else
              goto label_7;
          }
          else
            EditorApplication.isPaused = true;
        }
        else
        {
          EditorApplication.isPlaying = true;
          EditorApplication.isPaused = false;
        }
        JSONValue jsonValue = new JSONValue();
        jsonValue["oldstate"] = (JSONValue) str2;
        jsonValue["newstate"] = (JSONValue) this.CurrentState();
        return jsonValue;
      }
label_7:
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Invalid action: " + str1 };
    }

    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      JSONValue jsonValue = new JSONValue();
      jsonValue["state"] = (JSONValue) this.CurrentState();
      return jsonValue;
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/playmode", (Handler) new PlayModeRestHandler());
    }

    internal string CurrentState()
    {
      if (!EditorApplication.isPlayingOrWillChangePlaymode)
        return "stopped";
      return !EditorApplication.isPaused ? "playing" : "paused";
    }
  }
}
