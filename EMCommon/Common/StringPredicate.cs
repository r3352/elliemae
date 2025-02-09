// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.StringPredicate
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class StringPredicate
  {
    private string stringToMatch;
    private bool ignoreCase;

    public StringPredicate(string stringToMatch)
      : this(stringToMatch, false)
    {
    }

    public StringPredicate(string stringToMatch, bool ignoreCase)
    {
      this.stringToMatch = stringToMatch;
      this.ignoreCase = ignoreCase;
    }

    public bool Compare(string text)
    {
      return text != null && string.Compare(this.stringToMatch, text, this.ignoreCase) == 0;
    }

    public static implicit operator Predicate<string>(StringPredicate sp)
    {
      return new Predicate<string>(sp.Compare);
    }
  }
}
