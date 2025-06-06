namespace Chat.API.Hubs.Abstractions
{
    /// <summary>
    /// Атрибут для определения маршрута SignalR хаба
    /// </summary>
    /// <remarks>
    /// Используется для указания пути, по которому клиенты могут подключиться к хабу
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class HubRouteAttribute(string route) : Attribute
    {
        /// <summary>
        /// Маршрут хаба
        /// </summary>
        /// <value>Строка, представляющая endpoint для доступа к хабу</value>
        public string Route { get; private init; } = route;
    }
}
