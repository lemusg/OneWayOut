using UnityEngine;

public class InteractablePaper : Interactable
{
    [SerializeField] private string paperContent = "This is the content of the paper..."; // The text that will be displayed

    protected override void Start()
    {
        base.Start();
        dialogueText = paperContent;
    }
} 