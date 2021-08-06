using System;
namespace BrightSign.Core.Utility.Interface
{
    public interface IUserPreferences
    {
        string GetString(string key);

        void SetString(string key, string value);

        bool GetBool(string key);

        void SetBool(string key, bool value);

        int GetInt(string key);

        void SetInt(string key, int value);
    }
}
