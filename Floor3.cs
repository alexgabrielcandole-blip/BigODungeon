using System.Drawing;
using System.Text.RegularExpressions;

namespace BigODungeon;

// FLOOR 3 - THE LONG CORRIDOR: O(n) linear time
// Room 1: print every number    Room 2: find a name    Room 3: calculate the total
public class Floor3 : FloorBase
{
    public override string Title => "The Long Corridor";
    public override string BigO => "O(n)";

    // Caps keep the UI snappy (every step updates the STEPS label)
    private const int MaxTorches = 5000;
    private const int MaxGuests = 4000;
    private const int MaxChests = 3000;

    private int _steps;                  // our own copy of the step count, used in messages

    // Room 1 state
    private int _torches;                // how many torches are in the row
    private int _lit;                    // torches lit by hand so far
    private int _maxLit;                 // biggest row lit with lightall

    // Room 2 state
    private static readonly string[] Pool =
        { "Ana", "Ben", "Carlo", "David", "Elena", "Felix", "Gina", "Hugo", "Iris", "Jun", "Kira", "Leo" };
    private int _guests;                 // how many names are in the registry
    private int _maxScan;                // most names checked in one search

    // Room 3 state
    private int _chests;                 // how many chests are in the vault
    private int _maxTotalN;              // biggest pile that was totalled

    protected override void OnStart()
    {
        UI.Log("=== FLOOR 3: THE LONG CORRIDOR  (O(n) - Linear Time) ===", Color.Gold);
        UI.Log("Goal: see that when you must touch EVERY item, the work grows in a straight line with the data.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("Same controls as before: follow the yellow TASK bar. Type  help  or  hint  any time.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);

        StartRoom1(clearFirst: false);
    }

    // ---------- ROOM 1: The Torch Row (print every number) ----------
    private static int NumberAt(int i) => (i + 1) * 10;   // torch values: 10, 20, 30, ...

