// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LoggingContractResolver
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class LoggingContractResolver : CamelCasePropertyNamesContractResolver
  {
    protected override JsonContract CreateContract(Type objectType)
    {
      JsonContract contract = base.CreateContract(objectType);
      if (objectType == typeof (TransactionId))
        contract.Converter = (JsonConverter) new TransactionIdConverter();
      else if (objectType == typeof (Encompass.Diagnostics.Logging.LogLevel))
        contract.Converter = (JsonConverter) new LogLevelConverter();
      else if (typeof (Log).IsAssignableFrom(objectType))
        contract.Converter = (JsonConverter) new LogConverter();
      else if (objectType.IsEnum)
        contract.Converter = (JsonConverter) new StringEnumConverter();
      return contract;
    }
  }
}
