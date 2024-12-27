# AVClient

AVClient is a lightweight application designed as a simplified replacement for Windows Explorer, providing quick access to essential tools and applications. With an intuitive full-screen interface, it is especially useful for kiosk setups, shared systems, or environments requiring a streamlined user experience.

## Features

- **Customizable Application Launcher**: Load custom buttons and categories from a `buttons.json` file.
- **System Control Options**: Includes options to lock, restart, or exit the application.
- **Battery Status Monitoring**: Displays real-time battery percentage and charging status.
- **Time Display**: A real-time clock positioned on the interface.
- **Volume Control**: Quick access to the system volume mixer.
- **IP Address Display**: Shows the local IP address for quick reference.

## Getting Started

1. Place the `buttons.json` configuration file in the same directory as the executable.
2. Run the `AVClient.exe` file to launch the application.
3. Customize the application launcher by editing `buttons.json` to include your desired tools and applications.

## Configuration

The `buttons.json` file allows you to define categories and buttons:
```json
{
  "Categories": [
    {
      "Name": "Utilities",
      "Buttons": [
        {
          "Text": "Notepad",
          "AppPath": "notepad.exe",
          "Tooltip": "Open Notepad"
        }
      ]
    }
  ]
}
```

## Use Cases

- Kiosk or public systems requiring limited access.
- Environments prioritizing a clean, simple interface.
- Replacement for standard file explorer with a focus on usability and customization.

## Requirements

- Windows OS
- .NET Framework 4.8 or later

## License

This project is open-source. Feel free to contribute and adapt it to your needs.
