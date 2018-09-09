using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Cakewalk.Exceptions;

namespace Cakewalk.IoC.Core {

    public class Container {

        public Dictionary<Type, Type> Registrations { get; private set; }
        public Dictionary<Type, object> Instances { get; private set; }
        private List<Type> dependencyChain;

        public Container() {
            Registrations = new Dictionary<Type, Type>();
            Instances = new Dictionary<Type, object>();
            dependencyChain = new List<Type>();
        }

        /// <summary>
        /// Register a type and its mapped interface for injection.
        /// </summary>
        /// <typeparam name="TInterface">Mapped interface for registered type.</typeparam>
        /// <typeparam name="TClass">Type to register.</typeparam>
        public void Register<TInterface, TClass>() where TClass : class, TInterface {
            ValidateRegistration<TInterface, TClass>();
            Registrations.Add(typeof(TInterface), typeof(TClass));
        }

        /// <summary>
        /// Register a Type for injection.
        /// </summary>
        /// <typeparam name="TClass">Type to register.</typeparam>
        public void Register<TClass>() where TClass : class {
            if(Registrations.Values.Contains(typeof(TClass))) {
                throw new RegistrationException(typeof(TClass).ToString() + "is already registered.");
            }

            Registrations.Add(typeof(TClass), typeof(TClass));
        }

        /// <summary>
        /// Get the fields marked with [Dependency] attribute and inject them.
        /// </summary>
        /// <param name="obj">Object to inject into.</param>
        public void InjectDependencies(object obj)
        {
            Type type = obj.GetType();

            FieldInfo[] dependencyFields = GetDependencyFields(type);

            for (int i = 0; i < dependencyFields.Length; i++)
            {
                object instance = Resolve(dependencyFields[i].FieldType);
                dependencyFields[i].SetValue(obj, instance);
            }
        }


        private FieldInfo[] GetDependencyFields(Type type)
        {
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(typeof(Dependency), false)).ToArray();

            return fieldInfo;
        }

        /// <summary>
        /// Check that type are valid for injection.
        /// </summary>
        private void ValidateRegistration<TInterface, TClass>() {
            if(!typeof(TInterface).IsInterface) {
                throw new RegistrationException(typeof(TInterface).ToString() + "is not an interface.");
            }
            if(typeof(TClass).IsAbstract) {
                throw new RegistrationException(typeof(TClass).ToString() + "is abstract and cannot be instantiated.");
            }
        }

        /// <summary>
        /// Returns an object of type T with all its constructor dependencies resolved. 
        /// Mainly used for testing. Avoid using in game code. 
        /// </summary>
        /// <typeparam name="T">Type to resolve.</typeparam>
        public object Resolve<T>() {
            Type type = typeof(T);
            return Resolve(type);
        }

        /// <summary>
        /// Returns an object of type with all its constructor dependencies resolved. 
        /// </summary>
        /// <param name="type">Type to resolve.</param>
        private object Resolve(Type type) {
            object instance;

            if(Instances.TryGetValue(type, out instance)) {
                return instance;
            }

            if(dependencyChain.Contains(type)) {
                throw new CircularDependencyException("Circular dependency in " + type.ToString());
            }
            dependencyChain.Add(type);

            Type implementation;
            if(!Registrations.TryGetValue(type, out implementation)) {
                throw new NullBindingException("Attempt to instanciate a null-binding. " + type.ToString() + " is not registered.");
            }

            ConstructorInfo constructor = GetConstructor(implementation);
            object[] dependencies = ResolvedDependencies(constructor);

            if(dependencies.Length < 1) {
                dependencyChain.Clear();
            }

            instance = constructor.Invoke(dependencies);
            Instances.Add(type, instance);

            return instance;
        }

        /// <summary>
        /// Get the non-default constructor with the most parameters for an object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ConstructorInfo GetConstructor(Type type) {
            ConstructorInfo[] constructors = type.GetConstructors();
            if(constructors.Length < 1) {
                throw new ResolveException(type.ToString() + " has no public constructors and cannot be instanciated.");
            }
            // Get constructor with the most parameters
            ConstructorInfo correctConstructor = constructors.Aggregate((c1, c2) => c1.GetParameters().Count() > c2.GetParameters().Count() ? c1 : c2);
            return correctConstructor;
        }

        /// <summary>
        /// Get the resolved constructor-parameters for a given constructor.
        /// </summary>
        /// <param name="constructorInfo">The constructor to get and resolve parameters for.</param>
        /// <returns></returns>
        private object[] ResolvedDependencies(ConstructorInfo constructorInfo) {
            List<object> parameters = new List<object>();

            ParameterInfo[] parameterInfo = constructorInfo.GetParameters();

            foreach(ParameterInfo parameter in parameterInfo) {
                if(parameter.ParameterType.IsValueType) {
                    throw new ResolveException("Parameter " + parameter.ToString() + " in " + constructorInfo.ReflectedType + " is a valuetype. This is not allowed in injected classes");
                }

                Type parameterType = parameter.ParameterType;
                object instance = Resolve(parameterType);
                parameters.Add(instance);
            }

            return parameters.ToArray();
        }

    }

}
