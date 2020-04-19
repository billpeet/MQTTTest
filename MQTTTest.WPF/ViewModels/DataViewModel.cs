using System;

namespace MQTTTest.WPF.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public DataViewModel()
        {
        }

        public DataViewModel(DateTime dateTime, double value)
        {
            DateTime = dateTime;
            Value = value;
        }

    }
}