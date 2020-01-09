

using System.Collections.Generic;
using System.Text;



namespace XamrinProduct.ViewModels
{
    using GalaSoft.MvvmLight.Command;

    using System;

    using System.Windows.Input;

    using XamrinProduct.Services;

    using Xamarin.Forms;

    using XamrinProduct.Models;

    using XamrinProduct.Views;


    public class LoginViewModel:BaseViewModel
    {
        #region Attributes
        string email;
        string password;
        bool isrunning;
        bool isenabled;
        ApiService apiService;
        #endregion

        #region Properties
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                SetValue(ref this.email, value);
            }
        }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                SetValue(ref this.password, value);
            }
        }
        public bool IsRunning
        {
            get
            {
                return this.isrunning;
            }
            set
            {
                SetValue(ref this.isrunning, value);
            }
        }
        public bool IsEnabled
        {
            get
            {
                return this.isrunning;
            }
            set
            {
                SetValue(ref this.isrunning, value);
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(cmdLogin);
            }
        }

        private async void cmdLogin()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Email empty",
                                            "Enter a Valid E-mail",
                                            "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Password empty",
                                            "Enter a Valid Password",
                                            "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var conexion = await this.apiService.CheckConnection();
            if (!conexion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("ERROR",
                                                conexion.Message,
                                                "Accept");
                return;
            }

            TokenResponse token = await this.apiService.GetToken(
                   "https://localhost:44356",
                   this.Email,
                   this.Password);
            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   "Something was wrong, please try later.",
                   "Accept");
                return;
            }

            if(string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   token.ErrorDescription,
                   "Accept");
                this.Password = String.Empty;
                return;
            }

            MainViewModel mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;

            Application.Current.MainPage = new NavigationPage(new ProductPage());
            IsRunning = false;
            IsEnabled = true;
        }
        #endregion


    }
}
