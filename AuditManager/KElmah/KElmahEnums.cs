using System.ComponentModel;

namespace KElmah
{
    public enum KExceptionType : byte
    {
        [Description("System.ArgumentNullException")]
        ArgumentNullException = 1,
        [Description("System.Threading.Tasks.TaskCanceledException")]
        TaskCanceledException,
        [Description("System.NullReferenceException")]
        NullReferenceException,
        [Description("System.Net.Sockets.SocketException")]
        SocketException,
        [Description("Newtonsoft.Json.JsonReaderException")]
        JsonReaderException,
        [Description("System.ArgumentException")]
        ArgumentException,
        [Description("System.Net.WebException")]
        WebException,
        [Description("System.Data.SqlClient.SqlException")]
        SqlException,
        [Description("System.FormatException")]
        FormatException,
        [Description("System.ComponentModel.Win32Exception")]
        Win32Exception,
        [Description("BundleTransformer.Core.Translators.AssetTranslationException")]
        AssetTranslationException,
        [Description("AuditManager.Common.LogOnlyException")]
        LogOnlyException,
        [Description("System.Net.Http.HttpRequestException")]
        HttpRequestException,
        [Description("System.UnauthorizedAccessException")]
        UnauthorizedAccessException,
        [Description("System.Web.HttpException")]
        HttpException,
        [Description("System.IO.FileNotFoundException")]
        FileNotFoundException,
        [Description("System.IO.DirectoryNotFoundException")]
        DirectoryNotFoundException,
        [Description("System.Exception")]
        Exception,
        [Description("Microsoft.CSharp.RuntimeBinder.RuntimeBinderException")]
        RuntimeBinderException,
        [Description("System.IndexOutOfRangeException")]
        IndexOutOfRangeException,
        [Description("System.InvalidOperationException")]
        InvalidOperationException,
        [Description("System.Runtime.InteropServices.COMException")]
        COMException,
        [Description("KException")]
        KException,
    }
}
