// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BinaryObjectDownloader
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BinaryObjectDownloader
  {
    private string[] dataKeys;
    private string downloadFolder;
    private BinaryObjectDownloader.ObjectRetriever retriever;
    private IProgressFeedback feedback;

    public BinaryObjectDownloader(
      string downloadFolder,
      string[] dataKeys,
      BinaryObjectDownloader.ObjectRetriever retriever)
    {
      this.dataKeys = dataKeys;
      this.downloadFolder = downloadFolder;
      this.retriever = retriever;
    }

    public DialogResult DownloadFiles(object notUsed, IProgressFeedback feedback)
    {
      try
      {
        this.feedback = feedback;
        if (this.dataKeys.Length == 1)
          feedback.SetFeedback("Downloading file...", "", 0);
        else
          feedback.SetFeedback("Downloading files...", "", 0);
        foreach (string dataKey in this.dataKeys)
        {
          if (feedback.Cancel)
            return DialogResult.Cancel;
          if (!this.downloadFile(dataKey, this.dataKeys.Length == 1) && this.dataKeys.Length == 1)
            return DialogResult.No;
          if (this.dataKeys.Length > 1)
            this.feedback.Increment(1);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) this.feedback.MsgBox("Error during download: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return DialogResult.Abort;
      }
    }

    private bool downloadFile(string dataKey, bool trackProgress)
    {
      BinaryObject binaryObject = this.retriever(dataKey);
      if (binaryObject == null)
        return false;
      if (trackProgress)
      {
        this.feedback.MaxValue = (int) binaryObject.Length;
        binaryObject.DownloadProgress += new DownloadProgressEventHandler(this.onDownloadProgress);
      }
      binaryObject.Download();
      if (trackProgress)
        binaryObject.DownloadProgress -= new DownloadProgressEventHandler(this.onDownloadProgress);
      string path = Path.Combine(this.downloadFolder, dataKey);
      binaryObject.Write(path);
      return true;
    }

    private void onDownloadProgress(object source, DownloadProgressEventArgs e)
    {
      this.feedback.Value = (int) e.BytesDownloaded;
    }

    public delegate BinaryObject ObjectRetriever(string dataKey);
  }
}
