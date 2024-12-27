using Android.App;
using Android.Content.PM;
using Uno.AuthenticationBroker;

namespace Gethsemane.MusicMinistry.HarmonyHub.Droid;

[Activity(
    NoHistory = true,
    LaunchMode = LaunchMode.SingleTop,
    Exported = true)]
[IntentFilter(
    new[] { Android.Content.Intent.ActionView },
    Categories = new[]
    {
        Android.Content.Intent.CategoryDefault,
        Android.Content.Intent.CategoryBrowsable
    },
    DataScheme = CALLBACK_SCHEME)]
public class WebAuthenticatorActivity : Microsoft.Maui.Authentication.
    WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "harmonyhub";
}
