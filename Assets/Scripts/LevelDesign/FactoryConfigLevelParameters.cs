using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.LevelDesign
{
    [CreateAssetMenu(menuName = "Custom/Power up configuration")]
    public class FactoryConfigLevelParameters : MonoBehaviour
    {
        [SerializeField] private Enemy[] _enemies;
        private Dictionary<string, Enemy> _idEnemies;

        private void Awake()
        {

            _idEnemies = new Dictionary<string, Enemy>();
            foreach (var enemy in _enemies)
            {
                _idEnemies.Add(enemy.name, enemy);
            }
        }

        public Enemy GetEnemyPrefabByName(string name)
        {
            if (!_idEnemies.TryGetValue(name, out var enemy))
            {
                throw new Exception($"PowerUp with id {name} does not exit");
            }

            return enemy;
        }
    }
}
