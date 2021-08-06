#!/bin/bash

cd ..

# Change the Path to the Solution Project folder
SLNPath=$PWD"/"
# echo $SLNPath

MyPath=$SLNPath"BrightSign.Droid/"
cd $MyPath

AssemblyInfoPath=$MyPath"Properties/AssemblyInfo.cs"
echo $AssemblyInfoPath
# /p:AssemblyPath=AssemblyInfoPath

VersionNumber=$1 
BuildNumber=$2 

# Build the Project in release mode
MSBuild /t:Clean /p:Configuration=Release
MSBuild /t:SignAndroidPackage /p:Configuration=Release /p:SetVersion=True /p:VersionNumber=$VersionNumber /p:BuildNumber=$BuildNumber


# Path to the Generated Signed APK file
SignedAPKPath=$MyPath"bin/Release/*-Signed.apk"
echo $SignedAPKPath

#Make a new directory for Saving the builds
mkdir $SLNPath"Builds"

# New Path to save the Builds
NewPath=$SLNPath"Builds"
echo $NewPath

mv $SignedAPKPath $NewPath"/BrightSign.apk"
