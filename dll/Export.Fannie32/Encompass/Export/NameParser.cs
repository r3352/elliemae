// Decompiled with JetBrains decompiler
// Type: Encompass.Export.NameParser
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

#nullable disable
namespace Encompass.Export
{
  public class NameParser
  {
    private string firstName;
    private string lastName;
    private string middleInitial;
    private string title;

    public NameParser()
    {
    }

    public NameParser(string firstName, string lastName)
    {
      this.ParseFirstName(firstName);
      this.ParseLastName(lastName);
    }

    public void ParseFirstName(string firstName)
    {
      string str = firstName.Trim();
      this.firstName = str;
      this.middleInitial = string.Empty;
      int length = str.IndexOf(" ");
      if (length <= -1)
        return;
      this.firstName = str.Substring(0, length).Trim();
      this.middleInitial = str.Substring(length + 1).Trim();
      if (this.middleInitial.Length >= 3)
      {
        this.firstName = this.firstName + " " + this.middleInitial;
        this.middleInitial = string.Empty;
      }
      else
      {
        if (this.middleInitial.Length != 2 || !(this.middleInitial.Substring(1, 1) != "."))
          return;
        this.firstName = this.firstName + " " + this.middleInitial;
        this.middleInitial = string.Empty;
      }
    }

    public void ParseLastName(string lastName)
    {
      string str = lastName.Trim();
      string[] strArray = "Jr|Sr|Jr.|Sr.|JR|SR|JR.|SR.|jr|sr|jr.|sr.|2nd|3rd|4th|5th|II|III|IV|V".ToLower().Split('|');
      this.lastName = str;
      this.title = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (str.EndsWith(" " + strArray[index]))
        {
          this.title = strArray[index];
          this.lastName = str.Substring(0, str.LastIndexOf(strArray[index])).Trim();
        }
      }
    }

    public string FirstName => this.firstName;

    public string MiddleInitial => this.middleInitial;

    public string LastName => this.lastName;

    public string Title => this.title;
  }
}
