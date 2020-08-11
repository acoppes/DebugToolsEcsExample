using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace DebugToolsExample.Editor
{
    [CustomEditor(typeof(DebugEntityMonoBehaviour))]
    public class DebugEntityInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var debug = target as DebugEntityMonoBehaviour;
            
            if (GUILayout.Button("Perform Damage"))
            {
                var target = debug.entity;
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                var damageEntity = entityManager.CreateEntity(ComponentType.ReadWrite<Damage>());
                entityManager.SetComponentData(damageEntity, new Damage
                {
                    target = target,
                    damage = 5
                });
            }
            
            if (GUILayout.Button("Destroy"))
            {
                var target = debug.entity;
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entityManager.DestroyEntity(target);
            }
        }
    }
}
