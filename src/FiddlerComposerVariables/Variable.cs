using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiddlerComposerVariables
{
    public class Variable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _token;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                RaisePropertyChanged("Token");
            }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        private void RaisePropertyChanged(string field)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
    }
}
