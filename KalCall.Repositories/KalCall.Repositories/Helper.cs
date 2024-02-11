namespace KalCall.Repositories
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class Helper
    {
        public static async Task<string> FilePhone(string sourceBaseUrl, string sourceToken,
           string sourceDir)
        {
            var root = await Helper.GetFiles(sourceBaseUrl, sourceToken, sourceDir);
            string log = $"name, phone";
            if (root != null)
            {
                foreach (var file in root.files.OrderByDescending(x => x.name))
                {
                    try
                    {
                        log += $"{Environment.NewLine}{file.name}";

                        if (file.fileType == "AUDIO" || file.fileType == "TTS")
                        {
                            log += $"{Environment.NewLine}{file.name}, {file.phone}";
                        }
                    }
                    catch (Exception ex)
                    {
                        log += $" faild. {ex.Message}";
                    }
                }
            }
            System.IO.File.WriteAllText("log.svc", log);
            return log;
        }


        public static async Task<string> CopyFiles(string sourceBaseUrl, string sourceToken,
           string sourceDir, string targetBaseUrl, string targetToken, string targetDir, int fromNumber = -1, int toNumber = -1, DateTime? fromDate = null)
        {
            var root = await Helper.GetFiles(sourceBaseUrl, sourceToken, sourceDir);
            string log = $"from {sourceDir} to:{targetDir}";
            if (root != null)
            {
                foreach (var file in root.files.OrderBy(x => x.name))
                {
                    try
                    {
                        log += $"{Environment.NewLine}{file.name}";

                        if (file.fileType == "AUDIO" || file.fileType == "TTS")
                        {
                            int.TryParse(Path.GetFileNameWithoutExtension(file.name), out int fileNumber);
                            var fileDate = file.mtime != null ?
                                DateTime.ParseExact(file.mtime.Split(' ')[0], "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                : DateTime.MinValue;
                            if ((fromNumber == -1 || fileNumber >= fromNumber) &&
                                    (toNumber == -1 || fileNumber <= toNumber) &&
                                    (fromDate == null || fileDate.Date == fromDate.Value.Date))
                            {
                                var downloadUri = new Uri($"{sourceBaseUrl}/DownloadFile?token={sourceToken}&path=ivr2:/{sourceDir}/{file.name}");
                                var fileBytes = await Helper.ExectueReturnBytes(downloadUri);
                                var uploadUri = new Uri($"{targetBaseUrl}/UploadFile?token={targetToken}&convertAudio=0&tts=0&autoNumbering=1&path=ivr2:{targetDir}/{file.name}");
                                var result = await UploadFile(uploadUri, fileBytes, file.name);
                                log += " copied.";
                            }
                            else
                            {
                                log += " does not match the conditions.";
                            }
                        }
                        else
                        {
                            log += " not audio or tts.";
                        }
                    }
                    catch (Exception ex)
                    {
                        log += $" faild. {ex.Message}";
                    }
                }
            }
            return log;
        }


        public static async Task<string> DoFileAction(string action,
            string baseUrl, string token,
            string sourceDir, string targrtDir, int fromNumber = -1, int toNumber = -1, DateTime? fromDate = null)
        {
            var newFileName = await Helper.GetNewFileName(baseUrl, token, targrtDir);
            var log = string.Empty;
            var root = await Helper.GetFiles(baseUrl, token, sourceDir);
            if (root != null)
            {
                foreach (var file in root.files.OrderBy(x => x.name))
                {
                    try
                    {
                        log += $"{Environment.NewLine}{file.name}";
                        if (file.fileType == "AUDIO" || file.fileType == "TTS")
                        {
                            int.TryParse(Path.GetFileNameWithoutExtension(file.name), out int fileNumber);
                            var fileDate = DateTime.ParseExact(file.mtime.Split(' ')[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            if ((fromNumber == -1 || fileNumber >= fromNumber) &&
                                    (toNumber == -1 || fileNumber <= toNumber) &&
                                    (fromDate == null || fileDate.Date == fromDate.Value.Date))
                            {
                                var url = new Uri($"{baseUrl}/FileAction?token={token}&action={action}&what={file.what}&target=ivr2:{targrtDir}/{newFileName}");
                                await Helper.Exectue(url);
                                newFileName = Helper.GetNextFileName(newFileName);
                                log += " Done.";
                            }
                            else
                            {
                                log += " does not match the conditions.";
                            }
                        }
                        else
                        {
                            log += " not audio or tts.";
                        }
                    }
                    catch (Exception ex)
                    {
                        log += $" faild. {ex.Message}";
                    }
                }
            }
            return log;
        }


        public static async Task<string> UploadFile(Uri uri, byte[] fileBytes, string fileName)
        {
            using (HttpClient httpClient = new())
            {
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Add the file as a byte array
                    var byteContent = new ByteArrayContent(fileBytes);
                    // byteContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
                    multipartFormContent.Add(byteContent, "file", fileName);

                    //Send it
                    var response = await httpClient.PostAsync(uri, multipartFormContent);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<string?> Exectue(Uri uri)
        {
            using (HttpClient httpClient = new())
            {
                using HttpResponseMessage responseGet = await httpClient.GetAsync(uri);

                if (responseGet.IsSuccessStatusCode && responseGet.Content != null)
                {
                    return await responseGet.Content.ReadAsStringAsync();
                }
                return null;
            }
        }
        public static async Task<byte[]?> ExectueReturnBytes(Uri uri)
        {
            using (HttpClient httpClient = new())
            {
                using HttpResponseMessage responseGet = await httpClient.GetAsync(uri);

                if (responseGet.IsSuccessStatusCode && responseGet.Content != null)
                {
                    return await responseGet.Content.ReadAsByteArrayAsync();
                }
                return null;
            }
        }

        public static async Task<Root?> GetFiles(string baseURL, string token, string dir)
        {
            var uri = new Uri($"{baseURL}/GetIVR2Dir?token={token}&path=ivr2:/{dir}");
            var response = await Exectue(uri);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<Root>(response);
            }
            return null;

        }
        public static async Task<string> GetNewFileName(string baseURL, string token, string dir)
        {
            var root = await GetFiles(baseURL, token, dir);
            if (root != null && root.files != null)
            {
                foreach (var file in root.files)
                {
                    if (file.fileType == "AUDIO" || file.fileType == "TTS")
                    {
                        return GetNextFileName(file.name);
                    }
                }
            }
            return "000.wav";
        }
        public static string GetNextFileName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);

            if (int.TryParse(name, out int fileNumber))
            {
                return $"{fileNumber + 1}.wav".PadLeft(7, '0');
            }
            return "000.wav";
        }
    }
}