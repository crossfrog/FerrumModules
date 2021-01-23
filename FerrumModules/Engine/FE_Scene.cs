﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace FerrumModules.Engine
{
    public class FE_Scene : FE_ActiveElement
    {
        public List<FE_Entity> EntityList { get; private set; } = new List<FE_Entity>();
        private readonly Dictionary<string, FE_Entity> _entityNameDict = new Dictionary<string, FE_Entity>();
        public FE_Camera Camera = new FE_Camera();

        public Queue<FE_Entity> DeletionQueue = new Queue<FE_Entity>();

        public FE_Scene()
        {
            Camera.Centered = false;
        }

        public override void Update(float delta)
        {
            foreach (var e in DeletionQueue) Remove(e);
            DeletionQueue.Clear();

            foreach (var e in EntityList) e.Update(delta);
        }

        public override void Render(SpriteBatch spriteBatch, SpriteEffects spriteBatchEffects)
        {
            foreach (var e in EntityList)
            {
                e.Render(spriteBatch, spriteBatchEffects);
            }
        }

        #region Entity Management

        public EntityType Get<EntityType>(string entityName) where EntityType : FE_Entity
        {
            if (!Has(entityName)) throw new Exception("Entity \"" + entityName + "\" was requested, but did not exist.");
            return (EntityType)_entityNameDict[entityName];
        }

        public bool Has(string entityName)
        {
            return _entityNameDict.ContainsKey(entityName);
        }

        public bool Has<EntityType>(EntityType entity) where EntityType : FE_Entity
        {
            return EntityList.Contains(entity);
        }    

        public EntityType Add<EntityType>(EntityType entity) where EntityType : FE_Entity
        {
            if (Has(entity)) throw new Exception("Entity added which already exists in the scene.");
            EntityList.Add(entity);

            entity.Scene = this;
            entity.Init();
            return entity;
        }

        public void RegisterName(FE_Entity entity, string name)
        {
            if (!Has(entity)) throw new Exception("An entity outside of the current scene tried to register its name.");
            if (Has(name))
                throw new Exception
                    ("The entity name \"" + entity.Name + "\" already existed, and was overwritten.");

            if (!(entity.Name == "") && Has(entity.Name)) _entityNameDict.Remove(entity.Name);

            _entityNameDict[name] = entity;
        }

        private void Remove<EntityType>(EntityType entity) where EntityType : FE_Entity
        {
            if (!EntityList.Remove(entity))
                throw new Exception("Entity \"" + entity.Name + "\" does not exist in the scene or was already removed.");
            if (Has(entity.Name)) _entityNameDict.Remove(entity.Name);
        }

        public void Remove(string entityName)
        {
            if (!Has(entityName)) throw new Exception("Entity \"" + entityName + "\" was requested to be removed, but did not exist.");
            Get<FE_Entity>(entityName).Exit();
        }

        public List<EntityType> GetEntitiesWithBase<EntityType>() where EntityType : FE_Entity
        {
            var entitiesWithBase = new List<EntityType>();
            foreach (var e in EntityList)
            {
                if (e.GetType().IsSubclassOf(typeof(EntityType))) entitiesWithBase.Add((EntityType)e);
            }
            return entitiesWithBase;
        }

        #endregion
    }
}
