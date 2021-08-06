#!/bin/bash

VersionNumber="2.0.1"
BuildNumber="68"

# notes - optional, release notes as Textile or Markdown (after 5k characters note are truncated)
NOTES="
MOB-5 Fixed
"

Platform=$1

ReleaseAndroid=false
ReleaseiOS=false

if [ -z "${Platform}" ]; then
		ReleaseAndroid=true
        ReleaseiOS=true
	fi
if [ "$Platform" = "android" ] ; then
    ReleaseAndroid=true
    ReleaseiOS=false
fi

if [ "$Platform" = "ios" ] ; then
    ReleaseAndroid=false
    ReleaseiOS=true
fi
# Put your HockeyApp APP_TOKEN here. Find it in your HockeyApp account settings.
APP_TOKEN="12de7ad6f1c44da3a8452c3eb40b4179"

chmod 777 GenerateAPK.sh
chmod 777 GenerateIPA.sh
chmod 777 UploadFile.sh

cd ..

# Change the Path to the Solution Project folder
SLNPath=$PWD"/"
echo $SLNPath

cd GenerateBuildsFile

# Generate Android APK File
if [ "$ReleaseAndroid" = true ] ; then
    echo 'Generation APK file'
./GenerateAPK.sh $VersionNumber $BuildNumber
fi

#Generate IOS IPA File
if [ "$ReleaseiOS" = true ] ; then  
    echo 'Generation IPA file'  
./GenerateIPA.sh $VersionNumber $BuildNumber
fi

# New Path to save the Builds
NewPath=$SLNPath"Builds"
# echo $NewPath

BuildsPath=$SLNPath"GenerateBuildsFile"
# echo $BuildsPath
cd $BuildsPath


# optional, notify testers (can only be set with full-access tokens):
# 0 to not notify testers
# 1 to notify all testers that can install this app
# 2 - Notify all testers
NOTIFY="0"

# Path to the APK File
# ipa - required, file data of the .ipa for iOS, .app.zip for OS X, or .apk file for Android
IPA=$NewPath"/BrightSign.apk"
# echo $IPA

#Upload the Android APK file to Hockey APP
if [ "$ReleaseAndroid" = true ] ; then 
    echo 'Uploading APK file'
./UploadFile.sh $APP_TOKEN $IPA "$NOTES" $NOTIFY
fi

# Path to the IPA File
# ipa - required, file data of the .ipa for iOS, .app.zip for OS X, or .apk file for Android
IPA=$NewPath"/BrightSign.iOS.ipa"
# echo $IPA

#Upload the IOS file to Hockey APP
if [ "$ReleaseiOS" = true ] ; then  
    echo 'Uploading IPA file'
./UploadFile.sh $APP_TOKEN $IPA "$NOTES" $NOTIFY
fi









