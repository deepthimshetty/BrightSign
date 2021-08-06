using System;
using Android.Content;
using Android.Preferences;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;

namespace BrightSign.Droid.Utility.Interface
{
    public class AndroidUserPreferences : IUserPreferences
    {
        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext=Android.App.Application.Context;
        public AndroidUserPreferences()
        {
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }

        public bool GetBool(string key)
        {
            return mSharedPrefs.GetBoolean(key, false);
        }

        public int GetInt(string key)
        {
            return mSharedPrefs.GetInt(key, 0);
        }

        public string GetString(string key)
        {
            return mSharedPrefs.GetString(key, "");
        }

        public void SetBool(string key, bool value)
        {
            mPrefsEditor.PutBoolean(key,value);
            mPrefsEditor.Commit();
        }

        public void SetInt(string key, int value)
        {
            mPrefsEditor.PutInt(key, value);
            mPrefsEditor.Commit();
        }

        public void SetString(string key, string value)
        {
            mPrefsEditor.PutString(key, value);
            mPrefsEditor.Commit();
        }
    }
}
