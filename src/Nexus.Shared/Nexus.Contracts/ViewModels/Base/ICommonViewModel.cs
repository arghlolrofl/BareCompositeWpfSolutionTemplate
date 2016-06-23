using System.ComponentModel;

namespace Nexus.Contracts.ViewModels.Base {
    public interface ICommonViewModel : INotifyPropertyChanged {
        bool IsActive { get; set; }
    }
}
