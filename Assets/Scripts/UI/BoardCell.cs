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

	    GameManager.Instance.PlayerValidMove += OnPlayersValidMove;
	    GameManager.Instance.SuggestedValidMoveFound += OnSuggestedValidMoveFound;
	    GameManager.Instance.MoveRevered += OnMoveReverted;
    }

    public void Deinitialize()
    {
	    GameManager.Instance.PlayerValidMove -= OnPlayersValidMove;
	    GameManager.Instance.SuggestedValidMoveFound -= OnSuggestedValidMoveFound;
	    GameManager.Instance.MoveRevered -= OnMoveReverted;
    }

    public void ResetCell()
    {
	    hintImage.SetActive(false);
	    playersMarkImage.enabled = false;
    }

    public void BoardCellButton()
    {
	    GameManager.Instance.OnBoardCellClicked(cellCoordinates);
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
