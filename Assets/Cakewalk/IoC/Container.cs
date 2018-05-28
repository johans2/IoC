using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Cakewalk.IoC.Exceptions;
using Cakewalk.IoC;
using UnityEngine;

namespace Cakewalk.IoC.Core {

    public class Container {
        
        public Dictionary<Type, GameObject> PrefabRegistrations { get; private set; }
        public Dictionary<Type, object> Instances { get; private set; }
        
        private List<Type> dependencyChain;

        public Container() {
            Instances = new Dictionary<Type, object>();
            PrefabRegistrations = new Dictionary<Type, GameObject>();
            dependencyChain = new List<Type>();
        }

        /// <summary>
        /// Register a prefab available for injection. The prefab name must match the script you want to inject.
        /// </summary>
        /// <param name="mb">The prefab with attach Behaviour (cast to monobehaviour so we can extract the script from it).</param>
        public void RegisterPrefab(MonoBehaviour mb) {
            var prefabNameType = Assembly.GetAssembly(typeof(BootStrapper)).GetType(mb.name); // TODO: This restricts injection to the same assembly as the project, e.i. no external dlls.
            var registerMethod = GetType().GetMethod("RegisterPrefabWithBehaviour", BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = registerMethod.MakeGenericMethod(prefabNameType);
            generic.Invoke(this, new object[] { mb.gameObject });
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
        /// Register a MonoBehaviour and its specified prefab.
        /// </summary>
        /// <typeparam name="TBehaviour">The specified MonoBehaviour available for injection.</typeparam>
        /// <param name="prefab">The specified prefab the that will be instantiated with the MonoBehaviour attached to it.</param>
        private void RegisterPrefabWithBehaviour<TBehaviour>(GameObject prefab) where TBehaviour : MonoBehaviour {
            ValidatePrefabDependencies<TBehaviour>(prefab);
            PrefabRegistrations.Add(typeof(TBehaviour), prefab);
        }
        
        /// <summary>
        /// Validate that the registrated behaviour and prefab.
        /// </summary>
        /// <typeparam name="TBehaviour">Behaviour attached to the prefab.</typeparam>
        /// <param name="prefab">Prefab.</param>
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
        /// Returns an object of type with all its constructor dependencies resolved. 
        /// </summary>
        /// <param name="type">Type to resolve.</param>
        private object Resolve(Type type) {

            object instance;
            if(Instances.TryGetValue(type, out instance)) {
                return instance;
            }

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
                
            
            Instances.Add(type, instance);
            return instance;
        }
        
    }

}
