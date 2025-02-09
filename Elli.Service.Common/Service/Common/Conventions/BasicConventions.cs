// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Conventions.BasicConventions
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common.Conventions
{
  public class BasicConventions : IConventions
  {
    private readonly IDictionary<Type, Type> _requestResponseMappings = (IDictionary<Type, Type>) new Dictionary<Type, Type>();

    public BasicConventions(IRequestTypeRegistry configuration)
    {
      this.BuildRequestReponseMappings(configuration.GetRegisteredRequestTypes());
    }

    private void BuildRequestReponseMappings(IEnumerable<Type> requestTypes)
    {
      foreach (Type type in requestTypes.Where<Type>((Func<Type, bool>) (t => t.Name.EndsWith("Request"))))
        this._requestResponseMappings.Add(type, BasicConventions.DetermineResponseType(type));
    }

    private static Type DetermineResponseType(Type requestType)
    {
      string fullName = requestType.FullName;
      string name = BasicConventions.ReplaceRequestSuffix(fullName);
      Type type = requestType.Assembly.GetType(name);
      return !(type == (Type) null) ? type : throw new InvalidOperationException("Could not determine response type by convention for request of type " + fullName);
    }

    private static string ReplaceRequestSuffix(string requestTypeName)
    {
      int length = requestTypeName.LastIndexOf("Request");
      return requestTypeName.Substring(0, length) + "Response";
    }

    public Type GetResponseTypeFor(Request request)
    {
      return request != null ? this._requestResponseMappings[request.GetType()] : throw new ArgumentNullException(nameof (request));
    }
  }
}
