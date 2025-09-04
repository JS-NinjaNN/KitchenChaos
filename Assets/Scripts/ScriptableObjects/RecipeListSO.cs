using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu()]
public class RecipeListSO : ScriptableObject {
    [SerializeField] private List<RecipeSO> _recipeSOList;

    public IReadOnlyList<RecipeSO> RecipeSOList => _recipeSOList;
}