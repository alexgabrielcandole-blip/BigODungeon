using System.Drawing;

namespace BigODungeon;

// Shared logic for all floors: cleans up typed input and handles "help" / "hint".
// Each room just says which handler to use and what its help/hint text is.
public abstract class FloorBase : IFloor
{
    public abstract string Title { get; }
    public abstract string BigO { get; }

    protected IGameUI UI = null!;
    protected OpCounter Counter = null!;

    private Func<string, bool>? _handler; // returns true if it understood the input
    private string _help = "";
    private string _hint = "";

    public void Start(IGameUI ui, OpCounter counter)
    {
        UI = ui;
        Counter = counter;
        OnStart();
    }

    // Each floor builds its story here
    protected abstract void OnStart();

    // A room calls this to say "from now on, send typed input to this handler"
    protected void SetHandler(Func<string, bool> handler, string help, string hint)
    {
        _handler = handler;
        _help = help;
        _hint = hint;
    }

    public void HandleInput(string raw)
    {
        // lowercase + single spaces, so "  JUMP   2 " becomes "jump 2"
        string input = string.Join(' ',
            raw.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        if (input.Length == 0) return;

        if (input == "help")
        {
            UI.Log("  You can type right now:", Color.Cyan);
            foreach (string line in _help.Split('\n'))
                UI.Log("    " + line, Color.Cyan);
            UI.Log("    hint   - a nudge in the right direction", Color.Cyan);
            return;
        }

        if (input == "hint")
        {
            UI.Log("  HINT: " + _hint, Color.Cyan);
            return;
        }

        if (_handler == null || !_handler(input))
        {
            UI.Log($"  I don't understand \"{input}\". Type  help  to see what you can type now.", Color.Orange);
        }
    }
}