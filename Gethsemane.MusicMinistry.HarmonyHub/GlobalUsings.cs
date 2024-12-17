global using System.Collections.Immutable;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Gethsemane.MusicMinistry.HarmonyHub.Models;
global using Gethsemane.MusicMinistry.HarmonyHub.Presentation;
global using Gethsemane.MusicMinistry.HarmonyHub.DataContracts;
global using Gethsemane.MusicMinistry.HarmonyHub.DataContracts.Serialization;
global using Gethsemane.MusicMinistry.HarmonyHub.Services.Caching;
global using Gethsemane.MusicMinistry.HarmonyHub.Services.Endpoints;
#if MAUI_EMBEDDING
global using Gethsemane.MusicMinistry.HarmonyHub.MauiControls;
#endif
global using ApplicationExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;

[assembly: Uno.Extensions.Reactive.Config.BindableGenerationTool(3)]