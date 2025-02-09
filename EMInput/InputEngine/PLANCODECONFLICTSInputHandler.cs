// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PLANCODECONFLICTSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PLANCODECONFLICTSInputHandler(
    Sessions.Session session,
    IWin32Window owner,
    IHtmlInput input,
    HTMLDocument htmldoc,
    EllieMae.Encompass.Forms.Form form,
    object property) : InputHandlerBase(session, input, htmldoc, form, property)
  {
    private EllieMae.Encompass.Forms.Form form;
    private EllieMae.Encompass.Forms.Image imgConflict;
    private bool suspendRefresh;
    private bool allowEdit = true;
    private Plan planData;
    private List<FieldControl> loanFieldControls = new List<FieldControl>();
    private Dictionary<string, EllieMae.Encompass.Forms.Image> conflictImages = new Dictionary<string, EllieMae.Encompass.Forms.Image>();

    public PLANCODECONFLICTSInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public Plan Plan
    {
      get => this.planData;
      set => this.planData = value;
    }

    public bool AllowEdit
    {
      get => this.allowEdit;
      set => this.allowEdit = value;
    }

    internal override void CreateControls()
    {
      this.form = (EllieMae.Encompass.Forms.Form) this.GetAutomationFormObject();
      this.imgConflict = (EllieMae.Encompass.Forms.Image) this.form.FindControl("imgConflict");
      foreach (FieldControl allFieldControl in this.form.GetAllFieldControls())
      {
        if (allFieldControl.FieldSource == FieldSource.Other)
        {
          allFieldControl.DataBind += new DataBindEventHandler(this.bindPlanCodeField);
        }
        else
        {
          this.loanFieldControls.Add(allFieldControl);
          allFieldControl.ValueChanged += new EventHandler(this.onLoanValueChanged);
        }
      }
    }

    public override void RefreshContents()
    {
      using (CursorActivator.Wait())
      {
        this.suspendRefresh = true;
        try
        {
          base.RefreshContents();
          this.refreshConflictData();
        }
        finally
        {
          this.suspendRefresh = false;
        }
      }
    }

    private void onLoanValueChanged(object sender, EventArgs e)
    {
      if (this.suspendRefresh)
        return;
      this.refreshConflictData();
    }

    private void refreshConflictData()
    {
      FieldConflict[] fieldConflictArray = this.planData.CompareTo(this.inputData);
      Dictionary<string, FieldConflict> dictionary = new Dictionary<string, FieldConflict>();
      foreach (FieldConflict fieldConflict in fieldConflictArray)
        dictionary[fieldConflict.FieldID] = fieldConflict;
      foreach (FieldControl loanFieldControl in this.loanFieldControls)
      {
        if (loanFieldControl.Field.FieldID == "1177" && dictionary.ContainsKey("Terms.IntrOnly"))
        {
          loanFieldControl.Enabled = this.allowEdit;
          this.createConflictImage(loanFieldControl);
        }
        else if (dictionary.ContainsKey(loanFieldControl.Field.FieldID))
        {
          if (!loanFieldControl.Field.ReadOnly)
            loanFieldControl.Enabled = this.allowEdit;
          this.createConflictImage(loanFieldControl);
        }
        else
        {
          loanFieldControl.Enabled = false;
          this.clearConflictImage(loanFieldControl);
        }
      }
      this.enableDisableZoom("zoomprepayvbg", this.allowEdit && dictionary.ContainsKey("Terms.PrepyVrbgTyp"));
      this.enableDisableZoom("zoomadlsigvbg", this.allowEdit && dictionary.ContainsKey("PlanCode.AdtlSgVbgTyp"));
    }

    private void enableDisableZoom(string action, bool enabled)
    {
      foreach (IActionable actionable in this.form.FindControlsByType(typeof (IActionable)))
      {
        if (actionable.Action == action)
        {
          ((RuntimeControl) actionable).Enabled = enabled;
          break;
        }
      }
    }

    private void createConflictImage(FieldControl fieldCtrl)
    {
      string fieldId = fieldCtrl.Field.FieldID;
      if (this.conflictImages.ContainsKey(fieldId))
      {
        this.conflictImages[fieldId].Visible = true;
      }
      else
      {
        int num = fieldCtrl.Top + fieldCtrl.Size.Height / 2;
        Point position = new Point(this.imgConflict.Left, fieldCtrl.Top + 2);
        EllieMae.Encompass.Forms.Image image = new EllieMae.Encompass.Forms.Image();
        fieldCtrl.GetContainer().Controls.Insert((EllieMae.Encompass.Forms.Control) image, position);
        image.Size = this.imgConflict.Size;
        image.Stretch = false;
        image.Source = this.imgConflict.Source;
        image.Visible = true;
        this.conflictImages[fieldId] = image;
      }
    }

    private void clearConflictImage(FieldControl fieldCtrl)
    {
      string fieldId = fieldCtrl.Field.FieldID;
      if (!this.conflictImages.ContainsKey(fieldId))
        return;
      this.conflictImages[fieldId].Visible = false;
    }

    private void bindPlanCodeField(object sender, DataBindEventArgs e)
    {
      if (this.planData == null)
        return;
      FieldControl fieldControl = (FieldControl) sender;
      e.Value = this.planData.GetField(fieldControl.Field.FieldID);
    }
  }
}
