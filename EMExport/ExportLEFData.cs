// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportLEFData
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportLEFData
  {
    private const string typeName = "ExportLEFData";
    private static string traceSW = Tracing.SwImportExport;
    private PgpPublicKey publicKey;
    private const int MAXNUMBEROFRECORDS = 1000;

    public bool Export(LoanDataMgr loanDataMgr, string[] guids)
    {
      if (!File.Exists(SystemSettings.EpassDataDir + "publicLEFKey.asc"))
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass cannot find the public.asc key file for LEF file encryption.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        Stream inputStream = (Stream) File.OpenRead(SystemSettings.EpassDataDir + "publicLEFKey.asc");
        this.publicKey = this.readPublicKey(inputStream);
        inputStream.Close();
        if (this.publicKey == null)
          return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass cannot encrypt LEF file prior to submission due to this error: " + ex.Message);
        return false;
      }
      string outputFolder = string.Empty;
      string str1 = string.Empty;
      using (ExportLEFDialog exportLefDialog = new ExportLEFDialog())
      {
        if (exportLefDialog.ShowDialog((IWin32Window) Session.MainScreen) != DialogResult.OK)
          return false;
        outputFolder = exportLefDialog.SelectedFolder;
        str1 = exportLefDialog.SelectedFileName;
      }
      List<string[]> strArrayList = new List<string[]>();
      for (int index = 0; index < guids.Length; ++index)
      {
        try
        {
          ExportData exportData = new ExportData((LoanDataMgr) null, Session.LoanManager.OpenLoan(guids[index]).GetLoanData(false));
          if (exportData.Validate("LEF", true))
          {
            string[] strArray1 = exportData.Export("LEF").Split('\t');
            if (strArray1.Length > 1)
            {
              foreach (string str2 in strArray1)
              {
                char[] chArray = new char[1]{ '\r' };
                string[] strArray2 = str2.Split(chArray);
                strArrayList.Add(new string[3]
                {
                  strArray2[0].Replace("\n", ""),
                  strArray2[1].Replace("\n", ""),
                  strArray2[2].Replace("\n", "")
                });
              }
            }
            else
            {
              string[] strArray3 = strArray1[0].Split('\r');
              strArrayList.Add(new string[3]
              {
                strArray3[0].Replace("\n", ""),
                strArray3[1].Replace("\n", ""),
                strArray3[2].Replace("\n", "")
              });
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(ExportLEFData.traceSW, nameof (ExportLEFData), TraceLevel.Error, "Export LEF file error: " + ex.Message);
        }
      }
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      string str3 = string.Empty;
      string str4 = string.Empty;
      while (num1 < strArrayList.Count)
      {
        int num5 = 0;
        string empty = string.Empty;
        int num6 = 0;
        for (int index = num1; index < strArrayList.Count && num5 < 1000; ++index)
        {
          string[] strArray = strArrayList[index];
          if (Utils.ParseInt((object) strArray[2]) > num6)
            num6 = Utils.ParseInt((object) strArray[2]);
          ++num5;
        }
        int num7 = 0;
        for (int index1 = num1; index1 < strArrayList.Count && num7 < 1000; ++index1)
        {
          string[] strArray = strArrayList[index1];
          int num8 = Utils.ParseInt((object) strArray[2]);
          if (index1 == num1)
          {
            empty = strArray[0];
            for (int index2 = num8 + 1; index2 <= num6; ++index2)
              empty += ",FEE";
          }
          if (empty != string.Empty)
            empty += "\r\n";
          empty += strArray[1];
          for (int index3 = num8 + 1; index3 <= num6; ++index3)
            empty += ",";
          ++num7;
        }
        num1 += 1000;
        if (empty != string.Empty)
        {
          ++num2;
          try
          {
            FileStream fileStream = new FileStream(Path.GetTempPath() + "LicenseeExamDecrypted" + (num2 > 1 ? "_" + (object) num2 : "") + ".csv", FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] bytes = Encoding.ASCII.GetBytes(empty);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
          }
          catch (Exception ex)
          {
            int num9 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "You do not have access rights to write LEF file to Windows temporary folder for LEF export. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          string outputFileName = str1;
          if (num2 > 1)
          {
            int length = str1.ToLower().LastIndexOf(".lef");
            if (length > -1)
              outputFileName = str1.Substring(0, length) + "_" + (object) num2 + ".lef";
          }
          if (this.encryptLEFFile(empty, outputFileName, outputFolder))
          {
            str3 = str3 + (str3 != string.Empty ? "\r\n" : "") + outputFileName;
            ++num3;
          }
          else
          {
            str4 = str4 + (str4 != string.Empty ? "\r\n" : "") + outputFileName;
            ++num4;
          }
        }
      }
      if (num2 == 1 && str4 == string.Empty)
      {
        int num10 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "The LEF file has been saved to " + outputFolder + "\\" + str1, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return true;
      }
      if (num2 <= 1 || !(str3 != string.Empty) || num3 <= 0)
        return false;
      int num11 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "All loans have been exported to the following LEF file" + (num3 > 1 ? "s" : "") + " in folder '" + outputFolder + "':\r\n" + str3 + (str4 != string.Empty ? "\r\nThe following LEF file" + (num4 > 1 ? "s" : "") + " cannot be created due to error:\r\n" + str4 : ""), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return true;
    }

    private bool encryptLEFFile(string sourceString, string outputFileName, string outputFolder)
    {
      Stream sourceStream = (Stream) new MemoryStream(Encoding.Default.GetBytes(sourceString));
      Stream outputStream = (Stream) File.Create(outputFolder + "\\" + outputFileName);
      bool flag = this.encryptFile(sourceStream, outputStream, outputFileName, this.publicKey, false);
      outputStream.Close();
      sourceStream.Close();
      return flag;
    }

    private bool encryptFile(
      Stream sourceStream,
      Stream outputStream,
      string outputFileName,
      PgpPublicKey pgpPublicKey,
      bool useArmor)
    {
      PgpLiteralDataGenerator literalDataGenerator = (PgpLiteralDataGenerator) null;
      PgpCompressedDataGenerator compressedDataGenerator = (PgpCompressedDataGenerator) null;
      Stream stream = (Stream) null;
      Stream outStr = (Stream) null;
      bool flag = false;
      try
      {
        if (useArmor)
          outputStream = (Stream) new ArmoredOutputStream(outputStream);
        PgpEncryptedDataGenerator encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true, new SecureRandom());
        encryptedDataGenerator.AddMethod(pgpPublicKey);
        outStr = encryptedDataGenerator.Open(outputStream, new byte[65536]);
        compressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
        byte[] buffer1 = new byte[65536];
        literalDataGenerator = new PgpLiteralDataGenerator();
        stream = literalDataGenerator.Open(compressedDataGenerator.Open(outStr), 'b', outputFileName, DateTime.UtcNow, buffer1);
        byte[] buffer2 = new byte[buffer1.Length];
        int count;
        while ((count = sourceStream.Read(buffer2, 0, buffer2.Length)) > 0)
          stream.Write(buffer2, 0, count);
        flag = true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass cannot encrypt LEF file prior to submission due to this error: " + ex.Message);
      }
      finally
      {
        literalDataGenerator?.Close();
        compressedDataGenerator?.Close();
        stream?.Close();
        outStr?.Close();
      }
      return flag;
    }

    private PgpPublicKey readPublicKey(Stream inputStream)
    {
      inputStream = PgpUtilities.GetDecoderStream(inputStream);
      foreach (PgpPublicKeyRing keyRing in new PgpPublicKeyRingBundle(inputStream).GetKeyRings())
      {
        foreach (PgpPublicKey publicKey in keyRing.GetPublicKeys())
        {
          if (publicKey.IsEncryptionKey)
            return publicKey;
        }
      }
      int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass Can't find encryption key in key file.");
      return (PgpPublicKey) null;
    }
  }
}
