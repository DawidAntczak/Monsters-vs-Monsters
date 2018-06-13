using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    public const string defaultLayerName = "Default";
    public const int defaultLayer = 0;
    public const string enemyLayerName = "Enemy";
    public const int enemyLayer = 9;
    public const string defenderLayerName = "Defender";
    public const int defenderLayer = 10;
    public const string enemyProjectileLayerName = "EnemyProjectile";
    public const int enemyProjectileLayer = 11;
    public const string defenderProjectileLayerName = "DefenderProjectile";
    public const int defenderProjectileLayer = 15;
    public const string loseColliderLayerName = "LoseCollider";
    public const int loseColliderLayer = 12;
    public const string defenderAttackZoneLayerName = "DefenderAttackZone";
    public const int defenderAttackZoneLayer = 13;
    public const string enemyAttackZoneLayerName = "EnemyAttackZone";
    public const int enemyAttackZoneLayer = 14;
    public const string collectableLayerName = "Collectable";
    public const int collectableLayer = 16;
}