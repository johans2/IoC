using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using CakewalkIoC.Exceptions;
using CakewalkIoC.Injection;
using UnityEngine;

namespace CakewalkIoC.Core {

    public class Container {
        
        public Dictionary<Type, Type> Registrations { get; private set; }
        public Dictionary<Type, GameObject> PrefabRegistrations { get; private set; }
        public Dictionary<Type, object> Instances { get; private set; }
        
        private List<Type> dependencyChain;

        public Container() {
            Registrations = new Dictionary<Type, Type>();
            Instances = new Dictionary<Type, object>();
            PrefabRegistrations = new Dictionary<Type, GameObject>();
            dependencyChain = new List<Type>();
        }

        public void RegisterPrefab(MonoBehaviour mb) {

            var prefabNameType = Assembly.GetAssembly(typeof(TestBootStrapper)).GetType(mb.name);
            

            var registerMethod = GetType().GetMethod("RegisterPrefabWithBehaviour", BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = registerMethod.MakeGenericMethod(prefabNameType);
            generic.Invoke(this, new object[] { mb.gameObject });


        }

        /// <summary>
        /// Register a MonoBehaviour and its specified prefab.
        /// </summary>
        /// <typeparam name="TBehaviour">The specified MonoBehaviour available for injection.</typeparam>
        /// <param name="prefab">The specified prefab the that will be instantiated with the MonoBehaviour attached to it.</param>
        private void RegisterPrefabWithBehaviour<TBehaviour>(GameObject prefab) where TBehaviour : MonoBehaviour {
            ValidatePrefabDependencies<TBehaviour>(prefab);
            PrefabRegistrations.Add(typeof(TBehaviour), prefab);
        }

        /*
        /// <summary>
        /// Register a MonoBehaviour, its interface and its specified prefab.
        /// </summary>
        /// <typeparam name="TInterface">The specified Interface available for injection.</typeparam>
        /// <typeparam name="TBehaviour">The MonoBehaviour implementation of the interface.</typeparam>
        /// <param name="prefab">The specified prefab the that will be instantiated with the MonoBehaviour attached to it.</param>
        public void RegisterPrefab<TInterface , TBehaviour>(GameObject prefab) where TBehaviour : MonoBehaviour, TInterface {
            ValidatePrefabDependencies<TBehaviour>(prefab);
            PrefabRegistrations.Add(typeof(TInterface), prefab);
        }
        */
        private void ValidatePrefabDependencies<TBehaviour>(GameObject prefab) where TBehaviour : Component {
            if(prefab == null) {
                throw new GameObjectRegistrationException(string.Format("Registered dependency prefab {0} is null." , prefab));
            }
            
            Component attachedBehaviour = prefab.GetComponent<TBehaviour>();
            
            if(attachedBehaviour == null) {
                throw new GameObjectRegistrationException(string.Format("Prefab {0} has no behaviour of type {1} attached to it.", prefab.name, typeof(TBehaviour).ToString()));
            }

            CheckCircularDependencies(typeof(TBehaviour));

            if(PrefabRegistrations.ContainsKey(typeof(TBehaviour))) {
                throw new GameObjectRegistrationException(string.Format("Behaviour {0} (mapped to {1}) is already mapped to prefab {2}", typeof(TBehaviour).ToString(), prefab.name, PrefabRegistrations[typeof(TBehaviour)].name));
            }
        }

        /// <summary>
        /// Check that the given Type has no circular dependencies. Throws CircularDependencyException() if it has.
        /// </summary>
        /// <param name="type">The Type to be checked.</param>
        public void CheckCircularDependencies(Type type) {
            PropertyInfo[] properties = GetDependencyProperties(type);
            
            for(int i = 0; i < properties.Length; i++) {
                
                Type dependency = properties[i].PropertyType;

                if(dependencyChain.Contains(dependency)) {
                    throw new CircularDependencyException("Circular dependency in " + type.ToString());
                }
                
                dependencyChain.Add(dependency);

                CheckCircularDependencies(dependency);

            }

            dependencyChain.Clear();
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
        /// Get the properties marked with [Dependency] attribute and inject them.
        /// </summary>
        /// <param name="obj">Object to inject into.</param>
        public void InjectDependencies(object obj) {
            Type type = obj.GetType();

            PropertyInfo[] properties = GetDependencyProperties(type);

            foreach(PropertyInfo property in properties) {
                object instance = Resolve(property.PropertyType);
                property.SetValue(obj, instance, null);
            }
        }

        private PropertyInfo[] GetDependencyProperties(Type type) {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(typeof(Dependency), false)).ToArray();

            return properties;
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
            return Resolve(typeof(T));
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

            if(type.IsSubclassOf(typeof(MonoBehaviour))) {
                // Instantiate the prefab
                GameObject prefab;
                if(!PrefabRegistrations.TryGetValue(type, out prefab)) {
                    throw new NullBindingException("Attempt to instanciate a null-binding. " + type.ToString() + " is not registered to a prefab.");
                }
                
                GameObject gameObject = GameObject.Instantiate(prefab);
                GameObject.DontDestroyOnLoad(gameObject);
                instance = gameObject.GetComponent(type);

                if(GetDependencyProperties(type).Length == 0) {
                    dependencyChain.Clear();
                }
                
            }
            else {
                // Instantiate the object
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
            }
            
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
