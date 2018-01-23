// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.AssetRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class AssetRestHandler
  {
    internal static void Register()
    {
      Router.RegisterHandler("/unity/assets", (Handler) new AssetRestHandler.LibraryHandler());
      Router.RegisterHandler("/unity/assets/*", (Handler) new AssetRestHandler.AssetHandler());
    }

    internal class AssetHandler : JSONHandler
    {
      protected override JSONValue HandleDelete(Request request, JSONValue payload)
      {
        if (!AssetDatabase.DeleteAsset(request.Url.Substring("/unity/".Length)))
          throw new RestRequestException() { HttpStatusCode = HttpStatusCode.InternalServerError, RestErrorString = "FailedDeletingAsset", RestErrorDescription = "DeleteAsset() returned false" };
        return new JSONValue();
      }

      protected override JSONValue HandlePost(Request request, JSONValue payload)
      {
        string str = payload.Get("action").AsString();
        if (str != null)
        {
          if (!(str == "move"))
          {
            if (str == "create")
              this.CreateAsset(request.Url.Substring("/unity/".Length), Encoding.UTF8.GetString(Convert.FromBase64String(payload.Get("contents").AsString())));
            else
              goto label_5;
          }
          else
            this.MoveAsset(request.Url.Substring("/unity/".Length), payload.Get("newpath").AsString());
          return new JSONValue();
        }
label_5:
        throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Uknown action: " + str };
      }

      internal bool MoveAsset(string from, string to)
      {
        string str = AssetDatabase.MoveAsset(from, to);
        if (str.Length > 0)
          throw new RestRequestException(HttpStatusCode.BadRequest, "MoveAsset failed with error: " + str);
        return str.Length == 0;
      }

      internal void CreateAsset(string assetPath, string contents)
      {
        string fullPath = Path.GetFullPath(assetPath);
        try
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) File.OpenWrite(fullPath)))
          {
            streamWriter.Write(contents);
            streamWriter.Close();
          }
        }
        catch (Exception ex)
        {
          throw new RestRequestException(HttpStatusCode.BadRequest, "FailedCreatingAsset", "Caught exception: " + (object) ex);
        }
      }

      protected override JSONValue HandleGet(Request request, JSONValue payload)
      {
        int num = request.Url.ToLowerInvariant().IndexOf("/assets/");
        return this.GetAssetText(request.Url.ToLowerInvariant().Substring(num + 1));
      }

      internal JSONValue GetAssetText(string assetPath)
      {
        UnityEngine.Object @object = AssetDatabase.LoadAssetAtPath(assetPath, typeof (UnityEngine.Object));
        if (@object == (UnityEngine.Object) null)
          throw new RestRequestException(HttpStatusCode.BadRequest, "AssetNotFound");
        JSONValue jsonValue = new JSONValue();
        jsonValue["file"] = (JSONValue) assetPath;
        jsonValue["contents"] = (JSONValue) Convert.ToBase64String(Encoding.UTF8.GetBytes(@object.ToString()));
        return jsonValue;
      }
    }

    internal class LibraryHandler : JSONHandler
    {
      protected override JSONValue HandleGet(Request request, JSONValue payload)
      {
        JSONValue jsonValue = new JSONValue();
        jsonValue["assets"] = JSONHandler.ToJSON((IEnumerable<string>) AssetDatabase.FindAssets("", new string[1]
        {
          "Assets"
        }));
        return jsonValue;
      }
    }
  }
}
