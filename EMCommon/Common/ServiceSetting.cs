// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ServiceSetting
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.VersionInterface15;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ServiceSetting
  {
    private readonly string _id;
    private readonly string _categoryName;
    private readonly string _displayName;
    private readonly string _filePath;
    private readonly string _loanfileSpecific;
    private object _tag;
    private readonly string _useStandardValidationGrid;
    private readonly string _useLoanTab;
    private readonly string _dataServiceId;
    private readonly string _minVersionString;
    private readonly string _maxVersionString;
    private readonly bool _disableMenuItemForMultipleLoanSelection;
    private readonly bool _enableClientFilter;
    private readonly bool _toolStripSeparator;

    public ServiceSetting(
      string categoryName,
      string id,
      string displayName,
      string filePath,
      string loanfileSpecific,
      string useStandardValidationGrid,
      string useLoanTab,
      string dataServiceID,
      string minVersion,
      string maxVersion,
      bool disableMenuItemForMultipleLoanSelection = false,
      bool enableClientFilter = false,
      bool toolStripSeparator = false)
    {
      this._id = id;
      this._categoryName = categoryName;
      this._displayName = displayName;
      this._filePath = filePath;
      this._loanfileSpecific = loanfileSpecific;
      this._useLoanTab = useLoanTab;
      this._useStandardValidationGrid = useStandardValidationGrid;
      this._dataServiceId = dataServiceID;
      this._minVersionString = minVersion;
      this._maxVersionString = maxVersion;
      this._disableMenuItemForMultipleLoanSelection = disableMenuItemForMultipleLoanSelection;
      this._enableClientFilter = enableClientFilter;
      this._toolStripSeparator = toolStripSeparator;
    }

    public string ID => this._id;

    public string DataServiceID => this._dataServiceId;

    public string CategoryName => this._categoryName;

    public string DisplayName => this._displayName;

    public string FilePath => this._filePath;

    public bool LoanFileSpecific => this._loanfileSpecific.ToLower() == "true";

    public bool UseLoanTab => this._useLoanTab.ToLower() == "true";

    public bool UseStandardValidationGrid => this._useStandardValidationGrid.ToLower() == "true";

    public string MinVersion => this._minVersionString;

    public ServiceSetting Clone()
    {
      return new ServiceSetting(this._categoryName, this._id, this._displayName, this._filePath, this._loanfileSpecific, this._useStandardValidationGrid, this._useLoanTab, this._dataServiceId, this._minVersionString, this._maxVersionString, this._disableMenuItemForMultipleLoanSelection);
    }

    public bool SupportedInCurrentVersion()
    {
      if (this._minVersionString != "")
      {
        System.Version version = new System.Version(this._minVersionString);
        if (VersionInformation.CurrentVersion.Version.CompareTo((object) new JedVersion(version.Major, version.Minor, version.Revision)) < 0)
          return false;
      }
      if (this._maxVersionString != "")
      {
        System.Version version = new System.Version(this._maxVersionString);
        if (VersionInformation.CurrentVersion.Version.CompareTo((object) new JedVersion(version.Major, version.Minor, version.Revision)) > 0)
          return false;
      }
      return true;
    }

    public object Tag
    {
      get => this._tag;
      set => this._tag = value;
    }

    public bool DisableMenuItemForMultipleLoanSelection
    {
      get => this._disableMenuItemForMultipleLoanSelection;
    }

    public bool EnableClientFilter => this._enableClientFilter;

    public bool ToolStripSeparator => this._toolStripSeparator;

    public string[] EnabledClients { get; internal set; }
  }
}
