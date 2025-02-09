// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvParserForDdm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System.Collections;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvParserForDdm(TextReader dataReader, bool useQuotedStrings) : CsvParser(dataReader, useQuotedStrings)
  {
    protected override string[] parseLine(string text)
    {
      ArrayList arrayList = new ArrayList();
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (this.checkBeginningOfAdvCode(text, index))
        {
          flag2 = true;
          stringBuilder.Append(ch);
        }
        else if (this.checkEndOfAdvCode(text, index))
        {
          flag2 = false;
          stringBuilder.Append(ch);
        }
        else if (ch == ',' && !flag1 && !flag2)
        {
          arrayList.Add((object) stringBuilder.ToString());
          stringBuilder = new StringBuilder(text.Length);
        }
        else if (ch == '"' && !flag1 && !flag2 && this.useQuotedStrings)
          flag1 = true;
        else if (ch == '"' & flag1 && !flag2)
        {
          if (index < text.Length - 1 && text[index + 1] == '"')
            stringBuilder.Append(text[index++]);
          else
            flag1 = false;
        }
        else
          stringBuilder.Append(ch);
      }
      if (stringBuilder.Length > 0)
        arrayList.Add((object) stringBuilder.ToString());
      if (text[text.Length - 1] == ',')
        arrayList.Add((object) string.Empty);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private bool checkBeginningOfAdvCode(string text, int idx)
    {
      int length = 4;
      int startIndex = idx - (length - 1);
      return startIndex >= 0 && text.Substring(startIndex, length).ToLower().Equals("adv(");
    }

    private bool checkEndOfAdvCode(string text, int idx)
    {
      int length = 4;
      int startIndex = idx - (length - 1);
      return startIndex >= 0 && text.Substring(startIndex, length).ToLower().Equals("/adv");
    }
  }
}
