#region

using System;
using System.Text;
using Light.Native;

#endregion

namespace Light.Infrastructure
{
    /// <summary>
    /// Класс содержит методы для  работы с .ini файлами 
    /// </summary>
    public static class INIManager
    {
        private const string Path = ".\\config.ini";
        private const int MaxStringSize = 1024;

        /// <summary>
        /// Метод конвертирует строку из .ini файла
        /// </summary>
        /// <returns>Значение приведенное из строки к указанному типу</returns>
        public static T GetValue<T>(string section, string key, string defaultValue) where T : IConvertible
        {
            StringBuilder buffer = new(MaxStringSize);
            Kernel32.GetPrivateProfileString(section, key, defaultValue, buffer, MaxStringSize, Path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
        }

        /// <summary>
        /// Метод записывает данные в строку используя WinApi
        /// </summary>
        public static void WriteValue(string section, string key, string value) => Kernel32.WritePrivateProfileString(section, key, value, Path);
    }
}