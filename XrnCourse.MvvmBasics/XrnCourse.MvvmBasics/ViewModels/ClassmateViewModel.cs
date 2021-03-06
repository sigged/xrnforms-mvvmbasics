﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XrnCourse.MvvmBasics.Constants;
using XrnCourse.MvvmBasics.Domain.Models;
using XrnCourse.MvvmBasics.Domain.Services;

namespace XrnCourse.MvvmBasics.ViewModels
{
    public class ClassmateViewModel : INotifyPropertyChanged
    {
        private IClassmateService classmateService;
        private Classmate currentClassmate;
        private INavigation navigation;

        public ClassmateViewModel(Classmate classmate, 
            INavigation navigation, 
            IClassmateService cmService)
        {
            this.navigation = navigation;
            this.currentClassmate = classmate;
            classmateService = cmService;
            //initialize the properties with the given classmate;
            this.Name = currentClassmate.Name;
            this.Phone = currentClassmate.Phone;
            this.Birthdate = currentClassmate.Birthdate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event if it has handlers attached to it
        /// </summary>
        /// <param name="propertyName">name of the prop that was changed</param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string name;
        public string Name
        {
            get { return name; }
            set {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }


        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                RaisePropertyChanged(nameof(Phone));
            }
        }

        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
                RaisePropertyChanged(nameof(Birthdate));
                RaisePropertyChanged(nameof(Age));
            }
        }

        public int Age
        {
            get {
                int age = DateTime.Now.Year - birthdate.Year;
                if (birthdate > DateTime.Now.AddYears(-age)) age--; //adjust for leap year
                return age;
            }
        }

        public ICommand SaveCommand => new Command(
            async () => {
                currentClassmate.Name = Name;
                currentClassmate.Phone = Phone;
                currentClassmate.Birthdate = Birthdate;
                await classmateService.Save(currentClassmate);
                //publish message
                MessagingCenter.Send<ClassmateViewModel, Classmate>(this, 
                    MessageNames.ClassmateSaved, currentClassmate);
            });
    }
}
