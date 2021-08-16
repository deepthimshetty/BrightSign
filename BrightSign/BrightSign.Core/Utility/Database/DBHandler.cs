using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BrightSign.Core.Models;
using BrightSign.Core.Utility.Interface;
using MvvmCross;
using SQLite;

namespace BrightSign.Core.Utility.Database
{
    public class DBHandler
    {
        private readonly SQLiteConnection database;

        static DBHandler instance;
        public static DBHandler Instance => instance ?? (instance = new DBHandler());

        public DBHandler()
        {
            if (database == null)
            {
                database = Mvx.Resolve<ISQLite>().GetConnection();
                try
                {
                    database.CreateTable<BSDevice>();
                    database.CreateTable<BSUdpAction>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public List<BSDevice> GetDevicefromDB()
        {

            return ((from t in database.Table<BSDevice>() select t).ToList());
        }
        public void InsertorReplaceData(List<BSDevice> devices)
        {
            //var status = database.InsertOrReplaceAll(devices);
            foreach (var device in devices)
            {
                InsertorReplaceDevice(device);
            }
        }

        public void RemoveDevice(BSDevice device)
        {
            var status = database.Delete(device);
        }

        public void InsertorReplaceDevice(BSDevice device)
        {
            var status = database.InsertOrReplace(device);
        }

        public void RemoveAllDevicesFromDB()
        {
            var staus = database.DeleteAll<BSDevice>();
        }

        public List<BSUdpAction> GetActionsfromDB()
        {
            return ((from t in database.Table<BSUdpAction>() select t).ToList());
        }
        public void InsertorReplaceAllActions(List<BSUdpAction> devices)
        {
            if (devices != null)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    BSUdpAction action = devices[i];
                    var Sno = database.Query<BSUdpAction>("select * from BSUdpAction where Label=? and DataUDP=? and PresentationLabel=?", action.Label, action.DataUDP, Constants.ActivePresentation);

                    if (Sno.Count == 0)
                    {
                        database.Insert(action);
                    }
                    else
                    {
                        database.Update(action);
                    }

                }
            }
        }

        public void UpdateSno(List<BSUdpAction> devices){
            var status = database.UpdateAll(devices);
        }

        public void InsertAllActions(List<BSUdpAction> devices)
        {
            var status = database.InsertAll(devices);
        }

        public void InsertAction(BSUdpAction devices)
        {
            var status = database.Insert(devices);
        }

        public BSUdpAction GetActionWithName(String name)
        {
            var list = database.Query<BSUdpAction>("select * from BSUdpAction where Label=?", name);
            if (list != null & list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public BSUdpAction GetActionInstanceByNameLabel(String command){
            var list = database.Query<BSUdpAction>("select * from BSUdpAction where DataUDP=? and PresentationLabel=?", command, Constants.ActivePresentation);
            if (list != null & list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public int GetActionByNameAndlabel(String name, String command){
            var Sno = database.Query<BSUdpAction>("select * from BSUdpAction where Label=? and DataUDP=? and PresentationLabel=?", name, command, Constants.ActivePresentation);

            if (Sno.Count == 0)
            {
                return 1000;
            }
            else {
                return Sno[0].Sno; 
            } 
        }

        public void RemoveAction(BSUdpAction device)
        {
            var status = database.Delete(device);
        }

        public void InsertorReplaceAction(BSUdpAction device)
        {
            var status = database.InsertOrReplace(device);
        }

        public void RemoveAllActionsFromDB()
        {
            var staus = database.DeleteAll<BSUdpAction>();
        }

        public void RemoveDefaultActionsFromDB(string[] commands){
            try
            {
                database.Query<BSUdpAction>("Delete from BSUdpAction where IsUserDefined=? and DataUDP NOT IN ? and PresentationLabel=?", false, commands, Constants.ActivePresentation);
            }
            catch(Exception ex){
            }

          }

        public void RemoveDefaultActionsFromDB()
        {
            var actions = database.Table<BSUdpAction>().Where(o => !o.IsUserDefined);
            foreach (var item in actions)
            {
                var status = database.Delete(item);
            }

        }

        internal ObservableCollection<BSUdpAction> GetActionsfromDBforPresentation(string activePresentation, bool IsUserDefined = true)
        {
            var list = database.Query<BSUdpAction>("select * from BSUdpAction where PresentationLabel=? and IsUserDefined=?", activePresentation, IsUserDefined);
            if (list != null & list.Count > 0)
            {
                return new ObservableCollection<BSUdpAction>(list.OrderBy(o => o.Sno));
            }

            return new ObservableCollection<BSUdpAction>();
        }
    }
}
