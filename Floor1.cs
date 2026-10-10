using System.Drawing;
using System.Text.RegularExpressions;

namespace BigODungeon;

// FLOOR 1 - THE VAULT: O(1) constant time
// Room 1: array index    Room 2: add two numbers    Room 3: first item
public class Floor1 : FloorBase
{
    public override string Title => "The Vault";
    public override string BigO => "O(1)";

    private readonly int[] _numbers = { 10, 20, 30, 40, 50 };
    private readonly Random _rng = new();

    private bool _triedJump, _triedScan; // Room 1 progress
    private int _hugeCount;              // Room 2 progress
    private int _packSize;               // Room 3: pretend size of the pack
    private int _growCount;              // Room 3 progress
    private bool _taken;                 // Room 3: first item taken since the last grow?

    protected override void OnStart()
    {
        _triedJump = _triedScan = false;
        _hugeCount = 0;
        _packSize = 3;
        _growCount = 0;
        _taken = false;

        UI.Log("=== FLOOR 1: THE VAULT  (O(1) - Constant Time) ===", Color.Gold);
        UI.Log("Goal: see that the work stays the SAME no matter how much data you have.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("HOW TO PLAY", Color.Cyan);
        UI.Log("  1. The yellow TASK bar tells you exactly what to type next.", Color.Gainsboro);
        UI.Log("  2. Type in the box at the bottom and press Enter.", Color.Gainsboro);
        UI.Log("  3. Type  help  to see your options, or  hint  for a nudge.", Color.Gainsboro);
        UI.Log("  4. Watch STEPS on the right. It counts how much work the computer does.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);

        StartRoom1(clearFirst: false);
    }

    // ---------- ROOM 1: The Treasure Shelf (numbers[2]) ----------
    private void StartRoom1(bool clearFirst = true)
    {
        if (clearFirst) UI.ClearStory();
        Counter.Reset();
        UI.SetOptimal(1);
        UI.SetRoom("Room 1 of 3: The Treasure Shelf");

        UI.Log("--- Room 1: The Treasure Shelf ---", Color.Gold);
        UI.Log("Five chests sit in a row (an array called numbers):", Color.Gainsboro);
        UI.Log("   Value:  [10] [20] [30] [40] [50]", Color.Gainsboro);
        UI.Log("   Index:   0    1    2    3    4", Color.Gray);
        UI.Log("You need the chest holding 30.", Color.Gainsboro);

        UI.SetTask("Type  numbers[2]  to go straight to the 30, or type  scan  to walk chest by chest.");
        SetHandler(Room1Input,
            help: "numbers[2]  - go straight to index 2 (C# array access)\n" +
                  "scan        - check the chests one by one from the start\n" +
                  "continue    - go to Room 2 (after trying a method)",
            hint: "The chest you need holds 30. Its index is 2, so in C# you write numbers[2].");
    }

    private bool Room1Input(string input)
    {
        if (input == "scan")
        {
            Scan();
            return true;
        }

        if (input == "continue")
        {
            if (!_triedJump && !_triedScan)
                UI.Log("  Try at least one method first: numbers[2] or scan.", Color.Orange);
            else
                StartRoom2();
            return true;
        }

        Match m = Regex.Match(input, @"^numbers\s*\[\s*(-?\d+)\s*\]$");
        if (m.Success)
        {
            if (!int.TryParse(m.Groups[1].Value, out int index)) index = int.MaxValue;
            AccessChest(index);
            return true;
        }

        return false;
    }

    private void AccessChest(int index)
    {
        Counter.Reset();
        UI.Log("", Color.Gainsboro);

        if (index < 0 || index >= _numbers.Length)
        {
            UI.Log($"> numbers[{index}]  ->  IndexOutOfRangeException!", Color.OrangeRed);
            UI.Log("  The array only has indexes 0 to 4. Try again.", Color.Orange);
            return;
        }

        int value = _numbers[index]; // direct access by index
        Counter.Add();               // 1 operation

        if (index != 2)
        {
            UI.Log($"> numbers[{index}] = {value}   -> STEPS: 1", Color.Khaki);
            UI.Log("  That's the wrong chest. We need the 30. Which index holds it?", Color.Orange);
            UI.Log("  (Even a wrong guess costs just 1 step.)", Color.Gray);
            return;
        }

        _triedJump = true;
        UI.Log($"> numbers[2] = {value}   -> STEPS: 1 (the best possible)", Color.LightGreen);
        UI.Log("  WHY: the computer knows where index 2 is, so it goes straight there.", Color.Cyan);
        UI.Log("       It would still be 1 step with 1,000 chests.", Color.Cyan);
        AfterRoom1Attempt();
    }

    private void Scan()
    {
        Counter.Reset();
        UI.Log("", Color.Gainsboro);
        UI.Log("> You walk past the chests one by one...", Color.Khaki);
        for (int i = 0; i <= 2; i++)
        {
            Counter.Add();           // each chest checked = 1 operation
            UI.Log($"  Checking chest {i}: {_numbers[i]}", Color.Khaki);
        }
        _triedScan = true;
        UI.Log("  Found 30  -> STEPS: 3 (more than needed)", Color.Orange);
        UI.Log("  WHY: you checked chests 0 and 1 for nothing. With 1,000 chests this", Color.Cyan);
        UI.Log("       could take hundreds of steps. That is O(n), see Floor 3.", Color.Cyan);
        AfterRoom1Attempt();
    }

    private void AfterRoom1Attempt()
    {
        if (_triedJump && _triedScan)
            UI.SetTask("Compare the two results above, then type  continue  to go to Room 2.");
        else if (_triedJump)
            UI.SetTask("Now type  scan  to see the slow way and compare STEPS (or type  continue).");
        else
            UI.SetTask("Now type  numbers[2]  to see the fast way and compare STEPS (or type  continue).");
    }

    // ---------- ROOM 2: The Rune Lock (add two numbers) ----------
    private void StartRoom2()
    {
        UI.ClearStory();
        Counter.Reset();
        _hugeCount = 0;
        UI.SetOptimal(1);
        UI.SetRoom("Room 2 of 3: The Rune Lock");

        UI.Log("--- Room 2: The Rune Lock ---", Color.Gold);
        UI.Log("The door shows two runes: 10 and 20.", Color.Gainsboro);
        UI.Log("It opens when you enter their sum.", Color.Gainsboro);

        UI.SetTask("Type the sum of 10 + 20 and press Enter.");
        SetHandler(Room2SolveInput,
            help: "a number  - your answer for 10 + 20",
            hint: "Just add the two runes: 10 + 20.");
    }

    private bool Room2SolveInput(string input)
    {
        if (!int.TryParse(input, out int guess)) return false;

        if (guess != 30)
        {
            UI.Log($"  {guess} doesn't open the lock. Add 10 + 20 and try again.", Color.OrangeRed);
            return true;
        }

        Counter.Reset();
        int sum = 10 + 20;           // the addition
        Counter.Add();               // 1 operation

        UI.Log("", Color.Gainsboro);
        UI.Log($"> Click! The lock opens. sum = {sum}   -> STEPS: 1", Color.LightGreen);
        UI.Log("  WHY: adding two numbers is one single operation.", Color.Cyan);
        UI.Log("", Color.Gainsboro);
        UI.Log("Question: would HUGE numbers make the computer work harder?", Color.Gainsboro);

        UI.SetTask("Type  huge  to make the runes huge, and watch the STEPS counter.");
        SetHandler(Room2HugeInput,
            help: "huge      - swap the runes for huge numbers and add them\n" +
                  "continue  - go to Room 3 (after trying huge)",
            hint: "Type huge and watch the STEPS counter on the right.");
        return true;
    }

    private bool Room2HugeInput(string input)
    {
        if (input == "huge")
        {
            HugeRunes();
            return true;
        }

        if (input == "continue")
        {
            if (_hugeCount == 0)
                UI.Log("  Type  huge  at least once first, then watch STEPS.", Color.Orange);
            else
                StartRoom3();
            return true;
        }

        return false;
    }

    private void HugeRunes()
    {
        long a = _rng.NextInt64(1_000_000_000L, 9_000_000_000L);
        long b = _rng.NextInt64(1_000_000_000L, 9_000_000_000L);
        _hugeCount++;

        Counter.Reset();
        long sum = a + b;            // still just one addition
        Counter.Add();

        UI.Log("", Color.Gainsboro);
        UI.Log($"> a = {a:N0}   b = {b:N0}", Color.Khaki);
        UI.Log($"  a + b = {sum:N0}   -> STEPS: still 1", Color.LightGreen);
        UI.Log("  WHY: bigger numbers, same single addition. The size of the values", Color.Cyan);
        UI.Log("       doesn't change the work. That's O(1).", Color.Cyan);

        UI.SetTask("Type  huge  again for new numbers, or type  continue  to go to Room 3.");
    }

    // ---------- ROOM 3: The Supply Pack (fruits[0]) ----------
    private void StartRoom3()
    {
        UI.ClearStory();
        Counter.Reset();
        _packSize = 3;
        _growCount = 0;
        _taken = false;
        UI.SetOptimal(1);
        UI.SetRoom("Room 3 of 3: The Supply Pack");

        UI.Log("--- Room 3: The Supply Pack ---", Color.Gold);
        UI.Log("A pack holds 3 items (an array called fruits): Apple, Banana, Mango.", Color.Gainsboro);
        UI.Log("You only want the FIRST item.", Color.Gainsboro);

        UI.SetTask("Type  fruits[0]  to take the first item from the pack.");
        SetHandler(Room3Input,
            help: "fruits[0]  - take the first item (C# array access)\n" +
                  "grow       - make the pack 10x bigger (after taking an item)\n" +
                  "finish     - end the floor (after growing at least once)",
            hint: "The first item of an array is at index 0, so type fruits[0].");
    }

    private bool Room3Input(string input)
    {
        Match m = Regex.Match(input, @"^fruits\s*\[\s*(-?\d+)\s*\]$");
        if (m.Success)
        {
            if (!int.TryParse(m.Groups[1].Value, out int index)) index = int.MaxValue;
            TakeItem(index);
            return true;
        }

        if (input == "grow")
        {
            if (!_taken)
                UI.Log("  Take the first item first: type  fruits[0].", Color.Orange);
            else
                GrowPack();
            return true;
        }

        if (input == "finish")
        {
            if (_growCount == 0 || !_taken)
                UI.Log("  Grow the pack and take the first item again before finishing.", Color.Orange);
            else
                FinishFloor();
            return true;
        }

        return false;
    }

    private void TakeItem(int index)
    {
        UI.Log("", Color.Gainsboro);

        if (index < 0 || index >= _packSize)
        {
            UI.Log($"> fruits[{index}]  ->  IndexOutOfRangeException!", Color.OrangeRed);
            UI.Log($"  The pack only has indexes 0 to {_packSize - 1}.", Color.Orange);
            return;
        }

        if (index != 0)
        {
            UI.Log($"> fruits[{index}] is not the first item.", Color.Orange);
            UI.Log("  In C#, the first item is at index 0. Try again.", Color.Orange);
            return;
        }

        Counter.Reset();
        string item = "Apple";       // fruits[0], direct access
        Counter.Add();               // 1 operation
        _taken = true;

        UI.Log($"> fruits[0] = \"{item}\"   (pack size: {_packSize:N0})   -> STEPS: 1", Color.LightGreen);

        if (_growCount == 0)
            UI.SetTask("Type  grow  to make the pack 10x bigger, then take the first item again.");
        else
            UI.SetTask("Same 1 step again! Type  grow  once more, or type  finish  to end the floor.");
    }

    private void GrowPack()
    {
        if (_packSize >= 1_000_000)
        {
            UI.Log("  The pack can't grow any bigger. (You've already proven the point!)", Color.Orange);
            return;
        }

        _packSize *= 10;
        _growCount++;
        _taken = false;
        Counter.Reset();

        UI.Log("", Color.Gainsboro);
        UI.Log($"> The pack now holds {_packSize:N0} items. STEPS reset to 0.", Color.Khaki);
        UI.SetTask("Type  fruits[0]  again and watch STEPS. Does the bigger pack cost more?");
    }

    private void FinishFloor()
    {
        UI.SetRoom("Floor cleared!");
        UI.Log("", Color.Gainsboro);
        UI.Log("=== FLOOR 1 CLEARED ===", Color.Gold);
        UI.Log("Indexing, one addition, and 'take the first item' each cost 1 step.", Color.Gainsboro);
        UI.Log("3 items or 1,000,000 items: the same single step. That's O(1).", Color.Gainsboro);

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