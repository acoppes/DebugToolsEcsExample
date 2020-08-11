using System;
using Unity.Entities;
using UnityEngine;

namespace DebugToolsExample
{
    public class DebugEntitiesSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            // create the debug stuff...
            Entities
                .WithAll<UnitComponent>()
                .WithNone<DebugEntitySystemComponent>()
                .ForEach(delegate(Entity entity)
                {
                    var name = EntityManager.GetName(entity);

                    if (string.IsNullOrEmpty(name))
                        name = $"Entity{entity.Index}";

                    var go = new GameObject($"DebugFor-{name}");
                    
                    var debug = go.AddComponent<DebugEntityMonoBehaviour>();

                    PostUpdateCommands.AddSharedComponent(entity, new DebugEntitySystemComponent
                    {
                        debug = debug
                    });
                });
            
            // update the debug stuff
            Entities
                .WithAll<UnitComponent, DebugEntitySystemComponent, HealthComponent>()
                .ForEach(delegate(Entity entity, DebugEntitySystemComponent debug, ref HealthComponent health)
                {
                    debug.debug.entity = entity;
                    debug.debug.healthPercentage = health.current / (float) health.total;
                    debug.debug.current = health.current;
                    debug.debug.total = health.total;
                });
            
            // destroy the debug stuff...
            Entities
                .WithAll<DebugEntitySystemComponent>()
                .WithNone<UnitComponent>()
                .ForEach(delegate(Entity entity, DebugEntitySystemComponent debug)
                {
                    GameObject.Destroy(debug.debug.gameObject);
                    PostUpdateCommands.RemoveComponent<DebugEntitySystemComponent>(entity);
                });
            
        }
    }

    public class SceneController : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var e1 = entityManager.CreateEntity(ComponentType.ReadOnly<UnitComponent>());

                var total = UnityEngine.Random.Range(50, 100);
                var current = UnityEngine.Random.Range(0, total);
                
                entityManager.AddComponentData(e1, new HealthComponent
                {
                    total = total,
                    current = current
                });

            }
        }
    }
}
