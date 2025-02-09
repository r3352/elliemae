// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanLogSnapshotStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class LoanLogSnapshotStore
  {
    private const string className = "LoanLogSnapshotStore�";

    private LoanLogSnapshotStore()
    {
    }

    public static Dictionary<string, string> GetLoanSnapshot(
      Loan loan,
      LogSnapshotType type,
      Guid snapshotGuid,
      bool ucdExists)
    {
      try
      {
        Dictionary<string, string> loanSnapshot = new Dictionary<string, string>();
        switch (type)
        {
          case LogSnapshotType.DisclosureTracking:
            loanSnapshot = LoanLogSnapshotStore.getDisclosureTracking2015Snapshot(loan, snapshotGuid.ToString(), ucdExists);
            break;
          case LogSnapshotType.DisclosureTrackingUCD:
            loanSnapshot = LoanLogSnapshotStore.getDisclosureTracking2015UCDSnapshot(loan, snapshotGuid.ToString());
            break;
          case LogSnapshotType.LockRequest:
            loanSnapshot = LoanLogSnapshotStore.GetRequestLogSnapshot(loan, snapshotGuid.ToString());
            break;
          case LogSnapshotType.DocumentTracking:
            loanSnapshot = LoanLogSnapshotStore.GetDocTrackingLogSnapshot(loan, snapshotGuid.ToString());
            break;
        }
        return loanSnapshot;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanLogSnapshotStore), ex);
        return (Dictionary<string, string>) null;
      }
    }

    public static Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      Loan loan,
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids)
    {
      try
      {
        Dictionary<string, Dictionary<string, string>> loanSnapshots = new Dictionary<string, Dictionary<string, string>>();
        switch (type)
        {
          case LogSnapshotType.DisclosureTracking:
            using (Dictionary<string, bool>.Enumerator enumerator = snapshotGuids.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<string, bool> current = enumerator.Current;
                if (current.Key != "")
                  loanSnapshots.Add(current.Key.ToString(), LoanLogSnapshotStore.getDisclosureTracking2015Snapshot(loan, current.Key, current.Value));
              }
              break;
            }
          case LogSnapshotType.DisclosureTrackingUCD:
            using (Dictionary<string, bool>.Enumerator enumerator = snapshotGuids.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<string, bool> current = enumerator.Current;
                if (current.Key != "")
                  loanSnapshots.Add(current.Key.ToString(), LoanLogSnapshotStore.getDisclosureTracking2015UCDSnapshot(loan, current.Key));
              }
              break;
            }
          case LogSnapshotType.LockRequest:
            using (Dictionary<string, bool>.Enumerator enumerator = snapshotGuids.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<string, bool> current = enumerator.Current;
                if (current.ToString() != "")
                  loanSnapshots.Add(current.ToString(), LoanLogSnapshotStore.GetRequestLogSnapshot(loan, current.ToString()));
              }
              break;
            }
          case LogSnapshotType.DocumentTracking:
            using (Dictionary<string, bool>.Enumerator enumerator = snapshotGuids.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<string, bool> current = enumerator.Current;
                if (current.ToString() != "")
                  loanSnapshots.Add(current.ToString(), LoanLogSnapshotStore.GetDocTrackingLogSnapshot(loan, current.ToString()));
              }
              break;
            }
        }
        return loanSnapshots;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanLogSnapshotStore), ex);
        return (Dictionary<string, Dictionary<string, string>>) null;
      }
    }

    private static Dictionary<string, string> getDisclosureTracking2015UCDSnapshot(
      Loan loan,
      string guid)
    {
      XmlDocument doc = new XmlDocument();
      using (SnapshotObject supportingSnapshotData = loan.GetSupportingSnapshotData(LogSnapshotType.DisclosureTrackingUCD, new Guid(guid), SnapshotObject.GetLoanSnapshotFileName(LogSnapshotType.DisclosureTrackingUCD, guid), false))
      {
        if (supportingSnapshotData != null)
          doc.LoadXml(supportingSnapshotData.ToString());
        else
          TraceLog.WriteWarning(nameof (LoanLogSnapshotStore), string.Format("Error retreiving {0} UCD disclosure tracking snapshot for the loan guid {1}, clientID {2} and Snapshotguid {3}", (object) ("UCD" + guid + ".XML"), (object) loan.Identity.Guid, (object) ClientContext.CurrentRequest.Context.ClientID, (object) guid));
      }
      return new UCDXmlParser(doc).ParseXml();
    }

    private static Dictionary<string, string> getDisclosureTracking2015Snapshot(
      Loan loan,
      string guid,
      bool ucdExists)
    {
      Dictionary<string, string> tracking2015Snapshot = ucdExists ? LoanLogSnapshotStore.getDisclosureTracking2015UCDSnapshot(loan, guid) : new Dictionary<string, string>();
      string key = "DTSnapshot" + guid + ".TXT";
      SnapshotObject snapshotObject = loan.GetSupportingSnapshotData(LogSnapshotType.DisclosureTracking, new Guid(guid), SnapshotObject.GetLoanSnapshotFileName(LogSnapshotType.DisclosureTracking, guid), false);
      if (snapshotObject == null)
      {
        key = guid + ".TXT";
        BinaryObject supportingData = loan.GetSupportingData(key, false);
        if (supportingData != null)
          snapshotObject = new SnapshotObject()
          {
            Type = LogSnapshotType.DisclosureTracking,
            ParentId = new Guid(guid),
            Data = supportingData.ToString()
          };
      }
      if (snapshotObject != null)
      {
        IEnumerable<KeyValuePair<string, string>> fieldPairs = LoanLogSnapshotDataParser.ParseFieldPairs(snapshotObject.ToString());
        bool flag = false;
        string str = "";
        foreach (KeyValuePair<string, string> keyValuePair in fieldPairs)
        {
          if (keyValuePair.Key == "L726")
            flag = true;
          else if (keyValuePair.Key == "136")
            str = keyValuePair.Value;
          if (!tracking2015Snapshot.ContainsKey(keyValuePair.Key))
            tracking2015Snapshot.Add(keyValuePair.Key, keyValuePair.Value);
        }
        snapshotObject.Dispose();
        if (!flag)
          tracking2015Snapshot.Add("L726", str);
      }
      else
        TraceLog.WriteWarning(nameof (LoanLogSnapshotStore), string.Format("Error retrieving {0} disclosure tracking snapshot for the loan guid {1}, clientID {2} and Snapshotguid {3}", (object) key, (object) loan.Identity.Guid, (object) ClientContext.CurrentRequest.Context.ClientID, (object) guid));
      return tracking2015Snapshot;
    }

    private static Dictionary<string, string> GetRequestLogSnapshot(Loan loan, string guid)
    {
      return LoanLogSnapshotStore.GetSnapShot(loan.GetSupportingSnapshotData(LogSnapshotType.LockRequest, new Guid(guid), SnapshotObject.GetLoanSnapshotFileName(LogSnapshotType.LockRequest, guid), false));
    }

    private static Dictionary<string, string> GetDocTrackingLogSnapshot(Loan loan, string guid)
    {
      return LoanLogSnapshotStore.GetSnapShot(loan.GetSupportingSnapshotData(LogSnapshotType.DocumentTracking, new Guid(guid), SnapshotObject.GetLoanSnapshotFileName(LogSnapshotType.DocumentTracking, guid), false));
    }

    private static Dictionary<string, string> GetSnapShot(SnapshotObject obj)
    {
      XmlDocument xmlDocument = new XmlDocument();
      if (obj == null)
        return (Dictionary<string, string>) null;
      xmlDocument.LoadXml(obj.ToString());
      Dictionary<string, string> snapShot = new Dictionary<string, string>();
      foreach (XmlElement selectNode in xmlDocument.SelectSingleNode("Log").SelectNodes("FIELD"))
      {
        string attribute1 = selectNode.GetAttribute("id");
        string attribute2 = selectNode.GetAttribute("val");
        if (snapShot.ContainsKey(attribute1))
          snapShot[attribute1] = attribute2;
        else
          snapShot.Add(attribute1, attribute2);
      }
      return snapShot;
    }
  }
}
