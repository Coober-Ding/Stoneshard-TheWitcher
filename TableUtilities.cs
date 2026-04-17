using ModShardLauncher;
using System.Reflection;
using System.Runtime.Serialization;

namespace TheWitcher;

public static partial class TableUtils
{
    /*
    private static string? GetEnumMemberValue<T>(this T value)
     where T : Enum
    {
        return typeof(T)
            .GetTypeInfo()
            .DeclaredMembers
            .SingleOrDefault(x => x.Name == value.ToString())?
            .GetCustomAttribute<EnumMemberAttribute>(false)?
            .Value ?? value.ToString();
    }
    */
    public static void InjectItemsToTable(string table, string? anchor = null, int? defaultKey = null, params Dictionary<int, string>[] items)
    {
        List<string> lines = Msl.ThrowIfNull(ModLoader.GetTable(table));
        int colCount = lines[0].Split(';').Length;

        var targets = new List<string>(items.Length);
        foreach (var item in items)
        {
            var record = new string[colCount];
            foreach (var kv in item)
            {
                if (kv.Key >= 0 && kv.Key < colCount)
                    record[kv.Key] = kv.Value;
            }

            if (defaultKey.HasValue && item.TryGetValue(defaultKey.Value, out var defVal))
            {
                for (int c = 0; c < colCount; c++)
                {
                    if (record[c] == null)
                        record[c] = defVal;
                }
            }

            targets.Add(string.Join(';', record));
        }

        if (anchor != null)
        {
            int pos = lines.FindIndex(l => l.StartsWith(anchor));
            pos = pos < 0 ? lines.Count : pos;
            lines.InsertRange(pos + 1, targets);
        }
        else
        {
            lines.AddRange(targets);
        }

        ModLoader.SetTable(lines, table);
    }

    public static void InjectItemsToTable(string table, string? anchor = null, string? defaultKey = null, params Dictionary<string, string>[] items)
    {
        List<string> lines = Msl.ThrowIfNull(ModLoader.GetTable(table));
        var header = lines[0].Split(';');
        var name2idx = header
            .Select((name, idx) => new KeyValuePair<string, int>(name, idx))
            .ToDictionary(p => p.Key, p => p.Value);

        var mapped = new Dictionary<int, string>[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            var src = items[i];
            var dst = new Dictionary<int, string>(src.Count);
            foreach (var kv in src)
                if (name2idx.TryGetValue(kv.Key, out int idx))
                    dst[idx] = kv.Value;
            mapped[i] = dst;
        }

        int? defaultIdx = null;
        if (!string.IsNullOrEmpty(defaultKey) && name2idx.TryGetValue(defaultKey, out int tmpIdx))
            defaultIdx = tmpIdx;

        InjectItemsToTable(table, anchor, defaultIdx, mapped);
    }

    public static void ModifyItemsInTable(
        string table,
        string match,
        params Dictionary<int, string>[] items)
    {
        List<string> lines = Msl.ThrowIfNull(ModLoader.GetTable(table));
        int colCount = lines[0].Split(';').Length;

        // 收集所有命中行的行号（跳过表头）
        var matchedRows = new List<int>();
        for (int r = 1; r < lines.Count; r++)
        {
            if (lines[r].Contains(match))
                matchedRows.Add(r);
        }

        if (matchedRows.Count == 0)
            throw new KeyNotFoundException($"No row in '{table}' contains match text: \"{match}\".");

        if (items == null || items.Length == 0)
            return;

        // 规则：1个 item -> 所有命中行都用它；多个 item -> 顺序对应前 N 个命中行
        Dictionary<int, string>? pickItem(int idx)
            => (items.Length == 1) ? items[0] : (idx < items.Length ? items[idx] : null);

        for (int i = 0; i < matchedRows.Count; i++)
        {
            var rowIndex = matchedRows[i];
            var src = pickItem(i);
            if (src == null) continue;

            var existing = lines[rowIndex].Split(';');
            foreach (var kv in src)
            {
                if (kv.Key >= 0 && kv.Key < colCount)
                    existing[kv.Key] = kv.Value;
            }

            lines[rowIndex] = string.Join(';', existing);
        }

        ModLoader.SetTable(lines, table);
    }

    public static void ModifyItemsInTable(
        string table,
        string match,
        params Dictionary<string, string>[] items)
    {
        List<string> lines = Msl.ThrowIfNull(ModLoader.GetTable(table));

        var header = lines[0].Split(';');
        var name2idx = header
            .Select((name, idx) => new KeyValuePair<string, int>(name, idx))
            .ToDictionary(p => p.Key, p => p.Value);

        // 将 string-key 的 items 映射为 int-key
        var mapped = new Dictionary<int, string>[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            var src = items[i];
            var dst = new Dictionary<int, string>(src.Count);
            foreach (var kv in src)
                if (name2idx.TryGetValue(kv.Key, out int idx))
                    dst[idx] = kv.Value;
            mapped[i] = dst;
        }

        ModifyItemsInTable(table, match, mapped);
    }

}