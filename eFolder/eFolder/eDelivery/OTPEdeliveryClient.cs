// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.OTPEdeliveryClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class OTPEdeliveryClient : eDeliveryClient
  {
    public override void GetRecipientURL(Request request)
    {
      if (!(request is OTPRequest))
        return;
      OTPRequest otpRequest = request as OTPRequest;
      DMOSServiceClient dmosServiceClient = new DMOSServiceClient();
      GetDMOSRecipientURLRequest request1 = new GetDMOSRecipientURLRequest();
      request1.loanGuid = otpRequest.applicationGroupId;
      request1.caller = otpRequest.caller;
      List<Party> partyList = new List<Party>();
      foreach (OTPRecipient recipient in otpRequest.recipients)
      {
        Party party = dmosServiceClient.SetPartyDetails(recipient);
        if (party != null)
          partyList.Add(party);
      }
      request1.parties = partyList.ToArray();
      Task<GetDMOSRecipientURLResponse> recipientUrl = dmosServiceClient.GetRecipientURL(request1);
      Task.WaitAll((Task) recipientUrl);
      foreach (PartyResponse partyResponse1 in ((IEnumerable<PartyResponse>) recipientUrl.Result.parties).ToList<PartyResponse>())
      {
        PartyResponse partyResponse = partyResponse1;
        Recipient recipient = (Recipient) otpRequest.recipients.OfType<OTPRecipient>().ToList<OTPRecipient>().Find((Predicate<OTPRecipient>) (x => x.PartyId == partyResponse.id));
        if (recipient != null)
        {
          recipient.url = partyResponse.url;
          recipient.id = partyResponse.recipientId;
          recipient.recipientId = partyResponse.recipientId;
        }
      }
      if (otpRequest.notViewed != null)
      {
        List<string> stringList = new List<string>();
        foreach (OTPRecipient otpRecipient in otpRequest.recipients.FindAll((Predicate<Recipient>) (x => x.role != eDeliveryEntityType.Originator.ToString("g"))))
          stringList.Add(otpRecipient.id);
        otpRequest.notViewed.recipientIds = stringList.ToArray();
      }
      if (otpRequest.fulfillment == null || otpRequest.fulfillment.to == null)
        return;
      List<string> stringList1 = new List<string>();
      foreach (OTPRecipient otpRecipient in otpRequest.recipients.FindAll((Predicate<Recipient>) (x => x.role == eDeliveryEntityType.Borrower.ToString("g") || x.role == eDeliveryEntityType.Coborrower.ToString("g"))))
        stringList1.Add(otpRecipient.id);
      otpRequest.fulfillment.to.recipientIds = stringList1.ToArray();
    }
  }
}
