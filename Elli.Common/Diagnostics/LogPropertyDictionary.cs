// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.LogPropertyDictionary
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.Common.Diagnostics
{
  [Serializable]
  public class LogPropertyDictionary : Dictionary<string, object>
  {
    public LogPropertyDictionary()
    {
    }

    protected LogPropertyDictionary(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
