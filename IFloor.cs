using System.Drawing;

namespace BigODungeon;

// What a floor is allowed to ask the form to do (keeps floors independent of the UI)
public interface IGameUI
{
    void Log(string text, Color color);                                // write a line to the story box
    void ShowChoices(params (string Label, Action OnClick)[] choices); // replace the choice buttons
    void ClearChoices();                                               // remove all choice buttons
    void AskInput(string label, Action<string> onSubmit);              // let the player type an answer
    void StopInput();                                                  // stop expecting typed input
    void SetOptimal(int? steps);                                       // show optimal step count (null = "-")
    void FloorCleared();                                               // enables the "Next Floor" button
}

// Every floor (one per Big-O category) implements this
public interface IFloor
{
    string Title { get; }  // e.g. "The Vault"
    string BigO { get; }   // e.g. "O(1)"
    void Start(IGameUI ui, OpCounter counter);
}