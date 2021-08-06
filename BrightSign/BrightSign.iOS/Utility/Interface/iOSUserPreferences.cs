using System;
using BrightSign.Core.Utility.Interface;
using Foundation;

namespace BrightSign.iOS.Utility.Interface
{
    public class iOSUserPreferences : IUserPreferences
    {
        public void ClearAllValues()
        {
            NSUserDefaults.ResetStandardUserDefaults();
        }

        public bool GetBool(string key)
        {
            return NSUserDefaults.StandardUserDefaults.BoolForKey(key);
        }

        public int GetInt(string key)
        {
            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(key);
        }

        public long GetLong(string key)
        {
            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(key);
        }

        public string GetString(string key)
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(key);
        }

        public void SetBool(string key, bool value)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(value, key);
        }

        public void SetInt(string key, int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, key);
        }

        public void SetLong(string key, long value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt((int)value, key);
        }

        public void SetString(string key, string value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value, key);
        }
    }
}
