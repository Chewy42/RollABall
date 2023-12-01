using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameController gameController;
    public GameView gameView;

    public void UpgradeHealth()
    {
        print("Upgrading Health");
        playerController.SetMaxHealth(playerController.GetMaxHealth() + 1f);
        DoneLevelingUp();
    }

    public void UpgradeSpeed()
    {
        print("Upgrading Speed");
        playerController.SetSpeed(playerController.GetSpeed() + 1f);
        DoneLevelingUp();
    }

    public void UpgradeProjectileShots()
    {
        print("Upgrading Projectile Shots");
        playerController.SetProjectileShots(playerController.GetProjectileShots() + 1);
        DoneLevelingUp();
    }

    private void DoneLevelingUp()
    {
        playerController.SetIsCurrentlyLevelingUp(false);
        gameController.ResumeGame();
        gameView.HideLevelUpView();
    }
}
