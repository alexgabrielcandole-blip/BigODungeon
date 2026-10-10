using System.Drawing;
using System.Windows.Forms;

namespace BigODungeon;

public partial class Form1 : Form
{
    private int _testOps = 0; // TEMP: layout test only

    public Form1()
    {
        InitializeComponent();
    }

    // Appends one colored line to the story box (we'll reuse this everywhere)
    private void Log(string text, Color color)
    {
        rtbStory.SelectionStart = rtbStory.TextLength;
        rtbStory.SelectionColor = color;
        rtbStory.AppendText(text + Environment.NewLine);
        rtbStory.ScrollToCaret();
    }

    // Adds a styled choice button above the input row (reused by every floor)
    private Button AddActionButton(string text, EventHandler onClick)
    {
        var b = new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(110, 38),
            Padding = new Padding(8, 0, 8, 0),
            Margin = new Padding(0, 0, 8, 0),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 70, 100),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            UseVisualStyleBackColor = false
        };
        b.FlatAppearance.BorderColor = Color.FromArgb(120, 120, 160);
        b.Click += onClick;
        flowActions.Controls.Add(b);
        return b;
    }

    // Removes all choice buttons (call this when a room changes)
    private void ClearActions()
    {
        foreach (Control c in flowActions.Controls.Cast<Control>().ToList())
        {
            flowActions.Controls.Remove(c);
            c.Dispose();
        }
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        Log("Welcome to Dungeon Descent.", Color.Gold);
        Log("(Layout test) Type something and press Submit or Enter.", Color.Gainsboro);

        // TEMP: shows that action buttons can be added at runtime
        for (int i = 1; i <= 3; i++)
        {
            int n = i;
            AddActionButton($"Choice {n}", (s, ev) => Log($"You picked Choice {n}.", Color.Khaki));
        }
    }

    private void btnSubmit_Click(object? sender, EventArgs e)
    {
        // TEMP: bumps the counter and drains stamina to test the side panel
        _testOps++;
        lblOps.Text = _testOps.ToString();
        prgStamina.Value = Math.Max(0, 100 - _testOps * 10);
        Log($"> {txtInput.Text}", Color.LightGreen);
        txtInput.Clear();
        txtInput.Focus();
    }

    private void btnNext_Click(object? sender, EventArgs e)
    {
        Log("(Next Floor clicked - not wired yet)", Color.Orange);
    }
}