using System;
using System.Windows.Input;

namespace BrightSign.Core.Utility.Interface
{
    public interface IRemove
    {
        ICommand RemoveCommand { get; }
        int Selectedtab { get; }
    }
}
