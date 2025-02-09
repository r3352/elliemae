// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.DateTimeSettingDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public class DateTimeSettingDefinition : SettingDefinition
  {
    private DateTime defaultValue;
    private DateTime startDate = DateTime.MinValue;
    private DateTime endDate = DateTime.MaxValue;
    private bool checkValidation;
    private string errorMessage = "";

    public DateTimeSettingDefinition(
      string path,
      string displayName,
      string description,
      SettingTargetSystem appliesTo,
      DateTime defaultValue,
      bool requiresRestart,
      bool displayEnabled)
      : base(path, displayName, description, appliesTo, requiresRestart, displayEnabled)
    {
      this.defaultValue = defaultValue;
    }

    public override object DefaultValue => (object) this.defaultValue;

    public override object Parse(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (object) this.defaultValue;
      try
      {
        return (object) DateTime.Parse(value);
      }
      catch
      {
        return (object) this.defaultValue;
      }
    }

    public bool CheckValidation => this.checkValidation;

    public string ErrorMessage => this.errorMessage;

    public void SetDateRangeValidation(
      DateTime startDate,
      DateTime endDate,
      string errorMessage,
      bool checkValidation)
    {
      this.startDate = startDate;
      this.endDate = endDate;
      this.errorMessage = errorMessage;
      this.checkValidation = checkValidation;
    }

    public bool ValidateDateRange(DateTime inputDate)
    {
      return !this.checkValidation || !(inputDate.Date < this.startDate.Date) && !(inputDate.Date > this.endDate.Date);
    }
  }
}
