using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace BigODungeon;

// FLOOR 2 - THE HALVING HALLS: O(log n) logarithmic time
// Room 1: binary search    Room 2: keep dividing by 2    Room 3: higher/lower guessing game
public class Floor2 : FloorBase
{
    public override string Title => "The Halving Halls";
    public override string BigO => "O(log n)";

    private const int FreeGuesses = 7;   // Room 3: guesses before stamina starts draining
    private const int GuessCost = 10;    // Room 3: stamina lost per guess beyond the free ones

    private readonly Random _rng = new();
    private int _steps;                  // our own copy of the step count, used in messages

    // Room 1 state
    private readonly int[] _ledger = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };
    private int _target, _left, _right;
    private bool _found;
    private int _huntsWon;

    // Room 2 state
    private long _n;
    private bool _reachedOne;
    private long _maxTried;

    // Room 3 state
    private int _secret, _lo, _hi, _guesses, _stamina;

    protected override void OnStart()
    {
        _huntsWon = 0;

        UI.Log("=== FLOOR 2: THE HALVING HALLS  (O(log n) - Logarithmic Time) ===", Color.Gold);
        UI.Log("Goal: see that when every step throws away HALF the data, even huge data needs few steps.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("Same controls as Floor 1: follow the yellow TASK bar. Type  help  or  hint  any time.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);

        StartRoom1(clearFirst: false);
    }

    // ---------- ROOM 1: The Sorted Ledger (binary search) ----------
    private void StartRoom1(bool clearFirst = true)
    {
        if (clearFirst) UI.ClearStory();
        Counter.Reset();
        _steps = 0;
        _target = _ledger[_rng.Next(_ledger.Length)];
        _left = 0;
        _right = _ledger.Length - 1;
        _found = false;
        UI.SetOptimal(4);
        UI.SetRoom("Room 1 of 3: The Sorted Ledger");

        UI.Log("--- Room 1: The Sorted Ledger ---", Color.Gold);
        UI.Log("A ledger lists 15 prices in sorted order (an array). One of them is the hidden target.", Color.Gainsboro);
        UI.Log("For any entry you check, you only learn: is the target HIGHER or LOWER?", Color.Gainsboro);
        UI.Log("Binary search: always check the MIDDLE of what is still in play.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        ShowLedger();
        UI.Log("", Color.Gainsboro);
        UI.Log("BEST POSSIBLE STEPS (right) = the most this method ever needs: 4 checks for 15 values.", Color.Gray);

        UpdateRoom1Prompt();
    }

    // Prints the ledger; values already ruled out show as dots
    private void ShowLedger()
    {
        var values = new StringBuilder("Value:");
        var index = new StringBuilder("Index:");
        for (int i = 0; i < _ledger.Length; i++)
        {
            bool inPlay = i >= _left && i <= _right;
            values.Append(inPlay ? $"{_ledger[i],4}" : "   .");
            index.Append($"{i,4}");
        }
        UI.Log(values.ToString(), Color.Gainsboro);
        UI.Log(index.ToString(), Color.Gray);
    }

    private void UpdateRoom1Prompt()
    {
        int mid = _left + (_right - _left) / 2;

        if (_found)
        {
            UI.SetTask("Target found! Type  again  for a new target, or type  continue  to go to Room 2.");
            SetHandler(Room1Input,
                help: "again     - hunt a new hidden target\n" +
                      "continue  - go to Room 2",
                hint: "Type again to try another target, or continue to move on.");
            return;
        }

        UI.SetTask($"Type the middle index of {_left}..{_right} and press Enter.");
        SetHandler(Room1Input,
            help: "a number  - the middle index of the range still in play\n" +
                  "continue  - go to Room 2 (after finding the target once)\n" +
                  "again     - hunt a new target (after finding one)",
            hint: $"middle = left + (right - left) / 2 = {_left} + ({_right} - {_left}) / 2 = {mid}. Type {mid}.");
    }

    private bool Room1Input(string input)
    {
        if (input == "continue")
        {
            if (_huntsWon == 0)
                UI.Log("  Find the target first. Type the middle index.", Color.Orange);
            else
                StartRoom2();
            return true;
        }

        if (input == "again")
        {
            if (!_found)
                UI.Log("  Finish this hunt first. Type the middle index.", Color.Orange);
            else
                StartRoom1();
            return true;
        }

        if (int.TryParse(input, out int index))
        {
            if (_found)
                UI.Log("  You already found it. Type  again  or  continue.", Color.Orange);
            else
                CheckMiddle(index);
            return true;
        }

        return false;
    }

    private void CheckMiddle(int index)
    {
        UI.Log("", Color.Gainsboro);

        if (index < 0 || index >= _ledger.Length)
        {
            UI.Log($"  The ledger only has indexes 0 to {_ledger.Length - 1}.", Color.Orange);
            return;
        }

        if (index < _left || index > _right)
        {
            UI.Log($"  Index {index} is already ruled out. Still in play: {_left}..{_right}.", Color.Orange);
            return;
        }

        // (left + right) / 2 can overflow on huge arrays; this form can't
        int mid = _left + (_right - _left) / 2;
        if (index != mid)
        {
            UI.Log($"  Not the middle. middle = left + (right - left) / 2 = {_left} + ({_right} - {_left}) / 2 = {mid}.", Color.Orange);
            UI.Log($"  Type {mid}. (No penalty for a wrong try.)", Color.Orange);
            return;
        }

        Counter.Add();               // one comparison
        _steps++;
        int value = _ledger[mid];

        if (value == _target)
        {
            _found = true;
            _huntsWon++;
            UI.Log($"> ledger[{mid}] = {value}  ->  FOUND!   STEPS: {_steps}", Color.LightGreen);
            UI.Log("  WHY: every check threw away half of what was left.", Color.Cyan);
            UI.Log("       15 values need at most 4 checks. Scanning one by one could take up to 15.", Color.Cyan);
            UI.Log("       With 1,000,000 values binary search needs only about 20 checks.", Color.Cyan);
            UpdateRoom1Prompt();
            return;
        }

        int before = _right - _left + 1;
        if (value < _target)
        {
            UI.Log($"> ledger[{mid}] = {value}  ->  too low, the target is HIGHER.   STEPS: {_steps}", Color.Khaki);
            _left = mid + 1;
        }
        else
        {
            UI.Log($"> ledger[{mid}] = {value}  ->  too high, the target is LOWER.   STEPS: {_steps}", Color.Khaki);
            _right = mid - 1;
        }

        int after = _right - _left + 1;
        UI.Log($"  Threw away {before - after} of {before} values. {after} left.", Color.Cyan);
        UI.Log("", Color.Gainsboro);
        ShowLedger();
        UpdateRoom1Prompt();
    }

    // ---------- ROOM 2: The Halving Stairs (keep dividing by 2) ----------
    private void StartRoom2()
    {
        UI.ClearStory();
        Counter.Reset();
        _steps = 0;
        _n = 16;
        _reachedOne = false;
        _maxTried = 0;
        UI.SetOptimal(4);
        UI.SetRoom("Room 2 of 3: The Halving Stairs");

        UI.Log("--- Room 2: The Halving Stairs ---", Color.Gold);
        UI.Log("A staircase has n = 16 stairs. Each time the bell rings, HALF of the stairs vanish.", Color.Gainsboro);
        UI.Log("How many rings until only 1 stair is left? In C#:", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("    int n = 16;", Color.Khaki);
        UI.Log("    while (n > 1)", Color.Khaki);
        UI.Log("    {", Color.Khaki);
        UI.Log("        n = n / 2;", Color.Khaki);
        UI.Log("    }", Color.Khaki);

        UI.SetTask("Type  halve  to run  n = n / 2  once. Repeat until n is 1.");
        SetHandler(Room2Input,
            help: "halve  - run n = n / 2 once (1 step)",
            hint: "Just type halve. Keep going until n is 1.");
    }

    private bool Room2Input(string input)
    {
        if (input == "halve")
        {
            if (_reachedOne)
                UI.Log("  n is already 1. Try a big number: try 1000000", Color.Orange);
            else
                Halve();
            return true;
        }

        Match m = Regex.Match(input, @"^try\s+(\d+)$");
        if (m.Success)
        {
            if (!_reachedOne)
                UI.Log("  Halve 16 down to 1 first (type halve).", Color.Orange);
            else if (!long.TryParse(m.Groups[1].Value, out long big) || big < 1 || big > 1_000_000_000_000L)
                UI.Log("  Pick a whole number from 1 to 1,000,000,000,000.", Color.Orange);
            else
                TryNumber(big);
            return true;
        }

        if (input == "continue")
        {
            if (_maxTried < 1_000_000)
                UI.Log("  Try a big one first: try 1000000  (that is the whole point of this room).", Color.Orange);
            else
                StartRoom3();
            return true;
        }

        return false;
    }

    private void Halve()
    {
        _n /= 2;
        Counter.Add();
        _steps++;
        UI.Log("", Color.Gainsboro);
        UI.Log($"> n = n / 2   ->   n = {_n}     STEPS: {_steps}", Color.Khaki);

        if (_n > 1)
        {
            UI.SetTask("Keep going: type  halve  again. Stop when n is 1.");
            return;
        }

        _reachedOne = true;
        UI.Log($"  n is 1 after {_steps} halvings.  (16 = 2 x 2 x 2 x 2)", Color.LightGreen);
        UI.Log("  Now the real test: how many halvings does a BIG n need?", Color.Gainsboro);
        UI.Log("  Type  try 1000  then  try 1000000  then  try 1000000000", Color.Cyan);

        UI.SetTask("Type  try 1000000  (any number up to 1,000,000,000,000) and watch STEPS.");
        SetHandler(Room2Input,
            help: "try 1000000  - halve that number down to 1 and count the steps\n" +
                  "continue     - go to Room 3 (after trying a big number)",
            hint: "Type try 1000000 and compare STEPS with the size of the number.");
    }

    private void TryNumber(long n)
    {
        Counter.Reset();
        _steps = 0;
        long x = n;
        while (x > 1)
        {
            x /= 2;
            Counter.Add();
            _steps++;
        }
        _maxTried = Math.Max(_maxTried, n);
        UI.SetOptimal(_steps);

        UI.Log("", Color.Gainsboro);
        UI.Log($"> n = {n:N0}  ->  halved down to 1 in only {_steps} steps", Color.LightGreen);
        UI.Log($"  Walking down one stair at a time would take about {n:N0} steps. That is O(n).", Color.Cyan);
        UI.Log("  WHY: doubling n adds just ONE more halving. That is O(log n).", Color.Cyan);

        if (_maxTried >= 1_000_000)
            UI.SetTask("Try another number, or type  continue  to go to Room 3.");
        else
            UI.SetTask("Now a much bigger one:  try 1000000");
    }

    // ---------- ROOM 3: Higher or Lower (binary-search-style guessing) ----------
    // notice (optional) is shown at the top after the story box is cleared, e.g. after a collapse
    private void StartRoom3(string? notice = null)
    {
        UI.ClearStory();
        if (notice != null)
        {
            foreach (string line in notice.Split('\n'))
                UI.Log(line, Color.OrangeRed);
            UI.Log("", Color.Gainsboro);
        }

        Counter.Reset();
        _steps = 0;
        _secret = _rng.Next(1, 101);   // 1..100
        _lo = 1;
        _hi = 100;
        _guesses = 0;
        _stamina = 100;
        UI.SetStamina(_stamina);
        UI.SetOptimal(7);
        UI.SetRoom("Room 3 of 3: Higher or Lower");

        UI.Log("--- Room 3: Higher or Lower ---", Color.Gold);
        UI.Log("The guardian thinks of a secret number from 1 to 100.", Color.Gainsboro);
        UI.Log("After each guess it tells you HIGHER or LOWER.", Color.Gainsboro);
        UI.Log($"Your first {FreeGuesses} guesses are free. Every guess after that costs {GuessCost} STAMINA.", Color.Gainsboro);
        UI.Log("At 0 stamina you collapse and the hall resets with a new secret number.", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("BEST POSSIBLE STEPS (right) = 7: the most that always-guess-the-middle ever needs.", Color.Gray);
        UI.Log("The strategy in C#:", Color.Gainsboro);
        UI.Log("", Color.Gainsboro);
        UI.Log("    int low = 1;", Color.Khaki);
        UI.Log("    int high = 100;", Color.Khaki);
        UI.Log("    int middle = low + (high - low) / 2;   // guess this", Color.Khaki);
        UI.Log("    // too low?  low = middle + 1     too high?  high = middle - 1", Color.Khaki);

        UpdateRoom3Prompt();
    }

    private void UpdateRoom3Prompt()
    {
        int mid = _lo + (_hi - _lo) / 2;

        string task = _guesses < FreeGuesses
            ? $"Type  guess <number>  (still possible: {_lo}..{_hi}). Free guesses left: {FreeGuesses - _guesses}."
            : $"Type  guess <number>  (still possible: {_lo}..{_hi}). Each guess now costs {GuessCost} stamina!";
        UI.SetTask(task);

        SetHandler(Room3Input,
            help: "guess 50  - guess a number from 1 to 100 (just  50  works too)\n" +
                  "bot       - watch the best strategy play (reveals the secret and clears the floor)",
            hint: $"Guess the middle of what is still possible: {_lo} + ({_hi} - {_lo}) / 2 = {mid}.");
    }

    private bool Room3Input(string input)
    {
        if (input == "bot")
        {
            UI.Log("", Color.Gainsboro);
            UI.Log("> The bot takes over and plays on your secret number...", Color.Cyan);
            RunBot(_secret);
            ClearFloor();
            return true;
        }

        Match m = Regex.Match(input, @"^(?:guess\s*)?(-?\d+)$");
        if (m.Success)
        {
            if (!int.TryParse(m.Groups[1].Value, out int g)) g = int.MaxValue;
            Guess(g);
            return true;
        }

        return false;
    }

    private void Guess(int g)
    {
        UI.Log("", Color.Gainsboro);

        if (g < 1 || g > 100)
        {
            UI.Log("  The secret is between 1 and 100. That guess doesn't count.", Color.Orange);
            return;
        }

        _guesses++;
        _steps++;
        Counter.Add();

        // Every guess beyond the free ones costs stamina (a winning guess still wins)
        if (_guesses > FreeGuesses)
        {
            _stamina -= GuessCost;
            UI.SetStamina(_stamina);
        }

        if (g == _secret)
        {
            Win();
            return;
        }

        bool tooLow = g < _secret;
        UI.Log($"> guess {g}  ->  {(tooLow ? "HIGHER (your guess was too low)" : "LOWER (your guess was too high)")}   guesses: {_guesses}", Color.Khaki);

        if (g < _lo || g > _hi)
            UI.Log($"  Wasted guess: you already knew the secret was in {_lo}..{_hi}.", Color.Orange);

        if (tooLow) _lo = Math.Max(_lo, g + 1);
        else _hi = Math.Min(_hi, g - 1);

        UI.Log($"  Still possible: {_lo}..{_hi}  ({_hi - _lo + 1} numbers)", Color.Cyan);

        if (_guesses == FreeGuesses)
            UI.Log($"  That was your last free guess. From now on each guess costs {GuessCost} stamina.", Color.Orange);
        else if (_guesses > FreeGuesses)
            UI.Log($"  Stamina -{GuessCost}  ({Math.Max(_stamina, 0)}/100)", Color.OrangeRed);

        if (_stamina <= 0)
        {
            StartRoom3("You collapse from exhaustion! The hall resets with a NEW secret number and full stamina.\n" +
                       "Tip: always guess the middle, or type  bot  to watch how it's done.");
            return;
        }

        UpdateRoom3Prompt();
    }

    private void Win()
    {
        UI.Log($"> guess {_secret}  ->  CORRECT!   guesses: {_guesses}", Color.LightGreen);

        if (_guesses <= 7)
            UI.Log($"  Solved in {_guesses}. Always guessing the middle guarantees 7 or fewer for 1..100 (2^7 = 128 > 100).", Color.Cyan);
        else
            UI.Log($"  That took {_guesses} guesses. Always guessing the middle never needs more than 7. Type  bot  to watch it.", Color.Cyan);

        ClearFloor();
    }

    // Plays the optimal strategy on the given secret and prints every guess
    private void RunBot(int secret)
    {
        Counter.Reset();
        int steps = 0;
        int low = 1, high = 100;

        UI.Log("BOT: strategy = always guess the middle of low..high.", Color.Cyan);
        while (low <= high)
        {
            int middle = low + (high - low) / 2;
            Counter.Add();
            steps++;

            if (middle == secret)
            {
                UI.Log($"  Guess {steps}: {middle}  (range {low}..{high})  ->  CORRECT!", Color.LightGreen);
                break;
            }

            if (middle < secret)
            {
                UI.Log($"  Guess {steps}: {middle}  (range {low}..{high})  ->  higher", Color.Khaki);
                low = middle + 1;
            }
            else
            {
                UI.Log($"  Guess {steps}: {middle}  (range {low}..{high})  ->  lower", Color.Khaki);
                high = middle - 1;
            }
        }

        UI.Log($"BOT found {secret} in {steps} guesses. (The most it ever needs for 1..100 is 7.)", Color.LightGreen);
    }

    private void ClearFloor()
    {
        UI.SetRoom("Floor cleared!");
        UI.Log("", Color.Gainsboro);
        UI.Log("=== FLOOR 2 CLEARED ===", Color.Gold);
        UI.Log("Binary search, halving n, and higher/lower all throw away half the data every step.", Color.Gainsboro);
        UI.Log("1,000,000 items need about 20 steps. 1,000,000,000 need about 30. That's O(log n).", Color.Gainsboro);

        UI.SetTask("Floor cleared! Type  next  or click Next Floor >.");
        SetHandler(input =>
        {
            if (input == "bot")
                RunBot(_rng.Next(1, 101));   // demo on a fresh secret, no stamina involved
            UI.Log("  Floor cleared! Type  next  or click Next Floor >.", Color.Orange);
            return true;
        },
            help: "next  - go to the next floor\n" +
                  "bot   - watch the best strategy play again on a new secret",
            hint: "Type next.");
        UI.FloorCleared();
    }
}