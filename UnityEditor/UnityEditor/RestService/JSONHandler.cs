// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.JSONHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal abstract class JSONHandler : Handler
  {
    protected override void InvokeGet(Request request, string payload, Response writeResponse)
    {
      JSONHandler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandleGet));
    }

    protected override void InvokePost(Request request, string payload, Response writeResponse)
    {
      JSONHandler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandlePost));
    }

    protected override void InvokeDelete(Request request, string payload, Response writeResponse)
    {
      JSONHandler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandleDelete));
    }

    private static void CallSafely(Request request, string payload, Response writeResponse, Func<Request, JSONValue, JSONValue> method)
    {
      try
      {
        JSONValue jsonValue = (JSONValue) ((string) null);
        if (payload.Trim().Length == 0)
        {
          jsonValue = new JSONValue();
        }
        else
        {
          try
          {
            jsonValue = new JSONParser(request.Payload).Parse();
          }
          catch (JSONParseException ex)
          {
            JSONHandler.ThrowInvalidJSONException();
          }
        }
        writeResponse.SimpleResponse(HttpStatusCode.Ok, "application/json", method(request, jsonValue).ToString());
      }
      catch (JSONTypeException ex)
      {
        JSONHandler.ThrowInvalidJSONException();
      }
      catch (KeyNotFoundException ex)
      {
        JSONHandler.RespondWithException(writeResponse, new RestRequestException()
        {
          HttpStatusCode = HttpStatusCode.BadRequest
        });
      }
      catch (RestRequestException ex)
      {
        JSONHandler.RespondWithException(writeResponse, ex);
      }
      catch (Exception ex)
      {
        JSONHandler.RespondWithException(writeResponse, new RestRequestException()
        {
          HttpStatusCode = HttpStatusCode.InternalServerError,
          RestErrorString = "InternalServerError",
          RestErrorDescription = "Caught exception while fulfilling request: " + (object) ex
        });
      }
    }

    private static void ThrowInvalidJSONException()
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Invalid JSON" };
    }

    private static void RespondWithException(Response writeResponse, RestRequestException rre)
    {
      StringBuilder stringBuilder = new StringBuilder("{");
      if (rre.RestErrorString != null)
        stringBuilder.AppendFormat("\"error\":\"{0}\",", (object) rre.RestErrorString);
      if (rre.RestErrorDescription != null)
        stringBuilder.AppendFormat("\"errordescription\":\"{0}\"", (object) rre.RestErrorDescription);
      stringBuilder.Append("}");
      writeResponse.SimpleResponse(rre.HttpStatusCode, "application/json", stringBuilder.ToString());
    }

    protected virtual JSONValue HandleGet(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the GET verb." };
    }

    protected virtual JSONValue HandlePost(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the POST verb." };
    }

    protected virtual JSONValue HandleDelete(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the DELETE verb." };
    }

    protected static JSONValue ToJSON(IEnumerable<string> strings)
    {
      return new JSONValue((object) strings.Select<string, JSONValue>((Func<string, JSONValue>) (s => new JSONValue((object) s))).ToList<JSONValue>());
    }
  }
}
