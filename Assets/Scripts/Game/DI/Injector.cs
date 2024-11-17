using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.DI
{
    public class Injector
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly DIContainer container;

        public Injector(DIContainer container)
        {
            this.container = container;
        }

        public void InjectGameObjectRecursively(GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>().ToList();
            components.ForEach(InjectMembers);

            foreach (Transform child in gameObject.transform)
            {
                InjectGameObjectRecursively(child.gameObject);
            }
        }

        #region InjectMembers

        public void InjectMembers(object obj)
        {
            InjectFields(obj);
            InjectProperties(obj);
            InjectMethods(obj);
        }

        private void InjectFields(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields(BINDING_FLAGS);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>();
                if (attribute == null) continue;

                var valueType = field.FieldType;

                if (container.TryResolve(valueType, out var value))
                {
                    field.SetValue(obj, value);
                }
            }
        }

        private void InjectProperties(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties(BINDING_FLAGS);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<InjectAttribute>();
                if (attribute == null) continue;

                var valueType = property.PropertyType;

                if (container.TryResolve(valueType, out var value))
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(obj, value);
                    }
                    else
                    {
                        var fields = type.GetFields(BINDING_FLAGS);
                        var field = fields.First(x =>
                            x.Name.Contains($"<{property.Name}>") && x.Name.Contains("BackingField"));
                        field.SetValue(obj, value);
                    }
                }
            }
        }

        private void InjectMethods(object obj)
        {
            var type = obj.GetType();
            var methods = type.GetMethods(BINDING_FLAGS);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<InjectAttribute>();
                if (attribute == null) continue;

                var parameters = method.GetParameters().ToList();

                if (TryGetParameterValues(parameters, out var values))
                {
                    method.Invoke(obj, values);
                }
                else
                {
                    throw new Exception($"Can not inject {method.Name} method");
                }
            }
        }

        #endregion

        #region CreateInstance

        public object CreateInstance(Type type)
        {
            var constructors = type.GetConstructors(BINDING_FLAGS).ToList();

            constructors = constructors.OrderByDescending(x => x.GetParameters().Length).ToList();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters().ToList();

                if (TryGetParameterValues(parameters, out var values))
                {
                    var instance = constructor.Invoke(values);
                    InjectMembers(instance);
                    return instance;
                }
            }

            throw new Exception("Constructor not found");
        }

        private bool TryGetParameterValues(List<ParameterInfo> parameters, out object[] values)
        {
            values = new object[parameters.Count];

            foreach (var parameter in parameters)
            {
                if (container.TryResolve(parameter.ParameterType, out var value))
                {
                    values[parameter.Position] = value;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}