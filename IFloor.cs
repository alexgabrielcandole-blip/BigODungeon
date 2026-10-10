using System.Drawing;

namespace BigODungeon;

// What a floor is allowed to ask the form to do (keeps floors independent of the UI)
public interface IGameUI
{
    void Log(string text, Color color);   // write a line to the story box
    void ClearStory();                    // wipe the story box (new room)
    void SetTask(string text);            // the yellow "what to type now" bar
    void SetRoom(string text);            // header text, e.g. "Room 1 of 3"
    void SetOptimal(int? steps);          // best possible steps (null = "-")
    void SetStamina(int value);           // 0-100, drives the stamina bar
    void FloorCleared();                  // enables the "Next Floor" button
}

// Every floor (one per Big-O category) implements this
public interface IFloor
{
    string Title { get; }                 // e.g. "The Vault"
    string BigO { get; }                  // e.g. "O(1)"
    void Start(IGameUI ui, OpCounter counter);
    void HandleInput(string text);        // whatever the player typed
}