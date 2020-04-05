using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameLoop/EnemyDecideState")]
public class EnemyDecideState : GameLoopState
{
    public GameObject CardUIPrefab;
    public override IEnumerator Enter()
    {
        GameLoop.RoundText.text = "Enemies Deciding";
        yield return new WaitForSeconds(0.5f);

        foreach (Enemy enemy in GameLoop.enemies)
        {
            FunctionAction action = enemy.PickAction();

            GameObject cardUI = Instantiate(CardUIPrefab, GameLoop.DisplayCard.transform);
            cardUI.GetComponent<UICard>().Set(enemy.card);
            Destroy(cardUI.GetComponent<Button>());
            yield return action.Visualize();
            Destroy(cardUI);
        }
        stateMachine.ChangeState<PlayerState>();
    }
}
