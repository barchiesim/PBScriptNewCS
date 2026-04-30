namespace PBScriptNew.Forms;

public class ColumnSelectorDialog : Form
{
    private readonly string[] _auditCols = ["dat_utente_cre", "cod_utente_cre", "dat_utente_mod", "cod_utente_mod", "cod_azienda_cre"];

    private CheckedListBox clbKeys = null!;
    private CheckedListBox clbCols = null!;

    public List<string> SelectedKeyColumns { get; private set; } = new();
    public List<string> SelectedColumns    { get; private set; } = new();

    public ColumnSelectorDialog(IEnumerable<string> allColumns, IEnumerable<string> keyColumns, string tableName, string scriptType = "INSERT")
    {
        var keys    = keyColumns.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var allCols = allColumns.ToList();
        bool isDel  = scriptType.Equals("DELETE", StringComparison.OrdinalIgnoreCase);

        Text            = $"Seleziona colonne per {scriptType} – {tableName}";
        Size            = new Size(620, 520);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        Font            = new Font("Segoe UI", 9f);

        // ── Chiavi ────────────────────────────────────────────────────────
        var btnAllKeys = new Button { Text = "*", Location = new Point(12, 8), Size = new Size(24, 22), TabStop = false };
        Controls.Add(new Label { Text = "Chiavi", Location = new Point(40, 12), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
        clbKeys = new CheckedListBox
        {
            Location     = new Point(12, 36),
            Size         = new Size(isDel ? 570 : 270, 380),
            CheckOnClick = true,
            Font         = new Font("Courier New", 9f),
            ForeColor    = Color.DarkRed
        };
        foreach (var c in allCols.Where(c => keys.Contains(c)))
            clbKeys.Items.Add(c, true);
        btnAllKeys.Click += (_, _) => { for (int i = 0; i < clbKeys.Items.Count; i++) clbKeys.SetItemChecked(i, true); };
        Controls.Add(btnAllKeys);
        Controls.Add(clbKeys);

        // ── Colonne normali (nascoste per DELETE) ─────────────────────────
        if (!isDel)
        {
            var btnAllCols = new Button { Text = "*", Location = new Point(298, 8), Size = new Size(24, 22), TabStop = false };
            Controls.Add(new Label { Text = "Colonne", Location = new Point(326, 12), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            clbCols = new CheckedListBox
            {
                Location     = new Point(298, 36),
                Size         = new Size(284, 380),
                CheckOnClick = true,
                Font         = new Font("Courier New", 9f)
            };
            foreach (var c in allCols.Where(c => !keys.Contains(c)))
                clbCols.Items.Add(c, !_auditCols.Contains(c.ToLowerInvariant()));
            btnAllCols.Click += (_, _) => { for (int i = 0; i < clbCols.Items.Count; i++) clbCols.SetItemChecked(i, true); };
            Controls.Add(btnAllCols);
            Controls.Add(clbCols);
        }
        else
        {
            clbCols = new CheckedListBox(); // unused but not null
        }

        // ── Buttons ───────────────────────────────────────────────────────
        var btnOk     = new Button { Text = "Ok",      DialogResult = DialogResult.OK,     Location = new Point(220, 430), Size = new Size(90, 28) };
        var btnCancel = new Button { Text = "Annulla", DialogResult = DialogResult.Cancel, Location = new Point(320, 430), Size = new Size(90, 28) };
        btnOk.Click += (_, _) =>
        {
            SelectedKeyColumns = clbKeys.CheckedItems.Cast<string>().ToList();
            SelectedColumns    = clbCols.CheckedItems.Cast<string>().ToList();
        };
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }
}
