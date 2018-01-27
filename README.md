
# SortPics

[![Build Status](https://travis-ci.org/fxlv/SortPics.svg?branch=master)](https://travis-ci.org/fxlv/SortPics)

Simple Picture sorter / Organizer.
Finds all the Images and Videos in specified directory and moves them to destination directory.
Destination directories are organize by Year and Month.

## Why?

The use case is quite simple.
I have tons of images and videos that get automagically uploaded to my OneDrive from my phone.

This is great, but eventually it gets too big and hard to navigate, therefore this simple organizer can bring some order to the chaos.

## Build and Install

Build using Visual Studio.
Once built, it provides an Installer which will install it into "C:\Program Files ..." and will create a shortcut on your Desktop.

## Settings and First run

Upon the first time you run `SortPics.exe` it will ask you to confirm source and destination directories.

## Usage

The simplest way is to just run it with no args. It will find all the pics and videos and will offer you to move them.
Nothing will be moved unless you explicitly confirm the move. So it is safe to just run it an "see what happens".
```
.\SortPics.exe
```

You can also specify filtering, so that you can explicityly select to move only pictures from specific year and (optionally) month.

Find all images and videos from year 2015

```
.\SortPics.exe -y 2017
```

Find all Images and videos from March 2016
```
.\SortPics.exe -y 2016 -m 3
```

You get the idea...
