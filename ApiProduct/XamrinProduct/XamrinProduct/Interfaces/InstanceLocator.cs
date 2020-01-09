namespace XamrinProduct.ViewModels
{
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using ViewModels;
    public class InstanceLocator
    {
        #region Properties
        public MainViewModel Main { get; set; }
        #endregion        

        #region Constructors
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
        #endregion
    }
}
