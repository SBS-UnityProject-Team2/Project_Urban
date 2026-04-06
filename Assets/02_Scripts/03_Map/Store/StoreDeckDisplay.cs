using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StoreDeckDisplay : MonoBehaviour
{
	[Header("Display Settings")]
	[SerializeField] private Transform displayArea;
	[SerializeField] private GameObject cardPrefab;
	[SerializeField] private GameObject deckPanel;

	private Button openButton;

	private void Awake()
	{
		openButton = GetComponent<Button>();
		openButton.onClick.RemoveListener(OpenDeckDisplay);
		openButton.onClick.AddListener(OpenDeckDisplay);
	}

	private void OnDestroy()
	{
		openButton.onClick.RemoveListener(OpenDeckDisplay);
	}

	public void OpenDeckDisplay()
	{
		RenderDeck(DeckManager.Instance.Deck);
		deckPanel.SetActive(true);
	}

	public void CloseDeckDisplay()
	{
		deckPanel.SetActive(false);
	}

	private void RenderDeck(List<DeckCard> deckToRender)
	{
		UICard uiCardPrefab = cardPrefab.GetComponent<UICard>();

		foreach (Transform child in displayArea)
		{
			Destroy(child.gameObject);
		}

		foreach (DeckCard cardInstance in deckToRender)
		{
			UICard spawnedCard = Instantiate(uiCardPrefab, displayArea);

			if (cardInstance.IsEnchanted)
			{
				CardDataEntry enchantedData = CardManager.Instance.GetEnchantCardData(cardInstance.Name);
				Sprite enchantedImage = CardManager.Instance.GetEnchantCardImage(cardInstance.Name);
				spawnedCard.Init(enchantedData, enchantedImage);
			}
			else
			{
				spawnedCard.Init(cardInstance);
			}

			spawnedCard.transform.localScale = Vector3.one;

			Button cardButton = spawnedCard.GetComponent<Button>();
			cardButton.onClick.RemoveAllListeners();
		}
	}
}
