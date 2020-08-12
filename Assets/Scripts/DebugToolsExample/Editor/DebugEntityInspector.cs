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
            // base.OnInspectorGUI();

            var debug = target as DebugEntityMonoBehaviour;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EditorGUI.BeginChangeCheck();

            var newCurrent = EditorGUILayout.IntField("Current Health", debug.current);
            var newTotal = EditorGUILayout.IntField("Total Health", debug.total);
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("Percentage", debug.current * 100 / debug.total);
            EditorGUI.EndDisabledGroup();
            
            if (EditorGUI.EndChangeCheck())
            {
                entityManager.SetComponentData(debug.entity, new HealthComponent
                {
                    current = newCurrent,
                    total = newTotal
                });    
            }

            if (GUILayout.Button("Perform Damage"))
            {
                var target = debug.entity;
                
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
                entityManager.DestroyEntity(target);
            }
        }
    }
}
