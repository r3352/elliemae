// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactCreditScores
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  [Serializable]
  public class ContactCreditScores
  {
    public string ID;
    public string LastName;
    public string FirstName;
    public string SSN;
    public CreditScore[] Scores;

    public ContactCreditScores(
      string id,
      string firstName,
      string lastName,
      string ssn,
      CreditScore[] scores)
    {
      this.ID = id;
      this.FirstName = firstName;
      this.LastName = lastName;
      this.SSN = ssn;
      this.Scores = scores;
    }
  }
}
