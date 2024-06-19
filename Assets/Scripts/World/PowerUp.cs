using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float maxUpgradeMagnitude;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            MothershipController ship = other.GetComponent<MothershipController>();
            int choice = 7;//Random.Range(0,8);
            switch(choice){
                case 0:
                    ship.ChangeMotherHealth(Random.Range(ship.startMaxHealth,ship.maxMaxHealth*maxUpgradeMagnitude));
                    break;
                case 1:
                    ship.ChangeMotherSpeed(Random.Range(ship.startMovSpeed,ship.maxMovSpeed*maxUpgradeMagnitude));
                    break;
                case 2:
                    ship.ChangeMaxShips((int)Random.Range(ship.startMiniShips,ship.maxMiniShips*maxUpgradeMagnitude));
                    break;
                case 3:
                    ship.ChangeMiniHealth(Random.Range(ship.startMiniHealth,ship.maxMiniHealth*maxUpgradeMagnitude));
                    break;
                case 4:
                    ship.ChangeMiniSpeed(Random.Range(ship.startMiniSpeed,ship.maxMiniSpeed*maxUpgradeMagnitude));
                    break;
                case 5:
                    ship.ChangeMiniDamage(Random.Range(ship.startMiniDamage,ship.maxMiniDamage*maxUpgradeMagnitude));
                    break;
                case 6:
                    ship.ChangeMiniBulletSpeed(Random.Range(ship.startMiniBulletSpeed,ship.maxMiniBulletSpeed*maxUpgradeMagnitude));
                    break;
                case 7:
                    ship.ChangeMiniShootDelay(Random.Range(0.05f,0.7f));
                    break;
            }
            Destroy(gameObject);
        }
    }
}
