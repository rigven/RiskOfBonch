using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitedBonus : MonoBehaviour
{
    protected int modificationsNumber = 1;

    public void SetStartingModifications(int startingModificationsCount)
    {
        for (int i = 0; i < startingModificationsCount; i++)
        {
            AddModification();
        }
    }

    protected float CalculateProgressionCoeff(float start, float limit)
    {
        float coeff = 0;

        if (start < limit)
            coeff = (limit - start) / limit;
        else if (start > limit)
            coeff = (start - limit) / start;
        
        if (coeff == 0f || coeff == 1f)
            Debug.LogException(new System.ArithmeticException("Коэффициент геометрической прогрессии не может равняться 0 или 1."));

        return coeff;
    }

    protected void ProgressValue(ref float value, float startingValue, float progressCoeff)
    {
        value += startingValue * Mathf.Pow(progressCoeff, modificationsNumber);
    }

    protected void ProgressValueDown(ref float value, float limit, float progressCoeff)
    {
        value -= limit * Mathf.Pow(progressCoeff, modificationsNumber);
    }

    public abstract void AddModification();
}
