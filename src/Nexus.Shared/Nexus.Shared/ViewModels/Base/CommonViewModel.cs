using Autofac;
using Nexus.Contracts.ViewModels.Base;
using Prism.Events;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nexus.Shared.ViewModels.Base {
    public abstract class CommonViewModel : ICommonViewModel {
        protected IRegionManager _regionManager;
        protected ILifetimeScope _scope;
        protected IEventAggregator _eventAggregator;

        private bool _isActive;
        public virtual bool IsActive {
            get { return _isActive; }
            set { _isActive = value; RaisePropertyChanged(); }
        }


        public CommonViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) {
            _scope = scope;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Navigation Implementation

        protected virtual void NavigateToView(string viewName, string regionName, NavigationParameters parameters = null) {
            IRegion region = _regionManager.Regions[regionName];

            string queryString = parameters == null ? String.Empty : parameters.ToString();

            region.RequestNavigate(new Uri(viewName + queryString, UriKind.Relative));
        }

        #endregion
    }
}
