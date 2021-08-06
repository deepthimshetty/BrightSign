using System;
using System.Windows.Input;

namespace BrightSign.Core.Utility.Interface
{
    public interface IModify
    {
        ICommand RemoveCommand { get; }
        ICommand EditCommand { get; }
        int Selectedtab { get; }
    }
}
