// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Borrower
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class Borrower
  {
    private string firstName;
    private string lastName;
    private string id;
    private string _eid;

    public string FirstName => this.firstName;

    public string LastName => this.lastName;

    public string Id => this.id;

    public string EID => this._eid;

    public Borrower()
    {
    }

    public Borrower(string firstName, string lastName, string id)
    {
      this.firstName = firstName;
      this.lastName = lastName;
      this.id = id;
    }

    public Borrower(string firstName, string lastName, string id, string eid)
      : this(firstName, lastName, id)
    {
      this._eid = eid;
    }

    public override string ToString() => this.firstName + " " + this.lastName;

    public override int GetHashCode() => this.id.GetHashCode();

    public override bool Equals(object obj) => obj is Borrower borrower && this.id == borrower.id;
  }
}
