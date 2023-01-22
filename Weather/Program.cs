//Стешкин Георгий, ИП-19-3 (Прогноз погоды)
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Weather;
using Weather.Weather4days;

Console.OutputEncoding = Encoding.UTF8;
var APIkey = "0c5866bb0a95c5760159e404f48499cc";
Console.Write("Привет! Введи название города и получи прогноз погоды на ближайшие 4 дня: ");
var City = Console.ReadLine();
var client = new HttpClient();
var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={City}&appid={APIkey}&lang=ru&units=metric");
var response4Days = await client.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?q={City}&appid={APIkey}&lang=ru&units=metric");

if (response.IsSuccessStatusCode)
{
    var result = await response.Content.ReadAsStringAsync();
    var model = JsonConvert.DeserializeObject<WeatherJson>(result);
    Console.WriteLine($"Прогноз погоды на {DateTime.Now} для города {model.name}:");
    Console.WriteLine($"Текущая температура {model.main.temp}°, {model.weather[0].description}, ощущается как {model.main.feels_like}°");
    Console.WriteLine($"Скорость ветра {model.wind.speed} м/с, {WeatherDegToString(model.wind.deg)}, влажность {model.main.humidity}%, давление {model.main.pressure} мм рт. ст.");
}
else
{
    Console.WriteLine("Сори, неправильный город");
}

if (response4Days.IsSuccessStatusCode)
{
    var result4Days = await response4Days.Content.ReadAsStringAsync();
    var model4Days = JsonConvert.DeserializeObject<Weather4days>(result4Days);
    Console.WriteLine($"Прогноз погоды на промежуток времени с {DateTime.Now.ToString("d")} до {DateTime.Now.AddDays(3).ToString("d")} включительно для города {model4Days.city.name}:");
    Console.WriteLine($"{WetherStrToDateToStr(model4Days.list[0].dt_txt)}: минимальная температура {model4Days.list[0].main.temp_min}°, максимальная температура {model4Days.list[0].main.temp_max}°, {model4Days.list[0].weather[0].description}");
    Console.WriteLine($"{WetherStrToDateToStr(model4Days.list[8].dt_txt)}: минимальная температура {model4Days.list[8].main.temp_min}°, максимальная температура {model4Days.list[8].main.temp_max}°, {model4Days.list[8].weather[0].description}");
    Console.WriteLine($"{WetherStrToDateToStr(model4Days.list[16].dt_txt)}: минимальная температура {model4Days.list[16].main.temp_min}°, максимальная температура {model4Days.list[16].main.temp_max}°, {model4Days.list[16].weather[0].description}");
    Console.WriteLine($"{WetherStrToDateToStr(model4Days.list[24].dt_txt)}: минимальная температура {model4Days.list[24].main.temp_min}°, максимальная температура {model4Days.list[24].main.temp_max}°, {model4Days.list[24].weather[0].description}");
    Console.ReadKey();
}

string WeatherDegToString(int WindDeg)
{
    string str = "";
    if (WindDeg <= 15 && WindDeg >= 345)
    {
        str = "С";
    }
    if (WindDeg > 15 && WindDeg < 75)
    {
        str = "СВ";
    }
    if (WindDeg >= 75 && WindDeg <= 105)
    {
        str = "В";
    }
    if (WindDeg > 105 && WindDeg < 165)
    {
        str = "ЮВ";
    }
    if (WindDeg >= 165 && WindDeg <= 195)
    {
        str = "Ю";
    }
    if (WindDeg > 195 && WindDeg < 255)
    {
        str = "ЮЗ";
    }
    if (WindDeg >= 255 && WindDeg <= 285)
    {
        str = "З";
    }
    if (WindDeg > 285 && WindDeg < 345)
    {
        str = "СЗ";
    }
    return str;
}

string WetherStrToDateToStr(string str1)
{
    DateTime myDate1 = DateTime.Parse(str1);
    string strDay = myDate1.ToString("dddd");
    string strDay1 = myDate1.ToString("d");
    return strDay1 + ", " + strDay;
}