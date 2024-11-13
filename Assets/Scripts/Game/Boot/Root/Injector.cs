using Game.DI;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;

namespace Game.Root
{
    public class Injector
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public Injector(Scene scene, DIContainer container)
        {
            var objs = scene.GetRootGameObjects();

            foreach (var obj in objs)
            {
                InjectObject(container, obj.transform);
            }
        }

        private void InjectChilds(DIContainer container, Transform obj)
        {
            foreach (Transform child in obj.transform)
            {
                InjectObject(container, child);
            }
        }

        private void InjectObject(DIContainer container, Transform obj)
        {
            var monos = obj.GetComponents<MonoBehaviour>();

            InjectMonoBehaviours(container, monos);
            InjectChilds(container, obj.transform);
        }

        private void InjectMonoBehaviours(DIContainer container, MonoBehaviour[] monos)
        {
            foreach (var mono in monos)
            {
                InjectFields(container, mono);
                InjectMethods(container, mono);
            }
        }

        private void InjectFields(DIContainer container, MonoBehaviour mono)
        {
            var type = mono.GetType();
            var fields = type.GetFields(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute))).ToArray();

            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                field.SetValue(mono, container.Resolve(fieldType));
            }
        }

        private void InjectMethods(DIContainer container, MonoBehaviour mono)
        {
            var type = mono.GetType();
            var methods = type.GetMethods(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var method in methods)
            {
                var parameters = method.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                var instances = parameters.Select(r => container.Resolve(r)).ToArray();

                if (instances.Any(resolvedInstance => resolvedInstance != null))
                {
                    method.Invoke(mono, instances);
                }
            }
        }
    }
}