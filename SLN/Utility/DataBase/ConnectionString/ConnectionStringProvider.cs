namespace SLN.Utility.DataBase;

public class ConnectionStringProvider
{
    public static string? GetConnectionString(List<ConnectionStringItem> configList, string dataBaseName, bool write = false)
    {
        if (dataBaseName == null || configList == null)
        {
            return null;
        }

        var item = configList.FirstOrDefault(r => dataBaseName.Equals(r.Name, StringComparison.InvariantCultureIgnoreCase));
        if (item != null)
        {
            if (write)
            {
                return item.Write;
            }
            else if (item.Read?.Length > 0)
            {
                if (item.Read.Length == 1)
                {
                    return item.Read[0];
                }

                if (item.Read.Length == item.ReadWeight?.Length)
                {
                    var total = item.ReadWeight.Sum();
                    if (total > 0)
                    {
                        // ReadWeight项有效，示例：
                        // Read: ["str1", "str2", "str3"]
                        // ReadWeight: [1, 0, 5]
                        // 随机区间: [0,1), null, [1,6)
                        int lowWeight = 0;
                        var selected = new Random().Next(0, total);
                        for (int i = 0; i < item.Read.Length; i++)
                        {
                            int weight = item.ReadWeight[i];
                            if (weight == 0)
                            {
                                continue;
                            }

                            int highWeight = lowWeight + weight;
                            if (selected >= lowWeight && selected < highWeight)
                            {
                                return item.Read[i];
                            }

                            lowWeight = highWeight;
                        }
                    }
                }
                else
                {
                    return item.Read[new Random().Next(0, item.Read.Length)];
                }
            }
        }

        return null;
    }
}