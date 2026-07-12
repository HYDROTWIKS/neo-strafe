# NEO STRAFE

A keyboard macro utility for automated spacebar and WASD key pressing.

## Features
- **Toggle On/Off**: Press `O` to activate/deactivate the macro
- **Spacebar Auto-Press**: While holding spacebar, it auto-presses continuously
- **WASD Spam**: While spacebar is held, WASD keys are rapidly spammed (30ms intervals)
- **Normal Mode**: When spacebar is released, WASD works normally

## Configuration
- **Toggle Key**: O
- **WASD Speed**: 30ms (very fast spam)
- **Exit Key**: ESC

## How to Compile

### Requirements
- .NET 6.0 SDK or later
- Windows OS

### Steps

1. **Install .NET SDK**: Download from https://dotnet.microsoft.com/download

2. **Navigate to project folder**:
   ```bash
   cd neo-strafe
   ```

3. **Build the project**:
   ```bash
   dotnet build -c Release
   ```

4. **Run the EXE**:
   ```bash
   dotnet run
   ```

   Or find the compiled EXE at:
   ```
   bin\Release\net6.0-windows\neo-strafe.exe
   ```

## Usage

1. Run the compiled EXE
2. Press `O` to toggle the macro ON (you'll see "ACTIVATED" in the console)
3. Hold `SPACEBAR` - it will auto-press and WASD will spam
4. Release `SPACEBAR` - WASD returns to normal
5. Press `O` again to turn OFF
6. Press `ESC` to exit the program

## Notes
- Run as Administrator for best compatibility
- The program runs in a console window
- All key presses are logged with timestamps

## Troubleshooting

**Keys not working?**
- Run the program as Administrator
- Check if another macro/gaming software is interfering
- Restart your keyboard

**Program not responding?**
- Press ESC to exit

## License
Free to use and modify
