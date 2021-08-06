#!/bin/bash

cd ..

# Change the Path to the Solution Project folder
SLNPath=$PWD"/"
echo $SLNPath

# New Path to save the Builds
NewPath=$SLNPath"Builds"
echo $NewPath

cd ..
SLNPath=$PWD"/"
echo $SLNPath

# Navigate to the main SLN Folder
cd $SLNPath

VersionNumber=$1 
BuildNumber=$2

# Build the Project in release mode
MSBuild /p:Configuration="Release" /p:Platform="iPhone" /p:IpaPackageDir="$NewPath" /t:Clean BrightSign.sln
MSBuild /p:Configuration="Release" /p:Platform="iPhone" /p:SetVersion=True /p:VersionNumber=$VersionNumber /p:BuildNumber=$BuildNumber /p:IpaPackageDir="$NewPath" /t:Build BrightSign.sln
