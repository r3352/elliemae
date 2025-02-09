// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.IntentToProceedFields
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ePass.BamEnums;
using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
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
