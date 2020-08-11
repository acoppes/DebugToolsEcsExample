using Unity.Entities;

namespace DebugToolsExample
{
    [GenerateAuthoringComponent]
    public struct HealthComponent : IComponentData
    {
        public int current;
        public int total;
    }

    public struct Damage : IComponentData
    {
        public Entity target;
        public int damage;
    }

    public class DamageSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Damage>()
                .ForEach(delegate(Entity e, ref Damage damage)
                {
                    var target = damage.target;

                    var health = EntityManager.GetComponentData<HealthComponent>(target);
                    health.current -= damage.damage;
                    
                    PostUpdateCommands.SetComponent(target, health);
                    
                    PostUpdateCommands.DestroyEntity(e);
                });
        }
    }
}