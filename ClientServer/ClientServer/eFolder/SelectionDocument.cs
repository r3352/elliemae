// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.SelectionDocument
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  public class SelectionDocument
  {
    private string docName = string.Empty;
    private DocumentLog documentLog;
    private string requiredString = string.Empty;
    private bool checkBoxVisible = true;
    private bool isChecked;
    private bool optionalWarning;
    private bool requiredError;

    public string DocName
    {
      get => this.docName;
      set => this.docName = value;
    }

    public DocumentLog DocumentLog
    {
      get => this.documentLog;
      set => this.documentLog = value;
    }

    public string RequiredString
    {
      get => this.requiredString;
      set => this.requiredString = value;
    }

    public bool CheckBoxVisible
    {
      get => this.checkBoxVisible;
      set => this.checkBoxVisible = value;
    }

    public bool IsChecked
    {
      get => this.isChecked;
      set => this.isChecked = value;
    }

    public bool OptionalWarning
    {
      get => this.optionalWarning;
      set => this.optionalWarning = value;
    }

    public bool RequiredError
    {
      get => this.requiredError;
      set => this.requiredError = value;
    }
  }
}
