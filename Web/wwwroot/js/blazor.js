(() => Blazor.start())();

Blazor.defaultReconnectionHandler._reconnectCallback = (d) => {
    document.location.reload();
}