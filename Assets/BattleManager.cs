using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
    public enum InputState { STALL, WAIT }

    public BattleState battleState;
    public InputState inputState;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit, enemyUnit;

    public Text dialogueText;
    public BattleHUD playerBattleHUD, enemyBattleHUD;
    public PaceManager paceManager;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        battleState = BattleState.START;
        inputState = InputState.STALL;
        
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Fighting a " + enemyUnit.unitName + "...";

        playerBattleHUD.SetupHUD(playerUnit);
        enemyBattleHUD.SetupHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        battleState = BattleState.PLAYERTURN;
        StartOneTurn();

    }

    void EndBattle()
    {
        if (battleState == BattleState.WON || battleState == BattleState.LOST)
        {
            if (battleState == BattleState.WON)
                dialogueText.text = "You won!";
            else
                dialogueText.text = "You lost!";
        }
        else
        {
            // error report
        }
    }

    void StartOneTurn()
    {

        if (battleState == BattleState.PLAYERTURN) 
        {
            RunPlayerTurn();
        }  else if (battleState == BattleState.ENEMYTURN)
        {
            StartCoroutine(RunEnemyTurn());
        }
        // bug report
    }

    void EndOneTurn()
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            if (enemyUnit.IsDead())
            {
                battleState = BattleState.WON;
                EndBattle();
                return;
            }
            battleState = BattleState.ENEMYTURN;
            StartOneTurn();
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            if (playerUnit.IsDead())
            {
                battleState = BattleState.LOST;
                EndBattle();
                return;
            }
            battleState = BattleState.PLAYERTURN;
            StartOneTurn();
        }

    }
    void RunPlayerTurn()
    {
        inputState = InputState.WAIT;
        dialogueText.text = "Choose an action: ";
    }

    void PlayerAttack()
    {
        StartCoroutine(paceManager.Play());
    }

    public IEnumerator PlayCallBack(float damageBuff)
    {
        int damage = (int)(playerUnit.attack * damageBuff);
        int actualDamage = enemyUnit.TakeDamage(damage);
        enemyBattleHUD.UpdateHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful: " + actualDamage;

        yield return new WaitForSeconds(2f);
        EndOneTurn();
    }

    public void OnAttackButton()
    {
        if (inputState != InputState.WAIT || battleState != BattleState.PLAYERTURN)
            return;
        inputState = InputState.STALL;
        PlayerAttack();
    }

    IEnumerator RunEnemyTurn()
    {
        dialogueText.text = "The enemy is attacking.";

        yield return new WaitForSeconds(1f);

        playerUnit.TakeDamage(enemyUnit.attack);
        playerBattleHUD.UpdateHP(playerUnit.currentHP);
        dialogueText.text = "The attack is successful.";

        yield return new WaitForSeconds(2f);
        EndOneTurn();
    }
}
