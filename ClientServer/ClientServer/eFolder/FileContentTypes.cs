// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.FileContentTypes
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.IO;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  public static class FileContentTypes
  {
    public static string GetContentType(string filename)
    {
      string contentType;
      switch (Path.GetExtension(filename).Replace(".", string.Empty).ToLower())
      {
        case "b64":
          contentType = "base64/pdf";
          break;
        case "bmp":
          contentType = "image/bmp";
          break;
        case "creditprintfile":
          contentType = "text/plain";
          break;
        case "csv":
          contentType = "text/csv";
          break;
        case "doc":
          contentType = "application/msword";
          break;
        case "docx":
          contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
          break;
        case "emf":
          contentType = "application/emf";
          break;
        case "findingshtml":
          contentType = "text/html";
          break;
        case "gif":
          contentType = "image/gif";
          break;
        case "htm":
        case "html":
          contentType = "text/html";
          break;
        case "jpe":
        case "jpeg":
        case "jpg":
          contentType = "image/jpeg";
          break;
        case "json":
          contentType = "application/json";
          break;
        case "pdf":
          contentType = "application/pdf";
          break;
        case "png":
          contentType = "image/png";
          break;
        case "tif":
        case "tiff":
          contentType = "image/tiff";
          break;
        case "txt":
          contentType = "text/plain";
          break;
        case "xml":
          contentType = "application/xml";
          break;
        case "xps":
          contentType = "application/vnd.ms-xpsdocument";
          break;
        case "zip":
          contentType = "application/zip";
          break;
        default:
          contentType = "application/octet-stream";
          break;
      }
      return contentType;
    }

    public static string GetExtension(string contentType)
    {
      string extension = string.Empty;
      switch (contentType)
      {
        case "application/emf":
          extension = "emf";
          break;
        case "application/json":
          extension = "json";
          break;
        case "application/msword":
          extension = "doc";
          break;
        case "application/octet-stream":
          extension = string.Empty;
          break;
        case "application/pdf":
          extension = "pdf";
          break;
        case "application/vnd.ms-xpsdocument":
          extension = "xps";
          break;
        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
          extension = "docx";
          break;
        case "application/xml":
          extension = "xml";
          break;
        case "application/zip":
          extension = "zip";
          break;
        case "base64/pdf":
          extension = "b64";
          break;
        case "image/bmp":
          extension = "bmp";
          break;
        case "image/gif":
          extension = "gif";
          break;
        case "image/jpeg":
          extension = "jpg";
          break;
        case "image/png":
          extension = "png";
          break;
        case "image/tiff":
          extension = "tif";
          break;
        case "text/csv":
          extension = "csv";
          break;
        case "text/html":
          extension = "html";
          break;
        case "text/plain":
          extension = "txt";
          break;
      }
      return extension;
    }
  }
}
