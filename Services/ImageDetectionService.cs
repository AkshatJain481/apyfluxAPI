using System.Diagnostics;

namespace ImageDetector.Services
{
    public class ImageDetectionService
    {
        public async Task<string> DetectObjectsAsync(byte[] imageData)
        {
            var pythonExecutable = "python3"; // or "python" depending on your environment
            var pythonScriptPath = "ObjectDetectionModal.py";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonExecutable,
                Arguments = pythonScriptPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            // Convert image data to Base64 and write to standard input
            using (var streamWriter = new StreamWriter(process.StandardInput.BaseStream))
            {
                var base64Image = Convert.ToBase64String(imageData);
                await streamWriter.WriteAsync(base64Image);
            }

            // Read the output from the Python script
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
                throw new Exception($"Python script error: {error}");

            return output; // JSON result from Python
        }
    }
}
