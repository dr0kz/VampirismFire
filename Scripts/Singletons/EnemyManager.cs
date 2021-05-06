using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyManager : CharacterManager
{
    #region Singleton
    public static EnemyManager ENEMY_MANAGER;
    #endregion
    [SerializeField] private List<EnemyInstantiate> enemySpawnPoints;
    [SerializeField] private GameManager gameManager;
    private ResourceManager RESOURCE_MANAGER;
    protected override void Awake()
    {
        base.Awake();
        ENEMY_MANAGER = this;
    }
    public void Start()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public override void register(IUpdatable _object)
    {
        base.register(_object);
    }
    public override void remove(IUpdatable _object)
    {
        base.remove(_object);
        if (updatableObjects.Count == 0)
        {
            if (RESOURCE_MANAGER.GetWaveLevel() == RESOURCE_MANAGER.GetTotalWaves() - 1)
            {
                enemySpawnPoints
                .Where(instantiate => instantiate.GetInstantiatePermission())
                .ToList()
                .ForEach(instantiate => instantiate.InstantiateBoss());
            }
            else if (RESOURCE_MANAGER.GetWaveLevel() == RESOURCE_MANAGER.GetTotalWaves())
            {
                gameManager.GameStatus(GameManager.GameState.win);
            }
            else
            {
                enemySpawnPoints
                .Where(instantiate => instantiate.GetInstantiatePermission())
                .ToList()
                .ForEach(instantiate => instantiate.StartInstantiate());
            }
            RESOURCE_MANAGER.UpdateWaveLevel();

        }
    }
    public void notifyUpdatableOnBuildingUpgrade(Building building, Building nextBuilding)
    {
        updatableObjects.ForEach(character => ((IEnemyUpdatable)character).update(building, nextBuilding));
    }
}
