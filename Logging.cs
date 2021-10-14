using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Light
{
    public class Logging
    {
        private const string FilePath = ".\\";

        //string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        //    if (!Directory.Exists(pathToLog))
        //Directory.CreateDirectory(pathToLog);

        private static readonly object Sync = new();
        public static void Write(string logMessage, Exception ex = null, [CallerMemberName] string callerName = null)
        {
            var filename = Path.Combine(FilePath, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
            var exText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] " + $"Caller: {callerName}\n" +
                         $"[{logMessage}]\n";
            if (ex != null)
            {
                exText += $"[{ex.TargetSite.DeclaringType}.{ex.TargetSite.Name}()] " +
                          $"{ex.Message}]\n" +
                          $"[StackTrace]\n{ex.StackTrace}";
            }

            exText += "\r\n\n";

            lock (Sync)
            {
#if DEBUG
                Console.WriteLine($"[{callerName}] {exText}");
#endif
                File.AppendAllText(filename, exText, Encoding.GetEncoding("Windows-1251"));
            }
        }
    }

    public sealed class IoC
    {
        private readonly IDictionary<Type, RegisteredObject> _registeredObjects = new Dictionary<Type, RegisteredObject>();
        public void Register<TType>() where TType : class => Register<TType, TType>(false, null);
        public void Register<TType, TConcrete>() where TConcrete : class, TType => Register<TType, TConcrete>(false, null);
        public void RegisterSingleton<TType>() where TType : class => RegisterSingleton<TType, TType>();
        public void RegisterSingleton<TType, TConcrete>() where TConcrete : class, TType => Register<TType, TConcrete>(true, null);
        public void RegisterInstance<TType>(TType instance) where TType : class => RegisterInstance<TType, TType>(instance);
        public void RegisterInstance<TType, TConcrete>(TConcrete instance) where TConcrete : class, TType => Register<TType, TConcrete>(true, instance);
        public TTypeToResolve Resolve<TTypeToResolve>() => (TTypeToResolve)ResolveObject(typeof(TTypeToResolve));
        public object Resolve(Type type) => ResolveObject(type);

        private void Register<TType, TConcrete>(bool isSingleton, TConcrete instance)
        {
            var type = typeof(TType);
            if (!_registeredObjects.ContainsKey(type)) return;

            _registeredObjects.Remove(type); _registeredObjects.Add(type, new RegisteredObject(typeof(TConcrete), isSingleton, instance));
        }
        private object ResolveObject(Type type)
        {
            var registeredObject = _registeredObjects[type];
            if (registeredObject == null)
            {
                throw new ArgumentOutOfRangeException($"The type {type.Name} has not been registered");
            }
            return GetInstance(registeredObject);
        }
        private object GetInstance(RegisteredObject registeredObject)
        {
            var instance = registeredObject.SingletonInstance;
            if (instance != null) return instance;

            var parameters = ResolveConstructorParameters(registeredObject);
            instance = registeredObject.CreateInstance(parameters.ToArray());

            return instance;
        }
        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
        {
            var constructorInfo = registeredObject.ConcreteType.GetConstructors().First();
            return constructorInfo.GetParameters().Select(parameter => ResolveObject(parameter.ParameterType));
        }
        private class RegisteredObject
        {
            private readonly bool _isSingleton;
            public RegisteredObject(Type concreteType, bool isSingleton, object instance)
            {
                _isSingleton = isSingleton;
                ConcreteType = concreteType;
                SingletonInstance = instance;
            }
            public Type ConcreteType { get; private set; }
            public object SingletonInstance { get; private set; }
            public object CreateInstance(params object[] args)
            {
                var instance = Activator.CreateInstance(ConcreteType, args);
                if (_isSingleton)
                {
                    SingletonInstance = instance;
                }
                return instance;
            }
        }
        private static readonly Lazy<IoC> _instance = new(() => new IoC());
        public static IoC Instance => _instance.Value;
        private IoC() {}
    }
}