    private void StartRoom1(bool clearFirst = true)
    {
        if (clearFirst) UI.ClearStory();
        Counter.Reset();
        _steps = 0;
        _torches = 5;
        _lit = 0;
        _maxLit = 0;
        UI.SetOptimal(_torches);
        UI.SetRoom("Room 1 of 3: The Torch Row");

        UI.Log("--- Room 1: The Torch Row ---", Color.Gold);
        UI.Log("A corridor holds 5 torches, each showing a number (an array called numbers):", Color.Gainsboro);
        UI.Log("   int[] numbers = { 10, 20, 30, 40, 50 };", Color.Khaki);
        UI.Log("Your job: print every number. In C#:", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("    for (int i = 0; i < numbers.Length; i++)", Color.Khaki);
        UI.Log("    {", Color.Khaki);
        UI.Log("        Console.WriteLine(numbers[i]);", Color.Khaki);
        UI.Log("    }", Color.Khaki);
        UI.Log("", Color.Gainsboro);
        UI.Log("BEST POSSIBLE STEPS (right) = the fewest steps ANY method could take.", Color.Gray);
        UI.Log("Here that equals the number of torches, because every number has to be shown.", Color.Gray);

        UI.SetTask("Type  light  to light the first torch (one step).");
        SetHandler(Room1Input,
            help: "light      - light the next torch by hand (1 step)\n" +
                  "lightall   - run the whole loop and light every torch\n" +
                  "grow       - make the row 10x longer\n" +
                  "continue   - go to Room 2 (after lightall on 500+ torches)",
            hint: "Try light a few times, then grow twice and type lightall. Compare STEPS with the number of torches.");
    }

    private bool Room1Input(string input)
    {
        switch (input)
        {
            case "light":
                LightOne();
                return true;
            case "lightall":
                LightAll();
                return true;
            case "grow":
                GrowRow();
                return true;
            case "continue":
                if (_maxLit < 500)
                    UI.Log("  Light a row of at least 500 torches first: type grow twice, then lightall.", Color.Orange);
                else
                    StartRoom2();
                return true;
        }
        return false;
    }

    private void LightOne()
    {
        UI.Log("", Color.Gainsboro);

        if (_lit >= _torches)
        {
            UI.Log($"  All {_torches:N0} torches are already lit. Type  grow  for a longer row.", Color.Orange);
            return;
        }

        int i = _lit;
        Counter.Add();               // one torch = one step
        _steps++;
        _lit++;
        UI.Log($"> Console.WriteLine(numbers[{i}])  ->  {NumberAt(i):N0}     STEPS: {_steps}", Color.Khaki);

        if (_lit == _torches)
        {
            UI.Log($"  All {_torches:N0} torches lit. STEPS = {_steps:N0}: one per torch.", Color.LightGreen);
            UI.SetTask("Type  grow  to make the row 10x longer, then  lightall.");
        }
        else
        {
            UI.SetTask($"Keep going: type  light  ({_torches - _lit} left), or type  lightall.");
        }
    }

    private void LightAll()
    {
        Counter.Reset();
        _steps = 0;
        _lit = 0;
        UI.Log("", Color.Gainsboro);
        UI.Log($"> the for loop runs over {_torches:N0} torches...", Color.Khaki);

        for (int i = 0; i < _torches; i++)
        {
            Counter.Add();
            _steps++;
            _lit++;

            // Print the first 3 and last 2 lines for long rows, everything for short ones
            if (_torches <= 10 || i < 3 || i >= _torches - 2)
                UI.Log($"  Console.WriteLine(numbers[{i}])  ->  {NumberAt(i):N0}", Color.Khaki);
            else if (i == 3)
                UI.Log($"  ... {_torches - 5:N0} more torches ...", Color.Gray);
        }

        _maxLit = Math.Max(_maxLit, _torches);
        UI.Log($"  Lit all {_torches:N0} torches   ->  STEPS: {_steps:N0}", Color.LightGreen);
        UI.Log("  WHY: one step per torch. 10x more torches = 10x more steps. That is O(n).", Color.Cyan);

        if (_maxLit >= 500)
            UI.SetTask("Try grow for an even longer row, or type  continue  to go to Room 2.");
        else
            UI.SetTask("Type  grow  to make the row 10x longer, then  lightall  again. Watch STEPS.");
    }

    private void GrowRow()
    {
        UI.Log("", Color.Gainsboro);

        if (_torches >= MaxTorches)
        {
            UI.Log("  The corridor can't get any longer. (You've already proven the point!)", Color.Orange);
            return;
        }

        _torches *= 10;
        _lit = 0;
        _steps = 0;
        Counter.Reset();
        UI.SetOptimal(_torches);

        UI.Log($"> The corridor now holds {_torches:N0} torches. STEPS reset to 0.", Color.Khaki);
        UI.SetTask("Type  lightall  and compare STEPS with the number of torches.");
    }

    // ---------- ROOM 2: The Guest Registry (find a name) ----------
    private static string NameAt(int i) => i < Pool.Length ? Pool[i] : $"Guest{i + 1}";

    private void StartRoom2()
    {
        UI.ClearStory();
        Counter.Reset();
        _steps = 0;
        _guests = 4;
        _maxScan = 0;
        UI.SetOptimal(null);
        UI.SetRoom("Room 2 of 3: The Guest Registry");

        UI.Log("--- Room 2: The Guest Registry ---", Color.Gold);
        UI.Log("The registry lists 4 guests (an array called names): Ana, Ben, Carlo, David.", Color.Gainsboro);
        UI.Log("A visitor asks: is this guest on the list? You check the names one by one. In C#:", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("    foreach (string name in names)", Color.Khaki);
        UI.Log("    {", Color.Khaki);
        UI.Log("        if (name == \"Carlo\")", Color.Khaki);
        UI.Log("        {", Color.Khaki);
        UI.Log("            Console.WriteLine(\"Found!\");", Color.Khaki);
        UI.Log("            break;   // stop as soon as it is found", Color.Khaki);
        UI.Log("        }", Color.Khaki);
        UI.Log("    }", Color.Khaki);
        UI.Log("", Color.Gainsboro);
        UI.Log("(The break is our upgrade. The original example kept looping after finding Carlo.)", Color.Gray);
        UI.Log("BEST POSSIBLE STEPS shows  -  here: with luck it is 1 step, with bad luck it is every name.", Color.Gray);

        UI.SetTask("Type  find carlo  to search the registry.");
        UpdateRoom2Handler();
    }

    private void UpdateRoom2Handler()
    {
        string last = NameAt(_guests - 1).ToLowerInvariant();
        SetHandler(Room2Input,
            help: "find carlo  - search for a name (any name works, even one that isn't listed)\n" +
                  "grow        - make the list 10x longer\n" +
                  "continue    - go to Room 3 (after a search that checks 400+ names)",
            hint: $"Names are checked one by one. Try  find {last}  (the LAST guest) to see the worst case.");
    }

    private bool Room2Input(string input)
    {
        Match m = Regex.Match(input, @"^find\s+(\S+)$");
        if (m.Success)
        {
            FindName(m.Groups[1].Value);
            return true;
        }

        if (input == "grow")
        {
            GrowGuests();
            return true;
        }

        if (input == "continue")
        {
            if (_maxScan < 400)
                UI.Log("  Do a search that checks 400+ names first: type grow twice, then find the last guest.", Color.Orange);
            else
                StartRoom3();
            return true;
        }

        return false;
    }

    private void FindName(string target)   // target arrives lowercase
    {
        Counter.Reset();
        _steps = 0;
        int foundAt = -1;
        bool verbose = _guests <= 10;

        UI.Log("", Color.Gainsboro);
        for (int i = 0; i < _guests; i++)
        {
            Counter.Add();                 // one name checked = one step
            _steps++;
            bool match = NameAt(i).ToLowerInvariant() == target;

            if (verbose)
                UI.Log($"  Checking {NameAt(i)} ...  {(match ? "match!" : "no")}", Color.Khaki);

            if (match)
            {
                foundAt = i;
                break;                     // the break the original example was missing
            }
        }

        _maxScan = Math.Max(_maxScan, _steps);

        if (foundAt >= 0)
        {
            UI.Log($"> find {NameAt(foundAt)}  ->  FOUND at position {foundAt + 1:N0}   STEPS: {_steps:N0}", Color.LightGreen);
            if (_steps == 1)
                UI.Log("  Best case: it was the very first name. Just 1 step.", Color.Cyan);
            else if (foundAt == _guests - 1)
                UI.Log($"  Worst case: the LAST name. You checked all {_guests:N0} names.", Color.Cyan);
            else
                UI.Log($"  Found after {_steps:N0} of {_guests:N0} names. break stopped the loop right there.", Color.Cyan);
        }
        else
        {
            UI.Log($"> find {target}  ->  not on the list   STEPS: {_steps:N0}", Color.OrangeRed);
            UI.Log($"  Worst case: you can't know a name is missing until you've checked ALL {_guests:N0} names.", Color.Cyan);
        }
        UI.Log("  WHY: longer list, more names to check. Double the list, double the work. That is O(n).", Color.Cyan);

        if (_maxScan >= 400)
            UI.SetTask("Try another name, or type  continue  to go to Room 3.");
        else if (_guests == 4)
            UI.SetTask("Try  find ana  (best case) and  find david  (worst case), then type  grow.");
        else
            UI.SetTask($"Type  find {NameAt(_guests - 1).ToLowerInvariant()}  (the LAST guest) and watch STEPS. Or grow again.");
    }

    private void GrowGuests()
    {
        UI.Log("", Color.Gainsboro);

        if (_guests >= MaxGuests)
        {
            UI.Log("  The registry can't get any longer. (You've already proven the point!)", Color.Orange);
            return;
        }

        _guests *= 10;
        _steps = 0;
        Counter.Reset();
        UpdateRoom2Handler();

        string last = NameAt(_guests - 1);
        UI.Log($"> The registry now lists {_guests:N0} guests. The last one is {last}. STEPS reset to 0.", Color.Khaki);
        UI.SetTask($"Type  find {last.ToLowerInvariant()}  (the LAST guest) and watch STEPS.");
    }

    // ---------- ROOM 3: The Coin Count (calculate the total) ----------
    private static int CoinsAt(int i) => 5 * (i % 3 + 1);   // chests hold 5, 10, 15, 5, 10, 15, ...

    private void StartRoom3()
    {
        UI.ClearStory();
        Counter.Reset();
        _steps = 0;
        _chests = 3;
        _maxTotalN = 0;
        UI.SetOptimal(_chests);
        UI.SetRoom("Room 3 of 3: The Coin Count");

        UI.Log("--- Room 3: The Coin Count ---", Color.Gold);
        UI.Log("The vault holds 3 chests with 5, 10 and 15 coins (an array called numbers).", Color.Gainsboro);
        UI.Log("Goal: find the TOTAL number of coins. In C#:", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("    int[] numbers = { 5, 10, 15 };", Color.Khaki);
        UI.Log("    int total = 0;", Color.Khaki);
        UI.Log("    foreach (int number in numbers)", Color.Khaki);
        UI.Log("    {", Color.Khaki);
        UI.Log("        total = total + number;", Color.Khaki);
        UI.Log("    }", Color.Khaki);
        UI.Log("", Color.Gainsboro);
        UI.Log("BEST POSSIBLE STEPS (right) = one per chest. Every chest could change the total.", Color.Gray);

        UI.SetTask("Type  total  to open every chest and add up the coins.");
        SetHandler(Room3Input,
            help: "total    - open every chest and add up the coins\n" +
                  "grow     - make the vault 10x bigger\n" +
                  "finish   - end the floor (after totalling 300+ chests)",
            hint: "Type total, then grow twice and type total again. Compare STEPS with the number of chests.");
    }

    private bool Room3Input(string input)
    {
        switch (input)
        {
            case "total":
                CountCoins();
                return true;
            case "grow":
                GrowVault();
                return true;
            case "finish":
                if (_maxTotalN < 300)
                    UI.Log("  Total a bigger pile first: type grow twice, then total.", Color.Orange);
                else
                    FinishFloor();
                return true;
        }
        return false;
    }

    private void CountCoins()
    {
        Counter.Reset();
        _steps = 0;
        long total = 0;
        bool verbose = _chests <= 10;

        UI.Log("", Color.Gainsboro);
        for (int i = 0; i < _chests; i++)
        {
            int coins = CoinsAt(i);
            total = total + coins;       // the addition from the example
            Counter.Add();               // one chest = one step
            _steps++;

            if (verbose)
                UI.Log($"  chest {i + 1}: +{coins}  ->  running total {total}", Color.Khaki);
        }

        _maxTotalN = Math.Max(_maxTotalN, _chests);
        UI.Log($"> total = {total:N0}   ({_chests:N0} chests)   ->  STEPS: {_steps:N0}", Color.LightGreen);
        UI.Log("  WHY: every chest could change the answer, so none can be skipped.", Color.Cyan);
        UI.Log("       More chests = more steps, in a straight line. That is O(n).", Color.Cyan);
        UI.Log("       Compare Floor 1: numbers[2] cost 1 step no matter how many chests there were.", Color.Cyan);

        if (_maxTotalN >= 300)
            UI.SetTask("Type  finish  to clear the floor (or grow for an even bigger vault).");
        else
            UI.SetTask("Type  grow  to make the vault 10x bigger, then  total  again.");
    }

    private void GrowVault()
    {
        UI.Log("", Color.Gainsboro);

        if (_chests >= MaxChests)
        {
            UI.Log("  The vault can't get any bigger. (You've already proven the point!)", Color.Orange);
            return;
        }

        _chests *= 10;
        _steps = 0;
        Counter.Reset();
        UI.SetOptimal(_chests);

        UI.Log($"> The vault now holds {_chests:N0} chests. STEPS reset to 0.", Color.Khaki);
        UI.SetTask("Type  total  again and watch STEPS.");
    }

    private void FinishFloor()
    {
        UI.SetRoom("Floor cleared!");
        UI.Log("", Color.Gainsboro);
        UI.Log("=== FLOOR 3 CLEARED ===", Color.Gold);
        UI.Log("Printing every number, searching a list, and adding a total all take one step per item.", Color.Gainsboro);
        UI.Log("10x more items = 10x more steps. That's O(n).", Color.Gainsboro);

        UI.SetTask("Floor cleared! Type  next  or click Next Floor >.");
        SetHandler(input =>
        {
            UI.Log("  Floor cleared! Type  next  or click Next Floor >.", Color.Orange);
            return true;
        },
            help: "next  - go to the next floor",
            hint: "Type next.");
        UI.FloorCleared();
    }
}