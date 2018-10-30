using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StatusCallback(int nResult);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StatusCallback2(int nResult, [MarshalAs(UnmanagedType.LPStr)] string message);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void QueryRuntimeModeCallback(int nResult, int nMode);

    internal enum ELeaderboardDataRequest
    {
        k_ELeaderboardDataRequestGlobal = 0,
        k_ELeaderboardDataRequestGlobalAroundUser = 1,
        k_ELeaderboardDataRequestLocal = 2,
        k_ELeaderboardDataRequestLocaleAroundUser = 3,
    };

    internal enum ELeaderboardDataTimeRange
    {
        k_ELeaderboardDataScropeAllTime = 0,
        k_ELeaderboardDataScropeDaily = 1,
        k_ELeaderboardDataScropeWeekly = 2,
        k_ELeaderboardDataScropeMonthly = 3,
    };

    internal enum ELeaderboardSortMethod
    {
        k_ELeaderboardSortMethodNone,
        k_ELeaderboardSortMethodAscending,
        k_ELeaderboardSortMethodDescending,
    };

    internal enum ELeaderboardDisplayType
    {
        k_ELeaderboardDisplayTypeNone = 0,
        k_ELeaderboardDisplayTypeNumeric = 1,           // simple numerical score
        k_ELeaderboardDisplayTypeTimeSeconds = 2,       // the score represents a time, in seconds
        k_ELeaderboardDisplayTypeTimeMilliSeconds = 3,  // the score represents a time, in milliseconds
    };

    internal enum ELeaderboardUploadScoreMethod
    {
        k_ELeaderboardUploadScoreMethodNone = 0,
        k_ELeaderboardUploadScoreMethodKeepBest = 1,    // Leaderboard will keep user's best score
        k_ELeaderboardUploadScoreMethodForceUpdate = 2, // Leaderboard will always replace score with specified
    };

    internal enum ELocale
    {
        k_ELocaleUS = 0,
        k_ELocaleDE = 1,
        k_ELocaleJP = 2,
        k_ELocaleKR = 3,
        k_ELocaleRU = 4,
        k_ELocaleCN = 5,
        k_ELocaleTW = 6,
        k_ELocaleFR = 7,
    };

    internal enum EAchievementDisplayAttribute
    {
        k_EAchievementDisplayAttributeName = 0,
        k_EAchievementDisplayAttributeDesc = 1,
        k_EAchievementDisplayAttributeHidden = 2,
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct LeaderboardEntry_t
    {
        internal int m_nGlobalRank;       // [1..N], where N is the number of users with an entry in the leaderboard
        internal int m_nScore;            // score as set in the leaderboard
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        internal string m_pUserName;      // the user name showing in the leaderboard
    };

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct IAPCurrency_t
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        internal string m_pName;          // the name of user setting currency
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        internal string m_pSymbol;        // the symbol of user setting currency
    };

