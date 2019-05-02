# loglook

_loglook_ is a Windows application used to visualize the contents of a log file or text file. Given a search string, _loglook_ plots a histogram of occurences of that string on a time line so that at a glance the user can see the contents of the log file.

## Features
- Plotting occurences of a string in a log file to give an overall picture of the contents of the file
- The histogram plots the number of occurences by time
- Clicking on a data point will open the log file in Notepad++, which can then be used for detailed inspection
- allows multiple files to be open at once and so allowing content from multiple files to be compared visually

## Download

The _loglook__ installer can be downloaded from the Downloads folder in the loglook repository.

## Release Notes

### loglook-20190502.zip
- Fixed bug where timestamp was being interpreted as 12 hour instead of 24 hour.
- Fixed bug where notepad++ did not open if the log file path contained a space.
- If you had previously installed loglook, delete the file ``%appdata%\LogFileDescriptor.json`` before running the installer.

## Customization for new log file types
The default installation of _loglook_ can analyse the time stamp from several common log file formats. Custom formats can be added by editting the file: ``%appdata%\LogFileDescriptor.json``. To analyse a new type of log file, add a new entry to this configuration file and restart _loglook_. Entries take the form of:

```json
  {
    "FileType": "Simple Time",
    "FileTypeDescription": "Simple timestamp with hours to 10th of second",
    "DateTimeFormatString": "hh:mm:ss.ff",
    "ContentIndexStart": 0,
    "DatetimeIndexStart": 11,
    "DatetimeLength": 11
  }
  ```

## Building

### Prerequisites

This project requires Visual Studio 2019 with Windows Desktop workload installed, to build.

  ### Build Release version

  Open solution file and choose "Release" in the Solution Configurations. 
  
- Copy the file `LogFileDescriptor.json` to the folder ``%appdata%``. Alternatively, run the installer project by running the ``loglook-installer`` project.
-   Press ``Ctrl+F5`` to build and run the app.


  ### Build Debug version

  Open solution file and choose "Debug" in the Solution Configurations. 
  
- Copy the file `LogFileDescriptor.json` to the folder ``%appdata%``
- Press ``F5`` to build and run the app.

