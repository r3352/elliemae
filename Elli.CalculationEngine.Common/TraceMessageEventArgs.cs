// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.TraceMessageEventArgs
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public class TraceMessageEventArgs : EventArgs
  {
    public readonly string Category;
    public readonly string Message;
    public bool Cancel;

    public TraceMessageEventArgs(string category, string message)
    {
      this.Category = category;
      this.Message = message;
    }
  }
}