#if !UNITY_ANDROID
    internal partial class ArcadeLeaderboard
    {
        static ArcadeLeaderboard()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        // for ArcadeLeaderboards
        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void IsReady(StatusCallback IsReadyCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_DownloadLeaderboardScores", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nCount);

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_UploadLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, string pchUserName, int nScore);

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScoreCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardScoreCount();

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserRank", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardUserRank();

        [DllImport("viveport_api", EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardUserScore();
    }

    internal partial class Api
    {
        static Api()
        {
            LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_GetLicense", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetLicense(GetLicenseCallback callback, string appId, string appKey);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Init(StatusCallback initCallback, string appId);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Shutdown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Shutdown(StatusCallback initCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Version", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Version();

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_QueryRuntimeMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void QueryRuntimeMode(QueryRuntimeModeCallback queryRunTimeCallback);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        internal static void LoadLibraryManually(string dllName)
        {
#if UNITY_5
            return;
#else
            if (string.IsNullOrEmpty(dllName))
            {
                return;
            }

            var is64 = IntPtr.Size == 8;
            if (is64)
            {
                LoadLibrary("x64/" + dllName);
            }
            else
            {
                LoadLibrary("x86/" + dllName);
            }
#endif
        }
    }

    internal partial class User
    {
        static User()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserID", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserID(StringBuilder userId, int size);

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserName(StringBuilder userName, int size);

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserAvatarUrl", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserAvatarUrl(StringBuilder userAvatarUrl, int size);
    }

    internal partial class UserStats
    {
        static UserStats()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int IsReady(StatusCallback IsReadyCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_DownloadStats", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DownloadStats(StatusCallback downloadStatsCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetStat0", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetStat(string pchName, ref int pnData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetStat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetStat(string pchName, ref float pfData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetStat0", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetStat(string pchName, int nData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetStat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetStat(string pchName, float fData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_UploadStats", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int UploadStats(StatusCallback uploadStatsCallback);

        // for Achievements
        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetAchievement(string pchName, ref int pbAchieved);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetAchievementUnlockTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetAchievementUnlockTime(string pchName, ref int punUnlockTime);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetAchievement(string pchName);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_ClearAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ClearAchievement(string pchName);

        // for Leaderboards
        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_DownloadLeaderboardScores", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataRequest eLeaderboardDataRequest, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_UploadLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, int nScore);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardScoreCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardScoreCount();

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardSortMethod", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ELeaderboardSortMethod GetLeaderboardSortMethod();

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardDisplayType", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ELeaderboardDisplayType GetLeaderboardDisplayType();
    }

    internal partial class IAPurchase
    {
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IsReady(IAPurchaseCallback callback, string pchAppKey);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Request(IAPurchaseCallback callback, string pchPrice);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_RequestWithUserData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Request(IAPurchaseCallback callback, string pchPrice, string pchUserData);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Purchase", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Purchase(IAPurchaseCallback callback, string pchPurchaseId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Query", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Query(IAPurchaseCallback callback, string pchPurchaseId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_QueryList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Query(IAPurchaseCallback callback);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_GetBalance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetBalance(IAPurchaseCallback callback);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_RequestSubscription", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestSubscription(IAPurchaseCallback callback, string pchPrice, string pchFreeTrialType, int nFreeTrialValue,
            string pchChargePeriodType, int nChargePeriodValue, int nNumberOfChargePeriod, string pchPlanId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_RequestSubscriptionWithPlanID", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestSubscriptionWithPlanID(IAPurchaseCallback callback, string pchPlanId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Subscribe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Subscribe(IAPurchaseCallback callback, string pchSubscriptionId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_QuerySubscription", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QuerySubscription(IAPurchaseCallback callback, string pchSubscriptionId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_CancelSubscription", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CancelSubscription(IAPurchaseCallback callback, string pchSubscriptionId);
    }

    namespace Arcade
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SessionCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

        partial class Session
        {
            [DllImport("viveport_api", EntryPoint = "IViveportArcadeSession_IsReady", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void IsReady(SessionCallback callback);
            [DllImport("viveport_api", EntryPoint = "IViveportArcadeSession_Start", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void Start(SessionCallback callback);
            [DllImport("viveport_api", EntryPoint = "IViveportArcadeSession_Stop", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void Stop(SessionCallback callback);
        }
    }

    internal partial class DLC
    {
        static DLC() {
            Api.LoadLibraryManually("viveport_api.dll");
        }

		[DllImport("viveport_api", EntryPoint = "IViveportDlc_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int IsReady(StatusCallback callback);

		[DllImport("viveport_api", EntryPoint = "IViveportDlc_GetCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int GetCount();

        [DllImport("viveport_api", EntryPoint = "IViveportDlc_GetIsAvailable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetIsAvailable(int index, StringBuilder appId, out bool isAvailable);
    }

    internal partial class Token
    {
        static Token()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportToken_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int IsReady(StatusCallback IsReadyCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportToken_GetSessionToken", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetSessionToken(StatusCallback2 GetSessionTokenCallback);
    }
#endif
    }
