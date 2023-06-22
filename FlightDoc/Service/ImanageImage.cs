namespace FlightDoc.Service
{
    public interface ImanageImage
    {
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(string FileName);

    }
}
