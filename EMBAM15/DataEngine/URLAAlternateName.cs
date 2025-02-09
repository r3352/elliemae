// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.URLAAlternateName
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class URLAAlternateName
  {
    public string ID { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Suffix { get; set; }

    public string FullName { get; set; }

    public URLAAlternateName(
      string id,
      string firstName,
      string middleName,
      string lastName,
      string suffix,
      string fullName)
    {
      this.ID = id;
      this.FirstName = firstName;
      this.MiddleName = middleName;
      this.LastName = lastName;
      this.Suffix = suffix;
      this.FullName = fullName;
    }

    public bool IsBlank
    {
      get
      {
        return this.FirstName == "" && this.MiddleName == "" && this.LastName == "" && this.Suffix == "";
      }
    }
  }
}
