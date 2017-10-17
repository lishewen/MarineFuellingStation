using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 打印终端.Models
{
    public class Log
    {
        static Log()
        {
            _logs = "程序开始运行\n";
        }
        private static string _logs;
        public static string Logs
        {
            get { return _logs; }
            set
            {
                _logs = value;
                OnPropertyChanged("Logs");
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        static void OnPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}
