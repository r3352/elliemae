// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.IntentToProceedFields
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Export.BamEnums;
using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class IntentToProceedFields
  {
    public bool Intent { get; set; }

    public DateTime Date { get; set; }

    public string ReceivedBy { get; set; }

    public DisclosedMethodEnum ReceivedMethod { get; set; }

    public string ReceivedMethodOther { get; set; }

    public IntentToProceedFields(
      bool intent,
      DateTime date,
      string receivedBy,
      DisclosedMethodEnum receivedMethod,
      string receivedMethodOther,
      string comments)
    {
      this.Intent = intent;
      this.Date = date;
      this.ReceivedBy = receivedBy;
      this.ReceivedMethod = receivedMethod;
      this.ReceivedMethodOther = receivedMethodOther;
      this.Comments = comments;
    }

    public string Comments { get; set; }
  }
}
