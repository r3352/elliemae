// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.JsonFile`1
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class JsonFile<T> where T : class
  {
    private Control _parent;

    public JsonFile(Control parent) => this._parent = parent;

    public void DoExport(T toExport)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.FileName = string.Format("{0}_{1:yyyyMMdd-HHmm}", (object) nameof (T), (object) DateTime.Now);
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      this.SetFileDialogOptions((FileDialog) saveFileDialog2);
      if (saveFileDialog2.ShowDialog((IWin32Window) this._parent) != DialogResult.OK)
        return;
      using (FileStream fileStream = new FileStream(saveFileDialog2.FileName, FileMode.OpenOrCreate))
      {
        try
        {
          this.SerializeToStream(toExport, (Stream) fileStream);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this._parent, "Error while saving data:\n" + ex.Message);
        }
      }
    }

    public T DoImport()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      this.SetFileDialogOptions((FileDialog) openFileDialog);
      if (openFileDialog.ShowDialog((IWin32Window) this._parent) == DialogResult.OK)
      {
        using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
        {
          try
          {
            return this.DeserializeFromStream((Stream) fileStream);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this._parent, "Error while importing data:\n" + ex.Message);
          }
        }
      }
      return default (T);
    }

    private void SetFileDialogOptions(FileDialog fileDialog)
    {
      fileDialog.DefaultExt = "json";
      fileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
      fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }

    private void SerializeToStream(T toExport, Stream stream)
    {
      StreamWriter streamWriter = new StreamWriter(stream);
      JsonTextWriter jsonTextWriter1 = new JsonTextWriter((TextWriter) streamWriter);
      JsonSerializer jsonSerializer = new JsonSerializer();
      stream.Seek(0L, SeekOrigin.Begin);
      JsonTextWriter jsonTextWriter2 = jsonTextWriter1;
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) toExport;
      jsonSerializer.Serialize((JsonWriter) jsonTextWriter2, (object) local);
      streamWriter.Flush();
    }

    private T DeserializeFromStream(Stream stream)
    {
      JsonTextReader jsonTextReader = new JsonTextReader((TextReader) new StreamReader(stream));
      JsonSerializer jsonSerializer = new JsonSerializer();
      stream.Seek(0L, SeekOrigin.Begin);
      JsonTextReader reader = jsonTextReader;
      return jsonSerializer.Deserialize<T>((JsonReader) reader);
    }
  }
}
