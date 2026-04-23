using UnityEngine;
using System;
using System.Collections;

public class LevelTimer : MonoBehaviour
{
    private float remainingTime;
    private float timeScaleMultiplier = 1f;
    private bool isRunning = false;
    private bool hasStarted = false;
    private Coroutine slowRoutine;

    public Action OnTimeUp;

    private void StartTimer(float duration)
    {
        remainingTime = duration;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime * timeScaleMultiplier;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isRunning = false;

            OnTimeUp?.Invoke();
        }
    }

    public void StartTimerOnFirstTouch(float duration)
    {
        if (hasStarted) return;

        hasStarted = true;
        StartTimer(duration);
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void SetTimeScale(float scale)
    {
        timeScaleMultiplier = scale;
    }

    public void ResetTimeScale()
    {
        timeScaleMultiplier = 1f;
    }

    public void SlowTimeForSeconds(float scale, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowTimeRoutine(scale, duration));
    }

    private IEnumerator SlowTimeRoutine(float scale, float duration)
    {
        timeScaleMultiplier = scale;

        yield return new WaitForSecondsRealtime(duration);

        timeScaleMultiplier = 1f;
        slowRoutine = null;
    }

}