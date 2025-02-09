// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.OTPEdeliveryMessage
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class OTPEdeliveryMessage(LoanDataMgr loanDataMgr, eDeliveryMessageType msgType) : 
    eDeliveryMessage(loanDataMgr, msgType)
  {
    protected override void SetOTPRecipientDetails(
      Recipient recipient,
      eDeliveryRecipient eDeliveryRecipient)
    {
      if (!(recipient is OTPRecipient))
        return;
      OTPRecipient otpRecipient = recipient as OTPRecipient;
      otpRecipient.PhoneNumber = eDeliveryRecipient.PhoneNumber;
      otpRecipient.PartyId = eDeliveryRecipient.PartyId;
      otpRecipient.PhoneType = eDeliveryRecipient.PhoneType;
    }

    protected override Request GetRequestObject() => (Request) new OTPRequest();

    protected override Recipient GetRecipientObject() => (Recipient) new OTPRecipient();
  }
}
