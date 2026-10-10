using System.Drawing;
using System.Windows.Forms;

namespace BigODungeon;

public partial class Form1 : Form, IGameUI
{
    private const int TotalFloors = 5;
    private readonly OpCounter _counter = new();
    private readonly List<IFloor> _floors = new List<IFloor> { new Floor1(), new Floor2(), new Floor3() };
    private int _floorIndex = 0;
    private string _floorHeader = ""; // "Floor 1 of 5 - The Vault"

    public Form1()
    {
        InitializeComponent();
        rtbStory.HideSelection = false;    // lets the story box auto-scroll to the newest line
        _counter.Changed += n => lblOps.Text = n.ToString(); // live step counter
    }

    // ================= IGameUI (what floors can ask us to do) =================

    // Appends one colored line to the story box and scrolls to it
    public void Log(string text, Color color)
    {
        rtbStory.SelectionStart = rtbStory.TextLength;
        rtbStory.SelectionLength = 0;
        rtbStory.SelectionColor = color;
        rtbStory.AppendText(text + Environment.NewLine);
        rtbStory.SelectionStart = rtbStory.TextLength;
        rtbStory.ScrollToCaret();
    }

    public void ClearStory()
    {
        rtbStory.Clear();
    }

    // The yellow "what to type now" bar
    public void SetTask(string text)
    {
        lblTask.Text = "TASK:  " + text;
    }

    // Top-right header: floor name + current room
    public void SetRoom(string room)
    {
        lblFloor.Text = $"{_floorHeader}  |  {room}";
    }

    public void SetOptimal(int? steps)
    {
        lblOptimal.Text = steps?.ToString() ?? "-";
    }

    // Stamina bar (0-100); values outside the range are clamped
    public void SetStamina(int value)
    {
        prgStamina.Value = Math.Clamp(value, prgStamina.Minimum, prgStamina.Maximum);
    }

    public void FloorCleared()
    {
        SetNextEnabled(true);
    }

    // ================= Helpers =================

    // "Next Floor" is gray until the floor is cleared
    private void SetNextEnabled(bool enabled)
    {
        btnNext.Enabled = enabled;
        btnNext.BackColor = enabled ? Color.FromArgb(60, 140, 80) : Color.FromArgb(60, 60, 72);
    }

    private void StartFloor(int index)
    {
        IFloor floor = _floors[index];

        SetNextEnabled(false);
        rtbStory.Clear();
        _counter.Reset();
        SetOptimal(null);
        SetStamina(100);                   // every floor starts with full stamina

        _floorHeader = $"Floor {index + 1} of {TotalFloors} - {floor.Title}";
        lblFloor.Text = _floorHeader;
        lblBigO.Text = floor.BigO;
        SetTask("Read the story above, then follow the instructions here.");

        floor.Start(this, _counter);
        txtInput.Focus();
    }

    // ================= Event handlers =================

    private void Form1_Load(object? sender, EventArgs e)
    {
        StartFloor(0);
    }

    // Runs when the player presses Enter or clicks Submit
    private void btnSubmit_Click(object? sender, EventArgs e)
    {
        string text = txtInput.Text.Trim();
        txtInput.Clear();
        txtInput.Focus();

        if (text.Length == 0) return;

        Log($"> {text}", Color.LightGreen);

        // "next" works at any time, but only does something once the floor is cleared
        if (text.Equals("next", StringComparison.OrdinalIgnoreCase))
        {
            if (btnNext.Enabled)
                btnNext_Click(null, EventArgs.Empty);
            else
                Log("  You can't go to the next floor yet. Finish this floor first.", Color.Orange);
            return;
        }

        _floors[_floorIndex].HandleInput(text);
    }

    private void btnNext_Click(object? sender, EventArgs e)
    {
        if (_floorIndex + 1 < _floors.Count)
        {
            _floorIndex++;
            StartFloor(_floorIndex);
        }
        else
        {
            Log("Floors 4-5 aren't built yet. They're coming in the next rounds.", Color.Orange);
        }
        txtInput.Focus();
    }
}