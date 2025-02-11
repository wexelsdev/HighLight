# HighLight

## Overview
HighLight is a C# console application designed to demonstrate how to easily implement a modding and plugin system for games and applications. It features a command handler with alias support and a flexible plugin system.

## Features
- **Command handler** with alias support
- **Flexible plugin system** with DLL loading
- **Logging system** that saves events to a file
- **Plugin configuration support**

## Getting Started

### Requirements
- .NET 8.0 or later

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/Time2138/HighLight.git
   cd HighLight
   ```
2. Build the project:
   ```sh
   dotnet build
   ```
3. Run the application:
   ```sh
   dotnet run
   ```

## Creating Plugins
Plugins are implemented as separate DLL files and loaded dynamically. To create a plugin:
1. Create a new C# class library project.
2. Reference HighLight's core library (HighLight.dll).
3. Reference Timersky libraries (Timersky.Log.dll & Timersky.Config.dll).
4. Implement the `Plugin` class & use plugin attribute:
   ```csharp
   [Plugin("MyPlugin", "Your Name", "1.0.0", "An example plugin")]
   public class MyPlugin : Plugin<IConfig>
   {
       public override void OnEnable()
       {
           Log.Info("My Plugin!");
       }
   }
   ```
4. Compile your plugin and place the DLL in the `Plugins/` folder.

## Contributing
Contributions are welcome! Feel free to fork the repository and submit pull requests.

## License
This project is licensed under the MIT License.
