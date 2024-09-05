using System;
using AceLand.Input.PlayerLoopSystems;
using UnityEngine;

namespace AceLand.Input
{
    public static class InputProvider
    {
        #region Builder
        
        public static IProviderActionNameImporter<T> Importer<T>()
            where T : struct => new InputProviderImporter<T>();

        public interface IProviderActionNameImporter<T>
            where T : struct
        {
            IProviderMethodImporter<T> WithActionName(string name);
        }
        public interface IProviderMethodImporter<T>
            where T : struct
        {
            IProviderImporter WithMethod(Func<T, T> providerMethod);
        }
        
        public interface IProviderImporter
        {
            void Import();
        }

        private class InputProviderImporter<T> : IProviderActionNameImporter<T>, IProviderMethodImporter<T>, IProviderImporter
            where T : struct
        {
            internal InputProviderImporter()
            {
                _type = typeof(T);
                if (_type != typeof(float) && _type != typeof(Vector2))
                    throw new Exception($"Type Error: Input Provider accepts only float or Vector2");
            }

            private readonly Type _type;
            private string _name;
            private Func<T, T> _providedMethod;

            public void Import()
            {
                if (_type == typeof(float))
                {
                    if (_providedMethod is not Func<float, float> method) return;
                    InputManager.AxisInputSystem.AddProvideMethod(_name, method);
                    return;
                }

                if (_type == typeof(Vector2))
                {
                    if (_providedMethod is not Func<Vector2, Vector2> method) return;
                    InputManager.Axis2InputSystem.AddProvideMethod(_name, method);
                    return;
                }
            }

            public IProviderMethodImporter<T> WithActionName(string name)
            {
                _name = name;
                return this;
            }

            public IProviderImporter WithMethod(Func<T, T> providerMethod)
            {
                _providedMethod = providerMethod;
                return this;
            }
        }

        #endregion

        #region Remover

        public static void Remove<T>(string actionName)
            where T : struct
        {
            var type = typeof(T);
            if (type == typeof(float))
            {
                InputManager.AxisInputSystem.RemoveProvideMethod(actionName);
                return;
            }

            if (type == typeof(Vector2))
            {
                InputManager.Axis2InputSystem.RemoveProvideMethod(actionName);
                return;
            }
        }

        #endregion
    }
}