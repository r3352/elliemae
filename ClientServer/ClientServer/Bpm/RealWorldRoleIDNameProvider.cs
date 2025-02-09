// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.RealWorldRoleIDNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public class RealWorldRoleIDNameProvider : IEnumNameProvider
  {
    public object GetValue(string name)
    {
      switch (name)
      {
        case "Admin":
          return (object) RealWorldRoleID.Administrator;
        case "Broker":
          return (object) RealWorldRoleID.Broker;
        case "CL":
          return (object) RealWorldRoleID.LoanCloser;
        case "LO":
          return (object) RealWorldRoleID.LoanOfficer;
        case "LP":
          return (object) RealWorldRoleID.LoanProcessor;
        case "PSR":
          return (object) RealWorldRoleID.PrimarySalesRep;
        case "TLO":
          return (object) RealWorldRoleID.TPOLoanOfficer;
        case "TLP":
          return (object) RealWorldRoleID.TPOLoanProcessor;
        case "UW":
          return (object) RealWorldRoleID.Underwriter;
        default:
          return (object) RealWorldRoleID.None;
      }
    }

    public string GetName(object value)
    {
      switch ((RealWorldRoleID) value)
      {
        case RealWorldRoleID.LoanOfficer:
          return "LO";
        case RealWorldRoleID.LoanProcessor:
          return "LP";
        case RealWorldRoleID.LoanCloser:
          return "CL";
        case RealWorldRoleID.Broker:
          return "Broker";
        case RealWorldRoleID.Administrator:
          return "Admin";
        case RealWorldRoleID.Underwriter:
          return "UW";
        case RealWorldRoleID.PrimarySalesRep:
          return "PSR";
        case RealWorldRoleID.TPOLoanOfficer:
          return "TLO";
        case RealWorldRoleID.TPOLoanProcessor:
          return "TLP";
        default:
          return "";
      }
    }

    public string[] GetNames()
    {
      return (string[]) new ArrayList()
      {
        (object) this.GetName((object) RealWorldRoleID.LoanOfficer),
        (object) this.GetName((object) RealWorldRoleID.LoanProcessor),
        (object) this.GetName((object) RealWorldRoleID.LoanCloser),
        (object) this.GetName((object) RealWorldRoleID.Broker),
        (object) this.GetName((object) RealWorldRoleID.Administrator),
        (object) this.GetName((object) RealWorldRoleID.Underwriter),
        (object) this.GetName((object) RealWorldRoleID.PrimarySalesRep),
        (object) this.GetName((object) RealWorldRoleID.TPOLoanOfficer),
        (object) this.GetName((object) RealWorldRoleID.TPOLoanProcessor)
      }.ToArray(typeof (string));
    }
  }
}
