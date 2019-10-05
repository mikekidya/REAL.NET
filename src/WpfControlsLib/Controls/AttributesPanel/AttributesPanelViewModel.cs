﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLib.ViewModel;

namespace WpfControlsLib.Controls.AttributesPanel
{
    public class AttributesPanelViewModel : INotifyPropertyChanged
    {
        public IList<AttributeViewModel> Attributes
        {
            get => attributes;
            set
            {
                attributes = value;
                OnPropertyChanged("Attributes");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IList<AttributeViewModel> attributes;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
