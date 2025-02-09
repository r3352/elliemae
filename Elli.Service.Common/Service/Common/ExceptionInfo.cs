// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.ExceptionInfo
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Elli.Service.Common
{
  public class ExceptionInfo
  {
    public ExceptionInfo InnerException { get; set; }

    public string Message { get; set; }

    public string StackTrace { get; set; }

    public string Type { get; set; }

    public string FaultCode { get; set; }

    public ExceptionInfo()
    {
    }

    public ExceptionInfo(Exception exception)
    {
      this.Message = exception.Message;
      this.StackTrace = exception.StackTrace;
      this.Type = exception.GetType().ToString();
      if (exception.InnerException == null)
        return;
      this.InnerException = new ExceptionInfo(exception.InnerException);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}\n{1}", (object) "An ExceptionInfo whose value is:", (object) this.ToStringHelper(false));
    }

    private string ToStringHelper(bool isInner)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0}: {1}", (object) this.Type, (object) this.Message);
      if (this.InnerException != null)
        stringBuilder.AppendFormat(" ----> {0}", (object) this.InnerException.ToStringHelper(true));
      else
        stringBuilder.Append("\n");
      stringBuilder.Append(this.StackTrace);
      if (isInner)
        stringBuilder.AppendFormat("\n   {0}\n", (object) "--- End of inner ExceptionInfo stack trace ---");
      return stringBuilder.ToString();
    }
  }
}
