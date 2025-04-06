using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration settings
        string apiUrl = ConfigurationManager.AppSettings["apiURL"];
        string apiKey = ConfigurationManager.AppSettings["openAIKey"];
        string prompt = ConfigurationManager.AppSettings["prompt"];
        string gptmodel = ConfigurationManager.AppSettings["model"];

        // Check if the user provided a folder path
        if (args.Length != 1)
        {
            Console.WriteLine("Please provide the folder path as an argument.");
            return;
        }

        var folderPath = args[0];
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return;
        }

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        // Iterate through all image files in the folder
        foreach (var filePath in Directory.GetFiles(folderPath))
        {
            if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Convert image to Base64
                    var base64Image = Convert.ToBase64String(File.ReadAllBytes(filePath));

                    // Prepare request payload
                    var requestBody = new
                    {
                        model = gptmodel,
                        messages = new[]
                        {
                            new
                            {
                                role = "user",
                                content = new object[]
                                {
                                    new { type = "text", text = prompt },
                                    new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                                }
                            }
                        },
                        max_tokens = 3000
                    };

                    var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Send request to OpenAI API
                    var response = await client.PostAsync(apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Extract the generated description from the response
                    var responseObject = JObject.Parse(responseString);
                    var description = responseObject["choices"][0]["message"]["content"].ToString();

                    // Generate the output text file path
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    var outputFilePath = Path.Combine(folderPath, $"{fileNameWithoutExtension}.txt");

                    // Save the description to a text file
                    File.WriteAllText(outputFilePath, description);

                    Console.WriteLine($"Description saved to: {outputFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                }
            }
        }

        Console.WriteLine("Operation completed!");
    }
}
