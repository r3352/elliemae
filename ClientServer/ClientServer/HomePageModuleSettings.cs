// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.HomePageModuleSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class HomePageModuleSettings
  {
    private string moduleId;
    private string title;
    private string category;
    private string description;
    private string error;
    private bool _isLocked;
    private bool _isDefault;
    private bool _isAccessible;
    private bool isLocked;
    private bool isDefault;
    private bool isDefaultEnabled;
    private bool isAccessible;
    private bool isAccessibleEnabled;

    public string ModuleID => this.moduleId;

    public string Title => this.title;

    public string Category => this.category;

    public string Description => this.description;

    public string Error => this.error;

    public string IsLockedToString => !this.isLocked ? "0" : "1";

    public string IsAccessibleToString => !this.isAccessible ? "0" : "1";

    public string IsDefaultToString => !this.isDefault ? "0" : "1";

    public bool IsAccessible => this.isAccessible;

    public void SetIsAccessible(bool isAccessible, int accessibleCount)
    {
      if (accessibleCount > -1 && !isAccessible && accessibleCount == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "At least one module must be accessible", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.isAccessibleEnabled)
          this.isAccessible = isAccessible;
        this.ApplyCheckBoxLogic();
      }
    }

    public bool IsDefault => this.isDefault;

    public void SetIsDefault(bool isDefault)
    {
      if (this.isDefaultEnabled)
        this.isDefault = isDefault;
      this.ApplyCheckBoxLogic();
    }

    public bool IsLocked => this.isLocked;

    public void SetIsLocked(bool isLocked)
    {
      this.isLocked = isLocked;
      this.ApplyCheckBoxLogic();
    }

    public bool Load(XmlElement elm)
    {
      try
      {
        this.moduleId = elm.GetAttribute("ID");
        this.title = elm.GetAttribute("Title");
        this.category = elm.GetAttribute("Category");
        this.description = elm.GetAttribute("Description");
        this.isLocked = elm.GetAttribute("IsLocked") == "1";
        this.isDefault = elm.GetAttribute("IsDefault") == "1";
        this.isAccessible = elm.GetAttribute("IsAccessible") == "1";
        this.ApplyCheckBoxLogic();
        this._isLocked = this.isLocked;
        this._isDefault = this.isDefault;
        this._isAccessible = this.isAccessible;
        return true;
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return false;
      }
    }

    public bool MigrateSettings(XmlElement elm, bool isLocked)
    {
      try
      {
        this.moduleId = elm.GetAttribute("ID");
        this.title = elm.GetAttribute("Title");
        this.category = elm.GetAttribute("Category");
        this.description = elm.GetAttribute("Description");
        if (!isLocked)
        {
          this.isLocked = false;
          this.isDefault = elm.GetAttribute("IsDefault") == "1";
          this.isAccessible = elm.GetAttribute("IsAccessible") == "1";
        }
        else
        {
          this.isDefault = elm.GetAttribute("IsDefault") == "1";
          this.isLocked = this.isDefault;
          this.isAccessible = this.isDefault;
        }
        this.ApplyCheckBoxLogic();
        this._isLocked = this.isLocked;
        this._isDefault = this.isDefault;
        this._isAccessible = this.isAccessible;
        return true;
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return false;
      }
    }

    public bool Save(XmlDocument xml, XmlElement parentElm)
    {
      try
      {
        XmlElement element = xml.CreateElement("MODULE");
        element.SetAttribute("ID", this.moduleId);
        element.SetAttribute("IsLocked", this.isLocked ? "1" : "0");
        element.SetAttribute("IsDefault", this.isDefault ? "1" : "0");
        element.SetAttribute("IsAccessible", this.isAccessible ? "1" : "0");
        parentElm.AppendChild((XmlNode) element);
        this._isLocked = this.isLocked;
        this._isDefault = this.isDefault;
        this._isAccessible = this.isAccessible;
        return true;
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return false;
      }
    }

    public void ApplyCheckBoxLogic()
    {
      if (!this.isLocked)
      {
        this.isDefaultEnabled = true;
        if (this.isDefault)
        {
          this.isAccessible = true;
          this.isAccessibleEnabled = false;
        }
        else
          this.isAccessibleEnabled = true;
      }
      else
      {
        this.isDefault = true;
        this.isAccessible = true;
        this.isDefaultEnabled = false;
        this.isAccessibleEnabled = false;
      }
    }

    public bool IsModified
    {
      get
      {
        return this.isLocked != this._isLocked || this.IsDefault != this._isDefault || this.IsAccessible != this._isAccessible;
      }
    }

    public void ResetAll()
    {
      this.SetIsLocked(this._isLocked);
      this.SetIsDefault(this._isDefault);
      this.SetIsAccessible(this._isAccessible, -1);
    }
  }
}
