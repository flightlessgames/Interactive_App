using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionHistorySlot : MonoBehaviour
{
    [SerializeField] private List<displayIngredient> _recipeList = new List<displayIngredient>();
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private RawImage _potionColorImage = null;
}
