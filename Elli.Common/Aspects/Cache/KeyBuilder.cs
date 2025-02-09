// Decompiled with JetBrains decompiler
// Type: Elli.Common.Aspects.Cache.KeyBuilder
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using PostSharp.Aspects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Elli.Common.Aspects.Cache
{
  [Serializable]
  public class KeyBuilder
  {
    private ParameterInfo[] _methodParameters;
    private Dictionary<int, string> _parametersNameValueMapper;

    public string GroupName { get; set; }

    public string MethodName { get; set; }

    public string ParameterProperty { get; set; }

    public CacheSettings Settings { get; set; }

    public ParameterInfo[] MethodParameters
    {
      get => this._methodParameters;
      set
      {
        this._methodParameters = value;
        this.TransformParametersIntoNameValueMapper(this._methodParameters);
      }
    }

    public string BuildCacheKey(object instance, Arguments arguments)
    {
      StringBuilder cacheKeyBuilder = new StringBuilder();
      if (string.IsNullOrWhiteSpace(this.GroupName))
        cacheKeyBuilder.Append(this.MethodName);
      else
        cacheKeyBuilder.Append(this.GroupName);
      if (instance != null)
      {
        cacheKeyBuilder.Append(instance);
        cacheKeyBuilder.Append(";");
      }
      switch (this.Settings)
      {
        case CacheSettings.Default:
          for (int index = 0; index < arguments.Count; ++index)
            KeyBuilder.BuildDefaultKey(arguments.GetArgument(index), cacheKeyBuilder);
          break;
        case CacheSettings.IgnoreParameters:
          return cacheKeyBuilder.ToString();
        case CacheSettings.UseId:
          int argumentIndexByName1 = this.GetArgumentIndexByName("Id");
          cacheKeyBuilder.Append(arguments.GetArgument(argumentIndexByName1) ?? (object) "Null");
          break;
        case CacheSettings.UseProperty:
          int argumentIndexByName2 = this.GetArgumentIndexByName(this.ParameterProperty);
          cacheKeyBuilder.Append(arguments.GetArgument(argumentIndexByName2) ?? (object) "Null");
          break;
      }
      return cacheKeyBuilder.ToString();
    }

    private static void BuildDefaultKey(object argument, StringBuilder cacheKeyBuilder)
    {
      if (argument != null && typeof (ICollection).IsAssignableFrom(argument.GetType()))
      {
        cacheKeyBuilder.Append("{");
        foreach (object obj in (IEnumerable) argument)
          cacheKeyBuilder.Append(obj ?? (object) "Null");
        cacheKeyBuilder.Append("}");
      }
      else
        cacheKeyBuilder.Append(argument ?? (object) "Null");
    }

    private int GetArgumentIndexByName(string paramName)
    {
      return this._parametersNameValueMapper.SingleOrDefault<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (arg => string.Compare(arg.Value, paramName, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)).Key;
    }

    private void TransformParametersIntoNameValueMapper(ParameterInfo[] methodParameters)
    {
      this._parametersNameValueMapper = new Dictionary<int, string>();
      for (int key = 0; key < ((IEnumerable<ParameterInfo>) methodParameters).Count<ParameterInfo>(); ++key)
        this._parametersNameValueMapper.Add(key, methodParameters[key].Name);
    }
  }
}
