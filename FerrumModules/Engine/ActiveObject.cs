﻿using System;
using System.Collections.Generic;

namespace FerrumModules.Engine
{
    public abstract class ActiveObject
    {
        protected string _name = "";
        public virtual string Name { get => _name; set => _name = value; }
        public bool Paused;

        private bool _initalized = false;
        public bool Initialized
        {
            get { return _initalized; }
            set { if (_initalized == false) _initalized = true; }
        }

        protected Entity _parent;
        public virtual Entity Parent { get => _parent; set => _parent = value; }

        public virtual void Init() { }
        public virtual void Update(float delta) { }
        public virtual void Exit() { }

        #region Active Object List Generics
        protected ElementType GetFromObjectListByIndex<ElementType>(List<ElementType> list, int index) where ElementType : ActiveObject
        {
            return list[index];
        }

        protected ElementType GetFromObjectListByName<ElementType>(List<ElementType> list, string elementName)
            where ElementType : ActiveObject
        {
            if (elementName == "") throw new Exception("You cannot fetch an object with no name from \"" + Name + "\".");

            foreach (var e in list)
                if (e.Name == elementName) return e;

            throw new Exception("Object \"" + elementName + "\" was requested from \"" + Name + "\", but did not exist.");
        }

        protected void AssertNameIsUniqueInObjectList<ElementType>(List<ElementType> list, string elementName) where ElementType : ActiveObject
        {
            foreach (var e in list)
            {
                if (!((e.Name == "") || (e.Name == null)) && (e.Name == elementName)) throw new Exception("An object named \"" + elementName + "\" already existed in \"" + Name + "\".");
            }
        }

        protected bool ObjectListHas<ElementType>(List<ElementType> list, ElementType element) where ElementType : ActiveObject
        {
            return list.Contains(element);
        }

        protected bool ObjectListHas<ElementType>(List<ElementType> list, string name) where ElementType : ActiveObject
        {
            foreach (var e in list) if (e.Name == name) return true;
            return false;
        }

        protected NewObjectType AddObjectToList<ElementType, NewObjectType>(List<ElementType> list, List<ElementType> oldList, NewObjectType element)
            where ElementType : ActiveObject
            where NewObjectType : ElementType
        {
            if (ObjectListHas(list, element)) throw new Exception("Object \"" + element.Name + "\" added which already exists in \"" + Parent.Name + "\".");
            AssertNameIsUniqueInObjectList(list, element.Name);

            RemoveObjectFromList(oldList, element);
            list.Add(element);

            if (!element.Initialized)
            {
                element.Init();
                element.Initialized = true;
            }

            return element;
        }

        protected void RemoveObjectFromList<ElementType>(List<ElementType> list, ElementType element)
            where ElementType : ActiveObject
        {
            if (list != null && !list.Remove(element))
                throw new Exception("Object \"" + element.Name + "\" does not exist in \"" + Name + "\" or was already removed.");
        }

        protected List<BaseType> GetObjectsFromListWithBase<ElementType, BaseType>(List<ElementType> list)
            where BaseType : ElementType
            where ElementType : ActiveObject
        {
            var elementsWithBase = new List<BaseType>();
            foreach (var e in list)
            {
                var type = e.GetType();
                if (type.IsSubclassOf(typeof(BaseType)) || type == typeof(BaseType)) elementsWithBase.Add((BaseType)e);
            }
            return elementsWithBase;
        }

        #endregion
    }
}
