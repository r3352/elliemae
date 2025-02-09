// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.ConfigOverrideHandler
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class ConfigOverrideHandler : ICallHandler
  {
    private const string LocalValuePlaceholder = "[LOCAL_CONFIG]";

    public int Order { get; set; }

    public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
    {
      if (input.MethodBase.Name.StartsWith("set_"))
      {
        string propertyName = input.MethodBase.Name.Substring(4);
        (input.Target as IConfigDataSection).SetConfigValue(propertyName, input.Inputs[0]);
        return input.CreateMethodReturn((object) null);
      }
      if (!input.MethodBase.Name.StartsWith("get_"))
        return getNext()(input, getNext);
      string overrideValue = this.GetOverrideValue();
      IMethodReturn methodReturn = getNext()(input, getNext);
      if (string.IsNullOrEmpty(overrideValue))
        return methodReturn;
      if (string.Equals(overrideValue, "[LOCAL_CONFIG]", StringComparison.OrdinalIgnoreCase))
      {
        string propertyName = input.MethodBase.Name.Substring(4);
        object returnValue;
        if (!(input.Target as IConfigDataSection).TryGetConfigValue(propertyName, out returnValue))
          return methodReturn;
        return input.CreateMethodReturn(returnValue, (object) methodReturn.Outputs);
      }
      try
      {
        object obj;
        if (this.ShouldCreateNew(methodReturn.ReturnValue))
        {
          obj = !(((MethodInfo) input.MethodBase).ReturnType == typeof (string)) ? JsonConvert.DeserializeObject(overrideValue, ((MethodInfo) input.MethodBase).ReturnType) : (object) overrideValue;
        }
        else
        {
          obj = this.Clone(methodReturn.ReturnValue);
          JsonConvert.PopulateObject(overrideValue, obj);
        }
        return input.CreateMethodReturn(obj, (object) methodReturn.Outputs);
      }
      catch
      {
        return methodReturn;
      }
    }

    protected virtual string GetOverrideValue() => "[LOCAL_CONFIG]";

    private bool ShouldCreateNew(object value)
    {
      int num;
      switch (value)
      {
        case null:
        case string _:
        case DateTime _:
          num = 1;
          break;
        default:
          if (!value.GetType().IsPrimitive)
          {
            num = value.GetType().IsEnum ? 1 : 0;
            break;
          }
          goto case null;
      }
      return num != 0;
    }

    private object Clone(object value)
    {
      if (value == null)
        return (object) null;
      Type type = value.GetType();
      if ((!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof (Dictionary<,>))) && (!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof (List<>))))
        return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value));
      return Activator.CreateInstance(type, value);
    }
  }
}
