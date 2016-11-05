# simple-exif-reader
The problem is that for some reason I have never used any special tools for importing pictures from my camera and I ended up in a situation where I had my Media folder full of folders called something like "random stuff april", "more from Jan-March 2016" etc. 
The tool will scan your input directory for media files and, tries to get the date when the image/video was created by reading the [EXIF](https://en.wikipedia.org/wiki/Exif) metadata and copies the file new, more sensible path. The output structure looks like:

```
\\DESKTOP-BTH3I9B\TempPic.
├───2015
│   ├───1-January
│   ├───10-October
│   ├───11-November
│   ├───12-December
│   ├───2-February
│   ├───3-March
│   ├───4-April
│   ├───5-May
│   ├───6-June
│   ├───7-July
│   ├───8-August
│   └───9-September
├───2016
│   ├───1-January
│   ├───2-February
│   ├───3-March
│   ├───4-April
│   ├───5-May
│   ├───6-June
│   └───7-July
```

I've tested it with pictures/videos taken with 
+ Canon EOS 650d
+ HTC One mini
+ Sony Xperia Z5 compact