
using Dalamud.Plugin.Ipc;
using PvpStats.Types.Match;
using System;

namespace PvpStats.Services;

internal class IpcService : IDisposable {
    private Plugin _plugin;
    private readonly ICallGateProvider<string, bool> _onMatchSaved;
    private readonly ICallGateProvider<string, bool> _openMatchDetailsWindow;

    internal IpcService(Plugin plugin) {
        _plugin = plugin;

        _onMatchSaved = _plugin.PluginInterface.GetIpcProvider<string, bool>($"{_plugin.PluginInterface.InternalName}.OnMatchSaved");
        _openMatchDetailsWindow = _plugin.PluginInterface.GetIpcProvider<string, bool>($"{_plugin.PluginInterface.InternalName}.OpenMatchDetailsWindow");

        _openMatchDetailsWindow.RegisterFunc(OpenMatchDetailsWindow);
    }

    public void Dispose() {
        _openMatchDetailsWindow.UnregisterFunc();
    }

    private bool OpenMatchDetailsWindow(string id) {
        PvpMatch? match = _plugin.Storage.TryGetPvpMatch(id);
        if(match == null) return false;
        _plugin.WindowManager.OpenMatchDetailsWindow(match);
        return true;
    }

    public void OnMatchSaved(string id) {
        _onMatchSaved.SendMessage(id);
    }
}
