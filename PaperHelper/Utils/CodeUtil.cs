using System.Text;

namespace PaperHelper.Utils;

public static class CodeUtil
{
    // <summary>
    /// 生成单个随机数字
    /// </summary>
    private static int createNum()
    {
        var random = new Random(Guid.NewGuid().GetHashCode());
        var num = random.Next(10);
        return num;
    }

    /// <summary>
    /// 生成单个大写随机字母
    /// </summary>
    private static string createBigAbc()
    {
        //A-Z的 ASCII值为65-90
        var random = new Random(Guid.NewGuid().GetHashCode());
        var num = random.Next(65, 91);
        var abc = Convert.ToChar(num).ToString();
        return abc;
    }

    /// <summary>
    /// 生成单个小写随机字母
    /// </summary>
    private static string createSmallAbc()
    {
        //a-z的 ASCII值为97-122
        var random = new Random(Guid.NewGuid().GetHashCode());
        var num = random.Next(97, 123);
        var abc = Convert.ToChar(num).ToString();
        return abc;
    }


    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="length">字符串的长度</param>
    /// <returns></returns>
    public static string GenInvitationCode(int length)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            //随机选择里面其中的一种字符生成
            switch (random.Next(3))
            {
                case 0:
                    //调用生成生成随机数字的方法
                    sb.Append(createNum());
                    break;
                case 1:
                    //调用生成生成随机小写字母的方法
                    sb.Append(createSmallAbc());
                    break;
                case 2:
                    //调用生成生成随机大写字母的方法
                    sb.Append(createBigAbc());
                    break;
            }
        }
        return sb.ToString();
    }
}