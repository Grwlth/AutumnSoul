using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject[] characters;

    private int currentCharacterIndex;

    private void Start()
    {
        currentCharacterIndex = Random.Range(0, characters.Length);

        ActivateCharacter(currentCharacterIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        Vector3 currentPosition =
            characters[currentCharacterIndex].transform.position;

        characters[currentCharacterIndex].SetActive(false);

        currentCharacterIndex++;

        if (currentCharacterIndex >= characters.Length)
        {
            currentCharacterIndex = 0;
        }

        characters[currentCharacterIndex].transform.position =
            currentPosition;

        characters[currentCharacterIndex].SetActive(true);
    }

    private void ActivateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == index);
        }
    }
}