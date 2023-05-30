using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
	[SerializeField]
	private Vector2Int cellCoordinates;

	[SerializeField]
	private GameObject hintImage;
	
	[SerializeField]
	private Image playersMarkImage;
	
    private Sprite xMark,
                   oMark;

    private Coroutine hideHintCoroutine;

    public void Initialize(Sprite xMarkSprite, Sprite oMarkSprite)
    {
	    xMark = xMarkSprite;
	    oMark = oMarkSprite;
	    
	    ResetCell();

	    var gameManager = ServiceLocator.ResolveAndGet<GameManager>();

	    gameManager.PlayerValidMove += OnPlayersValidMove;
	    gameManager.SuggestedValidMoveFound += OnSuggestedValidMoveFound;
	    gameManager.MoveRevered += OnMoveReverted;
    }

    public void Deinitialize()
    {
	    var gameManager = ServiceLocator.ResolveAndGet<GameManager>();
	    
	    gameManager.PlayerValidMove -= OnPlayersValidMove;
	    gameManager.SuggestedValidMoveFound -= OnSuggestedValidMoveFound;
	    gameManager.MoveRevered -= OnMoveReverted;
    }

    public void ResetCell()
    {
	    hintImage.SetActive(false);
	    playersMarkImage.enabled = false;
    }

    public void BoardCellButton()
    {
	    ServiceLocator.ResolveAndGet<GameManager>().OnBoardCellClicked(cellCoordinates);
    }
    
    private void SetCellMark(Mark playersMark)
    {
	    playersMarkImage.sprite = playersMark == Mark.O ? oMark : xMark;
	    playersMarkImage.enabled = true;
    }

    private void ShowHint()
    {
	    if (hideHintCoroutine != null) StopCoroutine(hideHintCoroutine);
	    
	    hintImage.SetActive(true);

	    hideHintCoroutine = StartCoroutine(
		    DelayActionCoroutine(
			    1f,
			    () =>
			    {
				    hintImage.SetActive(false);
			    }));
    }

    private void OnPlayersValidMove(Vector2Int coordinates, Mark playersMark)
    {
	    if (cellCoordinates != coordinates) return;

	    SetCellMark(playersMark);
    }
    
    private void OnSuggestedValidMoveFound(Vector2Int? coordinates)
    {
	    if (cellCoordinates != coordinates) return;
	    
	    ShowHint();
    }

    private void OnMoveReverted(Vector2Int coordinates)
    {
	    if (cellCoordinates != coordinates) return;

	    ResetCell();
    }
    
    private IEnumerator DelayActionCoroutine(float delay, Action action)
    {
	    yield return new WaitForSecondsRealtime(delay);
        
	    action?.Invoke();
    }
}
