// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.FieldRegExMatch
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  internal static class FieldRegExMatch
  {
    public static readonly Regex RecipientFullName = new Regex("&lt;&lt;Recipient\\s+Full\\s+Name\\s*&gt;&gt;", RegexOptions.IgnorePatternWhitespace);
    public static readonly Regex RecipientFullNameStyle = new Regex("style =\"BACKGROUND\\s*-\\s*COLOR:\\s*gainsboro\"\\s*emid=\"Recipient\\s*Full\\s*Name\"", RegexOptions.IgnorePatternWhitespace);
  }
}
