// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvParser
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Collections;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvParser : IDisposable
  {
    private TextReader dataReader;
    protected bool useQuotedStrings = true;

    public CsvParser(TextReader dataReader, bool useQuotedStrings)
    {
      this.dataReader = dataReader;
      this.useQuotedStrings = useQuotedStrings;
    }

    public string[] NextRow()
    {
      string text;
      do
      {
        string str = this.dataReader.ReadLine();
        if (str == null)
          return (string[]) null;
        text = str.Trim();
      }
      while (text == "");
      return this.parseLine(text);
    }

    public string[][] NextRows(int count)
    {
      string[][] strArray = new string[count][];
      for (int index = 0; index < count; ++index)
        strArray[index] = this.NextRow();
      return strArray;
    }

    public string[][] RemainingRows()
    {
      ArrayList arrayList = new ArrayList();
      for (string[] strArray = this.NextRow(); strArray != null; strArray = this.NextRow())
        arrayList.Add((object) strArray);
      return (string[][]) arrayList.ToArray(typeof (string[]));
    }

    public void Dispose()
    {
      if (this.dataReader == null)
        return;
      this.dataReader.Close();
      this.dataReader = (TextReader) null;
    }

    protected virtual string[] parseLine(string text)
    {
      ArrayList arrayList = new ArrayList();
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      bool flag = false;
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (ch == ',' && !flag)
        {
          arrayList.Add((object) stringBuilder.ToString());
          stringBuilder = new StringBuilder(text.Length);
        }
        else if (ch == '"' && !flag && this.useQuotedStrings)
          flag = true;
        else if (ch == '"' & flag)
        {
          if (index < text.Length - 1 && text[index + 1] == '"')
            stringBuilder.Append(text[index++]);
          else
            flag = false;
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
  }
}
