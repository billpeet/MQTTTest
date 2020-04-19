using System;
using System.Threading.Tasks;
using System.Linq;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using PropertyChanged;

namespace MQTTTest.WPF.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }

        public double CurrentValue { get; set; }

        public TimeSpan TimePeriod { get; set; } = TimeSpan.FromSeconds(10);

        [DependsOn(nameof(TimePeriod))]
        public double MinDate => GetTicks(DateTime.Now - TimePeriod);

        public Func<double, string> XFormatter => x => new DateTime((long)(x * TimeSpan.FromMinutes(1).Ticks)).ToString("hh:mm:sstt");
        public Func<double, string> YFormatter => y => Math.Round(y, 1) + "°";

        private double GetTicks(DateTime x) => (double)x.Ticks / TimeSpan.FromMinutes(1).Ticks;

        public ChartViewModel()
        {

            var dayConfig = Mappers.Xy<DataViewModel>()
                .X(m => GetTicks(m.DateTime))
                .Y(m => m.Value);

            SeriesCollection = new SeriesCollection(dayConfig)
            {
                new LineSeries
                {
                    Title = "Boiler Temperature",
                    Values = new ChartValues<DataViewModel>(),
                },
            };
        }

        public void AddDataPoint(DataViewModel vm)
        {
            SeriesCollection[0].Values.Add(vm);
            CurrentValue = vm.Value;
            RaisePropertyChanged(nameof(MinDate));
        }

        public void AddDataPoint(double value, DateTime? dateTime = null)
        {
            if (dateTime == null)
                dateTime = DateTime.Now;
            AddDataPoint(new DataViewModel(dateTime.Value, value));
        }

    }
}