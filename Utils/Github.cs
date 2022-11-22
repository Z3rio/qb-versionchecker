namespace VersionChecker.Utils
{
    internal class Github
    {
        public static HttpClient client = new HttpClient();

        public static async Task<bool> IsValid(string url)
        {
            HttpResponseMessage res = await client.GetAsync(url);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string? GetVersionNumber(string fxmanifest)
        {
            List<string> lines = fxmanifest.Split(new[] { '\r', '\n' }).ToList();

            string? versionLine = null;

            foreach (string line in lines)
            {
                if (line.Contains("version") && !line.Contains("fx_version"))
                {
                    versionLine = line;
                }
            }

            if (versionLine != null)
            {
                bool versionStarted = false;
                string versionStr = "";

                for (int i = 0; i < versionLine.Length; i++)
                {
                    char letter = versionLine[i];

                    if (versionStarted == true)
                    {
                        if (letter == ',' || letter == '.' || char.IsDigit(letter))
                        {
                            versionStr += letter;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (letter == '\'' || letter == '\"')
                    {
                        versionStarted = true;
                    }
                }

                if (versionStr != "")
                {
                    return versionStr;
                }
            }

            return null;
        }
    }
}
