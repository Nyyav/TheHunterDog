using System;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Представляет объект в игровом мире. Объект - это набор компонентов, которые определяют его поведение и внешний вид.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Хранит словарь, где ключом является Type компонента, а значением - экземпляр Component.
        /// </summary>
        private Dictionary<Type, Component> _components;

        /// <summary>
        /// Указывает, активна ли сущность.
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// Инициализирует новый экземпляр класса Entity.
        /// </summary>
        /// <param name="isActive">Определяет, активна ли сущность. По умолчанию true.</param>
        public Entity(bool isActive = true)
        {
            _components = new Dictionary<Type, Component>();
            IsActive = isActive;
        }

        /// <summary>
        /// Добавляет компонент к сущности.
        /// </summary>
        /// <param name="component">Компонент для добавления.</param>
        public void AddComponent(Component component)
        {
            Type type = component.GetType();
            if (!_components.ContainsKey(type))
            {
                _components.Add(type, component);
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine($"Компонент типа {type} уже существует!");
            }
        }

        /// <summary>
        /// Удаляет компонент из сущности.
        /// </summary>
        /// <typeparam name="T">Тип компонента для удаления.</typeparam>
        public void RemoveComponent<T>() where T : Component
        {
            Type type = typeof(T);
            if (_components.ContainsKey(type))
            {
                _components.Remove(type);
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine("Попытка удалить компонент, который не существует!");
            }
        }

        /// <summary>
        /// Возвращает компонент указанного типа из сущности.
        /// </summary>
        /// <typeparam name="T">Тип компонента для получения.</typeparam>
        /// <returns>Компонент указанного типа или null, если он не существует.</returns>
        public T GetComponent<T>() where T : Component
        {
            Type type = typeof(T);
            if (_components.TryGetValue(type, out Component component) && component is T tComponent)
            {
                return tComponent;
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine("Попытка получить компонент, который не существует!");
            }
            return null;
        }

        /// <summary>
        /// Возвращает все компоненты объекта.
        /// </summary>
        /// <returns>Список всех компонентов объекта.</returns>
        public List<Component> GetAllComponents()
        {
            List<Component> componentList = new List<Component>(_components.Values);
            return componentList;
        }
    }
}
