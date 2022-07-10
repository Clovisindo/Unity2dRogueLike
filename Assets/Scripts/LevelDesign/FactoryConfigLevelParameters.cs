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

        [SerializeField] private fFloorMechanic[] _fPieces;
        private Dictionary<string, fFloorMechanic> _idfPieces;

        private void Awake()
        {

            _idEnemies = new Dictionary<string, Enemy>();
            foreach (var enemy in _enemies)
            {
                _idEnemies.Add(enemy.name, enemy);
            }

            _idfPieces = new Dictionary<string, fFloorMechanic>();
            foreach (var fPiece in _fPieces)
            {
                _idfPieces.Add(fPiece.name, fPiece);
            }
        }

        public Enemy GetEnemyPrefabByName(string name)
        {
            if (!_idEnemies.TryGetValue(name, out var enemy))
            {
                throw new Exception($"Enemy with id {name} does not exit");
            }

            return enemy;
        }

        public fFloorMechanic GetfPiecePrefabByName(string name)
        {
            if (!_idfPieces.TryGetValue(name, out var fPiece))
            {
                throw new Exception($"FPiece with id {name} does not exit");
            }

            return fPiece;
        }
    }
}
