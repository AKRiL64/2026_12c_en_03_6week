using System;
using UnityEngine;

public class Lever : MonoBehaviour, IPullable
{
    enum LeverPosition { Left, Right}
    
    public Action OnLeverSwitch;

    [SerializeField] private GameObject leftPositionSprite;
    [SerializeField] private GameObject rightPositionSprite;

    [SerializeField] private LeverPosition leverPosition = LeverPosition.Left;

    [SerializeField] private int activateDegree = 60;

    private void Awake()
    {
        SyncPosition();
    }
    
    private void ChangePosition()
    {
        if (leverPosition == LeverPosition.Left)
        {
            leverPosition = LeverPosition.Right;
        }
        else if (leverPosition == LeverPosition.Right)
        {
            leverPosition = LeverPosition.Left;
        }
    }

    private void SyncPosition()
    {
        leftPositionSprite.SetActive(false);
        rightPositionSprite.SetActive(false);
        if (leverPosition == LeverPosition.Left)
        {
            leftPositionSprite.SetActive(true);
        }
        else if (leverPosition == LeverPosition.Right)
        {
            rightPositionSprite.SetActive(true);
        }
    }

    public void NoConditionPull()
    {
        ChangePosition();
        SyncPosition();
        OnLeverSwitch?.Invoke();
    }

    public void Pull(Cannon cannon, float force)
    {
        Vector2 localPlayerPosition = transform.InverseTransformPoint(cannon.transform.position);

        if (leverPosition == LeverPosition.Left && localPlayerPosition.x <= 0f)
            return;

        if (leverPosition == LeverPosition.Right && localPlayerPosition.x >= 0f)
            return;

        float angle = Mathf.Atan2(
            localPlayerPosition.y,
            localPlayerPosition.x
        ) * Mathf.Rad2Deg;

        bool flag = false;

        if (leverPosition == LeverPosition.Left)
        {
            flag = angle >= -activateDegree && angle <= activateDegree;
        }
        else if (leverPosition == LeverPosition.Right)
        {
            flag = angle >= 180f - activateDegree || angle <= -180f + activateDegree;
        }

        if (flag)
        {
            ChangePosition();
            SyncPosition();
            OnLeverSwitch?.Invoke();
        }
    }
}
