using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DeleteBranchNSB;

public partial class Form1 : Form
{
    // ──────────────────────────────────────────────────────────────────────────
    // Inner model
    // ──────────────────────────────────────────────────────────────────────────
    private sealed class LibraryItem
    {
        public int    ID             { get; init; }
        public string Name           { get; init; } = string.Empty;
        public string IndexDataTable { get; init; } = string.Empty;
        public override string ToString() => Name;
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Constructor
    // ──────────────────────────────────────────────────────────────────────────
    public Form1()
    {
        InitializeComponent();
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Connect
    // ──────────────────────────────────────────────────────────────────────────
    private async void btnConnect_Click(object sender, EventArgs e)
    {
        string connStr = txtConnectionString.Text.Trim();
        if (string.IsNullOrEmpty(connStr))
        {
            MessageBox.Show("Please enter a connection string.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetUIEnabled(false);
        Log("Connecting to database...");

        try
        {
            await LoadDataAsync(connStr);
            Log($"✅ Connected. {clbLibraries.Items.Count} libraries, {cboBranch.Items.Count} branches loaded.");
            tsslStatus.Text = "Connected";
        }
        catch (Exception ex)
        {
            Log($"❌ Connection failed: {ex.Message}");
            tsslStatus.Text = "Connection failed";
            MessageBox.Show($"Failed to connect:\n{ex.Message}", "Connection Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetUIEnabled(true);
        }
    }

    private async Task LoadDataAsync(string connStr)
    {
        clbLibraries.Items.Clear();
        cboBranch.Items.Clear();

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        // ── Libraries ─────────────────────────────────────────────────────────
        // IndexDataTable is a uniqueidentifier column in SQL Server — read as Guid, then convert to string.
        const string libSql = "SELECT ID, Name, IndexDataTable FROM Portal ORDER BY Name";
        await using (var cmd = new SqlCommand(libSql, conn))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                string indexDataTable = string.Empty;
                if (!reader.IsDBNull(2))
                {
                    // GetValue handles both uniqueidentifier and nvarchar gracefully
                    object raw = reader.GetValue(2);
                    indexDataTable = raw is Guid g ? g.ToString() : raw.ToString() ?? string.Empty;
                }

                clbLibraries.Items.Add(new LibraryItem
                {
                    ID             = reader.GetInt32(0),
                    Name           = reader.IsDBNull(1) ? "(unnamed)" : reader.GetString(1),
                    IndexDataTable = indexDataTable
                });
            }
        }

        // ── Branches ──────────────────────────────────────────────────────────
        const string branchSql = "SELECT DISTINCT [Branch Name] FROM TempRefTable WHERE [Branch Name] IS NOT NULL ORDER BY [Branch Name]";
        await using (var cmd = new SqlCommand(branchSql, conn))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
                cboBranch.Items.Add(reader.GetString(0));
        }

        if (cboBranch.Items.Count > 0)
            cboBranch.SelectedIndex = 0;
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Delete
    // ──────────────────────────────────────────────────────────────────────────
    private async void btnDelete_Click(object sender, EventArgs e)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
        {
            MessageBox.Show("Please connect to the database first.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (cboBranch.SelectedItem is null)
        {
            MessageBox.Show("Please select a Branch.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (clbLibraries.CheckedItems.Count == 0)
        {
            MessageBox.Show("Please select at least one Library.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string branch  = cboBranch.SelectedItem.ToString()!;
        int    libCount = clbLibraries.CheckedItems.Count;

        var confirm = MessageBox.Show(
            $"You are about to delete documents for:\n\n" +
            $"  Branch    : {branch}\n" +
            $"  Libraries : {libCount}\n\n" +
            "This action cannot be undone. Proceed?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

        if (confirm != DialogResult.Yes) return;

        SetUIEnabled(false);
        pbProgress.Value = 0;
        Log("─────────────────────────────────────────────────────");
        Log($"🚀 Delete started  |  Branch: {branch}  |  Libraries: {libCount}");

        try
        {
            await DeleteDocumentsAsync(txtConnectionString.Text.Trim(), branch);
        }
        catch (Exception ex)
        {
            Log($"❌ Fatal error: {ex.Message}");
            MessageBox.Show($"An unexpected error occurred:\n{ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetUIEnabled(true);
            pbProgress.Value = 0;
            tsslStatus.Text = "Ready";
        }
    }

    private async Task DeleteDocumentsAsync(string connStr, string branch)
    {
        var libraries = clbLibraries.CheckedItems.Cast<LibraryItem>().ToList();
        int totalLibs   = libraries.Count;
        int totalDeleted = 0;
        int totalErrors  = 0;

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        // ── Phase 1 (0 → 50%): Collect all DocumentIDs ────────────────────────
        Log("📋 Phase 1/2 — Collecting document IDs...");

        // workList holds (LibraryName, DocumentID) for every document to delete
        var workList = new List<(string LibName, string DocId)>();
        int processedLibs = 0;

        foreach (var lib in libraries)
        {
            Log($"\n📚 Library: [{lib.Name}]");
            tsslStatus.Text = $"Scanning: {lib.Name}";

            var guids = lib.IndexDataTable
                           .Split(new[] { ',', '\n', '\r', '\t', ' ' },
                                  StringSplitOptions.RemoveEmptyEntries)
                           .Select(g => g.Trim())
                           .Where(g => g.Length > 0)
                           .Distinct()
                           .ToList();

            if (guids.Count == 0)
                Log("   ⚠️  No index tables found in IndexDataTable.");

            foreach (var guid in guids)
            {
                Log($"   🔍 Index table: [{guid}]");
                try
                {
                    // NOTE: table name comes from the database, not user input.
                    string selectSql = $"SELECT DocumentID FROM [{guid}] WHERE [Branch] = @branch";
                    await using var selCmd = new SqlCommand(selectSql, conn);
                    selCmd.Parameters.AddWithValue("@branch", branch);
                    selCmd.CommandTimeout = 60;

                    int found = 0;
                    await using var reader = await selCmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            workList.Add((lib.Name, reader.GetValue(0).ToString()!));
                            found++;
                        }
                    }
                    Log($"      Found {found} document(s)");
                }
                catch (Exception ex)
                {
                    Log($"   ⚠️  Cannot read [{guid}]: {ex.Message}");
                    totalErrors++;
                }
            }

            processedLibs++;
            // Phase 1 occupies 0–50% of the bar
            SetProgress((int)(50.0 * processedLibs / totalLibs));
        }

        // ── Phase 2 (50 → 100%): Delete each document ─────────────────────────
        int totalDocs = workList.Count;
        Log($"\n🗑  Phase 2/2 — Deleting {totalDocs} document(s)...");

        if (totalDocs == 0)
        {
            Log("   ℹ️  No documents found for the selected branch and libraries.");
            SetProgress(100);
        }
        else
        {
            int deletedSoFar = 0;

            foreach (var (libName, docId) in workList)
            {
                tsslStatus.Text = $"Deleting {deletedSoFar + 1}/{totalDocs}...";
                try
                {
                    await using var spCmd = new SqlCommand("[dbo].[deleteDocument]", conn)
                    {
                        CommandType    = CommandType.StoredProcedure,
                        CommandTimeout = 120
                    };
                    // Both parameters carry the same DocumentID as confirmed in spec.
                    spCmd.Parameters.AddWithValue("@docID_FROM", docId);
                    spCmd.Parameters.AddWithValue("@docID_TO", docId);

                    await spCmd.ExecuteNonQueryAsync();
                    Log($"   ✅ Deleted: {docId}  [{libName}]");
                    totalDeleted++;
                }
                catch (Exception ex)
                {
                    Log($"   ❌ Failed [{docId}]: {ex.Message}");
                    totalErrors++;
                }

                deletedSoFar++;
                // Phase 2 occupies 50–100% of the bar
                SetProgress(50 + (int)(50.0 * deletedSoFar / totalDocs));
            }
        }

        Log($"\n─────────────────────────────────────────────────────");
        Log($"📊 Summary — Deleted: {totalDeleted}  |  Errors: {totalErrors}");
        Log("─────────────────────────────────────────────────────");
        tsslStatus.Text = $"Done — {totalDeleted} deleted, {totalErrors} errors";
    }

    /// <summary>Thread-safe progress bar update (clamped 0–100).</summary>
    private void SetProgress(int value)
    {
        int clamped = Math.Clamp(value, 0, 100);
        if (pbProgress.InvokeRequired)
            pbProgress.Invoke(() => pbProgress.Value = clamped);
        else
            pbProgress.Value = clamped;
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Helper button handlers
    // ──────────────────────────────────────────────────────────────────────────
    private void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < clbLibraries.Items.Count; i++)
            clbLibraries.SetItemChecked(i, true);
    }

    private void btnClearAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < clbLibraries.Items.Count; i++)
            clbLibraries.SetItemChecked(i, false);
    }

    private void btnClearLog_Click(object sender, EventArgs e) => rtbLog.Clear();

    // ──────────────────────────────────────────────────────────────────────────
    // Utilities
    // ──────────────────────────────────────────────────────────────────────────
    private void Log(string message)
    {
        if (rtbLog.InvokeRequired)
        {
            rtbLog.Invoke(() => Log(message));
            return;
        }
        rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        rtbLog.ScrollToCaret();
    }

    private void SetUIEnabled(bool enabled)
    {
        btnConnect.Enabled      = enabled;
        btnDelete.Enabled       = enabled;
        btnSelectAll.Enabled    = enabled;
        btnClearAll.Enabled     = enabled;
        cboBranch.Enabled       = enabled;
        clbLibraries.Enabled    = enabled;
        txtConnectionString.Enabled = enabled;
        btnClearLog.Enabled     = enabled;
    }
}
