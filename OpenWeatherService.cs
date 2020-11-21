using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DevExpress.Demos.OpenWeatherService {
    public class CityWeather {
        OpenWeatherMapService.CityCurrentWeatherInfo cityWeatherInfo;
        List<CityWeather> forecast;
        Weather weather;
        List<WeatherDescription> weatherDescriptions;

        public DateTime CurrentDateTime { get { return GetTime(cityWeatherInfo.dt); } }
        public int CityID { get { return cityWeatherInfo.id; } }
        public string City { get { return cityWeatherInfo.name; } }
        public double Longitude { get { return cityWeatherInfo.coord.Lon; } }
        public double Latitude { get { return cityWeatherInfo.coord.Lat; } }
        public List<CityWeather> Forecast { get { return forecast; } }
        public Weather Weather { get { return weather; } }
        public string CelsiusDisplayText { get { return City + "\n" + weather.CelsiusTemperatureString; } }
        public string KelvinDisplayText { get { return City + "\n" + weather.KelvinTemperatureString; } }
        public string FahrenheitDisplayText { get { return City + "\n" + weather.FahrenheitTemperatureString; } }
        public List<WeatherDescription> WeatherDescriptions { get { return weatherDescriptions; } }
        public string WeatherIconPath { get; set; }
        public DateTime ForecastTime { get; set; }

        public event EventHandler ForecastUpdated;

        public CityWeather(OpenWeatherMapService.CityCurrentWeatherInfo cityWeatherInfo) {
            this.cityWeatherInfo = cityWeatherInfo;
            this.weather = new Weather(cityWeatherInfo.main);
            this.weatherDescriptions = new List<WeatherDescription>();
            foreach(OpenWeatherMapService.WeatherDescriptionInfo weatherDescription in cityWeatherInfo.weather)
                weatherDescriptions.Add(new WeatherDescription(weatherDescription));
        }
        public CityWeather(OpenWeatherMapService.ForecastInfo forecast, int index) {
            OpenWeatherMapService.CityForecastWeatherInfo info = forecast.list[index];
            OpenWeatherMapService.CityInfo cityInfo = forecast.city;
            this.cityWeatherInfo = new OpenWeatherMapService.CityCurrentWeatherInfo() {
                clouds = info.clouds, dt = info.dt, main = info.main, weather = info.weather, wind = info.wind, coord = cityInfo.coord, id = cityInfo.id, name = cityInfo.name
            };
            this.weather = new Weather(cityWeatherInfo.main);
            this.weatherDescriptions = new List<WeatherDescription>();
            foreach(OpenWeatherMapService.WeatherDescriptionInfo weatherDescription in cityWeatherInfo.weather)
                weatherDescriptions.Add(new WeatherDescription(weatherDescription));
        }

        DateTime GetTime(long seconds) {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dtDateTime.AddSeconds(seconds).ToLocalTime();
        }
        internal void SetForecast(OpenWeatherMapService.ForecastInfo forecast) {
            if(forecast == null) 
                return;
            List<CityWeather> cityWeatherList = new List<CityWeather>();
            for(int i = 0; i < forecast.list.Count; i++)
                cityWeatherList.Add(new CityWeather(forecast, i));

            this.forecast = cityWeatherList;
            if(ForecastUpdated != null) 
                ForecastUpdated(this, EventArgs.Empty);
        }
        public string GetTemperatureDataMember(TemperatureMeasureUnits measureUnits) {
            switch(measureUnits) {
                case TemperatureMeasureUnits.Fahrenheit:
                    return "FahrenheitDisplayText";
                case TemperatureMeasureUnits.Kelvin:
                    return "KelvinDisplayText";
                default:
                    return "CelsiusDisplayText";
            }
        }
    }

    public enum TemperatureMeasureUnits {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    public class Weather {
        OpenWeatherMapService.WeatherInfo weatherInfo;

        public int CelsiusTemperature { get { return (int)weatherInfo.temp; } }
        public int FahrenheitTemperature { get { return (int)weatherInfo.temp * 9 / 5 + 32; } }
        public int KelvinTemperature { get { return (int)(weatherInfo.temp + 273.15); } }
        public string CelsiusTemperatureString { get { return CelsiusTemperature.ToString("+#;-#;0") + " °C"; } }
        public string FahrenheitTemperatureString { get { return FahrenheitTemperature.ToString("+#;-#;0") + " °F"; } }
        public string KelvinTemperatureString { get { return KelvinTemperature.ToString("+#;-#;0") + " °K"; } }

        public Weather(OpenWeatherMapService.WeatherInfo weatherInfo) {
            this.weatherInfo = weatherInfo;
        }
        public string GetTemperatureString(TemperatureMeasureUnits measureUnits) {
            switch(measureUnits) {
                case TemperatureMeasureUnits.Fahrenheit:
                    return FahrenheitTemperatureString;
                case TemperatureMeasureUnits.Kelvin:
                    return KelvinTemperatureString;
                default:
                    return CelsiusTemperatureString;
            }
        }
    }
    public class WeatherDescription {
        OpenWeatherMapService.WeatherDescriptionInfo weatherDescriptionInfo;

        public string IconName { get { return weatherDescriptionInfo.icon; } }

        public WeatherDescription(OpenWeatherMapService.WeatherDescriptionInfo weatherDescriptionInfo) {
            this.weatherDescriptionInfo = weatherDescriptionInfo;
        }
    }
    public class OpenWeatherMapService: IDisposable {
        #region classes for JSON parsing

        [DataContract]
        public class ForecastInfo {
            [DataMember]
            public CityInfo city = null;
            [DataMember]
            public List<CityForecastWeatherInfo> list;
        }
        [DataContract]
        public class CityInfo {
            [DataMember]
            internal int id = 0;
            [DataMember]
            internal string name = "";
            [DataMember]
            internal Coordinates coord = null;
        }
        [DataContract]
        public class CityForecastWeatherInfo {
            [DataMember]
            internal long dt = 0;
            [DataMember]
            internal WeatherInfo main = null;
            [DataMember]
            internal List<WeatherDescriptionInfo> weather = null;
            [DataMember]
            internal CloudsInfo clouds = null;
            [DataMember]
            internal WindInfo wind = null;
        }
        [DataContract]
        public class WeatherDescriptionInfo {
            [DataMember]
            internal string main = null;
            [DataMember]
            internal string description = null;
            [DataMember]
            internal string icon = null;
        }
        [DataContract]
        public class CloudsInfo {
            [DataMember]
            internal int all = 0;
        }
        [DataContract]
        public class WindInfo {
            [DataMember]
            internal double speed = 0.0;
            [DataMember]
            internal double deg = 0.0;
        }
        [DataContract]
        public class WeatherInfo {
            [DataMember]
            internal double temp = 0.0;
            [DataMember]
            internal double pressure = 0.0;
            [DataMember]
            internal double humidity = 0.0;
        }
        [DataContract]
        public class Coordinates {
            [DataMember]
            internal double Lon = 0.0;
            [DataMember]
            internal double Lat = 0.0;
        }
        [DataContract]
        public class WorldWeatherInfo {
            [DataMember]
            public List<CityCurrentWeatherInfo> list = null;
        }
        [DataContract]
        public class CityCurrentWeatherInfo {
            [DataMember]
            internal int id = 0;
            [DataMember]
            internal string name = "";
            [DataMember]
            internal Coordinates coord = null;
            [DataMember]
            internal WeatherInfo main = null;
            [DataMember]
            internal long dt = 0;
            [DataMember]
            internal WindInfo wind = null;
            [DataMember]
            internal CloudsInfo clouds = null;
            [DataMember]
            internal List<WeatherDescriptionInfo> weather = null;
        }

        #endregion

        const string OpenWeatherUrl = "http://api.openweathermap.org/data/2.5/box/city?bbox=-180,-90,180,90&cluster=yes&APPID=";
        const string OpenWeatherIconPathPrefix = "http://openweathermap.org/img/w/";

        const string OpenWeatherKey = "fcbff6dbed7bd7f295489daf4ffef3f1";

        static readonly object forecastLocker = new object();

        List<CityWeather> weatherInCities;
        List<string> capitalNames;
        bool disposed = false;
        
        public CityWeather LosAngelesWeather { get; set; }
        public List<CityWeather> WeatherInCities { get { return weatherInCities; } }

        public event EventHandler ReadCompleted;

        public OpenWeatherMapService(List<string> capitalNames) {
            this.capitalNames = capitalNames;
        }
        ~OpenWeatherMapService() {
            Dispose(false);
        }

        void weatherClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            WebClient weatherClient = sender as WebClient;
            weatherClient.OpenReadCompleted -= weatherClient_OpenReadCompleted;

            if(e.Cancelled || e.Error != null)
                return;

            Stream stream = e.Result;
            Task.Factory.StartNew(() => {
                DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(WorldWeatherInfo));
                WorldWeatherInfo worldWeatherInfo = (WorldWeatherInfo)dc.ReadObject(stream);
                List<CityWeather> citiesWeather = new List<CityWeather>();
                foreach(CityCurrentWeatherInfo weatherInfo in worldWeatherInfo.list) {
                    CityWeather cityWeather = new CityWeather(weatherInfo);
                    if(cityWeather.City == "Los Angeles")
                        LosAngelesWeather = cityWeather;
                    if(cityWeather.WeatherDescriptions != null && cityWeather.WeatherDescriptions.Count > 0)
                        cityWeather.WeatherIconPath = OpenWeatherIconPathPrefix + cityWeather.WeatherDescriptions[0].IconName + ".png";

                    string cityWithId = string.Format("{0};{1}", cityWeather.City, cityWeather.CityID);
                    if(capitalNames.Contains(cityWeather.City) || capitalNames.Contains(cityWithId))
                        citiesWeather.Add(cityWeather);
                }
                weatherInCities = citiesWeather;
                RaiseReadComplete();
            });
        }
        WebClient forecastClient = new WebClient();    
        void forecastClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if(e.Error == null) {
                forecastClient.OpenReadCompleted -= forecastClient_OpenReadCompleted;
                Stream stream = e.Result;
                CityWeather cityWeatherInfo = (CityWeather)e.UserState;
                Task.Factory.StartNew(() => {
                    lock(forecastLocker) {
                        ForecastInfo forecast = ReadForecast(stream);
                        cityWeatherInfo.SetForecast(forecast);
                    }
                });
            }
        }
        ForecastInfo ReadForecast(Stream stream) {
            ForecastInfo forecast = null;
            try {
                DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(ForecastInfo));
                forecast = (ForecastInfo)dc.ReadObject(stream);
            }
            catch{
                forecast = null;
            }
            return forecast;
        }
        void RaiseReadComplete() {
            if(ReadCompleted != null) ReadCompleted(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                if (forecastClient != null) { 
                    forecastClient.OpenReadCompleted -= forecastClient_OpenReadCompleted;
                    forecastClient.Dispose();
                    forecastClient = null;
                }
            }
            disposed = true;
        }

        public void GetWeatherAsync() {
            WebClient weatherClient = new WebClient();
            weatherClient.OpenReadCompleted += weatherClient_OpenReadCompleted;
            weatherClient.OpenReadAsync(new Uri(OpenWeatherUrl + OpenWeatherKey));
        }
        public void GetForecastForCityAsync(CityWeather cityWeather) {
            string link = string.Format("http://api.openweathermap.org/data/2.5/forecast?units=metric&id={0}&APPID={1}", cityWeather.CityID.ToString(), OpenWeatherKey);
            if (forecastClient.IsBusy)
                return;
            forecastClient.OpenReadCompleted += forecastClient_OpenReadCompleted;
            forecastClient.OpenReadAsync(new Uri(link), cityWeather);
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
