using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameStateManager gameStateManager;
    public GameView gameView;

    public void UpgradeDamage()
    {
        playerController.SetDamage(playerController.GetDamage() + 1);
        DoneLevelingUp();
    }

    public void UpgradeSpeed()
    {
        playerController.SetSpeed(playerController.GetSpeed() + 1f);
        DoneLevelingUp();
    }

    public void UpgradeProjectileShots()
    {
        playerController.SetProjectileShots(playerController.GetProjectileShots() + 1);
        DoneLevelingUp();
    }

    private void DoneLevelingUp()
    {
        playerController.SetIsCurrentlyLevelingUp(false);
        playerController.SetCanShoot(true);
        playerController.StartCoroutine(playerController.AutoShoot());
        gameStateManager.ChangeState(GameStateManager.GameStates.GamePlaying);
        gameView.HideLevelUpView();
    }
}
