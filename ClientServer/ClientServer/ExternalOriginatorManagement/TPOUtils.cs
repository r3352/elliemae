// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.TPOUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  public static class TPOUtils
  {
    public static string returnRoles(int role)
    {
      string str = "";
      if ((role & 1) == 1)
        str += "Loan Officer";
      if ((role & 2) == 2)
        str += ", Loan Processor";
      if ((role & 4) == 4)
        str += ", Manager";
      if ((role & 8) == 8)
        str += ", Administrator";
      if (str.Length > 2 && str.Substring(0, 1).Trim() == ",")
        str = str.Substring(2);
      return str;
    }

    public static string ReturnPersonas(Persona[] persona)
    {
      string str = string.Empty;
      if (persona != null)
        str = string.Join(", ", ((IEnumerable<Persona>) persona).Select<Persona, string>((Func<Persona, string>) (obj => obj.Name)));
      return str;
    }

    public static bool IsLoanOfficer(int role) => (role & 1) == 1;

    public static bool IsLoanProcessor(int role) => (role & 2) == 2;

    public static bool IsManager(int role) => (role & 4) == 4;

    public static bool IsAdministrator(int role) => (role & 8) == 8;
  }
}
