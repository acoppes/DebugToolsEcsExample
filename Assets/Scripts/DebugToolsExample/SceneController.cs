using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
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
                    debug.entity = entity;

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
                    debug.debug.current = health.current;
                    debug.debug.total = health.total;
                });
            
            Entities
                .WithAll<UnitComponent, DebugEntitySystemComponent, AttackComponent>()
                .ForEach(delegate(Entity entity, DebugEntitySystemComponent debug, ref AttackComponent attack)
                {
                    debug.debug.attackRange = attack.range;
                });
            
            Entities
                .WithAll<UnitComponent, DebugEntitySystemComponent, Translation>()
                .ForEach(delegate(Entity entity, DebugEntitySystemComponent debug, ref Translation t)
                {
                    debug.debug.transform.position = t.Value;
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
            if (Input.GetKeyUp(KeyCode.Alpha1))
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
            
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var e1 = entityManager.CreateEntity(ComponentType.ReadOnly<UnitComponent>());

                entityManager.AddComponentData(e1, new AttackComponent()
                {
                    range = UnityEngine.Random.Range(0.5f, 2.0f)
                });
            }
            
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var e1 = entityManager.CreateEntity(ComponentType.ReadOnly<UnitComponent>());

                entityManager.AddComponentData(e1, new AttackComponent()
                {
                    range = UnityEngine.Random.Range(0.5f, 2.0f)
                });
                
                entityManager.AddComponentData(e1, new Translation()
                {
                    Value = new float3(UnityEngine.Random.Range(-2, 2), 
                        UnityEngine.Random.Range(-2, 2), 0)
                });

            }
            
        }
    }
}
