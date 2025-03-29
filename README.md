# Image Description Generator

## Overview
This project is a C# console application that processes all images in a specified folder and generates descriptive text files for each image using OpenAI's API.

## Features
- Reads images from a specified directory.
- Converts images to Base64 format and sends them to OpenAI.
- Receives AI-generated descriptions and saves them as `.txt` files.
- Supports `.jpg`, `.jpeg`, and `.png` formats.

## Requirements
- .NET Framework installed.
- An OpenAI API key.

## Configuration
The application requires the following settings to be defined in `App.config`:
```xml
<appSettings>
    <add key="apiURL" value="https://api.openai.com/v1/chat/completions"/>
    <add key="openAIKey" value="YOUR_OPENAI_API_KEY"/>
    <add key="prompt" value="Describe this image in detail."/>
</appSettings>
```
You can also use the local end point provided by LM Studio (http://localhost:1234/v1/chat/completions), in which case the API key does not matter.
My preferred local models are:
- https://huggingface.co/DevQuasar/ibm-granite.granite-vision-3.2-2b-GGUF
- https://huggingface.co/FiditeNemini/Llama-3.1-Unhinged-Vision-8B-GGUF
- https://huggingface.co/second-state/Llava-v1.5-7B-GGUF
  
This is an example obtained with IBM's Granite 3.2 vision model:  
<p align="center">
    <img src="https://github.com/user-attachments/assets/43e6f75a-3e36-4b16-93a3-4506079b9d27" width="256" height="256"></br>
    [Flip from Little Nemo comic book]
</p>
<b>Description</b>: "The image depicts a cartoon-style drawing of a frog wearing a red hat. The frog has a green face with black eyes, and its mouth is open as if it's speaking or singing. It is holding a cigar in its mouth, which adds to the whimsical nature of the scene. The frog is dressed in a red suit with a white collar, and there are yellow flowers on its lapel, adding a pop of color to the outfit. The background is a light beige color, providing a neutral backdrop that allows the frog to stand out. The overall style of the drawing is playful and imaginative, with exaggerated features and bright colors."

## How to Use
1. Compile the project.
2. Edit the configuration file.
3. Run the executable with the folder path as an argument:
   ```sh
   ImageDescriptionGenerator.exe "C:\path\to\image\folder"
   ```
4. The application processes images and generates `.txt` files with descriptions.

## Example Output
For an image `example.jpg`, the program creates `example.txt` containing the AI-generated description.

## Error Handling
- If no folder path is provided, the program exit and an error message is displayed.
- If the folder does not exist, an error message is displayed.
- If an issue occurs while processing an image, it is logged to the console.

## License
This project is open-source and available under the MIT License.

