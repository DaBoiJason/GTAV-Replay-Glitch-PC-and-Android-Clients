# GTAV Replay Glitch Client

This repository contains a client-server application designed to interact with a GTAV replay glitch system. It includes two parts:

1. **PC Client** (Windows Application)
2. **Android Client** (Mobile Application)

Both clients can communicate with a server over a TCP connection to trigger a replay glitch feature. The PC client can disable or enable network adapters based on instructions from the Android client. It also includes keep-alive functionality to maintain a stable connection.

## Table of Contents

- [Requirements](#requirements)
- [PC Client](#pc-client)
  - [Installation](#installation)
  - [Running as Administrator](#running-as-administrator)
  - [How It Works](#how-it-works)
- [Android Client](#android-client)
  - [Installation](#installation-1)
  - [How It Works](#how-it-works-1)
- [License](#license)

---

## Requirements

### PC Client
You can download the **PC Client** from the following link:  
[Download PC Client](https://github.com/DaBoiJason/GTAV-Replay-Glitch-PC-and-Android-Clients/tree/main/v1.0.0%20Release/Windows%20Client%20(exe))
- Windows 10 or newer
- .NET Framework 4.7.2 or higher
- Administrator privileges (to interact with network adapters)

### Android Client
You can download the **Android Client APK** from the following link:  
[Download APK](https://github.com/DaBoiJason/GTAV-Replay-Glitch-PC-and-Android-Clients/tree/main/v1.0.0%20Release/Android%20Client%20(apk))
- Android 5.0 (Lollipop) or newer
- Internet connection for communication with the server

---

## PC Client

The PC client is a Windows application built with C# and Windows Forms. It listens for incoming packets from the Android client and reacts accordingly by disabling/enabling network adapters based on received commands.


<div align="center">
  <img src="/assets/android.gif" width="25%" />
  <img src="/assets/windows.gif" width="65%" />
</div>


### Installation

1. Download the latest release from the **Releases** section.
2. Extract the contents of the ZIP file.
3. Run the `GTAVReplayGlitchPCClient.exe` file.

### Running as Administrator

The PC client needs to be run **as an administrator** in order to interact with network adapters and disable or enable them although it is allready set to be running with admin privilages incase it isn't you will have to do the following.

- Right-click on the `GTAVReplayGlitchPCClient.exe` file.
- Select **Run as Administrator**.

### How It Works

1. The PC client listens for incoming connections on a specified port.
- Click start to start the listener from windows.
- Connect from your phone entering the details provided by the Windows client and press connect.
- Select from the Windows client the adapter that you are currently using for your computer's connection.
- Do the heist and when you want to do the replay glitch just hit replay on the phone.
- After you are done to re-enable the adapter you need to select it from the list and click the "Enable Selected" button to enable the selected adapter again.
-After every replay glitch you will need to reconnect your phone with the Windows Client. 
2. When it receives a specific packet from the Android client (such as "KEEP_ALIVE" or "REPLAY_GLITCH"), it will either send a keep-alive response or trigger a glitch by enabling/disabling network adapters.
3. It will continue checking for packets and responding to the Android client until the application is closed.

---

## Android Client

The Android client is a mobile application built with Java and Android Studio. It allows you to connect to the server by specifying the server's IP address and port, and it can send commands to trigger the replay glitch feature on the PC.

### Installation

1. Download the latest release from the **Releases** section.
2. Extract the contents of the ZIP file.
3. Install the APK on your Android device.

### How It Works

1. Open the app and enter the server's IP address and port.
2. Click the **Connect** button to establish a connection to the PC client.
3. Once connected, you can click the **Replay Glitch** button to send a "REPLAY_GLITCH" packet to the PC client, which will trigger the glitch.
4. The app also sends a "KEEP_ALIVE" packet every 3 seconds to maintain the connection.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.