using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CardController : MonoBehaviour
{
    [SerializeField]
    private Animator CardAnimator;

    void Awake()
    {
        InitiateCardProperties();
    }

    void InitiateCardProperties()
    {
        CardAnimator = GetComponent<Animator>();
        CardAnimator.enabled = true;
    }


    public void CardCheckForMatching()
    {
        if (GameHandler.Instance.IsCardClickable)
        {
            GameHandler.Instance.IsCardClickable = false;
            GameHandler.Instance.CalculateClickCount();
            CardAnimator.SetBool(AnimationName.CardFlip, true);
            if (GameHandler.Instance.PreviousCard != null)
            {
                if (GameHandler.Instance.PreviousCard.CompareTag(gameObject.tag))
                {
                    print("Card Matched");
                    StartCoroutine(GameHandler.Instance.CardMatchFunction(gameObject));
                }
                else
                {
                    print("Card Not Matched");
                    StartCoroutine(GameHandler.Instance.CardNotMatchFunction
                        (CardAnimator,gameObject));
                }
            }
            else
            {
                StartCoroutine(GameHandler.Instance.SetFirstCardInfo(gameObject));
            }
        }
    }
    
    
}
